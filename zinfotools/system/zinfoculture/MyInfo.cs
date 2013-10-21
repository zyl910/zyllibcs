using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Reflection;
using zyllibcs.text;

namespace zinfoculture {
	/// <summary>
	/// My info: Culture(System.Globalization) infos.
	/// </summary>
	public static class MyInfo {
		public static bool ShowDetail = false;

		/// <summary>
		/// 输出多行_文化信息.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool outl_static_CultureInfo(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == iw) return false;
			Type tp = typeof(CultureInfo);
			if (!iw.Indent(tp)) return false;
			iw.WriteLine(string.Format("# <{0}>", tp.FullName));
			IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod | IndentedWriterMemberOptions.OnlyStatic;
			IndentedWriterUtil.ForEachMember(iw, null, tp, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
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
					else if (IndentedWriterUtil.StringComparer.Equals(name, "GetCultures")) {
						if (cntparam == 1) {
							e.IsCancel = true;
							IndentedWriterUtil.WriteLineValue(iw, e.MemberName, e.Value, e.ValueOptions, e.AppendComment);
							CultureInfo[] lst = null;
							iw.Indent(null);
							try {
								object[] args = new object[1];
								args[0] = CultureTypes.AllCultures;
								lst = (CultureInfo[])memberinfo.Invoke(null, args);
								// 显示短格式.
								foreach (CultureInfo p in lst) {
									int lcid = -1;
									iw.Write("{0}:\t", p.Name);
#if (!NETFX_PORTABLE)
									lcid = p.LCID;
#endif
									if (lcid != -1) {
										iw.Write("0x{0:X}", lcid);
									}
									iw.WriteLine("\t# {0}, {1}", p.EnglishName, p.NativeName);
								}
							}
							catch {
								// 忽略.
							}
							finally {
								iw.Unindent();
							}
							// 显示详细.
							if (ShowDetail) {
								IndentedWriterUtil.WriteLineValue(iw, e.MemberName, e.Value, e.ValueOptions, "Detail");
								IndentedObjectFunctor.CommonProc(iw, lst, context);
							}
						}
					}
#endif
//#if (!NETFX_CORE)
//                    else if (IndentedWriterUtil.StringComparer.Equals(name, "GetFolderPath") && cntparam <= 1) {
//                        e.IsCancel = true;
//                        IndentedWriterUtil.WriteLineValue(iw, e.MemberName, e.Value, e.ValueOptions, e.AppendComment);
//                        object[] args = new object[1];
//                        iw.Indent(null);
//                        foreach (Environment.SpecialFolder p in Enum.GetValues(typeof(Environment.SpecialFolder))) {
//                            iw.Write("{0:d}(0x{0:X}, {0}):\t", p);
//                            try {
//                                //string s = Environment.GetFolderPath(p);
//                                args[0] = p;
//                                string s = (string)memberinfo.Invoke(null, args);
//                                iw.Write(s);
//                            }
//                            catch {
//                            }
//                            iw.WriteLine();
//                        }
//                        iw.Unindent();
//                    }
//#endif
//                    else if (IndentedWriterUtil.StringComparer.Equals(name, "GetLogicalDrives")) {
//                        try {
//                            //string[] arr = Environment.GetLogicalDrives();
//                            string[] arr = (string[])memberinfo.Invoke(null, null);
//                            e.Value = string.Join(", ", arr);
//                            e.HasDefault = true;
//                        }
//                        catch {
//                            // 忽略.
//                        }
//                    }
//                    else if (IndentedWriterUtil.StringComparer.Equals(name, "GetEnvironmentVariables")) {
//                        if (cntparam <= 0) {
//                            e.IsCancel = true;
//                            IndentedWriterUtil.WriteLineValue(iw, e.MemberName, e.Value, e.ValueOptions, e.AppendComment);
//                            iw.Indent(null);
//                            try {
//                                //IDictionary environmentVariables = Environment.GetEnvironmentVariables();
//                                IDictionary environmentVariables = memberinfo.Invoke(null, null) as IDictionary;
//                                List<object> lst = new List<object>();
//                                foreach (object k in environmentVariables.Keys) {
//                                    lst.Add(k);
//                                }
//                                lst.Sort();
//                                foreach (object k in lst) {
//                                    iw.WriteLine("{0}:\t{1}", k, environmentVariables[k]);
//                                }
//                            }
//                            catch {
//                                // 忽略.
//                            }
//                            finally {
//                                iw.Unindent();
//                            }
//                        }
//                    }
				}
			}, context);
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// 输出多行_主函数.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool outl_main(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == iw) return false;
			iw.WriteLine("# zinfoenvironment");
			// Calendar: 各种日历.
			// CharUnicodeInfo: 不适合输出.
			// CompareInfo: 似乎需要枚举文件, 以后考虑.
			// CultureInfo
			iw.WriteLine("CultureInfo:");
			outl_static_CultureInfo(iw, null, context);
			//
			//iw.WriteLine("Environment:");
			////IndentedObjectFunctor.CommonProc(iw, Environment.OSVersion, context);
			//outl_Environment(iw, null, context);
			//iw.WriteLine("Application AssemblyName:");
			//IndentedObjectFunctor.CommonProc(iw, myAssembly.GetName(), context);
			//iw.WriteLine("IntPtr:");
			//outl_static_IntPtr(iw, null, context);
			return true;
		}
	}
}
