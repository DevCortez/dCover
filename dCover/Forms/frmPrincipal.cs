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
		
		private void updateInterfaceInformation()
		{
			clbProject.Items.Clear();

			foreach(string x in project.moduleFiles.Select(x => x.moduleFile).Distinct())
			{
				clbProject.Items.Add(Path.GetFileName(x));
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
			updateInterfaceInformation();
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
			updateInterfaceInformation();
		}

		private void clearWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			project = new Project();
			updateInterfaceInformation();
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
	}
}
