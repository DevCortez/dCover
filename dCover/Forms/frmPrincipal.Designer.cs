namespace dCover.Forms
{
	partial class frmPrincipal
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tcMainMenu = new System.Windows.Forms.TabControl();
			this.tProject = new System.Windows.Forms.TabPage();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.pnlProject = new System.Windows.Forms.Panel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.menuStrip1.SuspendLayout();
			this.tcMainMenu.SuspendLayout();
			this.tProject.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(558, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveWorkspaceToolStripMenuItem,
            this.loadWorkspaceToolStripMenuItem,
            this.clearWorkspaceToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.openToolStripMenuItem.Text = "Load delphi project";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveWorkspaceToolStripMenuItem
			// 
			this.saveWorkspaceToolStripMenuItem.Name = "saveWorkspaceToolStripMenuItem";
			this.saveWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.saveWorkspaceToolStripMenuItem.Text = "Save workspace";
			this.saveWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.saveWorkspaceToolStripMenuItem_Click);
			// 
			// loadWorkspaceToolStripMenuItem
			// 
			this.loadWorkspaceToolStripMenuItem.Name = "loadWorkspaceToolStripMenuItem";
			this.loadWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.loadWorkspaceToolStripMenuItem.Text = "Load workspace";
			this.loadWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.loadWorkspaceToolStripMenuItem_Click);
			// 
			// clearWorkspaceToolStripMenuItem
			// 
			this.clearWorkspaceToolStripMenuItem.Name = "clearWorkspaceToolStripMenuItem";
			this.clearWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.clearWorkspaceToolStripMenuItem.Text = "Clear workspace";
			this.clearWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.clearWorkspaceToolStripMenuItem_Click);
			// 
			// tcMainMenu
			// 
			this.tcMainMenu.Controls.Add(this.tProject);
			this.tcMainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcMainMenu.Location = new System.Drawing.Point(0, 24);
			this.tcMainMenu.Name = "tcMainMenu";
			this.tcMainMenu.SelectedIndex = 0;
			this.tcMainMenu.Size = new System.Drawing.Size(558, 388);
			this.tcMainMenu.TabIndex = 1;
			// 
			// tProject
			// 
			this.tProject.Controls.Add(this.splitter1);
			this.tProject.Controls.Add(this.pnlProject);
			this.tProject.Location = new System.Drawing.Point(4, 22);
			this.tProject.Name = "tProject";
			this.tProject.Size = new System.Drawing.Size(550, 362);
			this.tProject.TabIndex = 0;
			this.tProject.Text = "Project";
			this.tProject.UseVisualStyleBackColor = true;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(336, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 362);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// pnlProject
			// 
			this.pnlProject.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlProject.Location = new System.Drawing.Point(339, 0);
			this.pnlProject.Name = "pnlProject";
			this.pnlProject.Size = new System.Drawing.Size(211, 362);
			this.pnlProject.TabIndex = 0;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 390);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(558, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// frmPrincipal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(558, 412);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.tcMainMenu);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "frmPrincipal";
			this.Text = "D-Cover";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosing);
			this.Load += new System.EventHandler(this.frmPrincipal_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tcMainMenu.ResumeLayout(false);
			this.tProject.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveWorkspaceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadWorkspaceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearWorkspaceToolStripMenuItem;
		private System.Windows.Forms.TabControl tcMainMenu;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TabPage tProject;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel pnlProject;
	}
}

