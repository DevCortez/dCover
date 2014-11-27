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
			this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.runSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.terminateAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tcMainMenu = new System.Windows.Forms.TabControl();
			this.tabProject = new System.Windows.Forms.TabPage();
			this.clbProject = new System.Windows.Forms.CheckedListBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.pnlProjectInformation = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.lblApplication = new System.Windows.Forms.Label();
			this.btnApplication = new System.Windows.Forms.Button();
			this.txtApplication = new System.Windows.Forms.TextBox();
			this.btnHost = new System.Windows.Forms.Button();
			this.txtHost = new System.Windows.Forms.TextBox();
			this.chkHost = new System.Windows.Forms.CheckBox();
			this.lblUnitCount = new System.Windows.Forms.Label();
			this.lblRoutineCount = new System.Windows.Forms.Label();
			this.lblTotalPoints = new System.Windows.Forms.Label();
			this.lblParams = new System.Windows.Forms.Label();
			this.txtParams = new System.Windows.Forms.TextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.menuStrip1.SuspendLayout();
			this.tcMainMenu.SuspendLayout();
			this.tabProject.SuspendLayout();
			this.pnlProjectInformation.SuspendLayout();
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
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.openToolStripMenuItem.Text = "Load &delphi project";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveWorkspaceToolStripMenuItem
			// 
			this.saveWorkspaceToolStripMenuItem.Name = "saveWorkspaceToolStripMenuItem";
			this.saveWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.saveWorkspaceToolStripMenuItem.Text = "&Save workspace";
			this.saveWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.saveWorkspaceToolStripMenuItem_Click);
			// 
			// loadWorkspaceToolStripMenuItem
			// 
			this.loadWorkspaceToolStripMenuItem.Name = "loadWorkspaceToolStripMenuItem";
			this.loadWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.loadWorkspaceToolStripMenuItem.Text = "&Load workspace";
			this.loadWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.loadWorkspaceToolStripMenuItem_Click);
			// 
			// clearWorkspaceToolStripMenuItem
			// 
			this.clearWorkspaceToolStripMenuItem.Name = "clearWorkspaceToolStripMenuItem";
			this.clearWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.clearWorkspaceToolStripMenuItem.Text = "&Clear workspace";
			this.clearWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.clearWorkspaceToolStripMenuItem_Click);
			// 
			// projectToolStripMenuItem
			// 
			this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runSelectedToolStripMenuItem,
            this.terminateAllToolStripMenuItem});
			this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
			this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.projectToolStripMenuItem.Text = "&Project";
			// 
			// runSelectedToolStripMenuItem
			// 
			this.runSelectedToolStripMenuItem.Name = "runSelectedToolStripMenuItem";
			this.runSelectedToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.runSelectedToolStripMenuItem.Text = "&Run selected";
			this.runSelectedToolStripMenuItem.Click += new System.EventHandler(this.runSelectedToolStripMenuItem_Click);
			// 
			// terminateAllToolStripMenuItem
			// 
			this.terminateAllToolStripMenuItem.Name = "terminateAllToolStripMenuItem";
			this.terminateAllToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.terminateAllToolStripMenuItem.Text = "&Terminate all";
			this.terminateAllToolStripMenuItem.Click += new System.EventHandler(this.terminateAllToolStripMenuItem_Click);
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
			this.tabProject.Controls.Add(this.clbProject);
			this.tabProject.Controls.Add(this.splitter1);
			this.tabProject.Controls.Add(this.pnlProjectInformation);
			this.tabProject.Location = new System.Drawing.Point(4, 22);
			this.tabProject.Name = "tabProject";
			this.tabProject.Size = new System.Drawing.Size(419, 315);
			this.tabProject.TabIndex = 0;
			this.tabProject.Text = "Project";
			this.tabProject.UseVisualStyleBackColor = true;
			// 
			// clbProject
			// 
			this.clbProject.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.clbProject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clbProject.Location = new System.Drawing.Point(0, 0);
			this.clbProject.Name = "clbProject";
			this.clbProject.Size = new System.Drawing.Size(177, 315);
			this.clbProject.TabIndex = 2;
			this.clbProject.SelectedValueChanged += new System.EventHandler(this.clbProject_SelectedValueChanged);
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
			// pnlProjectInformation
			// 
			this.pnlProjectInformation.Controls.Add(this.btnCancel);
			this.pnlProjectInformation.Controls.Add(this.btnSave);
			this.pnlProjectInformation.Controls.Add(this.lblApplication);
			this.pnlProjectInformation.Controls.Add(this.btnApplication);
			this.pnlProjectInformation.Controls.Add(this.txtApplication);
			this.pnlProjectInformation.Controls.Add(this.btnHost);
			this.pnlProjectInformation.Controls.Add(this.txtHost);
			this.pnlProjectInformation.Controls.Add(this.chkHost);
			this.pnlProjectInformation.Controls.Add(this.lblUnitCount);
			this.pnlProjectInformation.Controls.Add(this.lblRoutineCount);
			this.pnlProjectInformation.Controls.Add(this.lblTotalPoints);
			this.pnlProjectInformation.Controls.Add(this.lblParams);
			this.pnlProjectInformation.Controls.Add(this.txtParams);
			this.pnlProjectInformation.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlProjectInformation.Location = new System.Drawing.Point(180, 0);
			this.pnlProjectInformation.Name = "pnlProjectInformation";
			this.pnlProjectInformation.Size = new System.Drawing.Size(239, 315);
			this.pnlProjectInformation.TabIndex = 0;
			this.pnlProjectInformation.Visible = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(179, 273);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(52, 21);
			this.btnCancel.TabIndex = 13;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(121, 273);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(52, 21);
			this.btnSave.TabIndex = 12;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lblApplication
			// 
			this.lblApplication.AutoSize = true;
			this.lblApplication.Location = new System.Drawing.Point(14, 69);
			this.lblApplication.Name = "lblApplication";
			this.lblApplication.Size = new System.Drawing.Size(59, 13);
			this.lblApplication.TabIndex = 11;
			this.lblApplication.Text = "Application";
			// 
			// btnApplication
			// 
			this.btnApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApplication.Location = new System.Drawing.Point(194, 85);
			this.btnApplication.Name = "btnApplication";
			this.btnApplication.Size = new System.Drawing.Size(37, 21);
			this.btnApplication.TabIndex = 10;
			this.btnApplication.Text = "...";
			this.btnApplication.UseVisualStyleBackColor = true;
			this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
			// 
			// txtApplication
			// 
			this.txtApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtApplication.Location = new System.Drawing.Point(17, 85);
			this.txtApplication.Name = "txtApplication";
			this.txtApplication.Size = new System.Drawing.Size(171, 20);
			this.txtApplication.TabIndex = 9;
			// 
			// btnHost
			// 
			this.btnHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHost.Location = new System.Drawing.Point(194, 176);
			this.btnHost.Name = "btnHost";
			this.btnHost.Size = new System.Drawing.Size(37, 21);
			this.btnHost.TabIndex = 8;
			this.btnHost.Text = "...";
			this.btnHost.UseVisualStyleBackColor = true;
			this.btnHost.Click += new System.EventHandler(this.btnHost_Click);
			// 
			// txtHost
			// 
			this.txtHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtHost.Location = new System.Drawing.Point(17, 177);
			this.txtHost.Name = "txtHost";
			this.txtHost.Size = new System.Drawing.Size(171, 20);
			this.txtHost.TabIndex = 7;
			// 
			// chkHost
			// 
			this.chkHost.AutoSize = true;
			this.chkHost.Location = new System.Drawing.Point(17, 154);
			this.chkHost.Name = "chkHost";
			this.chkHost.Size = new System.Drawing.Size(102, 17);
			this.chkHost.TabIndex = 6;
			this.chkHost.Text = "Host application";
			this.chkHost.UseVisualStyleBackColor = true;
			this.chkHost.CheckedChanged += new System.EventHandler(this.chkHost_CheckedChanged);
			// 
			// lblUnitCount
			// 
			this.lblUnitCount.AutoSize = true;
			this.lblUnitCount.Location = new System.Drawing.Point(14, 10);
			this.lblUnitCount.Name = "lblUnitCount";
			this.lblUnitCount.Size = new System.Drawing.Size(84, 13);
			this.lblUnitCount.TabIndex = 5;
			this.lblUnitCount.Text = "5 units in project";
			// 
			// lblRoutineCount
			// 
			this.lblRoutineCount.AutoSize = true;
			this.lblRoutineCount.Location = new System.Drawing.Point(14, 24);
			this.lblRoutineCount.Name = "lblRoutineCount";
			this.lblRoutineCount.Size = new System.Drawing.Size(59, 13);
			this.lblRoutineCount.TabIndex = 4;
			this.lblRoutineCount.Text = "56 routines";
			// 
			// lblTotalPoints
			// 
			this.lblTotalPoints.AutoSize = true;
			this.lblTotalPoints.Location = new System.Drawing.Point(14, 39);
			this.lblTotalPoints.Name = "lblTotalPoints";
			this.lblTotalPoints.Size = new System.Drawing.Size(104, 13);
			this.lblTotalPoints.TabIndex = 2;
			this.lblTotalPoints.Text = "210 coverage points";
			// 
			// lblParams
			// 
			this.lblParams.AutoSize = true;
			this.lblParams.Location = new System.Drawing.Point(14, 110);
			this.lblParams.Name = "lblParams";
			this.lblParams.Size = new System.Drawing.Size(110, 13);
			this.lblParams.TabIndex = 1;
			this.lblParams.Text = "Command line params";
			// 
			// txtParams
			// 
			this.txtParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtParams.Location = new System.Drawing.Point(17, 126);
			this.txtParams.Name = "txtParams";
			this.txtParams.Size = new System.Drawing.Size(214, 20);
			this.txtParams.TabIndex = 0;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 343);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(427, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
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
			this.pnlProjectInformation.ResumeLayout(false);
			this.pnlProjectInformation.PerformLayout();
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
		private System.Windows.Forms.CheckedListBox clbProject;
		private System.Windows.Forms.ToolStripMenuItem terminateAllToolStripMenuItem;
		private System.Windows.Forms.Label lblParams;
		private System.Windows.Forms.TextBox txtParams;
		private System.Windows.Forms.Label lblTotalPoints;
		private System.Windows.Forms.Label lblUnitCount;
		private System.Windows.Forms.Label lblRoutineCount;
		private System.Windows.Forms.CheckBox chkHost;
		private System.Windows.Forms.TextBox txtHost;
		private System.Windows.Forms.Button btnHost;
		private System.Windows.Forms.Button btnApplication;
		private System.Windows.Forms.TextBox txtApplication;
		private System.Windows.Forms.Label lblApplication;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
	}
}

