using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace dCover.Geral
{
	public class Project
	{		
		public List<CoveragePoint> coveragePointList = new List<CoveragePoint>();
		public List<SourceFolder> sourceFolders = new List<SourceFolder>();
		public List<ProjectModule> moduleFiles = new List<ProjectModule>();
		public List<uint> runningProcesses = new List<uint>();

		public void SaveToFile(string fileName)
		{
			XDocument fileBuffer = new XDocument();
			fileBuffer.Add(new XElement("coverage"));

			#region Saving source folders
			XElement sourceFoldersNode = new XElement("sourceFolders");
			fileBuffer.Element("coverage").Add(sourceFoldersNode);

			foreach (SourceFolder x in sourceFolders)
			{
				XElement sourceNode = new XElement("source");
				sourceNode.SetAttributeValue("module", x.moduleName);
				sourceNode.SetAttributeValue("path", x.path);

				sourceFoldersNode.Add(sourceNode);
			}			
			#endregion

			#region Saving module paths
			XElement moduleFilesNode = new XElement("moduleFiles");
			fileBuffer.Element("coverage").Add(moduleFilesNode);

			foreach (ProjectModule x in moduleFiles)
			{
				XElement moduleNode = new XElement("module");
				moduleNode.SetAttributeValue("file", x.moduleFile);
				moduleNode.SetAttributeValue("hash", x.hash);
				moduleNode.SetAttributeValue("active", x.isActive);
				moduleNode.SetAttributeValue("host", x.host);
				moduleNode.SetAttributeValue("param", x.parameters);
				moduleNode.SetAttributeValue("service", x.isService);			
				
				moduleFilesNode.Add(moduleNode);
			}
			#endregion

			#region Saving coverage points
			XElement coveragePointsNode = new XElement("coveragePoints");
			fileBuffer.Element("coverage").Add(coveragePointsNode);
			
			foreach(CoveragePoint x in coveragePointList)
			{
				XElement coveragePointNode = new XElement("p");
				coveragePointNode.SetAttributeValue("m", x.moduleName);
				coveragePointNode.SetAttributeValue("l", x.lineNumber);
				coveragePointNode.SetAttributeValue("o", x.offset);
				coveragePointNode.SetAttributeValue("n", x.routineName);
				coveragePointNode.SetAttributeValue("s", x.sourceFile);
				coveragePointNode.SetAttributeValue("c", x.wasCovered);

				coveragePointsNode.Add(coveragePointNode);
			}
			#endregion

			fileBuffer.Save(fileName);
		}

		public void LoadFromFile(string fileName)
		{
			XDocument fileBuffer = XDocument.Load(fileName);

			#region Loading sources
			foreach(XElement x in fileBuffer.Descendants("source"))
			{
				sourceFolders.Add(new SourceFolder(x.Attribute("module").Value, x.Attribute("path").Value));
			}
			#endregion

			#region Loading module list
			foreach(XElement x in fileBuffer.Descendants("module"))
			{
				ProjectModule projectModule = new ProjectModule();
				projectModule.moduleFile = x.Attribute("file").Value;
				projectModule.hash = x.Attribute("hash").Value;
				projectModule.host = x.Attribute("host").Value;
				projectModule.isActive = Convert.ToBoolean(x.Attribute("active").Value);
				projectModule.isService = Convert.ToBoolean(x.Attribute("service").Value);
				projectModule.parameters = x.Attribute("param").Value;

				moduleFiles.Add(projectModule);
			}
			#endregion

			#region Loading coverage points
			foreach(XElement x in fileBuffer.Descendants("p"))
			{
				CoveragePoint coveragePoint = new CoveragePoint();
				coveragePoint.wasCovered = Convert.ToBoolean(x.Attribute("c").Value);
				coveragePoint.sourceFile = x.Attribute("s").Value;
				coveragePoint.routineName = x.Attribute("n").Value;
				coveragePoint.offset = Convert.ToInt32(x.Attribute("o").Value);
				coveragePoint.lineNumber = Convert.ToInt32(x.Attribute("l").Value);
				coveragePoint.moduleName = x.Attribute("m").Value;

				coveragePointList.Add(coveragePoint);
			}
			#endregion
		}

		private void processMonitor()
		{
			while(true) //Ugly but necessary
			{
				foreach(uint x in runningProcesses)
				{
					uint exitCode = 0;
					GetExitCodeProcess(x, ref exitCode);

					if(exitCode != 259)
					{
						runningProcesses.Remove(x);
						Console.WriteLine("Process " + x + " exited with " + exitCode);
					}
				}

				Thread.Sleep(100); //Prevent overhead from this thread to improve performance
			}
		}

		[DllImport("kernel32.dll")]
		private static extern Boolean GetExitCodeProcess(uint hProcess, ref uint exitCode);
	}

	public class SourceFolder
	{
		public string moduleName;
		public string path;

		public SourceFolder(string _moduleName, string _path)
		{
			moduleName = _moduleName;
			path = _path;
		}
	}

	public class ProjectModule
	{
		public string moduleFile;
		public string hash = ""; //Remove initialization after complete implementation
		public string host = "";
		public string parameters = "";		
		public bool   isActive = true;
		public bool   isService = false;

		public IEnumerable<string> selectedSourceFiles = new List<string>(); //Must be implemented
	}

}
