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
		/// 输出多行_日历_列表.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool outl_static_Calendar_List(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == iw) return false;
			Type typeCalendar = typeof(Calendar);
#if (NETFX_CORE)
			TypeInfo tiCalendar = typeCalendar.GetTypeInfo();
#endif
			Assembly assemblyCalendar = typeCalendar.Assembly;
			if (!iw.Indent(typeCalendar)) return false;
			int cnt = 0;
			IEnumerable<Type> lst = null;
#if (NETFX_CORE)
			lst = assemblyCalendar.ExportedTypes;
#else
			lst = assemblyCalendar.GetExportedTypes();
#endif
			foreach (Type tp in assemblyCalendar.GetTypes()) {
				// check.
#if (NETFX_CORE)
				TypeInfo ti = tp.GetTypeInfo();
				if (!ti.IsAssignableFrom(tiCalendar)) continue;
#else
				if (tp.IsAbstract) continue;
				if (!tp.IsSubclassOf(typeCalendar)) continue;
#endif
				// new.
				object ob = null;
				try {
					ob = Activator.CreateInstance(tp);
				}
				catch {
					// 忽略.
				}
				if (null == ob) continue;
				// write.
				iw.WriteLine("[{0}]:\t# <{1}>", cnt, tp.Name);
				IndentedObjectFunctor.CommonProc(iw, ob, context);
				++cnt;
			}
			iw.Unindent();
			return true;
		}

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
			// CharUnicodeInfo: 工具类, 不适合输出.
			// CompareInfo: 似乎需要枚举文化, 暂不考虑.
			// CultureInfo:
			iw.WriteLine("CultureInfo:"); outl_static_CultureInfo(iw, null, context);
			// DateTimeFormatInfo:
			iw.WriteLine("DateTimeFormatInfo:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(DateTimeFormatInfo), context);
			// DateTimeFormatInfo:
			iw.WriteLine("NumberFormatInfo:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(NumberFormatInfo), context);
			// RegionInfo :
			iw.WriteLine("RegionInfo:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(RegionInfo), context);
			// StringInfo: 工具类, 不适合输出.
			// TextElementEnumerator: 不适合输出.
			// TextInfo: 不适合输出.
			// Calendar: 各种日历.
			iw.WriteLine("Calendar:"); outl_static_Calendar_List(iw, null, context);
			return true;
		}
	}
}
