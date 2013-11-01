using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using zyllibcs.system;
using zyllibcs.text;

namespace zinfoassemblylist {
	/// <summary>
	/// My info: Assembly List infos infos.
	/// </summary>
	public class MyInfo {
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
			ASM_DISPLAY_FLAGS flags = isfull ? ASM_DISPLAY_FLAGS.ASM_DISPLAYF_FULL : ASM_DISPLAY_FLAGS.ASM_DISPLAYF_RETARGET;
			//flags = ASM_DISPLAY_FLAGS.ASM_DISPLAYF_FULL;
			iw.WriteLine("# zinfoassemblylist");
			foreach (string str in GacUtil.GacGetAssemblyNameList(flags, true)) {
				iw.WriteLine(str);
			}
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
