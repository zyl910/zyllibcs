using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using zyllibcs.text;

namespace zinfoenvironment {
	/// <summary>
	/// My info.
	/// </summary>
	public static class MyInfo {

		/// <summary>
		/// 输出多行_环境.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="context">Context Object.</param>
		public static bool outl_Environment(IIndentedWriter iw, IndentedWriterContext context) {
			if (null == iw) return false;
			Type tp = typeof(System.Environment);
			if (!iw.Indent(tp)) return false;
			iw.WriteLine(string.Format("# <{0}>", tp.FullName));
			IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod | IndentedWriterMemberOptions.OnlyStatic;
			IndentedWriterUtil.ForEachMember(iw, null, tp, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
				// http://msdn.microsoft.com/zh-cn/library/system.environment(v=vs.110).aspx
				MethodInfo memberinfo = e.MemberInfo as MethodInfo;
				if (null != memberinfo) {
					//if (!memberinfo.IsSpecialName) {
					//    e.HasDefault = true;
					//}
					string name = memberinfo.Name;
					int cntparam = memberinfo.GetParameters().Length;
					if (false) {
					}
#if (!NETFX_CORE)
					else if (IndentedWriterUtil.StringComparer.Equals(name, "GetFolderPath") && cntparam <= 1) {
						e.IsCancel = true;
						IndentedWriterUtil.WriteLineValue(iw, e.MemberName, e.Value, e.ValueOptions, e.AppendComment);
						object[] args = new object[1];
						iw.Indent(null);
						foreach (Environment.SpecialFolder p in Enum.GetValues(typeof(Environment.SpecialFolder))) {
							iw.Write("{0:d}(0x{0:X}, {0}):\t", p);
							try {
								//string s = Environment.GetFolderPath(p);
								args[0] = p;
								string s = (string)memberinfo.Invoke(null, args);
								iw.Write(s);
							}
							catch {
							}
							iw.WriteLine();
						}
						iw.Unindent();
					}
#endif
					else if (IndentedWriterUtil.StringComparer.Equals(name, "GetLogicalDrives")) {
						try {
							//string[] arr = Environment.GetLogicalDrives();
							string[] arr = (string[])memberinfo.Invoke(null, null);
							e.Value = string.Join(", ", arr);
							e.HasDefault = true;
						}
						catch {
							// 忽略.
						}
					}
					else if (IndentedWriterUtil.StringComparer.Equals(name, "GetEnvironmentVariables")) {
						if (cntparam <= 0) {
							e.IsCancel = true;
							IndentedWriterUtil.WriteLineValue(iw, e.MemberName, e.Value, e.ValueOptions, e.AppendComment);
							iw.Indent(null);
							try {
								//IDictionary environmentVariables = Environment.GetEnvironmentVariables();
								IDictionary environmentVariables = memberinfo.Invoke(null, null) as IDictionary;
								List<object> lst = new List<object>();
								foreach (object k in environmentVariables.Keys) {
									lst.Add(k);
								}
								lst.Sort();
								foreach (object k in lst) {
									iw.WriteLine("{0}:\t{1}", k, environmentVariables[k]);
								}
							}
							catch {
								// 忽略.
							}
							finally {
								iw.Unindent();
							}
						}
					}
				}
			}, context);
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// 输出多行_IntPtr.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="context">Context Object.</param>
		public static bool outl_static_IntPtr(IIndentedWriter iw, IndentedWriterContext context) {
			if (null == iw) return false;
			Type tp = typeof(System.IntPtr);
			if (!iw.Indent(tp)) return false;
			iw.WriteLine(string.Format("# <{0}>", tp.FullName));
#if UNSAFE
			unsafe {
				iw.WriteLine(string.Format("# sizeof:\t{0}", sizeof(System.IntPtr)));
			}
#endif
			IndentedWriterUtil.ForEachMember(iw, null, tp, IndentedWriterMemberOptions.OnlyStatic, null, context);
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// 输出多行_主函数.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		public static void outl_main(IIndentedWriter iw) {
			// test.
			//iw.WriteLine("Hello!");
			//for (int i = 0; i < 3; ++i) {
			//    iw.WriteLine(i);
			//    iw.Indent();
			//}
			//iw.IndentLevel = 0;
			//IndentedWriterObjectLnProc lwo = outl_Version;
			//IndentedWriterObjectLnProc[] lwos = new IndentedWriterObjectLnProc[] { lwo };
			//IEnumerable<IndentedWriterObjectLnProc> lwoe = lwos;
			//foreach (IndentedWriterObjectLnProc p in lwoe) {
			//    p(iw, "");
			//}
			//lwo(iw, "");
			//lwo.Invoke(iw, "");
			//IndentedWriterUtil.WriteLineValue(iw, "enum", IndentedWriterValueOptions.ExistName, IndentedWriterValueOptions.Default, null);
			//IndentedWriterUtil.WriteLineValue(iw, "char", 'A', IndentedWriterValueOptions.Default, null);
			//IndentedWriterUtil.WriteLineValue(iw, "int", 255, IndentedWriterValueOptions.Default, null);
			//IndentedWriterUtil.WriteLineValue(iw, "string", Environment.NewLine, IndentedWriterValueOptions.Default, null);
			//IndentedWriterUtil.WriteLineValue(iw, "char(2)", '\t', IndentedWriterValueOptions.Default, null);
			//IndentedWriterUtil.WriteLineValue(iw, "string(2)", "\\\'\"", IndentedWriterValueOptions.Default, null);
			// root.
			iw.WriteLine("# zinfoenvironment");
			iw.WriteLine("Environment:");
			//decimal? dec = new decimal(1);
			//IndentedWriterUtil.WriteLineValue(iw, "dec", dec, IndentedWriterValueOptions.Default);
			//IndentedObjectFunctor.CommonProc(iw, dec);
			// Environment.
			//IndentedObjectFunctor.CommonProc(iw, Environment.OSVersion, null);
			//outl_Environment(iw);
			outl_Environment(iw, null);
			iw.WriteLine("IntPtr:");
			outl_static_IntPtr(iw, null);
		}

	}
}
