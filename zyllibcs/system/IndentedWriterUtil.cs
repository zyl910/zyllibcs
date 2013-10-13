using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace zyllibcs.system {
	/// <summary>
	/// 带缩进输出数值的选项.
	/// </summary>
	[Flags]
	public enum IndentedWriterValueOptions {
		/// <summary>
		/// 默认.
		/// </summary>
		Default = 0,
		/// <summary>
		/// 当没有名称时不输出.
		/// </summary>
		ExistName = 1,
		/// <summary>
		/// 当没有值时不输出.
		/// </summary>
		ExistValue = 2,
		/// <summary>
		/// 不显示值的内容与注释.
		/// </summary>
		HideValue = 4,
		/// <summary>
		/// 不显示注释. 例如枚举的实际值、整数的十六进制.
		/// </summary>
		HideComment = 8,
	}

	/// <summary>
	/// 带缩进输出成员的选项.
	/// </summary>
	[Flags]
	public enum IndentedWriterMemberOptions {
		/// <summary>
		/// 默认.
		/// </summary>
		Default = 0,
		/// <summary>
		/// 不查找默认输出过程集.
		/// </summary>
		NoDefaultProcs = 1,
		/// <summary>
		/// 不使用默认输出过程.
		/// </summary>
		NoCommonProcs = 2,
	}

	/// <summary>
	/// 带缩进输出对象的过程.
	/// </summary>
	/// <param name="iw">带缩进输出者.</param>
	/// <param name="obj">object. If <paramref name="obj"/> is null, result alway is false.</param>
	/// <returns>当<paramref name="iw"/>为null时, 返回是否支持输出. 否则返回是否成功输出.</returns>
	public delegate bool IndentedWriterObjectProc(IIndentedWriter iw, object obj);

	/// <summary>
	/// 输出成员信息时的处理过程.
	/// </summary>
	/// <param name="userdata">用户自定义对象.</param>
	/// <param name="mi">成员信息.</param>
	/// <param name="value">值.</param>
	/// <param name="writeproc">匹配的输出过程.</param>
	/// <param name="iwvo">输出数值的选项. 注意但不是字段或属性时, 其初始值不同.</param>
	/// <param name="isdefault">是否进行默认处理. 若不需要进行默认处理, 便返回false. 注意但不是字段或属性时, 其初始值不同.</param>
	/// <remarks>
	/// 若 <paramref name="isdefault"/> 为 true, 则默认会调用 <see cref="WriteLineValue"/> 输出值的信息行, 再使用 <paramref name="writeproc"/> 输出值的详细内容 .
	/// 如果你想定制输出信息, 请将 <paramref name="isdefault"/> 设为 false, 并自行调用 <see cref="WriteLineValue"/> 与 <see cref="IndentedWriterObjectLnProc"/> .
	/// 注意默认操作仅支持字段与(非索引化的)属性.
	/// </remarks>
	public delegate void IndentedWriterHandleMemberProc(object userdata, MemberInfo mi, object value, ref IndentedWriterObjectProc writeproc, ref IndentedWriterValueOptions iwvo, ref bool isdefault);

	/// <summary>
	/// 带缩进输出者工具.
	/// </summary>
	public static class IndentedWriterUtil {
		/// <summary>
		/// 常用成员(字段,属性,方法).
		/// </summary>
		public const BindingFlags BindingMember = BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.InvokeMethod;

		/// <summary>
		/// 公开的实例成员.
		/// </summary>
		public const BindingFlags PublicInstance = BindingMember | BindingFlags.Public | BindingFlags.Instance;

		/// <summary>
		/// 公开的静态成员.
		/// </summary>
		public const BindingFlags PublicStatic = BindingMember | BindingFlags.Public | BindingFlags.Static;

		/// <summary>
		/// 公开的静态与实例成员.
		/// </summary>
		public const BindingFlags PublicStaticInstance = PublicInstance | PublicStatic;

		/// <summary>
		/// 默认的输出过程集合.
		/// </summary>
		private static List<IndentedWriterObjectProc> m_WriteProcs = new List<IndentedWriterObjectProc>();

		/// <summary>
		/// 向默认集合中添加输出过程.
		/// </summary>
		/// <param name="proc"></param>
		public static void AddWriteProc(IndentedWriterObjectProc proc) {
			lock (m_WriteProcs) {
				m_WriteProcs.Add(proc);
			}
		}

		/// <summary>
		/// 查找匹配的输出过程.
		/// </summary>
		/// <param name="obj">对象.</param>
		/// <returns>返回匹配的输出过程, 失败时返回null.</returns>
		public static IndentedWriterObjectProc LookupWriteProc(object obj) {
			if (null == obj) return null;
			lock (m_WriteProcs) {
				return LookupWriteProcAt(obj, m_WriteProcs);
			}
		}

		/// <summary>
		/// 在集合中查找匹配的输出过程.
		/// </summary>
		/// <param name="obj">对象.</param>
		/// <param name="procs">输出过程的集合.</param>
		/// <returns>返回匹配的输出过程, 失败时返回null.</returns>
		public static IndentedWriterObjectProc LookupWriteProcAt(object obj, IEnumerable<IndentedWriterObjectProc> procs) {
			if (null == obj) return null;
			if (null == procs) return null;
			foreach (IndentedWriterObjectProc p in procs) {
				if (null == p) continue;
				if (p(null, obj)) return p;
			}
			return null;
		}

		/// <summary>
		/// 输出值的信息行.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="name">名称.</param>
		/// <param name="value">值.</param>
		/// <param name="options">选项.</param>
		/// <returns>若成功输出, 便返回true. 否则返回false.</returns>
		public static bool WriteLineValue(IIndentedWriter iw, string name, object value, IndentedWriterValueOptions options) {
			bool rt = false;
			// check.
			if (null == iw) return rt;
			if (string.IsNullOrEmpty(name)) {
				if ((options & IndentedWriterValueOptions.ExistName) != 0) return rt;
				name = string.Empty;
			}
			if (null == value) {
				if ((options & IndentedWriterValueOptions.ExistValue) != 0) return rt;
			}
			// format
			iw.Write("{0}:", name);
			if ((options & IndentedWriterValueOptions.ExistValue) == 0) {
				iw.Write('\t');
				if (value is char) {
					char ch = (char)value;
					if (ch != '\t' && char.IsControl(ch)) ch = '.';	// 将控制字符转为'.' .
					iw.Write(ch);
				}
				else {
					iw.Write(value);
				}
				if (null != value && (options & IndentedWriterValueOptions.HideComment) == 0) {
					Type tp = value.GetType();
					if (tp.IsEnum) {
						iw.Write("\t# 0x{0:X}, {0:d}, {1}", value, tp.Name);
					}
					else if (tp.IsPrimitive) {
						if (value is char) {
							iw.Write("\t# 0x{0:X}, {0:d}", (int)(char)value);
						}
						else if (value is sbyte
							|| value is byte
							|| value is short
							|| value is ushort
							|| value is int
							|| value is uint
							|| value is long
							|| value is ulong
							) {
							iw.Write("\t# 0x{0:X}", value);
						}
					}
				}
			}
			iw.WriteLine();
			// return.
			rt = true;
			return rt;
		}

		/// <summary>
		/// 检测成员类型.
		/// </summary>
		/// <param name="mt">成员类型.</param>
		/// <param name="bf">绑定标志.</param>
		/// <returns>返回是否匹配.</returns>
		public static bool CheckMemberType(MemberTypes mt, BindingFlags bf) {
			if (false) {
			}
			else if ((mt & MemberTypes.Constructor) != 0) {
				if ((bf & BindingFlags.CreateInstance)!=0) return true;
			}
			else if ((mt & MemberTypes.Event) != 0) {
			}
			else if ((mt & MemberTypes.Field) != 0) {
				if ((bf & BindingFlags.GetField) != 0) return true;
			}
			else if ((mt & MemberTypes.Method) != 0) {
				if ((bf & BindingFlags.InvokeMethod) != 0) return true;
			}
			else if ((mt & MemberTypes.Property) != 0) {
				if ((bf & BindingFlags.GetProperty) != 0) return true;
			}
			else if ((mt & MemberTypes.TypeInfo) != 0) {
			}
			else if ((mt & MemberTypes.Custom) != 0) {
			}
			else if ((mt & MemberTypes.NestedType) != 0) {
			}
			return false;
		}

		/// <summary>
		/// 输出对象的各个成员.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="owner">欲查询成员的对象. 查询静态成员时可以设为 null.</param>
		/// <param name="tp">类型. 当 <paramref name="owner"/> 非 null 时, 可设为null, 即自动设为 <c>owner.GetType()</c> . </param>
		/// <param name="bindingAttr">哪些成员.</param>
		/// <param name="options">输出选项.</param>
		/// <param name="procs">输出过程的集合.</param>
		/// <param name="userdata">用户自定义数据. 会传递给 <paramref name="handle"/> . </param>
		/// <param name="handle">每个成员的处理过程.</param>
		/// <returns>是否成功.</returns>
		public static bool ForEachMember(IIndentedWriter iw, object owner, Type tp, BindingFlags bindingAttr, IndentedWriterMemberOptions options, IEnumerable<IndentedWriterObjectProc> procs, object userdata, IndentedWriterHandleMemberProc handle) {
			bool rt = false;
			if (null == tp) {
				if (null == owner) return rt;
				tp = owner.GetType();
			}
			if (null == owner && (bindingAttr & BindingFlags.Instance) != 0) throw new ArgumentException("bindingAttr has Instance, but owner is null.", "bindingAttr");
			foreach (MemberInfo mi in tp.GetMembers(bindingAttr)) {
				if (!CheckMemberType(mi.MemberType,bindingAttr)) continue;
				const IndentedWriterValueOptions default_iwvo = IndentedWriterValueOptions.Default;
				object value = null;
				IndentedWriterObjectProc writeproc = null;
				IndentedWriterValueOptions iwvo = IndentedWriterValueOptions.ExistValue;
				bool isdefault = false;
				// get value.
				if (false) {
				}
				else if (mi is FieldInfo) {
					FieldInfo fi = mi as FieldInfo;
					if (true) {
						try {
							value = fi.GetValue(owner);
							iwvo = default_iwvo;
							isdefault = true;
						}
						catch (Exception ex) {
							Debug.WriteLine(ex);
						}
					}
				}
				else if (mi is PropertyInfo) {
					PropertyInfo pi = mi as PropertyInfo;
					if (pi.CanRead && pi.GetIndexParameters().Length <= 0) {
						try {
							value = pi.GetValue(owner, null);
							iwvo = default_iwvo;
							isdefault = true;
						}
						catch (Exception ex) {
							Debug.WriteLine(ex);
						}
					}
				}
				// get proc.
				if (null != value) {
					writeproc = LookupWriteProcAt(value, procs);
					if (null == writeproc && (options & IndentedWriterMemberOptions.NoDefaultProcs) == 0) {
						writeproc = LookupWriteProc(value);
					}
					if (null == writeproc && (options & IndentedWriterMemberOptions.NoCommonProcs) == 0) {
						if (IndentedObjectFunctor.CommonProc(null, owner)) writeproc = IndentedObjectFunctor.CommonProc;
					}
				}
				// handle
				if (null != handle) {
					handle(userdata, mi, value, ref writeproc, ref iwvo, ref isdefault);
				}
				// show.
				if (isdefault) {
					WriteLineValue(iw, mi.Name, value, iwvo);
					if (null != writeproc) {
						writeproc(iw, value);
					}
				}
			}
			rt = true;
			return rt;
		}

	}
}
