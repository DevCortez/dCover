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

namespace dCover.Forms
{
	public partial class frmPrincipal : Form
	{
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
	}
}
