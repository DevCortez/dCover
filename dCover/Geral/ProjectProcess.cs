using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using Microsoft.Runtime;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;


namespace dCover.Geral
{
	public class ProjectProcess
	{
        #region Constants
		const uint SECTION_OFFSET = 0x1000;
        
        const int EXCEPTION_DEBUG_EVENT = 1;
		const int CREATE_THREAD_DEBUG_EVENT = 2;
		const int CREATE_PROCESS_DEBUG_EVENT = 3;
		const int EXIT_THREAD_DEBUG_EVENT = 4;
		const int EXIT_PROCESS_DEBUG_EVENT = 5;
		const int LOAD_DLL_DEBUG_EVENT = 6;
		const int UNLOAD_DLL_DEBUG_EVENT = 7;
		const int OUTPUT_DEBUG_STRING_EVENT = 8;
		const int RIP_EVENT = 9;
		const uint STATUS_BREAKPOINT = 0x80000003;
		#endregion
		
		#region Variables
		private Thread debuggingThread = null;
		STARTUPINFO startupInfo = new STARTUPINFO();
		PROCESS_INFORMATION processInformation = new PROCESS_INFORMATION();
		private ProjectModule module;
        private Project mainProject;
		public uint handle;
		public uint processId;
		private uint baseAddress = 0x400000; //Should be made dynamic, will be reintroduced with system monitoring

		public bool status { get{ return debuggingThread == null ? true : debuggingThread.IsAlive; } }
		private bool breakpointsAreSet = false;
		#endregion
		
		private unsafe void setInitialBreakpoints()
		{
			foreach(CoveragePoint currentPoint in mainProject.coveragePointList.Where(x => module.moduleFile.Contains(x.moduleName) && !x.wasCovered).ToList())
			{
				if(currentPoint.wasCovered || !module.selectedRoutines.Contains(currentPoint.routineName) || !module.selectedSourceFiles.Contains(currentPoint.sourceFile))
					continue;

                uint currentAddress = (uint)(currentPoint.offset + baseAddress + SECTION_OFFSET);

                if (!currentPoint.originalCodeWasRead)
                {                    
                    uint bytesRead = 0;
                    ReadProcessMemory(handle, currentAddress, ref currentPoint.originalCode, 1, ref bytesRead);

                    if (bytesRead == 0)
                        Console.WriteLine(GetLastError());

                    currentPoint.originalCodeWasRead = true;
                }

				byte breakpointByte = 0xCC;
				uint bytesWritten = 0;
				WriteProcessMemory(handle, currentAddress, &breakpointByte, 1, ref bytesWritten);

				if(bytesWritten == 1)
                    currentPoint.isSet = true;
			}

			breakpointsAreSet = true;
		}

		private unsafe bool startProcess()
		{
            if (module.startDirectory.Length == 0)
                module.startDirectory = Environment.CurrentDirectory;
            
            if(module.isHosted)
			{
				#region Initialize hosted module
				string destinationModule = Path.GetDirectoryName(module.host) + "\\" + Path.GetFileName(module.moduleFile);
				string kernel32dll = "kernel32.dll";
				string loadLibraryAProc = "LoadLibraryA";
				uint kernel32Handle;

				uint loadLibraryAddress;

				fixed (void* k32dll = Encoding.ASCII.GetBytes(kernel32dll))
				{
					kernel32Handle = LoadLibraryA(k32dll);
				}

				fixed (void* loadLibProc = Encoding.ASCII.GetBytes(loadLibraryAProc))
				{
					loadLibraryAddress = GetProcAddress(kernel32Handle, loadLibProc);
				}

				try
				{
					File.Copy(module.moduleFile, destinationModule, true);
				}
				catch{}

				CreateProcess(module.host, module.parameters, 0, 0, false, 2, 0, module.startDirectory, ref startupInfo, out processInformation);

                uint stringReference = vAllocExNuma(processInformation.hProcess, 0, 0x1000, 0x1000, 4, 0);
				uint bytesWritten = 0;

				fixed (void* localBuffer = Encoding.ASCII.GetBytes(destinationModule))
				{
					WriteProcessMemory(processInformation.hProcess, stringReference, localBuffer, (uint)destinationModule.Length, ref bytesWritten); 
				}

				CreateRemoteThread(processInformation.hProcess, 0, 0, loadLibraryAddress, stringReference, 0, 0);
				#endregion
			}
			else if(module.isService)
			{
			}
			else
			{
				CreateProcess(module.moduleFile, module.parameters, 0, 0, false, 2, 0, module.startDirectory, ref startupInfo, out processInformation);
			}
			
			if(processInformation.hProcess == 0)
				return false;

			handle = processInformation.hProcess;
			processId = processInformation.dwProcessId;
			mainProject.runningProcesses.Add(Process.GetProcessById((int)processId));
			
			return true;
		}
		
