using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dCover.Geral;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace dCover.Forms
{
	public partial class frmPrincipal : Form
	{
		private TreeViewAdv projectTreeView = new TreeViewAdv();
		private TreeModel projectTreeModel = new TreeModel();
		
		private Project project = new Project();
		
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

		private void updateProjectInformation()
		{
			(projectTreeView.Model as TreeModel).Nodes.Clear();
			
			foreach(ProjectModule x in project.moduleFiles)
			{
				Node currentModuleNode = new Node();
				currentModuleNode.Text = x.moduleFile;

				foreach(string y in project.coveragePointList.Where(y => x.moduleFile.Contains(y.moduleName)).Select(y => y.sourceFile).Distinct())
				{
					Node currentSourceFileNode = new Node();
					currentSourceFileNode.Text = y;
					currentModuleNode.Nodes.Add(currentSourceFileNode);
				}

				projectTreeModel.Nodes.Add(currentModuleNode);
			}
		}

		private void frmPrincipal_Load(object sender, EventArgs e)
		{
			tProject.Controls.Add(projectTreeView);
			projectTreeView.SelectionMode = TreeSelectionMode.Multi;
			projectTreeView.FullRowSelect = true;
			projectTreeView.Dock = DockStyle.Fill;
			projectTreeView.Model = projectTreeModel;
			

			NodeCheckBox nodeCheckBox = new NodeCheckBox();
			nodeCheckBox.DataPropertyName = "IsChecked";
			nodeCheckBox.EditEnabled = true;
			projectTreeView.NodeControls.Add(nodeCheckBox);

			NodeTextBox nodeText = new NodeTextBox();
			nodeText.DataPropertyName = "Text";
			projectTreeView.NodeControls.Add(nodeText);
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

			updateProjectInformation();
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

			updateProjectInformation();
		}

		private void clearWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			project = new Project();

			updateProjectInformation();
		}
	}
}
