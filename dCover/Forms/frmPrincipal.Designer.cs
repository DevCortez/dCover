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
			this.tabProject = new System.Windows.Forms.TabPage();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.pnlProjectInformation = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.runSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.tcMainMenu.SuspendLayout();
			this.tabProject.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.projectToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(427, 24);
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
			this.tcMainMenu.Controls.Add(this.tabProject);
			this.tcMainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcMainMenu.Location = new System.Drawing.Point(0, 24);
			this.tcMainMenu.Name = "tcMainMenu";
			this.tcMainMenu.SelectedIndex = 0;
			this.tcMainMenu.Size = new System.Drawing.Size(427, 341);
			this.tcMainMenu.TabIndex = 1;
			// 
			// tabProject
			// 
			this.tabProject.Controls.Add(this.splitter1);
			this.tabProject.Controls.Add(this.pnlProjectInformation);
			this.tabProject.Location = new System.Drawing.Point(4, 22);
			this.tabProject.Name = "tabProject";
			this.tabProject.Size = new System.Drawing.Size(419, 315);
			this.tabProject.TabIndex = 0;
			this.tabProject.Text = "Project";
			this.tabProject.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 343);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(427, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// pnlProjectInformation
			// 
			this.pnlProjectInformation.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlProjectInformation.Location = new System.Drawing.Point(180, 0);
			this.pnlProjectInformation.Name = "pnlProjectInformation";
			this.pnlProjectInformation.Size = new System.Drawing.Size(239, 315);
			this.pnlProjectInformation.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(177, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 315);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// projectToolStripMenuItem
			// 
			this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runSelectedToolStripMenuItem});
			this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
			this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.projectToolStripMenuItem.Text = "Project";
			// 
			// runSelectedToolStripMenuItem
			// 
			this.runSelectedToolStripMenuItem.Name = "runSelectedToolStripMenuItem";
			this.runSelectedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.runSelectedToolStripMenuItem.Text = "Run selected";
			this.runSelectedToolStripMenuItem.Click += new System.EventHandler(this.runSelectedToolStripMenuItem_Click);
			// 
			// frmPrincipal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(427, 365);
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
			this.tabProject.ResumeLayout(false);
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
		private System.Windows.Forms.TabPage tabProject;
		private System.Windows.Forms.Panel pnlProjectInformation;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem runSelectedToolStripMenuItem;
	}
}

