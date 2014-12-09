using Aga.Controls.Tree;
using dCover.Geral;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace dCover.Forms
{
	public partial class frmPrincipal : Form
	{
		#region Classes
		private class BaseNode : Node
		{
			public ProjectModule module;
		}

		private class ModuleNode : BaseNode
		{
		}

		private class UnitNode : BaseNode
		{
			public string sourceFile;
		}

		private class RoutineNode : BaseNode
		{
			public string sourceFile;
		}
		#endregion

		#region Variables
		private Project project = new Project();
		private Thread processMonitor;
		private bool iconFlag = false;
		#endregion

		public frmPrincipal()
		{
			InitializeComponent();
			processMonitor = new Thread(new ThreadStart(processMonitorLoop));
			processMonitor.Start();

			tvaRoutines.Model = new TreeModel();
		}

		private void processMonitorLoop()
		{
			while (true)
			{
				if (processMonitorToolStripMenuItem.Checked)
				{
					List<string> modulesList = (from x in project.moduleFiles select Path.GetFileName(x.moduleFile).ToLower()).ToList();
					List<int> pidList = project.runningProcesses.Select(x => x.Id).ToList();
					List<Process> processList = Process.GetProcesses().ToList();

					foreach (Process currentProcess in processList)
					{
						try
						{
							if (pidList.Contains(currentProcess.Id))
								continue;

							if (modulesList.Contains(currentProcess.MainModule.ModuleName.ToLower()) && !currentProcess.HasExited)
							{
								//Main module itself should be covered
								ProjectModule projectModule = project.moduleFiles.Where(x => x.moduleFile.Contains(currentProcess.MainModule.ModuleName)).First();

								if (projectModule.isActive)
									new ProjectProcess().AttachToProcess(currentProcess, projectModule, project);
							}

							foreach (ProcessModule module in currentProcess.Modules)
							{
								if (modulesList.Contains(module.ModuleName.ToLower()))
								{
									//Attach to process and debug this module
								}
							}
						}
						catch
						{
							continue; //Expect exceptions due to access &| bitness
						}
					}
				}

				Thread.Sleep(0);
			}
		}

		#region Interface
		private void updateProjectOverview()
		{
			clbProject.Items.Clear();
			(tvaRoutines.Model as TreeModel).Nodes.Clear();

			bool doFiltering = txtFindRoutines.Text.Length > 0;
			List<string> searchPattern = txtFindRoutines.Text.ToLower().Split(' ').ToList();

			foreach (ProjectModule x in project.moduleFiles)
			{
				string moduleName = Path.GetFileName(x.moduleFile);
				clbProject.Items.Add(moduleName, x.isActive);

				bool addThisRoutine;
				bool addThisUnit;
				bool addThisModule;

				if (doFiltering)
					addThisModule = false;
				else
					addThisModule = true;

				ModuleNode moduleNode = new ModuleNode();
				moduleNode.Text = moduleName;
				moduleNode.module = x;
				moduleNode.CheckState = x.isActive ? CheckState.Checked : CheckState.Unchecked;

				foreach (string sourceFile in project.coveragePointList.Where(y => y.moduleName == moduleName).Select(y => y.sourceFile).Distinct())
				{
					string sourceFileName = Path.GetFileName(sourceFile);
					string sourceFilePath = FileHelper.recursiveFileSearch(sourceFileName, project.sourceFolders.Where(y => y.moduleName == moduleName).Select(y => y.path).First(), 3);

					if (doFiltering)
						addThisUnit = false;
					else
						addThisUnit = true;

					UnitNode unitNode = new UnitNode();
					unitNode.Text = sourceFileName;
					unitNode.Tag = unitNode;
					unitNode.sourceFile = sourceFilePath;
					unitNode.CheckState = x.selectedSourceFiles.Contains(sourceFileName) ? CheckState.Checked : CheckState.Unchecked;
					unitNode.module = x;


					foreach (string routine in project.coveragePointList.Where(y => y.sourceFile.Contains(sourceFileName)).Select(y => y.routineName).Distinct())
					{
						addThisRoutine = false;

						if (doFiltering)
						{
							if (searchPattern.Where(y => routine.ToLower().Contains(y)).Count() > 0)
							{
								addThisRoutine = true;
								addThisUnit = true;
								addThisModule = true;
							}
						}
						else
							addThisRoutine = true;

						if (addThisRoutine)
						{
							RoutineNode routineNode = new RoutineNode();
							routineNode.Text = routine;
							routineNode.Tag = routineNode;
							routineNode.sourceFile = sourceFilePath;
							routineNode.CheckState = x.selectedRoutines.Contains(routine) ? CheckState.Checked : CheckState.Unchecked;
							routineNode.module = x;
							unitNode.Nodes.Add(routineNode);
						}
					}

					if (addThisUnit)
						moduleNode.Nodes.Add(unitNode);
				}

				if (addThisModule)
					(tvaRoutines.Model as TreeModel).Nodes.Add(moduleNode);
			}

			if (doFiltering)
				tvaRoutines.ExpandAll();
		}

		private void updateProjectInformation()
		{
			if (clbProject.SelectedItems.Count == 1)
			{
				ProjectModule selectedModule = project.moduleFiles.Where(x => x.moduleFile.Contains(clbProject.SelectedItem.ToString())).First();

				#region Update information labels
				lblRoutineCount.Text = project.coveragePointList.Where(
									   x => selectedModule.moduleFile.Contains(x.moduleName)
									   ).Select(
									   x => x.routineName
									   ).Distinct().Count().ToString() + " routines";

				lblUnitCount.Text = project.coveragePointList.Where(
									   x => selectedModule.moduleFile.Contains(x.moduleName)
									   ).Select(
									   x => x.sourceFile
									   ).Distinct().Count().ToString() + " units in project";

				lblTotalPoints.Text = project.coveragePointList.Where(
									   x => selectedModule.moduleFile.Contains(x.moduleName)
									   ).Count().ToString() + " coverage points";
				#endregion

				#region Update project variables in the panel
				txtApplication.Text = selectedModule.moduleFile;

				txtParams.Text = selectedModule.parameters;

				chkHost.Checked = selectedModule.isHosted;
				txtHost.Text = selectedModule.host;
				txtHost.Enabled = selectedModule.isHosted;
				btnHost.Enabled = selectedModule.isHosted;

				txtDirectory.Text = selectedModule.startDirectory;
				#endregion

				pnlProjectInformation.Show();
			}
			else
			{
				pnlProjectInformation.Hide();
			}
		}

		private void validateSelectedPoints()
		{
			foreach (var x in tvaRoutines.AllNodes)
			{
				BaseNode buffer = x.Tag as BaseNode;

				if (buffer.module.selectedSourceFiles.Contains(buffer.Text) && !(buffer.CheckState == CheckState.Checked))
					buffer.module.selectedSourceFiles = buffer.module.selectedSourceFiles.Where(y => y != buffer.Text).ToList();
				else if (!buffer.module.selectedSourceFiles.Contains(buffer.Text) && (buffer.CheckState == CheckState.Checked))
					buffer.module.selectedSourceFiles.Add(buffer.Text);

				if (buffer.module.selectedRoutines.Contains(buffer.Text) && !(buffer.CheckState == CheckState.Checked))
					buffer.module.selectedRoutines = buffer.module.selectedRoutines.Where(y => y != buffer.Text).ToList();
				else if (!buffer.module.selectedRoutines.Contains(buffer.Text) && (buffer.CheckState == CheckState.Checked))
					buffer.module.selectedRoutines.Add(buffer.Text);
			}
		}

		private void enumerateLines(ref string[] input)
		{
			string enumPadding = "";
			enumPadding = enumPadding.PadLeft(input.Count().ToString().Length, '0');

			for(var x = 0; x < input.Count(); x++)
				input[x] = x.ToString(enumPadding) + (char)9 + input[x];
		}

		private void updateSourceCodeSnippets()
		{
			txtCodeSnippet.SuspendLayout();
			txtCodeSnippet.Hide();

			foreach (var x in tvaRoutines.SelectedNodes)
			{
				if (x.Tag is UnitNode)
				{
					RichTextBox contentHolder = new RichTextBox();
					contentHolder.WordWrap = false;

					var linesBuffer = File.ReadAllLines((x.Tag as UnitNode).sourceFile);
										
					enumerateLines(ref linesBuffer);					
					contentHolder.Lines = linesBuffer;

					contentHolder.SelectAll();
					contentHolder.SelectionFont = new Font("Verdana", 10);

					foreach (CoveragePoint y in project.coveragePointList.Where(y => (x.Tag as UnitNode).sourceFile.ToLower().Contains(y.sourceFile.ToLower())))
					{
						contentHolder.Select(contentHolder.GetFirstCharIndexFromLine(y.lineNumber - 1), contentHolder.Lines[y.lineNumber - 1].Length);

						if (y.wasCovered)
							contentHolder.SelectionColor = Color.Green;
						else
							contentHolder.SelectionColor = Color.Red;
					}

					txtCodeSnippet.Select(txtCodeSnippet.Text.Length, 0);
					txtCodeSnippet.SelectedRtf = contentHolder.Rtf;

					contentHolder.Dispose();
				}
				else if (x.Tag is RoutineNode)
				{
					RichTextBox contentHolder = new RichTextBox();
					contentHolder.WordWrap = false;

					var linesBuffer = File.ReadAllLines((x.Tag as RoutineNode).sourceFile);

					enumerateLines(ref linesBuffer);
					contentHolder.Lines = linesBuffer;

					contentHolder.SelectAll();
					contentHolder.SelectionFont = new Font("Verdana", 10);
					List<int> relevantLines = new List<int>();

					txtCodeSnippet.AppendText("{" + (x.Tag as RoutineNode).Text + "}" + System.Environment.NewLine);
					txtCodeSnippet.Select(txtCodeSnippet.GetFirstCharIndexFromLine(txtCodeSnippet.Lines.Count() - 2), txtCodeSnippet.GetFirstCharIndexFromLine(txtCodeSnippet.Lines.Count() - 1));
					txtCodeSnippet.SelectionColor = Color.BlueViolet;

					foreach (CoveragePoint y in project.coveragePointList.Where(y => (x.Tag as RoutineNode).Text == y.routineName))
					{
						for (int i = -3; i < 2; i++)
							if (y.lineNumber + i + 1 < contentHolder.Lines.Count())
								relevantLines.Add(y.lineNumber + i);

						contentHolder.SelectAll();
						contentHolder.SelectionColor = Color.Black;

						contentHolder.Select(contentHolder.GetFirstCharIndexFromLine(y.lineNumber - 1), contentHolder.Lines[y.lineNumber - 1].Length);

						if (y.wasCovered)
							contentHolder.SelectionColor = Color.Green;
						else
							contentHolder.SelectionColor = Color.Red;

						relevantLines = relevantLines.OrderBy(z => z).ToList();

						contentHolder.Select(contentHolder.GetFirstCharIndexFromLine(relevantLines.First()), contentHolder.GetFirstCharIndexFromLine(relevantLines.Last() + 1) - contentHolder.GetFirstCharIndexFromLine(relevantLines.First()));


						txtCodeSnippet.Select(txtCodeSnippet.Text.Length, 0);
						txtCodeSnippet.SelectedRtf = contentHolder.SelectedRtf;
						txtCodeSnippet.AppendText(System.Environment.NewLine);

						relevantLines.Clear();
					}

					contentHolder.Dispose();
				}
				else if (x.Tag is ModuleNode)
				{

				}
			}

			txtCodeSnippet.ResumeLayout();
			txtCodeSnippet.Show();
		}
		#endregion

		#region Form events
		private unsafe void frmPrincipal_Load(object sender, EventArgs e)
		{
			foreach (Match param in Regex.Matches(Environment.CommandLine, @"\s+[\-\/\\](.)\:?((?(?=\"")(\""[^""]*)|([^\s]*)))"))
			{
				#region Command line
				switch (param.Groups[1].Value)
				{
					case "w":
						{
							try
							{
								string projectPath = param.Groups[2].Value.Remove(0, 1);
								project.LoadFromFile(projectPath);
								updateProjectOverview();
							}
							catch
							{
								Console.WriteLine("Cannot load project " + param.Groups[2].Value);
							}
							break;
						}

					case "s":
						{
							break;
						}

					case "m":
						{
							break;
						}

					case "c":
						{
							break;
						}
				}
				#endregion
			}
		}

		private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}
		#endregion

		#region ToolStripMenu
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ProjectLoader.LoadNewDelphiProject(project);
			updateProjectOverview();
		}

		private void saveWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveDialog = new SaveFileDialog();
			saveDialog.Title = "Save workspace to...";
			saveDialog.Filter = "dCover workspace|*.dcw";
			saveDialog.CheckPathExists = true;
			saveDialog.DefaultExt = ".dcw";

			if (saveDialog.ShowDialog() != DialogResult.OK)
				return;

			project.SaveToFile(saveDialog.FileName);
		}

		private void loadWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			project = new Project();

			OpenFileDialog loadDialog = new OpenFileDialog();
			loadDialog.Title = "Load workspace";
			loadDialog.Filter = "dCover workspace|*.dcw";
			loadDialog.CheckFileExists = true;

			if (loadDialog.ShowDialog() != DialogResult.OK)
				return;

			project.LoadFromFile(loadDialog.FileName);
			updateProjectOverview();
			updateProjectInformation();
		}

		private void clearWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			project = new Project();
			updateProjectOverview();
		}

		private void runSelectedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (var x in clbProject.CheckedItems)
				new ProjectProcess().CreateProcess(project.moduleFiles.Where(y => y.moduleFile.Contains(x.ToString())).First(), project);
		}

		private void terminateAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (Process x in project.runningProcesses)
				try
				{
					x.Kill();
				}
				catch
				{
					Console.WriteLine("Tried to kill an unknown process");
				}
		}

		private void runApplicationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Console.WriteLine(sender.ToString());
		}
		#endregion

		#region tvaRoutines
		private void tvaRoutines_SelectionChanged(object sender, EventArgs e)
		{
			txtCodeSnippet.Clear();

			updateSourceCodeSnippets();
		}

		private void tvaRoutines_Click(object sender, EventArgs e)
		{
			validateSelectedPoints();
		}

		private void tvaRoutines_KeyDown(object sender, KeyEventArgs e)
		{
			validateSelectedPoints();
		}
		#endregion

		#region txtFindRoutines
		private void txtFindRoutines_KeyPress(object sender, KeyPressEventArgs e)
		{
			updateProjectOverview();
		}

		private void txtFindRoutines_Enter(object sender, EventArgs e)
		{
			txtFindRoutines.SetBounds(txtFindRoutines.Location.X, txtFindRoutines.Location.Y, 200, 20);
		}

		private void txtFindRoutines_Leave(object sender, EventArgs e)
		{
			if (txtFindRoutines.Text.Length == 0)
				txtFindRoutines.SetBounds(txtFindRoutines.Location.X, txtFindRoutines.Location.Y, 20, 20);
		}
		#endregion

		private void clbProject_SelectedValueChanged(object sender, EventArgs e)
		{
			updateProjectInformation();
		}

		private void chkHost_CheckedChanged(object sender, EventArgs e)
		{
			txtHost.Enabled = chkHost.Checked;
			btnHost.Enabled = chkHost.Checked;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			ProjectModule selectedModule = project.moduleFiles.Where(x => x.moduleFile.Contains(clbProject.SelectedItem.ToString())).First();
			selectedModule.moduleFile = txtApplication.Text;
			selectedModule.isHosted = chkHost.Checked;
			selectedModule.host = txtHost.Text;
			selectedModule.parameters = txtParams.Text;
			selectedModule.startDirectory = txtDirectory.Text;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			updateProjectInformation();
		}

		private void btnApplication_Click(object sender, EventArgs e)
		{
			OpenFileDialog findApplication = new OpenFileDialog();
			ProjectModule selectedModule = project.moduleFiles.Where(x => x.moduleFile.Contains(clbProject.SelectedItem.ToString())).First();

			findApplication.Title = "Find application...";
			findApplication.Filter = Path.GetFileName(selectedModule.moduleFile) + "|" + Path.GetFileName(selectedModule.moduleFile);
			findApplication.CheckFileExists = true;

			if (findApplication.ShowDialog() != DialogResult.OK)
				return;

			txtApplication.Text = findApplication.FileName;
		}

		private void btnHost_Click(object sender, EventArgs e)
		{
			OpenFileDialog findHost = new OpenFileDialog();
			ProjectModule selectedModule = project.moduleFiles.Where(x => x.moduleFile.Contains(clbProject.SelectedItem.ToString())).First();

			findHost.Title = "Find application...";
			findHost.Filter = "Executable|*.exe";
			findHost.CheckFileExists = true;

			if (findHost.ShowDialog() != DialogResult.OK)
				return;

			txtHost.Text = findHost.FileName;
		}

		private void iconAnimator_Tick(object sender, EventArgs e)
		{
			if(iconFlag)
				Icon = dCover.Properties.Resources.Red;
			else
				Icon = dCover.Properties.Resources.Green;

			iconFlag = !iconFlag;
		}
	}
}
