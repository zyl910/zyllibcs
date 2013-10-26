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
		/// �������_������Ϣ_����.
		/// </summary>
		/// <param name="iw">�����������.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>�����Ƿ�ɹ����.</returns>
		public static bool outl_EncodingInfo_Array(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == obj) return false;
			IEnumerable<EncodingInfo> lst = obj as IEnumerable<EncodingInfo>;
			if (null == lst) return false;
			if (null == iw) return true;
			Type tp = obj.GetType();
			if (!iw.Indent(tp)) return false;
			iw.WriteLine(string.Format("# <{0}>", tp.FullName));
			foreach (EncodingInfo p in lst) {
				iw.WriteLine("{1}:\t{0}\t# 0x{0:X}, {2}", p.CodePage, p.Name, p.DisplayName);
			}
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// �������_�Ļ���Ϣ.
		/// </summary>
		/// <param name="iw">�����������.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>�����Ƿ�ɹ����.</returns>
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
								// ��ʾ�̸�ʽ.
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
								// ����.
							}
							finally {
								iw.Unindent();
							}
							// ��ʾ��ϸ.
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
		/// �������_����.
		/// </summary>
		/// <param name="iw">�����������.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>�����Ƿ�ɹ����.</returns>
		public static bool outl_static_Encoding(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == iw) return false;
			Type tp = typeof(Encoding);
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
					else if (IndentedWriterUtil.StringComparer.Equals(name, "GetEncodings")) {
						if (cntparam == 0) {
							e.IsCancel = true;
							IndentedWriterUtil.WriteLineValue(iw, e.MemberName, e.Value, e.ValueOptions, e.AppendComment);
							object lst = null;
							try {
								lst = memberinfo.Invoke(null, null);
							}
							catch {
								// ����.
							}
							finally {
							}
							if (null != lst) {
								outl_EncodingInfo_Array(iw, lst, context);
							}
						}
					}
				}
			}, context);
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// �������_������.
		/// </summary>
		/// <param name="iw">�����������.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>�����Ƿ�ɹ����.</returns>
		public static bool outl_main(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == iw) return false;
			iw.WriteLine("# zinfoenvironment");
			// CharUnicodeInfo: ������, ���ʺ����.
			// CompareInfo: �ƺ���Ҫö���Ļ�, �ݲ�����.
			// CultureInfo:
			iw.WriteLine("CultureInfo:"); outl_static_CultureInfo(iw, null, context);
			// DateTimeFormatInfo:
			iw.WriteLine("DateTimeFormatInfo:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(DateTimeFormatInfo), context);
			// DateTimeFormatInfo:
			iw.WriteLine("NumberFormatInfo:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(NumberFormatInfo), context);
			// RegionInfo :
			iw.WriteLine("RegionInfo:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(RegionInfo), context);
			// StringInfo: ������, ���ʺ����.
			// TextElementEnumerator: ���ʺ����.
			// TextInfo: ���ʺ����.
			// Calendar: ��������.
			iw.WriteLine("Calendar:"); IndentedWriterUtil.WriteDerivedClass(iw, typeof(Calendar), null, context);
			// Encoding:
			iw.WriteLine("Encoding:"); outl_static_Encoding(iw, null, context);
			//IndentedWriterUtil.WriteTypeStatic(iw, typeof(Encoding), context);
			return true;
		}
	}
}
