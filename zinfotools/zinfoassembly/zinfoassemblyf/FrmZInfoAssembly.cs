﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using zyllibcs.text;
using zinfoassemblydata;

namespace zinfoassemblyf {
	public partial class FrmZInfoAssembly : Form {
		/// <summary>
		/// 旧的程序集名称.
		/// </summary>
		private string m_OldAssemblyName = null;

		/// <summary>
		/// 当前程序集.
		/// </summary>
		private Assembly m_CurAssembly = null;

		public FrmZInfoAssembly() {
			InitializeComponent();
		}

		/// <summary>
		/// 刷新信息.
		/// </summary>
		private void RefreshInfo() {
			string name = cboAssembly.SelectedItem as string;
			if (null == name) return;
			// 重新获取程序集.
			if (!name.Equals(m_OldAssemblyName, StringComparison.Ordinal)) {
				try {
					m_CurAssembly = InfoAssembly.LoadAssembly(name);
					m_OldAssemblyName = name;
				}
				catch (Exception ex) {
					m_OldAssemblyName = null;
					txtInfo.Text = ex.ToString();
				}
			}
			if (null == m_CurAssembly) return;
			// 显示信息.
			InfoMode mode = (InfoMode)cboMode.SelectedItem;
			StringBuilder sb = new StringBuilder();
			IIndentedWriter iw = new TextIndentedWriter(new StringWriter(sb));
			this.UseWaitCursor = true;
			Application.DoEvents();
			try {
				InfoAssembly.WriteInfo(iw, null, m_CurAssembly, mode);
			}
			catch (Exception ex) {
				sb.AppendLine(ex.ToString());
			}
			this.UseWaitCursor = false;
			txtInfo.Text = sb.ToString();
		}

		private void FrmZInfoAssembly_Load(object sender, EventArgs e) {
			// init.
			dlgFont.Font = txtInfo.Font;
			cboAssembly.Items.Clear();
			try {
				foreach (string str in InfoAssembly.AssemblyList) {
					cboAssembly.Items.Add(str);
				}
			}
			catch (Exception ex) {
				txtInfo.Text = ex.ToString();
			}
			cboMode.Items.Clear();
			try {
				foreach (InfoMode p in Enum.GetValues(typeof(InfoMode))) {
					cboMode.Items.Add(p);
				}
				cboMode.SelectedIndex = 0;
			}
			catch{
				// 忽略.
			}
		}

		private void FrmZInfoAssembly_FormClosing(object sender, FormClosingEventArgs e) {
			//
		}

		private void FrmZInfoAssembly_FormClosed(object sender, FormClosedEventArgs e) {
			//
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

		private void cboAssembly_SelectedIndexChanged(object sender, EventArgs e) {
			RefreshInfo();
		}

		private void cboMode_SelectedIndexChanged(object sender, EventArgs e) {
			RefreshInfo();
		}

		private void chkSort_CheckedChanged(object sender, EventArgs e) {
			InfoAssembly.IsSort = chkSort.Checked;
			RefreshInfo();
		}

	}
}