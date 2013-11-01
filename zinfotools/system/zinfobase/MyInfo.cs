using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using zyllibcs.text;

namespace zinfobase {
	/// <summary>
	/// My info: Base(System) infos.
	/// </summary>
	public static class MyInfo {
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
			iw.WriteLine("# zinfobase");
#if (!NETFX_CORE && !NETFX_PORTABLE)
			iw.WriteLine("AppDomain:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(AppDomain), context);
			iw.WriteLine("Console:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Console), context);
#endif
			iw.WriteLine("GC:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(GC), context);
			iw.WriteLine("Math:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(Math), context);
#if (!NETFX_CORE && !NETFX_PORTABLE)
			iw.WriteLine("TimeZone:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(TimeZone), context);
#endif
#if (DOTNET3A)
			iw.WriteLine("TimeZoneInfo:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(TimeZoneInfo), context);
#endif
			iw.WriteLine("Uri:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(Uri), context);
			iw.WriteLine("DateTime:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(DateTime), context);
			iw.WriteLine("DateTimeOffset:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(DateTimeOffset), context);
			iw.WriteLine("TimeSpan:"); IndentedWriterUtil.WriteTypeStaticM(iw, typeof(TimeSpan), context);
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
