using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
//using System.Windows.Forms;
using zyllibcs.system;
using zyllibcs.text;
using zinfoassembly;

namespace zinfoassemblyp {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		/// <summary>
		/// 旧的程序集名称.
		/// </summary>
		private string m_OldAssemblyName = null;

		/// <summary>
		/// 当前程序集.
		/// </summary>
		private Assembly m_CurAssembly = null;

		/// <summary>
		/// 构造 MainWindow.
		/// </summary>
		public MainWindow() {
			InitializeComponent();
		}


		/// <summary>
		/// 刷新模式列表.
		/// </summary>
		private void RefreshModeList() {
			cboMode.Items.Clear();
			// mode.
			foreach (InfoMode p in Enum.GetValues(typeof(InfoMode))) {
				cboMode.Items.Add(p);
			}
			// static type.
			cboMode.Items.Add("---");
			if (null != m_CurAssembly) {
				IndentedWriterMemberOptions options = IndentedWriterMemberOptions.OnlyStatic | IndentedWriterMemberOptions.AllowMethod;
				try {
					// 获取类型列表.
					IEnumerable<Type> lst = TypeUtil.GetExportedTypes(m_CurAssembly);
					if (true) {
						List<Type> lst1 = new List<Type>(lst);
						lst1.Sort(InfoAssembly.DefaultTypeNameComparer);
						lst = lst1;
					}
					// 枚举类型.
					foreach (Type tp in lst) {
						if (IndentedWriterUtil.TypeHasStatic(tp, options)) {
							cboMode.Items.Add(tp);
						}
					}
				}
				catch (Exception ex) {
					Debug.WriteLine(ex);
				}
			}
			// selectindex
			try {
				cboMode.SelectedIndex = 0;
			}
			catch {
				// 忽略.
			}
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
					RefreshModeList();
				}
				catch (Exception ex) {
					m_OldAssemblyName = null;
					txtInfo.Text = ex.ToString();
				}
			}
			if (null == m_CurAssembly) return;
			// 计算显示方式.
			InfoMode mode = 0;
			Type tp = null;
			bool isshow = false;
			object obj = cboMode.SelectedItem;
			if (obj is Type) {
				tp = obj as Type;
				isshow = true;
			}
			else if (obj is InfoMode) {
				mode = (InfoMode)obj;
				isshow = true;
			}
			if (!isshow) return;
			// 显示信息.
			StringBuilder sb = new StringBuilder();
			IIndentedWriter iw = new TextIndentedWriter(new StringWriter(sb));
			IndentedWriterContext context = new IndentedWriterContext();
			context.VisitOnce = (bool)chkVisitOnce.IsChecked;
			//this.UseWaitCursor = true;
			this.Cursor = Cursors.Wait;
			//Application.DoEvents();
			try {
				if (null != tp) {
					IndentedWriterMemberOptions options = IndentedWriterMemberOptions.OnlyStatic;
					if (chkMethod.IsChecked!=false) options |= IndentedWriterMemberOptions.AllowMethod;
					InfoAssembly.WriteTypeStatic(iw, context, tp, options);
				}
				else {
					InfoAssembly.WriteInfo(iw, context, m_CurAssembly, mode);
				}
			}
			catch (Exception ex) {
				sb.AppendLine(ex.ToString());
			}
			//this.UseWaitCursor = false;
			this.Cursor = null;
			txtInfo.Text = sb.ToString();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			// init.
			//dlgFont.Font = txtInfo.Font;
			cboAssembly.Items.Clear();
			try {
				foreach (string str in InfoAssembly.AssemblyList) {
					cboAssembly.Items.Add(str);
				}
			}
			catch (Exception ex) {
				txtInfo.Text = ex.ToString();
			}
			RefreshModeList();
		}

		private void Window_Unloaded(object sender, RoutedEventArgs e) {
			//
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			//
		}

		private void Window_Closed(object sender, EventArgs e) {
			//
		}

		private void btnFont_Click(object sender, RoutedEventArgs e) {
			// WPF字体 与 winform字体对话框并不完全兼容.
			//System.Windows.Forms.FontDialog dlgFont = new System.Windows.Forms.FontDialog();
			////dlgFont.Font = txtInfo.Font;
			//// show.
			//if (dlgFont.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
			//    //txtInfo.Font = dlgFont.Font;
			//}
		}

		private void chkWordWrap_Checked(object sender, RoutedEventArgs e) {
			txtInfo.TextWrapping = (chkWordWrap.IsChecked != false) ? TextWrapping.Wrap : TextWrapping.NoWrap;
		}

		private void btnFind_Click(object sender, RoutedEventArgs e) {
			string key = txtFind.Text;
			if (string.IsNullOrEmpty(key)) return;
			String sall = txtInfo.Text;
			if (string.IsNullOrEmpty(sall)) return;
			int start = txtInfo.SelectionStart + txtInfo.SelectionLength;
			if (start < 0) start = 0;
			if (start >= sall.Length) return;
			int p = sall.IndexOf(key, start, StringComparison.CurrentCultureIgnoreCase);
			if (p < 0) return;
			// 设置选区.
			try {
				txtInfo.Select(p, 0);	// 先滚动到起始位置.
				txtInfo.Select(p, key.Length);	// 在尝试选取区域.
			}
			catch (Exception ex) {
				Debug.WriteLine(ex);
			}
			// 设置焦点.
			try {
				txtInfo.Focus();
			}
			catch (Exception ex) {
				Debug.WriteLine(ex);
			}
		}

		private void btnSave_Click(object sender, RoutedEventArgs e) {
			System.Windows.Forms.SaveFileDialog dlgSave = new System.Windows.Forms.SaveFileDialog();
			dlgSave.Filter = "Text file(*.txt)|*.txt|All file(*.*)|*.*";
			if (dlgSave.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				try {
					File.WriteAllText(dlgSave.FileName, txtInfo.Text, Encoding.UTF8);
				}
				catch (Exception ex) {
					MessageBox.Show(this, ex.ToString(), "Save failed", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void cboAssembly_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			RefreshInfo();
		}

		private void cboMode_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			RefreshInfo();
		}

		private void chkMethod_Checked(object sender, RoutedEventArgs e) {
			RefreshInfo();
		}

		private void chkSort_Checked(object sender, RoutedEventArgs e) {
			InfoAssembly.IsSort = chkSort.IsChecked != false;
			RefreshInfo();
		}
	}
}
