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
			project.SaveToFile(@"d:\projeto.xml");
		}

		private void frmPrincipal_Load(object sender, EventArgs e)
		{
			project.LoadFromFile(@"d:\projeto.xml");
		}
	}
}
