using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using zyllibcs.text;
using zinfotools;

namespace zinfotoolsf {
	/// <summary>
	/// zinfotools-winform Form.
	/// </summary>
	public partial class FrmZInfo : Form {
		public FrmZInfo() {
			InitializeComponent();
		}

		private void FrmZInfo_Load(object sender, EventArgs e) {
			// init.
			dlgFont.Font = txtInfo.Font;
			cboMode.Items.Clear();
			for (int i = 0; i < InfoData.NameProcs.Length; ++i) {
				cboMode.Items.Add(InfoData.NameProcs[i].Key);
			}
			try {
				cboMode.SelectedIndex = InfoData.DefaultNameIndex;
			}
			catch {
			}
		}

		private void FrmZInfo_FormClosing(object sender, FormClosingEventArgs e) {
			//
		}

		private void FrmZInfo_FormClosed(object sender, FormClosedEventArgs e) {
			//
		}

		private void cboMode_SelectedIndexChanged(object sender, EventArgs e) {
			int idx = cboMode.SelectedIndex;
			if (idx < 0 || idx >= InfoData.NameProcs.Length) return;
			IndentedWriterObjectProc proc = InfoData.NameProcs[idx].Value;
			StringBuilder sb = new StringBuilder();
			IIndentedWriter iw = new TextIndentedWriter(new StringWriter(sb));
			try {
				proc(iw, null, null);
			}
			catch (Exception ex) {
				sb.AppendLine(ex.ToString());
			}
			txtInfo.Text = sb.ToString();
		}

		private void btnFont_Click(object sender, EventArgs e) {
			if (dlgFont.ShowDialog(this) == DialogResult.OK) {
				txtInfo.Font = dlgFont.Font;
			}
		}

		private void chkWordWrap_CheckedChanged(object sender, EventArgs e) {
			txtInfo.WordWrap = chkWordWrap.Checked;
		}

		private void btnFind_Click(object sender, EventArgs e) {
			string key = txtFind.Text;
			if (string.IsNullOrEmpty(key)) return;
			int start = txtInfo.SelectionStart + txtInfo.SelectionLength;
			//MessageBox.Show(string.Format("Selection: {0}, {1}", txtInfo.SelectionStart, txtInfo.SelectionLength));
			int p = txtInfo.Find(key, start, RichTextBoxFinds.None);
			//MessageBox.Show(string.Format("Find: {0}, {1}, {2}", txtInfo.SelectionStart, txtInfo.SelectionLength, p));
			if (p >= 0) {
				// 解决 mono 下不会自动选择文本 问题.
				if (p != txtInfo.SelectionStart) {
					try {
						txtInfo.SelectionStart = p;
						txtInfo.SelectionLength = key.Length;
					}
					catch {
					}
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e) {
			if (dlgSave.ShowDialog(this) == DialogResult.OK) {
				try {
					File.WriteAllText(dlgSave.FileName, txtInfo.Text, Encoding.UTF8);
				}
				catch (Exception ex) {
					MessageBox.Show(this, ex.ToString(), "Save failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}