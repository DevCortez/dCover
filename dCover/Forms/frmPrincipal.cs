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

namespace dCover.Forms
{
	public partial class frmPrincipal : Form
	{
		private Project project = new Project();
		
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
				lblModulePath.Text = selectedModule.moduleFile;
				
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
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ProjectLoader.LoadNewDelphiProject(project);
			
			ProjectProcess proc = new ProjectProcess();
			proc.CreateProcess(project.moduleFiles.First(), project);

            project.SaveToFile(@"c:\projeto_bacon.xml");
		}

		private void frmPrincipal_Load(object sender, EventArgs e)
		{
			project.LoadFromFile(@"c:\projeto.xml");
		}

        private void button2_Click(object sender, EventArgs e)
        {
            project.SaveToFile(@"c:\projeto_bacon.xml");
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
			OpenFileDialog loadDialog = new OpenFileDialog();
			loadDialog.Title = "Load workspace";
			loadDialog.Filter = "dCover workspace|*.dcw";
			loadDialog.CheckFileExists = true;

			if(loadDialog.ShowDialog() != DialogResult.OK)
				return;

			project.LoadFromFile(loadDialog.FileName);
			updateProjectOverview();
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
			foreach(Process x in project.runningProcesses.Where(x => !x.HasExited))
				x.Kill();
		}

		private void clbProject_SelectedValueChanged(object sender, EventArgs e)
		{
			updateProjectInformation();
		}
	}
}