		unsafe void debuggingLoop()
		{
            uint continueStatus;            
			
			if(!startProcess())
				return;

			if(!module.isHosted && !module.isService)
				setInitialBreakpoints();
			
			while(status)
			{
				DEBUG_EVENT debugEvent = new DEBUG_EVENT();

				if(!WaitForDebugEvent(out debugEvent, 0xFFFFFFFF))
					continue;

				continueStatus = 0x00010002;

				if(!breakpointsAreSet)
				{
					if (module.isHosted)
					{
						try
						{
							Process targetProcess = Process.GetProcessById((int)processId);
							ProcessModule targetModule = (from ProcessModule x in targetProcess.Modules where module.moduleFile.Contains(x.FileName) select x).FirstOrDefault();
												
							if (targetModule != null)
							{
								baseAddress = (uint)targetModule.BaseAddress;
								setInitialBreakpoints();
							}
						}
						catch{}
					}
				}
				
				switch(debugEvent.dwDebugEventCode)
				{
					case LOAD_DLL_DEBUG_EVENT:
                        {
                            if (debugEvent.LoadDll.lpImageName == 0)
                                break;
                            
                            #region Check modules being loaded
                            byte[] dllNameBuffer;
							uint dllNamePointer = 0;
							uint bytesRead = 0;

							ReadProcessMemory(handle, debugEvent.LoadDll.lpImageName, ref dllNamePointer, 4, ref bytesRead);						

							int dllNameSize;

							if (debugEvent.LoadDll.fUnicode == 0)
							{
								for(dllNameSize = 0; dllNameSize < 0x4ff; dllNameSize++)
								{
									byte currentByte = 0;
									ReadProcessMemory(handle, Convert.ToUInt32(dllNamePointer + dllNameSize), ref currentByte, 1, ref bytesRead);

									if(currentByte == 0)
										break;
								}
							}
							else
							{
								for (dllNameSize = 0; dllNameSize < 0x4ff; dllNameSize += 2)
								{
									byte currentByte = 0;
									ReadProcessMemory(handle, Convert.ToUInt32(dllNamePointer + dllNameSize), ref currentByte, 1, ref bytesRead);

									if (currentByte == 0)
										break;
								}
							}

							dllNameBuffer = new byte[dllNameSize];
							
							fixed(void* buffer = dllNameBuffer)
							{								
								ReadProcessMemory(handle, dllNamePointer, buffer, (uint)dllNameSize, ref bytesRead);
							}

							string finalNameBuffer;

							if(debugEvent.LoadDll.fUnicode == 0)							
								finalNameBuffer = Encoding.ASCII.GetString(dllNameBuffer);
							else
								finalNameBuffer = Encoding.Unicode.GetString(dllNameBuffer);

							if(finalNameBuffer.Contains(Path.GetFileName(module.moduleFile)))
							{
								baseAddress = debugEvent.LoadDll.lpBaseOfDll;
								setInitialBreakpoints();
							}

                            //Console.WriteLine("[" + Path.GetFileName(module.moduleFile) + "] Loaded " + finalNameBuffer);
							break;
                            #endregion
                        }

                    case EXIT_PROCESS_DEBUG_EVENT:
                        {
                            Console.WriteLine("Process " + module.moduleFile + " died");
                            mainProject.runningProcesses.Remove(mainProject.runningProcesses.Where(x => x.Id == processId).First());
                            return;
                            break;
                        }

                    default: //Could be reverted
						{
							if(debugEvent.Exception.ExceptionRecord.ExceptionCode != STATUS_BREAKPOINT)
							{
								continueStatus = 0x80010001;
								break;
							}

                            CoveragePoint currentPoint = mainProject.coveragePointList.Where(x => x.offset + baseAddress + SECTION_OFFSET == debugEvent.Exception.ExceptionRecord.ExceptionAddress && module.moduleFile.Contains(x.moduleName)).FirstOrDefault();
                            
                            if(currentPoint != null && currentPoint.isSet)
                            {
                                #region Handle breakpoint
                                Console.WriteLine("[" + currentPoint.moduleName + "] Executed line " + currentPoint.lineNumber + " -> " + currentPoint.sourceFile + " @ " + currentPoint.routineName);

                                byte originalValue = currentPoint.originalCode;
                                uint bytesWritten = 0;

								WriteProcessMemory(handle, debugEvent.Exception.ExceptionRecord.ExceptionAddress, &originalValue, 1, ref bytesWritten);
								currentPoint.wasCovered = true;
								currentPoint.isSet = false;
                                
                                ThreadContext threadContext = new ThreadContext();
                                threadContext.ContextFlags = 0x10001;
                                uint threadHandle = OpenThread(0x001F03FF, false, debugEvent.dwThreadId); //THREAD_ALL_ACCESS

                                if (!GetThreadContext(threadHandle, ref threadContext))
                                    Console.WriteLine(GetLastError());

                                threadContext.Eip--;
                                SetThreadContext(threadHandle, ref threadContext);
                                CloseHandle(threadHandle);
                                #endregion
                            }
                            
							break;
						}
				}

				ContinueDebugEvent(debugEvent.dwProcessId, debugEvent.dwThreadId, continueStatus);
			}
		}

