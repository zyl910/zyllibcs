namespace zinfoassemblyf {
	partial class FrmZInfoAssembly {
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			this.txtInfo = new System.Windows.Forms.RichTextBox();
			this.dlgFont = new System.Windows.Forms.FontDialog();
			this.pnlInfo = new System.Windows.Forms.Panel();
			this.pnlTool = new System.Windows.Forms.Panel();
			this.cboAssembly = new System.Windows.Forms.ComboBox();
			this.cboMode = new System.Windows.Forms.ComboBox();
			this.dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.pnlToolInfo = new System.Windows.Forms.Panel();
			this.chkMethod = new System.Windows.Forms.CheckBox();
			this.chkSort = new System.Windows.Forms.CheckBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnFind = new System.Windows.Forms.Button();
			this.txtFind = new System.Windows.Forms.TextBox();
			this.chkWordWrap = new System.Windows.Forms.CheckBox();
			this.btnFont = new System.Windows.Forms.Button();
			this.spcSrc = new System.Windows.Forms.SplitContainer();
			this.pnlInfo.SuspendLayout();
			this.pnlTool.SuspendLayout();
			this.pnlToolInfo.SuspendLayout();
			this.spcSrc.Panel1.SuspendLayout();
			this.spcSrc.Panel2.SuspendLayout();
			this.spcSrc.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtInfo
			// 
			this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInfo.HideSelection = false;
			this.txtInfo.Location = new System.Drawing.Point(0, 32);
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.ReadOnly = true;
			this.txtInfo.Size = new System.Drawing.Size(592, 310);
			this.txtInfo.TabIndex = 1;
			this.txtInfo.Text = "";
			this.txtInfo.WordWrap = false;
			// 
			// pnlInfo
			// 
			this.pnlInfo.Controls.Add(this.txtInfo);
			this.pnlInfo.Controls.Add(this.pnlToolInfo);
			this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlInfo.Location = new System.Drawing.Point(0, 24);
			this.pnlInfo.Name = "pnlInfo";
			this.pnlInfo.Size = new System.Drawing.Size(592, 342);
			this.pnlInfo.TabIndex = 3;
			// 
			// pnlTool
			// 
			this.pnlTool.Controls.Add(this.spcSrc);
			this.pnlTool.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTool.Location = new System.Drawing.Point(0, 0);
			this.pnlTool.Name = "pnlTool";
			this.pnlTool.Size = new System.Drawing.Size(592, 24);
			this.pnlTool.TabIndex = 0;
			// 
			// cboAssembly
			// 
			this.cboAssembly.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboAssembly.DropDownHeight = 480;
			this.cboAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAssembly.FormattingEnabled = true;
			this.cboAssembly.IntegralHeight = false;
			this.cboAssembly.Location = new System.Drawing.Point(0, 0);
			this.cboAssembly.Name = "cboAssembly";
			this.cboAssembly.Size = new System.Drawing.Size(400, 20);
			this.cboAssembly.TabIndex = 0;
			this.cboAssembly.SelectedIndexChanged += new System.EventHandler(this.cboAssembly_SelectedIndexChanged);
			// 
			// cboMode
			// 
			this.cboMode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMode.FormattingEnabled = true;
			this.cboMode.Location = new System.Drawing.Point(0, 0);
			this.cboMode.Name = "cboMode";
			this.cboMode.Size = new System.Drawing.Size(188, 20);
			this.cboMode.TabIndex = 2;
			this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
			// 
			// dlgSave
			// 
			this.dlgSave.Filter = "Text file(*.txt)|*.txt|All file(*.*)|*.*";
			// 
			// pnlToolInfo
			// 
			this.pnlToolInfo.Controls.Add(this.chkMethod);
			this.pnlToolInfo.Controls.Add(this.chkSort);
			this.pnlToolInfo.Controls.Add(this.btnSave);
			this.pnlToolInfo.Controls.Add(this.btnFind);
			this.pnlToolInfo.Controls.Add(this.txtFind);
			this.pnlToolInfo.Controls.Add(this.chkWordWrap);
			this.pnlToolInfo.Controls.Add(this.btnFont);
			this.pnlToolInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlToolInfo.Location = new System.Drawing.Point(0, 0);
			this.pnlToolInfo.Name = "pnlToolInfo";
			this.pnlToolInfo.Size = new System.Drawing.Size(592, 32);
			this.pnlToolInfo.TabIndex = 0;
			// 
			// chkMethod
			// 
			this.chkMethod.AutoSize = true;
			this.chkMethod.Location = new System.Drawing.Point(2, 8);
			this.chkMethod.Name = "chkMethod";
			this.chkMethod.Size = new System.Drawing.Size(60, 16);
			this.chkMethod.TabIndex = 0;
			this.chkMethod.Text = "&Method";
			this.chkMethod.UseVisualStyleBackColor = true;
			this.chkMethod.CheckedChanged += new System.EventHandler(this.chkMethod_CheckedChanged);
			// 
			// chkSort
			// 
			this.chkSort.AutoSize = true;
			this.chkSort.Location = new System.Drawing.Point(65, 8);
			this.chkSort.Name = "chkSort";
			this.chkSort.Size = new System.Drawing.Size(48, 16);
			this.chkSort.TabIndex = 1;
			this.chkSort.Text = "&Sort";
			this.chkSort.UseVisualStyleBackColor = true;
			this.chkSort.Click += new System.EventHandler(this.chkSort_CheckedChanged);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(114, 3);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(64, 24);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnFind
			// 
			this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFind.Location = new System.Drawing.Point(525, 3);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(64, 24);
			this.btnFind.TabIndex = 6;
			this.btnFind.Text = "F&ind";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// txtFind
			// 
			this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFind.Location = new System.Drawing.Point(332, 6);
			this.txtFind.Name = "txtFind";
			this.txtFind.Size = new System.Drawing.Size(187, 21);
			this.txtFind.TabIndex = 5;
			// 
			// chkWordWrap
			// 
			this.chkWordWrap.AutoSize = true;
			this.chkWordWrap.Location = new System.Drawing.Point(184, 8);
			this.chkWordWrap.Name = "chkWordWrap";
			this.chkWordWrap.Size = new System.Drawing.Size(72, 16);
			this.chkWordWrap.TabIndex = 3;
			this.chkWordWrap.Text = "&WordWrap";
			this.chkWordWrap.UseVisualStyleBackColor = true;
			this.chkWordWrap.CheckedChanged += new System.EventHandler(this.chkWordWrap_CheckedChanged);
			// 
			// btnFont
			// 
			this.btnFont.Location = new System.Drawing.Point(262, 3);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new System.Drawing.Size(64, 24);
			this.btnFont.TabIndex = 4;
			this.btnFont.Text = "&Font";
			this.btnFont.UseVisualStyleBackColor = true;
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// spcSrc
			// 
			this.spcSrc.Dock = System.Windows.Forms.DockStyle.Top;
			this.spcSrc.Location = new System.Drawing.Point(0, 0);
			this.spcSrc.Name = "spcSrc";
			// 
			// spcSrc.Panel1
			// 
			this.spcSrc.Panel1.Controls.Add(this.cboAssembly);
			// 
			// spcSrc.Panel2
			// 
			this.spcSrc.Panel2.Controls.Add(this.cboMode);
			this.spcSrc.Size = new System.Drawing.Size(592, 24);
			this.spcSrc.SplitterDistance = 400;
			this.spcSrc.TabIndex = 3;
			// 
			// FrmZInfoAssembly
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 366);
			this.Controls.Add(this.pnlInfo);
			this.Controls.Add(this.pnlTool);
			this.Name = "FrmZInfoAssembly";
			this.Text = "zInfoAssembly(显示程序集信息)";
			this.Load += new System.EventHandler(this.FrmZInfoAssembly_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmZInfoAssembly_FormClosed);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmZInfoAssembly_FormClosing);
			this.pnlInfo.ResumeLayout(false);
			this.pnlTool.ResumeLayout(false);
			this.pnlToolInfo.ResumeLayout(false);
			this.pnlToolInfo.PerformLayout();
			this.spcSrc.Panel1.ResumeLayout(false);
			this.spcSrc.Panel2.ResumeLayout(false);
			this.spcSrc.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox txtInfo;
		private System.Windows.Forms.FontDialog dlgFont;
		private System.Windows.Forms.Panel pnlInfo;
		private System.Windows.Forms.Panel pnlTool;
		private System.Windows.Forms.ComboBox cboMode;
		private System.Windows.Forms.SaveFileDialog dlgSave;
		private System.Windows.Forms.ComboBox cboAssembly;
		private System.Windows.Forms.Panel pnlToolInfo;
		private System.Windows.Forms.CheckBox chkMethod;
		private System.Windows.Forms.CheckBox chkSort;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.TextBox txtFind;
		private System.Windows.Forms.CheckBox chkWordWrap;
		private System.Windows.Forms.Button btnFont;
		private System.Windows.Forms.SplitContainer spcSrc;
	}
}

