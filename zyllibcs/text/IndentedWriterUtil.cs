using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace zyllibcs.text {
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
		/// 不显示值的内容.
		/// </summary>
		HideValue = 4,
		/// <summary>
		/// 不显示默认注释. 例如枚举的实际值、整数的十六进制、字符串的长度、数组或集合的长度.
		/// </summary>
		HideDefaultComment = 8,
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
		/// 不查找默认输出过程.
		/// </summary>
		NoCommonProcs = 2,
		//TODO: 是否将遍历数组整合到IndentedObjectFunctor, 与集合一起处理?
		///// <summary>
		///// 不自动遍历数组.
		///// </summary>
		//NoForEachArray = 4,
		///// <summary>
		///// 遍历数组时, 不自动简化简单类型（基元类型或<see cref="String"/>等）的信息.
		///// </summary>
		//ForEachArrayNoSimple = 8,
		/// <summary>
		/// 允许枚举方法. 默认情况下仅枚举字段与属性, 加上次标识后才枚举方法.
		/// </summary>
		AllowMethod = 0x100,
	}

	/// <summary>
	/// 输出成员信息时的处理过程.
	/// </summary>
	/// <param name="userdata">用户自定义数据.</param>
	/// <param name="mi">成员信息.</param>
	/// <param name="value">值.</param>
	/// <param name="writeproc">匹配的输出过程.</param>
	/// <param name="iwvo">输出数值的选项. 注意但不是字段或属性时, 其初始值不同.</param>
	/// <param name="isdefault">是否进行默认处理. 若不需要进行默认处理, 便返回false. 注意但不是字段或属性时, 其初始值不同.</param>
	/// <remarks>
	/// 若 <paramref name="isdefault"/> 为 true, 则默认会调用 <see cref="IndentedWriterUtil.WriteLineValue"/> 输出值的信息行, 再使用 <paramref name="writeproc"/> 输出值的详细内容 .
	/// 如果你想定制输出信息, 请将 <paramref name="isdefault"/> 设为 false, 并自行调用 <see cref="IndentedWriterUtil.WriteLineValue"/> 与 <see cref="IndentedWriterObjectProc"/> .
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
		public const BindingFlags BindingMember = 0;

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
		/// <param name="stateobject">State Object. Can be NULL.</param>
		/// <returns>返回匹配的输出过程, 失败时返回null.</returns>
		public static IndentedWriterObjectProc LookupWriteProc(object obj, object stateobject) {
			if (null == obj) return null;
			lock (m_WriteProcs) {
				return LookupWriteProcAt(obj, stateobject, m_WriteProcs);
			}
		}

		/// <summary>
		/// 在集合中查找匹配的输出过程.
		/// </summary>
		/// <param name="obj">对象.</param>
		/// <param name="stateobject">State Object. Can be NULL.</param>
		/// <param name="procs">输出过程的集合.</param>
		/// <returns>返回匹配的输出过程, 失败时返回null.</returns>
		public static IndentedWriterObjectProc LookupWriteProcAt(object obj, object stateobject, IEnumerable<IndentedWriterObjectProc> procs) {
			if (null == obj) return null;
			if (null == procs) return null;
			foreach (IndentedWriterObjectProc p in procs) {
				if (null == p) continue;
				if (p(null, obj, stateobject)) return p;
			}
			return null;
		}

		/// <summary>
		/// C转码字符，并自动加上首尾引号.
		/// </summary>
		/// <param name="ch">字符.</param>
		/// <returns>返回该字符的C转码的串. 注意会根据需要自动加上首尾引号.</returns>
		public static string EscapeCCharAuto(char ch) {
			return StringEscapeC.EscapeChar(ch, StringEscapeCMode.QuoteAuto);
		}

		/// <summary>
		/// C转码字符串，并自动加上首尾引号.
		/// </summary>
		/// <param name="src">源字符串.</param>
		/// <returns>返回该字符串的C转码的串. 注意会根据需要自动加上首尾引号.</returns>
		public static string EscapeCStringAuto(string src) {
			return StringEscapeC.EscapeString(src, StringEscapeCMode.QuoteAuto);
		}

		/// <summary>
		/// 简单类型集.
		/// </summary>
		private static readonly Type[] SimpleTypes = new Type[] {
			typeof(string),
			typeof(StringBuilder),
			typeof(Decimal),
			typeof(DateTime),
			typeof(TimeSpan),
		};

		/// <summary>
		/// 判断简单类型. 如 <see cref="String"/>, <see cref="StringBuilder"/>, <see cref="Decimal"/>, <see cref="DateTime"/>, <see cref="TimeSpan"/> .
		/// </summary>
		/// <param name="tp">类型.</param>
		/// <returns></returns>
		public static bool IsSimpleType(Type tp) {
			if (null == tp) return false;
			foreach (Type p in SimpleTypes) {
				if (tp.Equals(p)) return true;
				if (tp.IsSubclassOf(p)) return true;
			}
			return false;
		}

		/// <summary>
		/// 计算简单值的文本. 
		/// </summary>
		/// <param name="value">值.</param>
		/// <returns>返回文本. 对于不支持类型返回null.</returns>
		public static string GetSimpleValueText(object value) {
			string rt = null;
			if (null == value) return rt;
			if (value is char) {
				rt = EscapeCCharAuto((char)value);
			}
			else if (value is string || value is StringBuilder) {
				rt = EscapeCStringAuto(value.ToString());
			}
			return rt;
		}

		/// <summary>
		/// 计算默认注释.
		/// </summary>
		/// <param name="value">值.</param>
		/// <returns>返回默认注释. 失败时返回null.</returns>
		public static string GetCommentText(object value) {
			string rt = null;
			if (null == value) return rt;
			Type tp = value.GetType();
			if (tp.IsEnum) {
				return string.Format("0x{0:X}, {0:d}, {1}", value, tp.Name);
			}
			else if (tp.IsPrimitive) {
				if (value is char) {
					return string.Format("0x{0:X}, {0:d}", (int)(char)value);
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
					return string.Format("0x{0:X}", value);
				}
			}
			else if (value is string) {
				return string.Format("Length={0:d} (0x{0:X})", (value as string).Length);
			}
			else if (value is StringBuilder) {
				return string.Format("Length={0:d} (0x{0:X})", (value as StringBuilder).Length);
			}
			else if (tp.IsArray) {
				return string.Format("Length={0:d} (0x{0:X})", (value as Array).Length);
			}
			return rt;
		}

		/// <summary>
		/// 输出值的信息行.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="name">名称.</param>
		/// <param name="value">值.</param>
		/// <param name="options">选项.</param>
		/// <param name="appendcomment">追加注释. 可为 null 或 <c>String.Empty</c>. </param>
		/// <returns>若成功输出, 便返回true. 否则返回false.</returns>
		/// <remarks>信息行的格式为: <c>"[name]:\t[value]\t# [DefaultComment] . [appendcomment]"</c></remarks>
		public static bool WriteLineValue(IIndentedWriter iw, string name, object value, IndentedWriterValueOptions options, string appendcomment) {
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
				string s = GetSimpleValueText(value);
				if (null != s) {
					iw.Write(s);
				}
				else {
					iw.Write(value);
				}
			}
			string defaultcomment = null;
			if (null != value && (options & IndentedWriterValueOptions.HideDefaultComment) == 0) {
				defaultcomment = GetCommentText(value);
			}
			if (!string.IsNullOrEmpty(defaultcomment) || !string.IsNullOrEmpty(appendcomment)) {
				iw.Write("\t# ");
				if (!string.IsNullOrEmpty(defaultcomment)) iw.Write(defaultcomment);
				if (!string.IsNullOrEmpty(defaultcomment) && !string.IsNullOrEmpty(appendcomment)) iw.Write(" . ");
				if (!string.IsNullOrEmpty(appendcomment)) iw.Write(appendcomment);
			}
			iw.WriteLine();
			// return.
			rt = true;
			return rt;
		}

		///// <summary>
		///// 输出对象的各个成员.
		///// </summary>
		///// <param name="iw">带缩进输出者.</param>
		///// <param name="owner">欲查询成员的对象. 查询静态成员时可以设为 null.</param>
		///// <param name="tp">类型. 当 <paramref name="owner"/> 非 null 时, 可设为null, 即自动设为 <c>owner.GetType()</c> . </param>
		///// <param name="bindingAttr">绑定标志.</param>
		///// <param name="options">成员选项.</param>
		///// <param name="procs">输出过程的集合.</param>
		///// <param name="userdata">用户自定义数据. 会传递给 <paramref name="handle"/> . </param>
		///// <param name="handle">每个成员的处理过程.</param>
		///// <returns>是否成功.</returns>
		//public static bool ForEachMember(IIndentedWriter iw, object owner, Type tp, BindingFlags bindingAttr, IndentedWriterMemberOptions options, IEnumerable<IndentedWriterObjectProc> procs, object userdata, IndentedWriterHandleMemberProc handle) {
		//    bool rt = false;
		//    if (null == tp) {
		//        if (null == owner) return rt;
		//        tp = owner.GetType();
		//    }
		//    if (null == owner && (bindingAttr & BindingFlags.Instance) != 0) throw new ArgumentException("bindingAttr has Instance, but owner is null.", "bindingAttr");
		//    foreach (MemberInfo mi in tp.GetMembers(bindingAttr)) {
		//        //if (!CheckMemberType(mi.MemberType,bindingAttr)) continue;
		//        bool bOk = false;
		//        const IndentedWriterValueOptions default_iwvo = IndentedWriterValueOptions.Default;
		//        object value = null;
		//        IndentedWriterObjectProc writeproc = null;
		//        IndentedWriterValueOptions iwvo = IndentedWriterValueOptions.ExistValue;
		//        bool isdefault = false;
		//        // get value.
		//        if (false) {
		//        }
		//        else if (mi is FieldInfo) {
		//            FieldInfo fi = mi as FieldInfo;
		//            bOk = true;
		//            if (true) {
		//                try {
		//                    value = fi.GetValue(owner);
		//                    iwvo = default_iwvo;
		//                    isdefault = true;
		//                }
		//                catch (Exception ex) {
		//                    Debug.WriteLine(ex);
		//                }
		//            }
		//        }
		//        else if (mi is PropertyInfo) {
		//            PropertyInfo pi = mi as PropertyInfo;
		//            bOk = true;
		//            if (pi.CanRead && pi.GetIndexParameters().Length <= 0)
		//            {
		//                try {
		//                    value = pi.GetValue(owner, null);
		//                    iwvo = default_iwvo;
		//                    isdefault = true;
		//                }
		//                catch (Exception ex) {
		//                    Debug.WriteLine(ex);
		//                }
		//            }
		//        }
		//        else if (mi is MethodInfo)
		//        {
		//            if ((options & IndentedWriterMemberOptions.AllowMethod) != 0) {
		//                bOk = true;
		//            }
		//        }
		//        if (!bOk) continue;
		//        // get proc.
		//        if (null != value) {
		//            writeproc = LookupWriteProcAt(value, userdata, procs);
		//            if (null == writeproc && (options & IndentedWriterMemberOptions.NoDefaultProcs) == 0) {
		//                writeproc = LookupWriteProc(value, userdata);
		//            }
		//            if (null == writeproc && (options & IndentedWriterMemberOptions.NoCommonProcs) == 0) {
		//                if (IndentedObjectFunctor.CommonProc(null, owner, userdata)) writeproc = IndentedObjectFunctor.CommonProc;
		//            }
		//        }
		//        // handle
		//        if (null != handle) {
		//            handle(userdata, mi, value, ref writeproc, ref iwvo, ref isdefault);
		//        }
		//        // show.
		//        if (isdefault) {
		//            WriteLineValue(iw, mi.Name, value, iwvo, null);
		//            if (null != writeproc) {
		//                writeproc(iw, value, userdata);
		//            }
		//        }
		//    }
		//    rt = true;
		//    return rt;
		//}

		/// <summary>
		/// 输出对象的各个成员.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="owner">欲查询成员的对象. 查询静态成员时可以设为 null.</param>
		/// <param name="tp">类型. 当 <paramref name="owner"/> 非 null 时, 可设为null, 即自动设为 <c>owner.GetType()</c> . </param>
		/// <param name="bindingAttr">绑定标志.</param>
		/// <param name="options">成员选项.</param>
		/// <param name="procs">输出过程的集合, 可以为 null.</param>
		/// <param name="handle">每个成员的处理过程, 可以为 null. 默认在调用时会将其 <c>sender</c>参数设为null. </param>
		/// <param name="stateobject">状态对象, 可以为 null. 会传递给 <paramref name="procs"/>,<paramref name="handle"/> , 会嵌套传递. </param>
		/// <returns>是否成功.</returns>
		public static bool ForEachMember(IIndentedWriter iw, object owner, Type tp, BindingFlags bindingAttr, IndentedWriterMemberOptions options, IEnumerable<IndentedWriterObjectProc> procs, EventHandler<IndentedWriterMemberEventArgs> handle, object stateobject) {
			bool rt = false;
			if (null == tp) {
				if (null == owner) return rt;
				tp = owner.GetType();
			}
			if (null == owner && (bindingAttr & BindingFlags.Instance) != 0) throw new ArgumentException("bindingAttr has Instance, but owner is null.", "bindingAttr");
			// args
			IndentedWriterMemberEventArgs args = new IndentedWriterMemberEventArgs();
			args.IsCancel = false;
			args.IsCancelAll = false;
			args.HasDefault = false;
			args.IndentedWriter = iw;
			args.Owner = owner;
			args.OwnerType = tp;
			args.BindingAttr = bindingAttr;
			args.MemberOptions = options;
			args.Procs = procs;
			//args.Tag = stateobject;
			// foreach.
			foreach (MemberInfo mi in tp.GetMembers(bindingAttr)) {
				//if (!CheckMemberType(mi.MemberType,bindingAttr)) continue;
				const IndentedWriterValueOptions default_iwvo = IndentedWriterValueOptions.Default;
				args.IsCancel = true;
				args.HasDefault = false;
				args.MemberInfo = mi;
				args.MemberName = mi.Name;
				args.Value = mi;
				args.ValueOptions = IndentedWriterValueOptions.ExistValue;
				args.AppendComment = null;
				args.WriteProc = null;
				//
				//bool bOk = false;
				//object value = null;
				//IndentedWriterObjectProc writeproc = null;
				//IndentedWriterValueOptions iwvo = IndentedWriterValueOptions.ExistValue;
				//bool isdefault = false;
				// get value.
				if (false) {
				}
				else if (mi is FieldInfo) {
					FieldInfo fi = mi as FieldInfo;
					args.IsCancel = false;
					if (true) {
						try {
							args.Value = fi.GetValue(owner);
							args.ValueOptions = default_iwvo;
							args.HasDefault = true;
						}
						catch (Exception ex) {
							Debug.WriteLine(ex);
						}
					}
				}
				else if (mi is PropertyInfo) {
					PropertyInfo pi = mi as PropertyInfo;
					args.IsCancel = false;
					if (pi.CanRead && pi.GetIndexParameters().Length <= 0) {
						try {
							args.Value = pi.GetValue(owner, null);
							args.ValueOptions = default_iwvo;
							args.HasDefault = true;
						}
						catch (Exception ex) {
							Debug.WriteLine(ex);
						}
					}
				}
				else if (mi is MethodInfo) {
					if ((options & IndentedWriterMemberOptions.AllowMethod) != 0) {
						args.IsCancel = false;
					}
				}
				if (args.IsCancel) continue;
				// get proc.
				if (null != args.Value) {
					args.WriteProc = LookupWriteProcAt(args.Value, stateobject, procs);
					if (null == args.WriteProc && (options & IndentedWriterMemberOptions.NoDefaultProcs) == 0) {
						args.WriteProc = LookupWriteProc(args.Value, stateobject);
					}
					if (null == args.WriteProc && (options & IndentedWriterMemberOptions.NoCommonProcs) == 0) {
						if (IndentedObjectFunctor.CommonProc(null, args.Value, stateobject)) args.WriteProc = IndentedObjectFunctor.CommonProc;
					}
				}
				// handle
				if (null != handle) {
					//handle(stateobject, mi, value, ref writeproc, ref iwvo, ref isdefault);
					handle(null, args);
				}
				if (args.IsCancelAll) break;
				if (args.IsCancel) continue;
				// show.
				if (args.HasDefault) {
					WriteLineValue(iw, args.MemberName, args.Value, args.ValueOptions, args.AppendComment);
					if (null != args.WriteProc) {
						args.WriteProc(iw, args.Value, stateobject);
					}
				}
			}
			rt = true;
			return rt;
		}

	}
}
