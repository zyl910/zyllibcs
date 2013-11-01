﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using zyllibcs.system;
using zyllibcs.text;

namespace zinfoassemblydata {
	/// <summary>
	/// 信息模式.
	/// </summary>
	public enum InfoMode {
		/// <summary>
		/// 程序集的属性.
		/// </summary>
		AssemblyProperty,
		/// <summary>
		/// 类型名
		/// </summary>
		TypeName,
		/// <summary>
		/// 类型全名.
		/// </summary>
		TypeNameFull,
		/// <summary>
		/// 类型的静态属性.
		/// </summary>
		TypeStaticProperty,
		/// <summary>
		/// 类型的静态属性及方法.
		/// </summary>
		TypeStaticPropertyMethod,
	}

	/// <summary>
	/// 程序集信息.
	/// </summary>
	public static class InfoAssembly {
		/// <summary>
		/// 程序集列表.
		/// </summary>
		private static List<string> m_AssemblyList = null;

		/// <summary>
		/// 程序集列表.
		/// </summary>
		public static List<string> AssemblyList {
			get {
				if (null == m_AssemblyList) m_AssemblyList = GacUtil.GacGetAssemblyNameList(ASM_DISPLAY_FLAGS.ASM_DISPLAYF_FULL, true);
				return m_AssemblyList;
			}
		}

		/// <summary>
		/// 根据名称加载程序集.
		/// </summary>
		/// <param name="name">程序集全名.</param>
		/// <returns>返回程序集.</returns>
		public static Assembly LoadAssembly(string name) {
			AssemblyName an = new AssemblyName(name);
			return Assembly.Load(an);
		}

		/// <summary>
		/// 输出信息.
		/// </summary>
		/// <param name="iw">输出者.</param>
		/// <param name="context">输出时的环境.</param>
		/// <param name="assembly">程序集.</param>
		/// <param name="mode">模式.</param>
		/// <returns>是否成功.</returns>
		public static bool WriteInfo(IIndentedWriter iw, IndentedWriterContext context, Assembly assembly, InfoMode mode) {
			bool rt = false;
			switch (mode) {
				case InfoMode.AssemblyProperty:
					rt = WriteInfo_AssemblyProperty(iw, context, assembly);
					break;
				case InfoMode.TypeName:
					rt = WriteInfo_TypeName(iw, context, assembly, false);
					break;
				case InfoMode.TypeNameFull:
					rt = WriteInfo_TypeName(iw, context, assembly, true);
					break;
				case InfoMode.TypeStaticProperty:
					break;
				case InfoMode.TypeStaticPropertyMethod:
					break;
			}
			return rt;
		}

		/// <summary>
		/// 输出信息_程序集的属性.
		/// </summary>
		/// <param name="iw">输出者.</param>
		/// <param name="context">输出时的环境.</param>
		/// <param name="assembly">程序集.</param>
		/// <returns>是否成功.</returns>
		private static bool WriteInfo_AssemblyProperty(IIndentedWriter iw, IndentedWriterContext context, Assembly assembly) {
			bool rt = false;
			if (null == iw) return false;
			if (null == assembly) return false;
			iw.WriteLine("Assembly:");
			IndentedObjectFunctor.CommonProc(iw, assembly, context);
			return rt;
		}

		/// <summary>
		/// 输出信息_类型名.
		/// </summary>
		/// <param name="iw">输出者.</param>
		/// <param name="context">输出时的环境.</param>
		/// <param name="assembly">程序集.</param>
		/// <param name="isfull">是否显示全名.</param>
		/// <returns>是否成功.</returns>
		private static bool WriteInfo_TypeName(IIndentedWriter iw, IndentedWriterContext context, Assembly assembly, bool isfull) {
			bool rt = false;
			if (null == iw) return false;
			if (null == assembly) return false;
			// 获取类型列表.
			IEnumerable<Type> lst = TypeUtil.GetExportedTypes(assembly);
			// 枚举类型.
			foreach (Type tp in lst) {
				string tpname = tp.Name;
				if (isfull) tpname = tp.FullName;
				iw.WriteLine(tpname);
			}
			return rt;
		}

	}
}
