using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using dCover.Geral;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace dCover.Forms
{
	public partial class frmPrincipal : Form
	{
		private Project project = new Project();
		private Thread processMonitor;

		private void processMonitorLoop()
		{
			while(true)
			{
				if(processMonitorToolStripMenuItem.Checked)
				{
					List<string> modulesList = (from x in project.moduleFiles select Path.GetFileName(x.moduleFile).ToLower()).ToList();	
					List<int> pidList = project.runningProcesses.Select(x => x.Id).ToList();				
					List<Process> processList = Process.GetProcesses().ToList();

					foreach(Process currentProcess in processList)
					{
						try
						{							
							//If already debugging this guy
							if(pidList.Contains(currentProcess.Id))
								continue;
							
							if(modulesList.Contains(currentProcess.MainModule.ModuleName.ToLower()) && !currentProcess.HasExited)
							{
								//Main module itself should be covered
								ProjectModule projectModule = project.moduleFiles.Where(x => x.moduleFile.Contains(currentProcess.MainModule.ModuleName)).First();
								new ProjectProcess().AttachToProcess(currentProcess, projectModule, project);
							}

							foreach(ProcessModule module in currentProcess.Modules)
							{
								if(modulesList.Contains(module.ModuleName.ToLower()))
								{
									//Attach to process and debug this module
								}
							}
						}
						catch
						{
							continue; //Expect exceptions due to access
						}
					}
				}
				
				Thread.Sleep(1);
			}
		}
		
		private void updateProjectOverview()
		{
			clbProject.Items.Clear();

			foreach(string x in project.moduleFiles.Select(x => x.moduleFile).Distinct())
			{
				clbProject.Items.Add(Path.GetFileName(x), project.moduleFiles.Where(y => y.moduleFile == x).First().isActive);
			}
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

				lblTotalPoints.Text  = project.coveragePointList.Where(
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
				#endregion

				pnlProjectInformation.Show();
			}
			else
			{
				pnlProjectInformation.Hide();
			}
		}
		
		public frmPrincipal()
		{
			InitializeComponent();
			processMonitor = new Thread(new ThreadStart(processMonitorLoop));  
			processMonitor.Start();    
			
			#region Project routine treeview
			
			#endregion  
		}

		private unsafe void frmPrincipal_Load(object sender, EventArgs e)
		{
			foreach (Match param in Regex.Matches(Environment.CommandLine, @"\s+[\-\/\\](.)\:?((?(?=\"")(\""[^""]*)|([^\s]*)))"))
            {
                #region Command line
				switch(param.Groups[1].Value)
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
			
			if(saveDialog.ShowDialog() != DialogResult.OK)
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

			if(loadDialog.ShowDialog() != DialogResult.OK)
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
			foreach(var x in clbProject.CheckedItems)
				new ProjectProcess().CreateProcess(project.moduleFiles.Where(y => y.moduleFile.Contains(x.ToString())).First() , project);
		}

		private void terminateAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach(Process x in project.runningProcesses)
				try
				{
					x.Kill();
				}
				catch
				{
					Console.WriteLine("Tried to kill an unknown process");
				}
		}

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
			selectedModule.moduleFile =  txtApplication.Text;
			selectedModule.isHosted = chkHost.Checked;
			selectedModule.host = txtHost.Text;
			selectedModule.parameters = txtParams.Text;
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
			
			if(findApplication.ShowDialog() != DialogResult.OK)
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
	}
}
