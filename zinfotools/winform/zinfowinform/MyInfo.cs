#if (!NETFX_CORE && !NETFX_PORTABLE)
using System;
using System.Collections.Generic;
//using System.Text;
using System.Reflection;
using zyllibcs.text;

using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace zinfowinform {
	/// <summary>
	/// My info: System.Windows.Forms infos.
	/// </summary>
	public static class MyInfo {

		/// <summary>
		/// 输出多行_静态_OS特性.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool outl_static_OSFeature(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == iw) return false;
			Type tp = typeof(OSFeature);
			if (!iw.Indent(tp)) return false;
			iw.WriteLine(string.Format("# <{0}>", tp.FullName));
			IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod | IndentedWriterMemberOptions.OnlyStatic;
			IndentedWriterUtil.ForEachMember(iw, null, tp, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
				// FieldInfo.
				FieldInfo fi = e.MemberInfo as FieldInfo;
				if (null != fi) {
					// GetVersionPresent.
					if (null != e.Value && fi.FieldType.Equals(typeof(object))) {
						Version ver = null;
						try {
							ver = OSFeature.Feature.GetVersionPresent(e.Value);
						}
						catch {
							// 忽略.
						}
						if (null != ver) {
							IndentedWriterUtil.WriteLineValue(iw, string.Format("GetVersionPresent({1}.{0})", fi.Name, fi.DeclaringType.Name), ver, e.ValueOptions, null);
							return;
						}
					}
				}
				// MethodInfo.
				MethodInfo memberinfo = e.MemberInfo as MethodInfo;
				if (null != memberinfo) {
					if (IndentedWriterUtil.WriteSimpleMethod(iw, memberinfo, null, null, IndentedWriterValueOptions.AutoHideValue, e.AppendComment)) {
						e.IsCancel = true;
						return;
					}
				}
			}, context);
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// 输出多行_主函数_内部.
		/// </summary>
		/// <param name="isfull">显示全部信息.</param>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		private static bool outl_main_core(bool isfull, IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == iw) return false;
			if (null == context) context = new IndentedWriterContext();
			if (null == context.Procs) {
				List<IndentedWriterObjectProc> lst = new List<IndentedWriterObjectProc>();
				lst.Add((new IndentedObjectFunctor(typeof(Form), IndentedObjectFunctorOptions.NotWrite)).WriterObject);
				context.Procs = lst;
			}
			iw.WriteLine("# zinfowinform");
			iw.WriteLine("Application:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Application), context);
			iw.WriteLine("ButtonRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(ButtonRenderer), context);
			iw.WriteLine("CheckBoxRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(CheckBoxRenderer), context);
			iw.WriteLine("Clipboard:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(Clipboard), context);
			iw.WriteLine("ComboBoxRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(ComboBoxRenderer), context);
			iw.WriteLine("ControlPaint:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(ControlPaint), context);
			iw.WriteLine("Cursor:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Cursor), context);
			iw.WriteLine("Cursors:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Cursors), context);
			iw.WriteLine("DataFormats:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(DataFormats), context);
			// FeatureSupport: 见 OSFeature.
			iw.WriteLine("GroupBoxRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(GroupBoxRenderer), context);
			iw.WriteLine("InputLanguage:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(InputLanguage), context);
			iw.WriteLine("OSFeature:"); outl_static_OSFeature(iw, null, context);
			iw.WriteLine("ProfessionalColors:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(ProfessionalColors), context);
			iw.WriteLine("ProgressBarRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(ProgressBarRenderer), context);
			iw.WriteLine("RadioButtonRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(RadioButtonRenderer), context);
			iw.WriteLine("Screen:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Screen), context);
			iw.WriteLine("ScrollBarRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(ScrollBarRenderer), context);
			iw.WriteLine("SystemInformation:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(SystemInformation), context);
			iw.WriteLine("TabRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(TabRenderer), context);
			iw.WriteLine("TextBoxRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(TextBoxRenderer), context);
			iw.WriteLine("TrackBarRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(TrackBarRenderer), context);
			iw.WriteLine("VisualStyleInformation:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(VisualStyleInformation), context);
			iw.WriteLine("VisualStyleRenderer:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(VisualStyleRenderer), context);
			// TextRenderer: 无信息成员.
			//if (isfull) {
			//    iw.WriteLine("ProfessionalColors:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(ProfessionalColors), context);
			//}
			//iw.WriteLine("ImageCodecInfo:"); outl_static_ImageCodecInfo(iw, null, context);
			//iw.WriteLine("ImageFormat:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(ImageFormat), context);
			//iw.WriteLine("InstalledFontCollection:"); IndentedObjectFunctor.CommonProc(iw, new InstalledFontCollection(), context);	//IndentedWriterUtil.WriteTypeStatic(iw, typeof(InstalledFontCollection), context);
			return true;
		}

		/// <summary>
		/// 输出多行_主函数. 不会枚举 颜色、画笔、刷子信息.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool outl_main(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			return outl_main_core(false, iw, obj, context);
		}

		/// <summary>
		/// 输出多行_主函数_全部.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		private static bool outl_main_full(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			return outl_main_core(true, iw, obj, context);
		}

	}
}

#endif
