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
			this.btnSave = new System.Windows.Forms.Button();
			this.txtInfo = new System.Windows.Forms.RichTextBox();
			this.dlgFont = new System.Windows.Forms.FontDialog();
			this.btnFind = new System.Windows.Forms.Button();
			this.pnlInfo = new System.Windows.Forms.Panel();
			this.txtFind = new System.Windows.Forms.TextBox();
			this.chkWordWrap = new System.Windows.Forms.CheckBox();
			this.pnlTool = new System.Windows.Forms.Panel();
			this.btnFont = new System.Windows.Forms.Button();
			this.cboMode = new System.Windows.Forms.ComboBox();
			this.dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.cboAssembly = new System.Windows.Forms.ComboBox();
			this.lblMode = new System.Windows.Forms.Label();
			this.pnlInfo.SuspendLayout();
			this.pnlTool.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(1, 29);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(64, 24);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// txtInfo
			// 
			this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInfo.HideSelection = false;
			this.txtInfo.Location = new System.Drawing.Point(0, 0);
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.ReadOnly = true;
			this.txtInfo.Size = new System.Drawing.Size(592, 309);
			this.txtInfo.TabIndex = 0;
			this.txtInfo.Text = "";
			this.txtInfo.WordWrap = false;
			// 
			// btnFind
			// 
			this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFind.Location = new System.Drawing.Point(526, 27);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(64, 24);
			this.btnFind.TabIndex = 7;
			this.btnFind.Text = "F&ind";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// pnlInfo
			// 
			this.pnlInfo.Controls.Add(this.txtInfo);
			this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlInfo.Location = new System.Drawing.Point(0, 57);
			this.pnlInfo.Name = "pnlInfo";
			this.pnlInfo.Size = new System.Drawing.Size(592, 309);
			this.pnlInfo.TabIndex = 3;
			// 
			// txtFind
			// 
			this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFind.Location = new System.Drawing.Point(220, 30);
			this.txtFind.Name = "txtFind";
			this.txtFind.Size = new System.Drawing.Size(300, 21);
			this.txtFind.TabIndex = 6;
			// 
			// chkWordWrap
			// 
			this.chkWordWrap.AutoSize = true;
			this.chkWordWrap.Location = new System.Drawing.Point(72, 32);
			this.chkWordWrap.Name = "chkWordWrap";
			this.chkWordWrap.Size = new System.Drawing.Size(72, 16);
			this.chkWordWrap.TabIndex = 4;
			this.chkWordWrap.Text = "&WordWrap";
			this.chkWordWrap.UseVisualStyleBackColor = true;
			this.chkWordWrap.CheckedChanged += new System.EventHandler(this.chkWordWrap_CheckedChanged);
			// 
			// pnlTool
			// 
			this.pnlTool.Controls.Add(this.lblMode);
			this.pnlTool.Controls.Add(this.cboAssembly);
			this.pnlTool.Controls.Add(this.btnSave);
			this.pnlTool.Controls.Add(this.btnFind);
			this.pnlTool.Controls.Add(this.txtFind);
			this.pnlTool.Controls.Add(this.chkWordWrap);
			this.pnlTool.Controls.Add(this.btnFont);
			this.pnlTool.Controls.Add(this.cboMode);
			this.pnlTool.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTool.Location = new System.Drawing.Point(0, 0);
			this.pnlTool.Name = "pnlTool";
			this.pnlTool.Size = new System.Drawing.Size(592, 57);
			this.pnlTool.TabIndex = 2;
			// 
			// btnFont
			// 
			this.btnFont.Location = new System.Drawing.Point(150, 27);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new System.Drawing.Size(64, 24);
			this.btnFont.TabIndex = 5;
			this.btnFont.Text = "&Font";
			this.btnFont.UseVisualStyleBackColor = true;
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// cboMode
			// 
			this.cboMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMode.FormattingEnabled = true;
			this.cboMode.Location = new System.Drawing.Point(436, 3);
			this.cboMode.Name = "cboMode";
			this.cboMode.Size = new System.Drawing.Size(153, 20);
			this.cboMode.TabIndex = 2;
			this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
			// 
			// dlgSave
			// 
			this.dlgSave.Filter = "Text file(*.txt)|*.txt|All file(*.*)|*.*";
			// 
			// cboAssembly
			// 
			this.cboAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboAssembly.DropDownHeight = 480;
			this.cboAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAssembly.FormattingEnabled = true;
			this.cboAssembly.IntegralHeight = false;
			this.cboAssembly.Location = new System.Drawing.Point(1, 3);
			this.cboAssembly.Name = "cboAssembly";
			this.cboAssembly.Size = new System.Drawing.Size(388, 20);
			this.cboAssembly.TabIndex = 0;
			this.cboAssembly.SelectedIndexChanged += new System.EventHandler(this.cboAssembly_SelectedIndexChanged);
			// 
			// lblMode
			// 
			this.lblMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMode.AutoSize = true;
			this.lblMode.Location = new System.Drawing.Point(395, 6);
			this.lblMode.Name = "lblMode";
			this.lblMode.Size = new System.Drawing.Size(35, 12);
			this.lblMode.TabIndex = 1;
			this.lblMode.Text = "&Mode:";
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
			this.pnlTool.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.RichTextBox txtInfo;
		private System.Windows.Forms.FontDialog dlgFont;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.Panel pnlInfo;
		private System.Windows.Forms.TextBox txtFind;
		private System.Windows.Forms.CheckBox chkWordWrap;
		private System.Windows.Forms.Panel pnlTool;
		private System.Windows.Forms.Button btnFont;
		private System.Windows.Forms.ComboBox cboMode;
		private System.Windows.Forms.SaveFileDialog dlgSave;
		private System.Windows.Forms.Label lblMode;
		private System.Windows.Forms.ComboBox cboAssembly;
	}
}

