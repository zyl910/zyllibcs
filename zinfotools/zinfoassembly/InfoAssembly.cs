using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using zyllibcs.system;
using zyllibcs.text;
#if (NETFX_CORE)
#endif

namespace zinfoassembly {
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
		/// 类型的字符串
		/// </summary>
		TypeString,
		/// <summary>
		/// 拥有静态属性的类型.
		/// </summary>
		TypeStaticHas,
		/// <summary>
		/// 拥有静态属性方法的类型.
		/// </summary>
		TypeStaticHasM,
		///// <summary>
		///// 类型的静态属性.
		///// </summary>
		//TypeStaticProperty,
		///// <summary>
		///// 类型的静态属性及方法.
		///// </summary>
		//TypeStaticPropertyMethod,
	}

	/// <summary>
	/// 程序集信息.
	/// </summary>
	public static class InfoAssembly {
		/// <summary>
		/// 默认字符串比较规则.
		/// </summary>
		public static readonly StringComparison DefaultStringComparison = StringComparison.OrdinalIgnoreCase;

		/// <summary>
		/// 默认字符串比较器.
		/// </summary>
		public static readonly StringComparer DefaultStringComparer = StringComparer.OrdinalIgnoreCase;

		/// <summary>
		/// 默认类型比较器.
		/// </summary>
		public static readonly TypeNameComparer DefaultTypeNameComparer = TypeNameComparer.Default;

		/// <summary>
		/// 是否排序.
		/// </summary>
		public static bool IsSort = false;

		/// <summary>
		/// 程序集列表.
		/// </summary>
		private static List<string> m_AssemblyList = null;

		/// <summary>
		/// 程序集词典.
		/// </summary>
		private static Dictionary<string, Assembly> m_AssemblyDictionary = null;

		/// <summary>
		/// 程序集列表.
		/// </summary>
		public static List<string> AssemblyList {
			get {
				if (null == m_AssemblyList) {
					m_AssemblyList = GacUtil.GacGetAssemblyNameList(ASM_DISPLAY_FLAGS.ASM_DISPLAYF_FULL, GacGetListOptions.AutoSort | GacGetListOptions.Fallback);
#if (NETFX_CORE)
					// Windows Store App 下绝大多数GAC程序集无法反射. 于是手动追加程序集.
					m_AssemblyDictionary = new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);
					AssemblyDictionaryAddByType(typeof(Windows.System.ProcessorArchitecture));
					foreach (string s in m_AssemblyDictionary.Keys) {
						m_AssemblyList.Add(s);
					}
					// 貌似不支持使用 Assembly.Load 加载 WinRT 的程序集, 于是改用上面的办法使用m_AssemblyDictionary记录程序集.
					//m_AssemblyList.Add(typeof(string).GetTypeInfo().Assembly.FullName);
					//m_AssemblyList.Add(typeof(Windows.System.ProcessorArchitecture).GetTypeInfo().Assembly.FullName);
#endif
				}
				return m_AssemblyList;
			}
		}

		/// <summary>
		/// 在 m_AssemblyDictionary 中添加项目, 根据类型自动计算程序集.
		/// </summary>
		/// <param name="tp"></param>
		private static void AssemblyDictionaryAddByType(Type tp) {
			if (null == tp) return;
			if (null == m_AssemblyDictionary) return;
			Assembly ass = tp.GetTypeInfo().Assembly;
			m_AssemblyDictionary.Add(ass.FullName, ass);
		}

		/// <summary>
		/// 根据名称加载程序集.
		/// </summary>
		/// <param name="name">程序集全名.</param>
		/// <returns>返回程序集.</returns>
		public static Assembly LoadAssembly(string name) {
			Assembly ass = null;
			if (m_AssemblyDictionary.TryGetValue(name, out ass)) return ass;
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
				case InfoMode.TypeString:
					rt = WriteInfo_TypeName(iw, context, assembly, true);
					break;
				case InfoMode.TypeStaticHas:
					rt = WriteInfo_TypeStaticHas(iw, context, assembly, IndentedWriterMemberOptions.OnlyStatic);
					break;
				case InfoMode.TypeStaticHasM:
					rt = WriteInfo_TypeStaticHas(iw, context, assembly, IndentedWriterMemberOptions.OnlyStatic | IndentedWriterMemberOptions.AllowMethod);
					break;
				//case InfoMode.TypeStaticProperty:
				//    rt = WriteInfo_TypeStatic(iw, context, assembly, IndentedWriterMemberOptions.OnlyStatic);
				//    break;
				//case InfoMode.TypeStaticPropertyMethod:
				//    rt = WriteInfo_TypeStatic(iw, context, assembly, IndentedWriterMemberOptions.OnlyStatic | IndentedWriterMemberOptions.AllowMethod);
				//    break;
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
#if (NETFX_CORE)
			// Windows Store App 下反射获取Assembly的属性值时总是失败. 于是手动获取属性值.
			iw.WriteLine("Assembly info 2:");
			iw.Indent(assembly);
			iw.WriteLine("# <System.Reflection.Assembly>");
			iw.WriteLine("ToString():\t{0}", assembly.ToString());
			iw.WriteLine("FullName:\t{0}", assembly.FullName);
			iw.WriteLine("IsDynamic:\t{0}", assembly.IsDynamic);
			iw.WriteLine("GetName():");
			IndentedObjectFunctor.CommonProc(iw, assembly.GetName(), context);
			iw.WriteLine("ManifestModule:");
			IndentedObjectFunctor.CommonProc(iw, assembly.ManifestModule, context);
			iw.WriteLine("Modules:");
			IndentedObjectFunctor.CommonProc(iw, assembly.Modules, context);
			iw.Unindent();
#endif
			return rt;
		}

		/// <summary>
		/// 输出信息_类型名.
		/// </summary>
		/// <param name="iw">输出者.</param>
		/// <param name="context">输出时的环境.</param>
		/// <param name="assembly">程序集.</param>
		/// <param name="istostring">是否使用 ToString 名称.</param>
		/// <returns>是否成功.</returns>
		private static bool WriteInfo_TypeName(IIndentedWriter iw, IndentedWriterContext context, Assembly assembly, bool istostring) {
			bool rt = false;
			if (null == iw) return false;
			if (null == assembly) return false;
			// 获取类型列表.
			IEnumerable<Type> lst = TypeUtil.GetExportedTypes(assembly);
			if (IsSort) {
				List<Type> lst1 = new List<Type>(lst);
				lst1.Sort(DefaultTypeNameComparer);
				lst = lst1;
			}
			// 枚举类型.
			foreach (Type tp in lst) {
				string tpname = istostring ? tp.ToString() : tp.FullName;
				iw.WriteLine(tpname);
			}
			return rt;
		}

		/// <summary>
		/// 输出含有静态成员的类型.
		/// </summary>
		/// <param name="iw">输出者.</param>
		/// <param name="context">输出时的环境.</param>
		/// <param name="assembly">程序集.</param>
		/// <param name="options"></param>
		/// <returns>是否成功.</returns>
		private static bool WriteInfo_TypeStaticHas(IIndentedWriter iw, IndentedWriterContext context, Assembly assembly, IndentedWriterMemberOptions options) {
			bool rt = false;
			if (null == iw) return false;
			if (null == assembly) return false;
			// 获取类型列表.
			IEnumerable<Type> lst = TypeUtil.GetExportedTypes(assembly);
			if (IsSort) {
				List<Type> lst1 = new List<Type>(lst);
				lst1.Sort(DefaultTypeNameComparer);
				lst = lst1;
			}
			// 枚举类型.
			foreach (Type tp in lst) {
				//string tpname = istostring ? tp.ToString() : tp.FullName;
				//iw.WriteLine(tpname);
				if (IndentedWriterUtil.TypeHasStatic(tp, options)) {
					iw.WriteLine(tp.FullName);
				}
			}
			return rt;
		}

		///// <summary>
		///// 输出类型的静态成员.
		///// </summary>
		///// <param name="iw">输出者.</param>
		///// <param name="context">输出时的环境.</param>
		///// <param name="assembly">程序集.</param>
		///// <param name="options"></param>
		///// <returns>是否成功.</returns>
		//private static bool WriteInfo_TypeStatic(IIndentedWriter iw, IndentedWriterContext context, Assembly assembly, IndentedWriterMemberOptions options) {
		//    bool rt = false;
		//    if (null == iw) return false;
		//    if (null == assembly) return false;
		//    // 获取类型列表.
		//    IEnumerable<Type> lst = TypeUtil.GetExportedTypes(assembly);
		//    if (IsSort) {
		//        List<Type> lst1 = new List<Type>(lst);
		//        lst1.Sort(DefaultTypeNameComparer);
		//        lst = lst1;
		//    }
		//    // 枚举类型.
		//    foreach (Type tp in lst) {
		//        //string tpname = istostring ? tp.ToString() : tp.FullName;
		//        //iw.WriteLine(tpname);
		//        if (IndentedWriterUtil.TypeHasStatic(tp, options)) {
		//            iw.WriteLine("{0}:", tp.FullName);
		//            IndentedWriterUtil.WriteTypeStatic(iw, tp, context, options);
		//        }
		//    }
		//    return rt;
		//}

		/// <summary>
		/// 输出静态成员.
		/// </summary>
		/// <param name="iw">输出者.</param>
		/// <param name="context">输出时的环境.</param>
		/// <param name="tp">类型.</param>
		/// <param name="options"></param>
		/// <returns>是否成功.</returns>
		public static bool WriteTypeStatic(IIndentedWriter iw, IndentedWriterContext context, Type tp, IndentedWriterMemberOptions options) {
			bool rt = false;
			if (null == iw) return false;
			if (null == tp) return false;
			iw.WriteLine("{0}:", tp.FullName);
			rt = IndentedWriterUtil.WriteTypeStatic(iw, tp, context, options);
			return rt;
		}

	}
}
