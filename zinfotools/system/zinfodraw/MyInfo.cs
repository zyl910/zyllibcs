﻿#if (!NETFX_CORE && !NETFX_PORTABLE)
using System;
using System.Collections.Generic;
//using System.Text;
using System.Reflection;
using zyllibcs.text;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Diagnostics;

namespace zinfodraw {
	/// <summary>
	/// My info: System.Drawing infos.
	/// </summary>
	public static class MyInfo {

        /// <summary>
        /// 输出多行_图像编码参数.
        /// </summary>
        /// <param name="iw">带缩进输出者.</param>
        /// <param name="obj">object. Can be null.</param>
        /// <param name="context">State Object. Can be null.</param>
        /// <returns>返回是否成功输出.</returns>
        public static bool outl_static_GetEncoderParameterList(IIndentedWriter iw, object obj, IndentedWriterContext context) {
            if (null == iw) return false;
            Type tp = typeof(Bitmap);
            if (!iw.Indent(tp)) return false;
            iw.WriteLine(string.Format("# <{0}>", "GetEncoderParameterList"));
            Bitmap bm = new Bitmap(64, 64, PixelFormat.Format24bppRgb);
            ImageCodecInfo[] arr = ImageCodecInfo.GetImageEncoders();
            iw.WriteLine(string.Format("Count: {0}", arr.Length));
            for (int i = 0; i < arr.Length; ++i) {
                ImageCodecInfo cur = arr[i];
                iw.WriteLine(string.Format("[{0}]:\t{1}\t # <{2}>", i, cur, cur.GetType().Name));
                iw.Indent(cur);
                iw.WriteLine(string.Format("Clsid:\t{0}", cur.Clsid));
                iw.WriteLine(string.Format("CodecName:\t{0}", cur.CodecName));
                iw.WriteLine(string.Format("DllName:\t{0}", cur.DllName));
                iw.WriteLine(string.Format("FilenameExtension:\t{0}", cur.FilenameExtension));
                iw.WriteLine(string.Format("FormatDescription:\t{0}", cur.FormatDescription));
                iw.WriteLine(string.Format("FormatID:\t{0}", cur.FormatID));
                iw.WriteLine(string.Format("MimeType:\t{0}", cur.MimeType));
                iw.WriteLine(string.Format("Version:\t{0}", cur.Version));
                // GetImageDecoders.
                try {
                    EncoderParameters encoderParameters = bm.GetEncoderParameterList(cur.Clsid);
                    iw.WriteLine(string.Format("GetEncoderParameterList():\t{0}", encoderParameters));
                    if (null != encoderParameters && null != encoderParameters.Param) {
                        EncoderParameter[] list = encoderParameters.Param;
                        iw.Indent(list);
                        iw.WriteLine(string.Format("# Count: {0}", list.Length));
                        //for (int j = 0; j < list.Length; ++j) {
                        //}
                        IndentedWriterUtil.WriteLineValue(iw, "Value:", list, 0, null);
                        iw.Unindent();
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(cur.CodecName);
                    Debug.WriteLine(ex);
                    iw.WriteLine(string.Format("GetEncoderParameterList():\t#{0}", ex.Message));
                }
                // done.
                iw.Unindent();
            }
            iw.Unindent();
            return true;
        }

		/// <summary>
		/// 输出多行_图像编码信息.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. Can be null.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool outl_static_ImageCodecInfo(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			if (null == iw) return false;
			Type tp = typeof(ImageCodecInfo);
			if (!iw.Indent(tp)) return false;
			iw.WriteLine(string.Format("# <{0}>", tp.FullName));
			IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod | IndentedWriterMemberOptions.OnlyStatic;
			IndentedWriterUtil.ForEachMember(iw, null, tp, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
				MethodInfo memberinfo = e.MemberInfo as MethodInfo;
				if (null != memberinfo) {
					string name = memberinfo.Name;
					int cntparam = memberinfo.GetParameters().Length;
					if (false) {
					}
					else if (IndentedWriterUtil.StringComparer.Equals(name, "GetImageDecoders")
						|| IndentedWriterUtil.StringComparer.Equals(name, "GetImageEncoders")) {
						if (cntparam == 0) {
							try {
								e.Value = memberinfo.Invoke(null, null);
								if (null == e.WriteProc) e.WriteProc = IndentedObjectFunctor.CommonProc;
								e.HasDefault = true;
							}
							catch {
								// 忽略.
							}
						}
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
			iw.WriteLine("# zinfodraw");
			if (isfull) {
				iw.WriteLine("Brushes:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Brushes), context);
			}
			iw.WriteLine("BufferedGraphicsManager:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(BufferedGraphicsManager), context);
			// Font: 无静态属性. 可用 SystemFonts 枚举 .
			iw.WriteLine("FontFamily:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(FontFamily), context);
			if (isfull) {
				iw.WriteLine("Pens:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Pens), context);
				iw.WriteLine("StringFormat:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(StringFormat), context);
				iw.WriteLine("SystemBrushes:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(SystemBrushes), context);
				iw.WriteLine("SystemColors:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(SystemColors), context);
			}
			iw.WriteLine("SystemFonts:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(SystemFonts), context);
			iw.WriteLine("SystemIcons:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(SystemIcons), context);
			if (isfull) {
				iw.WriteLine("SystemPens:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(SystemPens), context);
				iw.WriteLine("Color:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Color), context);
			}
			iw.WriteLine("Encoder:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Encoder), context);
			iw.WriteLine("FrameDimension:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(FrameDimension), context);
			iw.WriteLine("ImageCodecInfo:"); outl_static_ImageCodecInfo(iw, null, context);
			iw.WriteLine("ImageFormat:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(ImageFormat), context);
            if (isfull) {
                iw.WriteLine("InstalledFontCollection:"); IndentedObjectFunctor.CommonProc(iw, new InstalledFontCollection(), context);	//IndentedWriterUtil.WriteTypeStatic(iw, typeof(InstalledFontCollection), context);
            }
			iw.WriteLine("ImageCodecInfo:"); outl_static_GetEncoderParameterList(iw, null, context);
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
		public static bool outl_main_full(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			return outl_main_core(true, iw, obj, context);
		}

	}
}

#endif