		public bool CreateProcess(ProjectModule targetModule, Project project)
		{
			startupInfo.cb = (uint)System.Runtime.InteropServices.Marshal.SizeOf(startupInfo);
			startupInfo.dwFlags = 1;
			module = targetModule;
            mainProject = project;

			debuggingThread = new Thread(new ThreadStart(debuggingLoop));
			debuggingThread.Start();
		
			ResumeThread(processInformation.hThread);			
			return true;
		}

		public bool AttachToProcess(Process target, ProjectModule targetModule, Project project)
		{
			mainProject = project;
			
			if(!DebugActiveProcess((uint)target.Id))
			{
				Console.WriteLine("Cannot attach to process");
				return false;
			}

            List<ProcessThread> originalStates = (from ProcessThread x in target.Threads select x).ToList();

            foreach(ProcessThread x in target.Threads)
            {
                if(x.ThreadState == System.Diagnostics.ThreadState.Running)
                {
                    uint threadHandle = OpenThread(0x001F03FF, false, (uint)x.Id);
                    SuspendThread(threadHandle);
                    CloseHandle(threadHandle);
                }                
            }

			module = targetModule;
			baseAddress = (uint)target.MainModule.BaseAddress;
            handle = OpenProcess(0x001F0FFF, false, (uint)target.Id);
            
            setInitialBreakpoints();
			
			debuggingThread = new Thread(new ThreadStart(debuggingLoop));
			debuggingThread.Start();

            foreach(ProcessThread x in target.Threads)
            {
                if(x.ThreadState != (originalStates.Where(y => y.Id == x.Id).First()).ThreadState)
                {
                    uint threadHandle = OpenThread(0x001F03FF, false, (uint)x.Id);
                    ResumeThread(threadHandle);
                    CloseHandle(threadHandle);
                }
            }
			
			project.runningProcesses.Add(target);			
			return true;
		}

		[Flags]
		public enum HookRegister
		{
			None = 0,
			DR0 = 1,
			DR1 = 2,
			DR2 = 4,
			DR3 = 8
		}

		#region Structs


