namespace zinfotoolsf {
	partial class FrmZInfo {
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
			this.pnlTool = new System.Windows.Forms.Panel();
			this.chkWordWrap = new System.Windows.Forms.CheckBox();
			this.btnFont = new System.Windows.Forms.Button();
			this.cboMode = new System.Windows.Forms.ComboBox();
			this.pnlInfo = new System.Windows.Forms.Panel();
			this.txtInfo = new System.Windows.Forms.RichTextBox();
			this.dlgFont = new System.Windows.Forms.FontDialog();
			this.txtFind = new System.Windows.Forms.TextBox();
			this.btnFind = new System.Windows.Forms.Button();
			this.pnlTool.SuspendLayout();
			this.pnlInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlTool
			// 
			this.pnlTool.Controls.Add(this.btnFind);
			this.pnlTool.Controls.Add(this.txtFind);
			this.pnlTool.Controls.Add(this.chkWordWrap);
			this.pnlTool.Controls.Add(this.btnFont);
			this.pnlTool.Controls.Add(this.cboMode);
			this.pnlTool.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTool.Location = new System.Drawing.Point(0, 0);
			this.pnlTool.Name = "pnlTool";
			this.pnlTool.Size = new System.Drawing.Size(584, 24);
			this.pnlTool.TabIndex = 0;
			// 
			// chkWordWrap
			// 
			this.chkWordWrap.AutoSize = true;
			this.chkWordWrap.Location = new System.Drawing.Point(249, 5);
			this.chkWordWrap.Name = "chkWordWrap";
			this.chkWordWrap.Size = new System.Drawing.Size(72, 16);
			this.chkWordWrap.TabIndex = 2;
			this.chkWordWrap.Text = "&WordWrap";
			this.chkWordWrap.UseVisualStyleBackColor = true;
			this.chkWordWrap.CheckedChanged += new System.EventHandler(this.chkWordWrap_CheckedChanged);
			// 
			// btnFont
			// 
			this.btnFont.Location = new System.Drawing.Point(327, 0);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new System.Drawing.Size(64, 24);
			this.btnFont.TabIndex = 1;
			this.btnFont.Text = "&Font";
			this.btnFont.UseVisualStyleBackColor = true;
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// cboMode
			// 
			this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMode.FormattingEnabled = true;
			this.cboMode.Location = new System.Drawing.Point(3, 3);
			this.cboMode.Name = "cboMode";
			this.cboMode.Size = new System.Drawing.Size(240, 20);
			this.cboMode.TabIndex = 0;
			this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
			// 
			// pnlInfo
			// 
			this.pnlInfo.Controls.Add(this.txtInfo);
			this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlInfo.Location = new System.Drawing.Point(0, 24);
			this.pnlInfo.Name = "pnlInfo";
			this.pnlInfo.Size = new System.Drawing.Size(584, 338);
			this.pnlInfo.TabIndex = 1;
			// 
			// txtInfo
			// 
			this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInfo.HideSelection = false;
			this.txtInfo.Location = new System.Drawing.Point(0, 0);
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.ReadOnly = true;
			this.txtInfo.Size = new System.Drawing.Size(584, 338);
			this.txtInfo.TabIndex = 0;
			this.txtInfo.Text = "";
			this.txtInfo.WordWrap = false;
			// 
			// txtFind
			// 
			this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFind.Location = new System.Drawing.Point(397, 2);
			this.txtFind.Name = "txtFind";
			this.txtFind.Size = new System.Drawing.Size(114, 21);
			this.txtFind.TabIndex = 3;
			// 
			// btnFind
			// 
			this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFind.Location = new System.Drawing.Point(517, -1);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(64, 24);
			this.btnFind.TabIndex = 4;
			this.btnFind.Text = "F&ind";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// FrmZInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 362);
			this.Controls.Add(this.pnlInfo);
			this.Controls.Add(this.pnlTool);
			this.Name = "FrmZInfo";
			this.Text = "zinfotools-winform";
			this.Load += new System.EventHandler(this.FrmZInfo_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmZInfo_FormClosed);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmZInfo_FormClosing);
			this.pnlTool.ResumeLayout(false);
			this.pnlTool.PerformLayout();
			this.pnlInfo.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlTool;
		private System.Windows.Forms.Panel pnlInfo;
		private System.Windows.Forms.ComboBox cboMode;
		private System.Windows.Forms.Button btnFont;
		private System.Windows.Forms.RichTextBox txtInfo;
		private System.Windows.Forms.FontDialog dlgFont;
		private System.Windows.Forms.CheckBox chkWordWrap;
		private System.Windows.Forms.TextBox txtFind;
		private System.Windows.Forms.Button btnFind;
	}
}

