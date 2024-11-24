namespace Plugin.ProxyServer
{
	partial class PanelProxyLog
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
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelProxyLog));
			this.tabPayload = new System.Windows.Forms.TabControl();
			this.tabHeaders = new System.Windows.Forms.TabPage();
			this.propHeaders = new System.Windows.Forms.PropertyGrid();
			this.tabRequest = new System.Windows.Forms.TabPage();
			this.txtRequest = new System.Windows.Forms.TextBox();
			this.tabResponse = new System.Windows.Forms.TabPage();
			this.txtResponse = new System.Windows.Forms.TextBox();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsbnProxyChangeState = new System.Windows.Forms.ToolStripButton();
			this.tsddlPluginCompiler = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmiPluginCompilerRequest = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiPluginCompilerResponse = new System.Windows.Forms.ToolStripMenuItem();
			this.tsddlPluginCryptoUI = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmiPluginCryptoUIGenerate = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiPluginCryptoUISave = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiPluginCryptoUIDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.lvData = new Plugin.ProxyServer.UI.MessageListView();
			this.cmsData = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiDataClear = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDataGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.tabPayload.SuspendLayout();
			this.tabHeaders.SuspendLayout();
			this.tabRequest.SuspendLayout();
			this.tabResponse.SuspendLayout();
			this.tsMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.cmsData.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPayload
			// 
			this.tabPayload.Controls.Add(this.tabHeaders);
			this.tabPayload.Controls.Add(this.tabRequest);
			this.tabPayload.Controls.Add(this.tabResponse);
			this.tabPayload.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPayload.Location = new System.Drawing.Point(0, 0);
			this.tabPayload.Margin = new System.Windows.Forms.Padding(4);
			this.tabPayload.Name = "tabPayload";
			this.tabPayload.SelectedIndex = 0;
			this.tabPayload.Size = new System.Drawing.Size(200, 75);
			this.tabPayload.TabIndex = 0;
			// 
			// tabHeaders
			// 
			this.tabHeaders.Controls.Add(this.propHeaders);
			this.tabHeaders.Location = new System.Drawing.Point(4, 25);
			this.tabHeaders.Margin = new System.Windows.Forms.Padding(4);
			this.tabHeaders.Name = "tabHeaders";
			this.tabHeaders.Size = new System.Drawing.Size(192, 46);
			this.tabHeaders.TabIndex = 2;
			this.tabHeaders.Text = "Headers";
			this.tabHeaders.UseVisualStyleBackColor = true;
			// 
			// propHeaders
			// 
			this.propHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propHeaders.LineColor = System.Drawing.SystemColors.ControlDark;
			this.propHeaders.Location = new System.Drawing.Point(0, 0);
			this.propHeaders.Margin = new System.Windows.Forms.Padding(4);
			this.propHeaders.Name = "propHeaders";
			this.propHeaders.Size = new System.Drawing.Size(192, 46);
			this.propHeaders.TabIndex = 0;
			this.propHeaders.ToolbarVisible = false;
			// 
			// tabRequest
			// 
			this.tabRequest.Controls.Add(this.txtRequest);
			this.tabRequest.Location = new System.Drawing.Point(4, 25);
			this.tabRequest.Margin = new System.Windows.Forms.Padding(4);
			this.tabRequest.Name = "tabRequest";
			this.tabRequest.Padding = new System.Windows.Forms.Padding(4);
			this.tabRequest.Size = new System.Drawing.Size(192, 46);
			this.tabRequest.TabIndex = 0;
			this.tabRequest.Text = "Request";
			this.tabRequest.UseVisualStyleBackColor = true;
			// 
			// txtRequest
			// 
			this.txtRequest.AcceptsReturn = true;
			this.txtRequest.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtRequest.Location = new System.Drawing.Point(4, 4);
			this.txtRequest.Margin = new System.Windows.Forms.Padding(4);
			this.txtRequest.Multiline = true;
			this.txtRequest.Name = "txtRequest";
			this.txtRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtRequest.Size = new System.Drawing.Size(184, 38);
			this.txtRequest.TabIndex = 0;
			// 
			// tabResponse
			// 
			this.tabResponse.Controls.Add(this.txtResponse);
			this.tabResponse.Location = new System.Drawing.Point(4, 25);
			this.tabResponse.Margin = new System.Windows.Forms.Padding(4);
			this.tabResponse.Name = "tabResponse";
			this.tabResponse.Padding = new System.Windows.Forms.Padding(4);
			this.tabResponse.Size = new System.Drawing.Size(192, 46);
			this.tabResponse.TabIndex = 1;
			this.tabResponse.Text = "Response";
			this.tabResponse.UseVisualStyleBackColor = true;
			// 
			// txtResponse
			// 
			this.txtResponse.AcceptsReturn = true;
			this.txtResponse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtResponse.Location = new System.Drawing.Point(4, 4);
			this.txtResponse.Margin = new System.Windows.Forms.Padding(4);
			this.txtResponse.Multiline = true;
			this.txtResponse.Name = "txtResponse";
			this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtResponse.Size = new System.Drawing.Size(184, 38);
			this.txtResponse.TabIndex = 0;
			// 
			// tsMain
			// 
			this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnProxyChangeState,
            this.tsddlPluginCompiler,
            this.tsddlPluginCryptoUI});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(200, 27);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "tsMain";
			this.tsMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsMain_ItemClicked);
			// 
			// tsbnProxyChangeState
			// 
			this.tsbnProxyChangeState.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tsbnProxyChangeState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnProxyChangeState.Image = global::Plugin.ProxyServer.Properties.Resources.bnStart;
			this.tsbnProxyChangeState.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnProxyChangeState.Name = "tsbnProxyChangeState";
			this.tsbnProxyChangeState.Size = new System.Drawing.Size(24, 24);
			// 
			// tsddlPluginCompiler
			// 
			this.tsddlPluginCompiler.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsddlPluginCompiler.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPluginCompilerRequest,
            this.tsmiPluginCompilerResponse});
			this.tsddlPluginCompiler.Image = ((System.Drawing.Image)(resources.GetObject("tsddlPluginCompiler.Image")));
			this.tsddlPluginCompiler.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsddlPluginCompiler.Name = "tsddlPluginCompiler";
			this.tsddlPluginCompiler.Size = new System.Drawing.Size(34, 24);
			this.tsddlPluginCompiler.ToolTipText = "Edit dynamic code in compiler plugin";
			this.tsddlPluginCompiler.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsddlPluginCompiler_DropDownItemClicked);
			// 
			// tsmiPluginCompilerRequest
			// 
			this.tsmiPluginCompilerRequest.Name = "tsmiPluginCompilerRequest";
			this.tsmiPluginCompilerRequest.Size = new System.Drawing.Size(166, 26);
			this.tsmiPluginCompilerRequest.Text = "On&Request";
			// 
			// tsmiPluginCompilerResponse
			// 
			this.tsmiPluginCompilerResponse.Name = "tsmiPluginCompilerResponse";
			this.tsmiPluginCompilerResponse.Size = new System.Drawing.Size(166, 26);
			this.tsmiPluginCompilerResponse.Text = "On&Response";
			// 
			// tsddlPluginCryptoUI
			// 
			this.tsddlPluginCryptoUI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsddlPluginCryptoUI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPluginCryptoUIGenerate,
            this.tsmiPluginCryptoUISave,
            this.tsmiPluginCryptoUIDelete});
			this.tsddlPluginCryptoUI.Image = ((System.Drawing.Image)(resources.GetObject("tsddlPluginCryptoUI.Image")));
			this.tsddlPluginCryptoUI.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsddlPluginCryptoUI.Name = "tsddlPluginCryptoUI";
			this.tsddlPluginCryptoUI.Size = new System.Drawing.Size(34, 24);
			this.tsddlPluginCryptoUI.ToolTipText = "Generate root certificate for MitM proxy";
			this.tsddlPluginCryptoUI.DropDownOpening += new System.EventHandler(this.tsddlPluginCryptoUI_DropDownOpening);
			this.tsddlPluginCryptoUI.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsddlPluginCryptoUI_DropDownItemClicked);
			// 
			// tsmiPluginCryptoUIGenerate
			// 
			this.tsmiPluginCryptoUIGenerate.Name = "tsmiPluginCryptoUIGenerate";
			this.tsmiPluginCryptoUIGenerate.Size = new System.Drawing.Size(144, 26);
			this.tsmiPluginCryptoUIGenerate.Text = "&Generate";
			// 
			// tsmiPluginCryptoUISave
			// 
			this.tsmiPluginCryptoUISave.Name = "tsmiPluginCryptoUISave";
			this.tsmiPluginCryptoUISave.Size = new System.Drawing.Size(144, 26);
			this.tsmiPluginCryptoUISave.Text = "&Export";
			// 
			// tsmiPluginCryptoUIDelete
			// 
			this.tsmiPluginCryptoUIDelete.Name = "tsmiPluginCryptoUIDelete";
			this.tsmiPluginCryptoUIDelete.Size = new System.Drawing.Size(144, 26);
			this.tsmiPluginCryptoUIDelete.Text = "&Delete";
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.Location = new System.Drawing.Point(0, 27);
			this.splitMain.Margin = new System.Windows.Forms.Padding(4);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.lvData);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.tabPayload);
			this.splitMain.Size = new System.Drawing.Size(200, 158);
			this.splitMain.SplitterDistance = 78;
			this.splitMain.SplitterWidth = 5;
			this.splitMain.TabIndex = 1;
			this.splitMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.splitMain_MouseDoubleClick);
			// 
			// lvData
			// 
			this.lvData.ContextMenuStrip = this.cmsData;
			this.lvData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvData.FullRowSelect = true;
			this.lvData.GroupType = Plugin.ProxyServer.UI.MessageGroupType.None;
			this.lvData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvData.HideSelection = false;
			this.lvData.Location = new System.Drawing.Point(0, 0);
			this.lvData.Margin = new System.Windows.Forms.Padding(4);
			this.lvData.Name = "lvData";
			this.lvData.Size = new System.Drawing.Size(200, 78);
			this.lvData.TabIndex = 0;
			this.lvData.UseCompatibleStateImageBehavior = false;
			this.lvData.View = System.Windows.Forms.View.Details;
			this.lvData.SelectedIndexChanged += new System.EventHandler(this.lvData_SelectedIndexChanged);
			this.lvData.DoubleClick += new System.EventHandler(this.lvData_DoubleClick);
			// 
			// cmsData
			// 
			this.cmsData.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.cmsData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDataClear,
            this.tsmiDataGroup});
			this.cmsData.Name = "cmsData";
			this.cmsData.Size = new System.Drawing.Size(120, 52);
			this.cmsData.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsData_ItemClicked);
			// 
			// tsmiDataClear
			// 
			this.tsmiDataClear.Name = "tsmiDataClear";
			this.tsmiDataClear.Size = new System.Drawing.Size(119, 24);
			this.tsmiDataClear.Text = "&Clear";
			// 
			// tsmiDataGroup
			// 
			this.tsmiDataGroup.Name = "tsmiDataGroup";
			this.tsmiDataGroup.Size = new System.Drawing.Size(119, 24);
			this.tsmiDataGroup.Text = "Group";
			this.tsmiDataGroup.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsmiDataGroup_DropDownItemClicked);
			// 
			// PanelProxyLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.tsMain);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "PanelProxyLog";
			this.Size = new System.Drawing.Size(200, 185);
			this.tabPayload.ResumeLayout(false);
			this.tabHeaders.ResumeLayout(false);
			this.tabRequest.ResumeLayout(false);
			this.tabRequest.PerformLayout();
			this.tabResponse.ResumeLayout(false);
			this.tabResponse.PerformLayout();
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.cmsData.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.SplitContainer splitMain;
		private ProxyServer.UI.MessageListView lvData;
		private System.Windows.Forms.TabPage tabHeaders;
		private System.Windows.Forms.PropertyGrid propHeaders;
		private System.Windows.Forms.TabPage tabRequest;
		private System.Windows.Forms.TextBox txtRequest;
		private System.Windows.Forms.TabPage tabResponse;
		private System.Windows.Forms.TextBox txtResponse;
		private System.Windows.Forms.TabControl tabPayload;
		private System.Windows.Forms.ToolStripButton tsbnProxyChangeState;
		private System.Windows.Forms.ContextMenuStrip cmsData;
		private System.Windows.Forms.ToolStripMenuItem tsmiDataClear;
		private System.Windows.Forms.ToolStripDropDownButton tsddlPluginCompiler;
		private System.Windows.Forms.ToolStripMenuItem tsmiPluginCompilerRequest;
		private System.Windows.Forms.ToolStripMenuItem tsmiPluginCompilerResponse;
		private System.Windows.Forms.ToolStripMenuItem tsmiDataGroup;
		private System.Windows.Forms.ToolStripDropDownButton tsddlPluginCryptoUI;
		private System.Windows.Forms.ToolStripMenuItem tsmiPluginCryptoUIGenerate;
		private System.Windows.Forms.ToolStripMenuItem tsmiPluginCryptoUISave;
		private System.Windows.Forms.ToolStripMenuItem tsmiPluginCryptoUIDelete;
	}
}
