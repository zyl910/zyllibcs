using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace zinfoassemblyf {
	static class Program {
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() {
			//处理未捕获的异常  
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			//处理UI线程异常  
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FrmZInfoAssembly());
		}

		/// <summary>
		/// 应用程序_UI异常.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
			Exception ex = e.Exception as Exception;
			string s = string.Format("Application ThreadError:{0}", e);
			if (ex != null) {
				s = string.Format("出现应用程序未处理的异常/n异常类型：{0}/n异常消息：{1}/n异常位置：{2}/n", ex.GetType().Name, ex.Message, ex.StackTrace);
			}
			MessageBox.Show(s, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}