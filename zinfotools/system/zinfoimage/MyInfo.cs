using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using zyllibcs.text;
using System.Reflection;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace zinfoimage {
    /// <summary>
    /// My info: Image infos.
    /// </summary>
    class MyInfo {
        /// <summary>File path.</summary>
        public static string FilePath { get; set; }
        /// <summary>Show bytes.</summary>
        public static bool ShowBytes { get; set; }

        /// <summary>
        /// 输出多行_图像编码参数.
        /// </summary>
        /// <param name="iw">带缩进输出者.</param>
        /// <param name="obj">object. Can be null.</param>
        /// <param name="context">State Object. Can be null.</param>
        /// <returns>返回是否成功输出.</returns>
        public static bool outl_GetEncoderParameterList(IIndentedWriter iw, Image obj, IndentedWriterContext context) {
            if (null == iw) return false;
            ImageCodecInfo[] arr = ImageCodecInfo.GetImageEncoders();
            Type tp = arr.GetType();
            if (!iw.Indent(arr)) return false;
            //System.Drawing.Imaging.Encoder;
            //iw.WriteLine(string.Format("# <{0}>", "GetImageEncoders"));
            iw.WriteLine(string.Format("# <{0}>", tp.FullName));
            iw.WriteLine(string.Format("# Count: {0}", arr.Length));
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
                    EncoderParameters encoderParameters = obj.GetEncoderParameterList(cur.Clsid);
                    iw.WriteLine(string.Format("GetEncoderParameterList():\t{0}", encoderParameters));
                    if (null != encoderParameters && null != encoderParameters.Param) {
                        iw.Indent(encoderParameters);
                        IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod;
                        IndentedWriterUtil.ForEachMember(iw, encoderParameters, null, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
                            //Debug.WriteLine(e.MemberName);
                        }, context);
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
        /// 输出多行_Image.
        /// </summary>
        /// <param name="iw">带缩进输出者.</param>
        /// <param name="obj">object. Can be null.</param>
        /// <param name="context">State Object. Can be null.</param>
        /// <returns>返回是否成功输出.</returns>
        private static bool outl_Image(IIndentedWriter iw, Image obj, IndentedWriterContext context) {
            if (null == obj) return false;
            if (null == iw) return false;
            Type tp = obj.GetType();
            if (!iw.Indent(obj)) return false;
            iw.WriteLine(string.Format("# <{0}>", tp.FullName));
            IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod;
            IndentedWriterUtil.ForEachMember(iw, obj, tp, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
                //Debug.WriteLine(e.MemberName);
                if (IndentedWriterUtil.StringComparer.Equals(e.MemberName, "PropertyItems")) {
                    if (!ShowBytes) {
                        e.IsCancel = true;
                    }
                }
            }, context);
            iw.Unindent();
            return true;
        }

        /// <summary>
        /// 输出多行_主函数_内部.
        /// </summary>
        /// <param name="iw">带缩进输出者.</param>
        /// <param name="obj">object. Can be null.</param>
        /// <param name="context">State Object. Can be null.</param>
        /// <returns>返回是否成功输出.</returns>
        private static bool outl_main_core(IIndentedWriter iw, object obj, IndentedWriterContext context) {
            if (null == iw) return false;
            IndentedWriterUtil.AddEnumType(typeof(System.Drawing.Imaging.Encoder));
            IndentedWriterUtil.AddEnumType(typeof(System.Drawing.Imaging.FrameDimension));
            IndentedWriterUtil.AddEnumType(typeof(System.Drawing.Imaging.ImageFormat));
            // body.
            iw.WriteLine("# zinfoimage");
            iw.WriteLine("Syntax: zinfoimage <FilePath>");
            iw.WriteLine("---");
            iw.WriteLine("FilePath:\t{0}", FilePath);
            if (string.IsNullOrEmpty(FilePath)) {
                iw.WriteLine("FilePath is empty!");
                return true;
            }
            Image image = null;
            try {
                image = Image.FromFile(FilePath);
            } catch (Exception ex) {
                iw.WriteLine(ex);
            }
            if (null == image) return true;
            iw.WriteLine("Image.FromFile done.");
            iw.WriteLine("Image:");
            outl_Image(iw, image, context);
            // GetImageEncoders.
            iw.WriteLine("ImageCodecInfo.GetImageEncoders():");
            outl_GetEncoderParameterList(iw, image, context);
            // Others.
            iw.WriteLine("Encoder:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Encoder), context);
            iw.WriteLine("FrameDimension:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(FrameDimension), context);
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
            return outl_main_core(iw, obj, context);
        }
    }
}