		[StructLayout(LayoutKind.Sequential)]
		private unsafe struct DEBUG_EVENT
		{
			public readonly uint dwDebugEventCode;
			public readonly uint dwProcessId;
			public readonly uint dwThreadId;


			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 86, ArraySubType = UnmanagedType.U1)]
			private readonly byte[] debugInfo;


			public EXCEPTION_DEBUG_INFO Exception
			{
				get
				{
					if (debugInfo == null)
						return new EXCEPTION_DEBUG_INFO();


					fixed (byte* ptr = debugInfo)
					{
						return *(EXCEPTION_DEBUG_INFO*)ptr;
					}
				}
			}


			public LOAD_DLL_DEBUG_INFO LoadDll
			{
				get
				{
					if (debugInfo == null)
						return new LOAD_DLL_DEBUG_INFO();


					fixed (byte* ptr = debugInfo)
					{
						return *(LOAD_DLL_DEBUG_INFO*)ptr;
					}
				}
			}
		}


		[StructLayout(LayoutKind.Sequential)]
		private struct EXCEPTION_DEBUG_INFO
		{
			public EXCEPTION_RECORD ExceptionRecord;
			public readonly uint dwFirstChance;
		}


		[StructLayout(LayoutKind.Sequential)]
		private struct EXCEPTION_RECORD
		{
			public readonly uint ExceptionCode;
			public readonly uint ExceptionFlags;
			public readonly uint ExceptionRecord;
			public readonly uint ExceptionAddress;
			public readonly uint NumberParameters;

			public unsafe fixed uint ExceptionInformation[15];
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct FLOATING_SAVE_AREA
		{
			public uint ControlWord;
			public uint StatusWord;
			public uint TagWord;
			public uint ErrorOffset;
			public uint ErrorSelector;
			public uint DataOffset;
			public uint DataSelector;

			public unsafe fixed byte RegisterArea[80];


			public uint Cr0NpxState;
		}


		[StructLayout(LayoutKind.Sequential)]
		private struct LOAD_DLL_DEBUG_INFO
		{
			public readonly uint hFile;
			public readonly uint lpBaseOfDll;
			public readonly uint dwDebugInfoFileOffset;
			public readonly uint nDebugInfoSize;
			public readonly uint lpImageName;
			public readonly ushort fUnicode;
		}


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct THREADENTRY32
		{
			internal UInt32 dwSize;
			internal readonly UInt32 cntUsage;
			internal readonly UInt32 th32ThreadID;
			internal readonly UInt32 th32OwnerProcessID;
			internal readonly UInt32 tpBasePri;
			internal readonly UInt32 tpDeltaPri;
			internal readonly UInt32 dwFlags;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct ThreadContext
		{
			public uint ContextFlags; //set this to an appropriate value 
			// Retrieved by CONTEXT_DEBUG_REGISTERS 
			public uint Dr0;
			public uint Dr1;
			public uint Dr2;
			public uint Dr3;
			public uint Dr6;
			public uint Dr7;
			// Retrieved by CONTEXT_FLOATING_POINT 
			public FLOATING_SAVE_AREA FloatSave;
			// Retrieved by CONTEXT_SEGMENTS 
			public uint SegGs;
			public uint SegFs;
			public uint SegEs;
			public uint SegDs;
			// Retrieved by CONTEXT_INTEGER 
			public uint Edi;
			public uint Esi;
			public uint Ebx;
			public uint Edx;
			public uint Ecx;
			public uint Eax;
			// Retrieved by CONTEXT_CONTROL 
			public uint Ebp;
			public uint Eip;
			public uint SegCs;
			public uint EFlags;
			public uint Esp;
			public uint SegSs;
			// Retrieved by CONTEXT_EXTENDED_REGISTERS 
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
			public byte[] ExtendedRegisters;
		}

		public struct PROCESS_INFORMATION
		{
			public uint hProcess;
			public uint hThread;
			public uint dwProcessId;
			public uint dwThreadId;
		}

		public struct STARTUPINFO
		{
			public uint cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public uint dwX;
			public uint dwY;
			public uint dwXSize;
			public uint dwYSize;
			public uint dwXCountChars;
			public uint dwYCountChars;
			public uint dwFillAttribute;
			public uint dwFlags;
			public short wShowWindow;
			public short cbReserved2;
			public uint lpReserved2;
			public uint hStdInput;
			public uint hStdOutput;
			public uint hStdError;
		}

		public struct SECURITY_ATTRIBUTES
		{
			public int length;
			public uint lpSecurityDescriptor;
			public bool bInheritHandle;
		}



		#endregion

		#region Imports


		[DllImport("kernel32.dll")]
		private static extern bool DebugActiveProcess(uint processId);

		[DllImport("kernel32.dll")]
		private static extern uint OpenThread(uint dwDesiredAccess, bool bInheritHandle,
			uint dwThreadId);

		[DllImport("kernel32.dll")]
		private static extern uint GetLastError();

		[DllImport("kernel32.dll")]
		private static extern unsafe uint LoadLibraryA(void* pModule);

		[DllImport("kernel32.dll")]
		private static extern unsafe uint GetProcAddress(uint pHandle, void* pFunction);

		[DllImport("kernel32.dll")]
		private static extern uint CreateRemoteThread(uint hProcess,
													  uint lpThreadAttributes,
													  uint dwStackSize,
													  uint lpStartAddress,
													  uint lpParameter,
													  uint dwCreationFlags,
													  uint lpThreadId);


        [DllImport("kernel32.dll", EntryPoint = "VirtualAllocExNuma")]
		private static extern uint vAllocExNuma(uint hProcess,
												  uint lpAddress,
												  uint dwSize,
												  uint flAllocationType,
												  uint flProtect,
                                                  uint numa);

        [DllImport("kernel32.dll")]
        private unsafe static extern uint WriteProcessMemory(uint hProcess, uint address, void* buffer, uint size, ref uint bytesWritten);

		[DllImport("kernel32.dll")]
		private unsafe static extern uint ReadProcessMemory(uint hProcess, uint address, ref byte buffer, uint size, ref uint bytesRead);

		[DllImport("kernel32.dll")]
		private unsafe static extern uint ReadProcessMemory(uint hProcess, uint address, void* buffer, uint size, ref uint bytesRead);

		[DllImport("kernel32.dll")]
		private unsafe static extern uint ReadProcessMemory(uint hProcess, uint address, ref uint buffer, uint size, ref uint bytesRead);

		[DllImport("kernel32.dll")]
		private static extern uint OpenProcess(uint dwDesiredAccess, bool bInheritHandle,
			uint dwThreadId);


		[DllImport("kernel32.dll")]
		private static extern bool Thread32First(uint hSnapshot, ref THREADENTRY32 lpte);


		[DllImport("kernel32.dll")]
		private static extern bool Thread32Next(uint hSnapshot, out THREADENTRY32 lpte);


		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern uint CreateToolhelp32Snapshot(int dwFlags, uint th32ProcessID);


		[DllImport("kernel32.dll")]
		private static extern uint SuspendThread(uint hThread);


		[DllImport("kernel32.dll")]
		private static extern bool GetThreadContext(uint hThread, ref ThreadContext lpContext);


		[DllImport("kernel32.dll")]
		private static extern bool SetThreadContext(uint hThread,
			[In] ref ThreadContext lpContext);


		[DllImport("kernel32.dll")]
		private static extern uint ResumeThread(uint hThread);


		[DllImport("kernel32.dll")]
		private static extern bool DebugSetProcessKillOnExit(uint dwProcessId);


		[DllImport("kernel32.dll", EntryPoint = "WaitForDebugEvent")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool WaitForDebugEvent(out DEBUG_EVENT lpDebugEvent, uint dwMilliseconds);


		[DllImport("kernel32.dll")]
		private static extern bool ContinueDebugEvent(uint dwProcessId, uint dwThreadId,
			uint dwContinueStatus);


		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(uint hObject);

		[DllImport("kernel32.dll")]
		static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, uint lpProcessAttributes, uint lpThreadAttributes,
								bool bInheritHandles, uint dwCreationFlags, uint lpEnvironment,
								string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);
	}
	#endregion

}

