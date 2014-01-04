using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using zyllibcs.system;

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
		/// 自动隐藏值的内容. 当发现值为null时不显示.
		/// </summary>
		AutoHideValue = 4,
		/// <summary>
		/// 不显示值的内容.
		/// </summary>
		HideValue = 8,
		/// <summary>
		/// 不显示默认注释. 例如枚举的实际值、整数的十六进制、字符串的长度、数组或集合的长度.
		/// </summary>
		HideDefaultComment = 0x10,
		/// <summary>
		/// 不剪裁值. 没有此标志时, 输出时会剪裁值得前 <see cref="IndentedWriterUtil.WriterValueMaxLength"/> 个字符. 带了此标志后就不会剪裁.
		/// </summary>
		NoClipValue = 0x20,
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
		/// 不查找默认输出过程.
		/// </summary>
		NoCommonProcs = 1,

        /// <summary>
        /// 仅枚举静态成员. 默认情况下仅枚举实例成员.
        /// </summary>
        OnlyStatic = 0x100,
        /// <summary>
		/// 允许枚举方法. 默认情况下仅枚举字段与属性, 加上此标识后才枚举方法.
		/// </summary>
		AllowMethod = 0x200,
	}

	/// <summary>
	/// 输出对象成员的通知.
	/// </summary>
	/// <param name="iw">带缩进输出者.</param>
	/// <param name="owner">欲查询成员的对象.</param>
	/// <param name="tp">类型.</param>
	/// <param name="options">成员选项. </param>
	/// <param name="handle">每个成员的处理过程. </param>
	/// <param name="context">环境对象. </param>
	/// <returns>若在开始枚举成员之前, 返回值表示是否允许枚举. 其他时候忽略.</returns>
	public delegate bool IndentedWriterForEachMemberNotify(IIndentedWriter iw, object owner, Type tp, IndentedWriterMemberOptions options, EventHandler<IndentedWriterMemberEventArgs> handle, IndentedWriterContext context);

	/// <summary>
	/// 带缩进输出者工具.
	/// </summary>
	public static class IndentedWriterUtil {

		/// <summary>
		/// 默认类型名称选项.
		/// </summary>
		public static readonly MemberNameOptions DefaultTypeNameOption = MemberNameOptions.FullName | MemberNameOptions.TypeParamActual | MemberNameOptions.ParamSeparator;

		/// <summary>
		/// 默认成员名称选项.
		/// </summary>
		public static readonly MemberNameOptions DefaultMemberNameOption = MemberNameOptions.TypeParamSeparator | MemberNameOptions.ParamSeparator;

		/// <summary>
		/// 默认字符串比较器. 区分大小写.
		/// </summary>
		public static readonly StringComparer StringComparer = StringComparer.Ordinal;

		/// <summary>
		/// 输出值时的最大长度. 与 <see cref="IndentedWriterValueOptions.NoClipValue"/> 标志有关.
		/// </summary>
		public static readonly int WriterValueMaxLength = 1000;


		/// <summary>
		/// 在集合中查找匹配的输出过程.
		/// </summary>
		/// <param name="obj">对象.</param>
		/// <param name="context">Context Object. Can be NULL.</param>
		/// <param name="procs">输出过程的集合.</param>
		/// <returns>返回匹配的输出过程, 失败时返回null.</returns>
		public static IndentedWriterObjectProc LookupWriteProcAt(object obj, IndentedWriterContext context, IEnumerable<IndentedWriterObjectProc> procs) {
			if (null == obj) return null;
			if (null == procs) return null;
			foreach (IndentedWriterObjectProc p in procs) {
				if (null == p) continue;
				if (p(null, obj, context)) return p;
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
		/// 简单类型全名数组.
		/// </summary>
		private static readonly String[] SimpleTypeNames = new String[] {
			"System.String",
			"System.StringBuilder",
			"System.Decimal",
			"System.DateTime",
			"System.TimeSpan",
			//"System.Drawing.Color",
			"System.Drawing.Point",
			"System.Drawing.PointF",
			"System.Drawing.Rectangle",
			"System.Drawing.RectangleF",
			"System.Drawing.Size",
			"System.Drawing.SizeF",
		};

		/// <summary>
		/// 简单类型全名集合.
		/// </summary>
		private static Dictionary<string, object> SimpleTypeNameSet = null;

		/// <summary>
		/// 判断简单类型. 如 <see cref="String"/>, <see cref="StringBuilder"/>, <see cref="Decimal"/>, <see cref="DateTime"/>, <see cref="TimeSpan"/> .
		/// </summary>
		/// <param name="tp">类型.</param>
		/// <returns></returns>
		public static bool IsSimpleType(Type tp) {
			if (null == tp) return false;
			// init.
			if (null == SimpleTypeNameSet) {
				Dictionary<string, object> dict = new Dictionary<string, object>(StringComparer);
				foreach (string s in SimpleTypeNames) {
					if (null == s) continue;
					try {
						dict.Add(s, null);
					}
					catch {
					}
				}
				SimpleTypeNameSet = dict;
			}
			if (null == SimpleTypeNameSet) return false;
			// check.
			string name = tp.FullName;
			if (null == name) return false;
			return SimpleTypeNameSet.ContainsKey(name);
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
			if (false) {
			}
			else if (value is Enum) {
				return string.Format("0x{0:X}, {0:d}, <{1}>", value, tp.Name);
			}
			else if (value is char) {
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
				return string.Format("0x{0:X}, <{1}>", value, tp.Name);
			}
			else if (value is IntPtr) {
				return string.Format("0x{0:X}, <{1}>", ((IntPtr)value).ToInt64(), tp.Name);
			}
			else if (value is string) {
				return string.Format("Length={0:d} (0x{0:X})", (value as string).Length);
			}
			else if (value is StringBuilder) {
				return string.Format("Length={0:d} (0x{0:X}), <{1}>", (value as StringBuilder).Length, tp.Name);
			}
			else if (tp.IsArray) {
				return string.Format("Length={0:d} (0x{0:X})", (value as Array).Length);
			}
			else {
				rt = string.Format("<{0}>", tp.Name);
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
			bool showvalue = ((options & IndentedWriterValueOptions.HideValue) == 0 && (null != value || (options & IndentedWriterValueOptions.AutoHideValue) == 0));
			if (showvalue) {
				iw.Write('\t');
				string s = GetSimpleValueText(value);
				if (null == s && null != value) s = EscapeCStringAuto(value.ToString());
				// clip.
				if (null != s && 0 == (options & IndentedWriterValueOptions.NoClipValue)) {
					Debug.Assert(WriterValueMaxLength > 0);
					if (s.Length >= WriterValueMaxLength) {
						string strmore = string.Format("... (Length={0})", s.Length);
						s = s.Substring(0, WriterValueMaxLength);
						if ('\"' == s[0]) {
							s += "\"";
						}
						s += strmore;
					}
				}
				// write.
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

		/// <summary>
		/// 取得成员列表. 注意这里是宽松条件, 调用者最好再过滤一下其中的条目.
		/// </summary>
		/// <param name="tp">类型.</param>
		/// <param name="options">选项.</param>
		/// <returns>返回成员列表.</returns>
		public static IEnumerable<MemberInfo> GetMembers(Type tp, IndentedWriterMemberOptions options) {
			if (null == tp) return null;
			bool onlystatic = (options & IndentedWriterMemberOptions.OnlyStatic) != 0;
			bool allowmethod = (options & IndentedWriterMemberOptions.AllowMethod) != 0;
#if NETFX_CORE
			TypeInfo ti = tp.GetTypeInfo();
			if (null == ti) return null;
			List<MemberInfo> rt = new List<MemberInfo>();
			foreach (FieldInfo p in ti.DeclaredFields) {
				if (p.IsStatic == onlystatic && p.IsPublic) rt.Add(p);
			}
			foreach (PropertyInfo p in ti.DeclaredProperties) {
				//if (p.IsStatic == onlystatic) rt.Add(p);
				// 不支持 IsStatic 与 GetGetMethod.
				if (p.GetMethod.IsStatic == onlystatic && p.CanRead) rt.Add(p);
			}
			if (allowmethod) {
				foreach (MethodInfo p in ti.DeclaredMethods) {
					if (p.IsStatic == onlystatic && p.IsPublic) rt.Add(p);
				}
			}
			return rt;
#else
			BindingFlags bf = BindingFlags.Public | BindingFlags.Instance;
			if ((options & IndentedWriterMemberOptions.OnlyStatic) !=0) bf = BindingFlags.Public | BindingFlags.Static;
			//List<MemberInfo> rt = new List<MemberInfo>();
			//foreach (MemberInfo mi in tp.GetMembers(bindingAttr)) {
			//}
			return tp.GetMembers(bf);
#endif
		}

		/// <summary>
		/// 输出对象的各个成员.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="owner">欲查询成员的对象. 查询静态成员时可以设为 null.</param>
		/// <param name="tp">类型. 当 <paramref name="owner"/> 非 null 时, 可设为null, 即自动设为 <c>owner.GetType()</c> . </param>
		/// <param name="options">成员选项. 实际的成员选项是本参数与 <see cref="IndentedWriterContext.MemberOptions"/> 属性做或运算后的值. </param>
		/// <param name="handle">每个成员的处理过程, 可以为 null. 默认在调用时会将其 <c>sender</c>参数设为null. </param>
		/// <param name="context">环境对象, 可以为 null. 会嵌套传递. </param>
		/// <returns>是否成功.</returns>
		public static bool ForEachMember(IIndentedWriter iw, object owner, Type tp, IndentedWriterMemberOptions options, EventHandler<IndentedWriterMemberEventArgs> handle, IndentedWriterContext context) {
			bool rt = false;
			if (null == tp) {
				if (null == owner) return rt;
				tp = owner.GetType();
			}
            if (null == owner && (options & IndentedWriterMemberOptions.OnlyStatic) == 0) throw new ArgumentException("options not has Static, but owner is null.", "bindingAttr");
			// notify begin.
			if (null == context) context = new IndentedWriterContext();	// 自动创建环境. 有可能以后调整设计, 所以后面还是检查 context.
			if (null != context) {
				if (!context.NotifyForEachMemberBegin(iw, owner, tp, options, handle, context)) return false;
			}
			// args
			IndentedWriterMemberEventArgs args = new IndentedWriterMemberEventArgs();
			args.IsCancel = false;
			args.IsCancelAll = false;
			args.HasDefault = false;
			args.IndentedWriter = iw;
			args.Owner = owner;
			args.OwnerType = tp;
			args.MemberOptions = options;
			//args.Procs = procs;
			args.Context = context;
			// foreach.
			IndentedWriterMemberOptions realoptions = options;
			if (null != context) realoptions |= context.MemberOptions;
			foreach (MemberInfo mi in GetMembers(tp, realoptions)) {
				string AppendComment2 = null;	// 备用的附加注释. 可能会在 args.AppendComment 为null时 使用.
				args.IsCancel = true;
				args.HasDefault = false;
				args.MemberInfo = mi;
				args.MemberName = TypeUtil.GetMemberNameAuto(mi, DefaultMemberNameOption);//mi.Name;
				args.Value = null;
				args.ValueOptions = IndentedWriterValueOptions.ExistValue;
				args.AppendComment = null;
				args.WriteProc = null;
				//Debug.WriteLine(mi.ToString());
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
							args.ValueOptions = IndentedWriterValueOptions.Default;
							args.HasDefault = true;
							AppendComment2 = string.Format("<{0}>", fi.FieldType.Name);
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
							args.ValueOptions = IndentedWriterValueOptions.Default; ;
							args.HasDefault = true;
							AppendComment2 = string.Format("<{0}>", pi.PropertyType.Name);
						}
						catch (Exception ex) {
							Debug.WriteLine(ex);
						}
					}
				}
				else if (mi is MethodInfo) {
					if ((realoptions & IndentedWriterMemberOptions.AllowMethod) != 0) {
						MethodInfo methodinfo = mi as MethodInfo;
						args.IsCancel = false;
						args.ValueOptions = IndentedWriterValueOptions.AutoHideValue;
						//args.MemberName = string.Format("{0}()", mi.Name);
						AppendComment2 = string.Format("<{0}>", methodinfo.ReturnType.Name);
					}
				}
				if (args.IsCancel) continue;
				// get proc.
				if (null != args.Value) {
					//args.WriteProc = LookupWriteProcAt(args.Value, context, procs);
					if (null != context) {
						args.WriteProc = LookupWriteProcAt(args.Value, context, context.Procs);
					}
					//if (null == args.WriteProc && (realoptions & IndentedWriterMemberOptions.NoDefaultProcs) == 0) {
					//    args.WriteProc = LookupWriteProc(args.Value, context);
					//}
					if (null == args.WriteProc && (realoptions & IndentedWriterMemberOptions.NoCommonProcs) == 0) {
						if (IndentedObjectFunctor.CommonProc(null, args.Value, context)) args.WriteProc = IndentedObjectFunctor.CommonProc;
					}
				}
				// handle
				if (null != handle) {
					//handle(context, mi, value, ref writeproc, ref iwvo, ref isdefault);
					handle(null, args);
				}
				if (args.IsCancelAll) break;
				if (args.IsCancel) continue;
				// show.
				if (args.HasDefault) {
					if (null == args.Value && null == args.AppendComment) args.AppendComment = AppendComment2;
					WriteLineValue(iw, args.MemberName, args.Value, args.ValueOptions, args.AppendComment);
					if (null != args.WriteProc) {
						args.WriteProc(iw, args.Value, context);
					}
				}
			}
			// notify begin.
			if (null != context) {
				context.NotifyForEachMemberEnd(iw, owner, tp, options, handle, context);
			}
			rt = true;
			return rt;
		}

		/// <summary>
		/// <see cref="WriteSimpleMethod"/>的bool数组.
		/// </summary>
		private static readonly bool[] WriteSimpleMethod_List_bool = new bool[] { false, true };
		
		/// <summary>
		/// 输出简单方法的信息.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="mi">方法信息.</param>
		/// <param name="owner">所在对象.</param>
		/// <param name="name">方法名称. 为null表示默认.</param>
		/// <param name="options">选项.</param>
		/// <param name="appendcomment">追加注释. 可为 null 或 <c>String.Empty</c>. </param>
		/// <returns>若成功输出, 便返回true. 否则返回false.</returns>
		/// <remarks>
		/// 当 <paramref name="options"/> 具有 <see cref="IndentedWriterMemberOptions.AllowMethod"/> 标志时，还会显示方法信息. 方法必须有返回值, 且没有泛型参数, 其他情况有:
		/// <para>(暂不支持: 参数数量为0个);</para>
		/// <para>参数数量为1个, 且参数类型为枚举或bool.</para>
		/// </remarks>
		public static bool WriteSimpleMethod(IIndentedWriter iw, MethodInfo mi, object owner, string name, IndentedWriterValueOptions options, string appendcomment) {
			if (null == iw) return false;
			if (null == mi) return false;
			if (mi.IsSpecialName) return false;
			if (null == mi.ReturnType) return false;
			if (mi.ReturnType.Equals(typeof(void))) return false;
			ParameterInfo[] pis = mi.GetParameters();
			if (false) {
			}
			else if (0 == pis.Length) {
				// 暂不支持.
				return false;
			}
			else if (1 == pis.Length && !pis[0].IsOut) {
				Type argtype0 = pis[0].ParameterType;
#if (NETFX_CORE)
				TypeInfo argtype0info = argtype0.GetTypeInfo();
#endif
				IEnumerable lst = null;	// 参数可用值列表.
				string nameformat = null;	// 标题的格式.
				if (false) {
				}
#if (NETFX_CORE)
				else if (argtype0info.IsEnum) {
#else
				else if (argtype0.IsEnum) {
#endif
					lst = TypeUtil.GetEnumValues(argtype0);
					nameformat = "{0:d}(0x{0:X}, {0})";
				}
				else if (argtype0.Equals(typeof(bool))) {
					lst = WriteSimpleMethod_List_bool;
					nameformat = "{0}";
				}
				// show.
				if (null != lst) {
					object[] args = new object[1];
					if (null == name) name = TypeUtil.GetMemberName(mi, DefaultMemberNameOption);
					IndentedWriterUtil.WriteLineValue(iw, name, null, options | IndentedWriterValueOptions.AutoHideValue, appendcomment);
					iw.Indent(null);
					foreach (object p in lst) {
						string strname = string.Format(nameformat, p);
						string strappend = null;
						object v = null;
						try {
							args[0] = p;
							v = mi.Invoke(owner, args);
						}
						catch {
						}
						if (null == v) strappend = string.Format("<{0}>", mi.ReturnType.Name);
						IndentedWriterUtil.WriteLineValue(iw, strname, v, options, strappend);
					}
					iw.Unindent();
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 检查类型是否存在可输出的静态成员.
		/// </summary>
		/// <param name="tp"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static bool TypeHasStatic(Type tp, IndentedWriterMemberOptions options) {
			if (null == tp) return false;
			if (0==(options & IndentedWriterMemberOptions.OnlyStatic)) return false;
#if (NETFX_CORE)
			if (tp.GetTypeInfo().IsEnum) return false;
#else
			if (tp.IsEnum) return false;
#endif
			foreach (MemberInfo mi in GetMembers(tp, options)) {
				FieldInfo fi = mi as FieldInfo;
				if (null != fi) {
					if (fi.IsStatic) return true;
				}
				PropertyInfo pi = mi as PropertyInfo;
				if (null != pi) {
					if (pi.CanRead) return true;
				}
				if (0 != (options | IndentedWriterMemberOptions.AllowMethod)) {
					MethodInfo mti = mi as MethodInfo;
					if (null != mti) {
						if (mti.IsStatic && !mti.IsSpecialName) return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// 输出类型的静态成员, 拥有选项参数.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="tp">type.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <param name="options">选项. 必须有 <see cref="IndentedWriterMemberOptions.OnlyStatic"/> 标志.</param>
		/// <returns>返回是否成功输出.</returns>
		/// <remarks>
		/// 当 <paramref name="options"/> 具有 <see cref="IndentedWriterMemberOptions.AllowMethod"/> 标志时，还会显示方法信息. 方法必须有返回值, 且没有泛型参数, 其他情况有:
		/// <para>参数数量为0个;</para>
		/// <para>参数数量为1个, 且参数类型为枚举或bool.</para>
		/// </remarks>
		public static bool WriteTypeStatic(IIndentedWriter iw, Type tp, IndentedWriterContext context, IndentedWriterMemberOptions options) {
			if (null == iw) return false;
			if (null == tp) return false;
			if (0 == (options & IndentedWriterMemberOptions.OnlyStatic)) return false;
			if (!iw.Indent(tp)) return false;
			iw.WriteLine(string.Format("# <{0}>", tp.FullName));
			//IndentedWriterUtil.ForEachMember(iw, null, tp, options, null, context);
			IndentedWriterUtil.ForEachMember(iw, null, tp, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
				MethodInfo memberinfo = e.MemberInfo as MethodInfo;
				if (null != memberinfo) {
					//string name = memberinfo.Name;
					if (memberinfo.IsStatic
							&& !memberinfo.IsSpecialName
							&& null != memberinfo.ReturnType
							&& !memberinfo.ReturnType.Equals(typeof(void))) {
						ParameterInfo[] pis = memberinfo.GetParameters();
						if (false) {
						}
						else if (0 == pis.Length) {
							try {
								e.Value = memberinfo.Invoke(null, null);
								if (null == e.WriteProc) e.WriteProc = IndentedObjectFunctor.CommonProc;
								if (null == e.Value && null == e.AppendComment) e.AppendComment = string.Format("<{0}>", memberinfo.ReturnType.Name);
								e.HasDefault = true;
							}
							catch {
								// 忽略.
							}
						}
						else if (WriteSimpleMethod(iw, memberinfo, null, null, IndentedWriterValueOptions.Default, e.AppendComment)) {
							e.IsCancel = true;
						}
					}
				}
			}, context);
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// 输出类型的静态成员, 默认输出字段与属性.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="tp">type.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool WriteTypeStatic(IIndentedWriter iw, Type tp, IndentedWriterContext context) {
			return WriteTypeStatic(iw, tp, context, IndentedWriterMemberOptions.OnlyStatic);
		}

		/// <summary>
		/// 输出类型的静态成员, 会输出字段,属性与方法.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="tp">type.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool WriteTypeStaticM(IIndentedWriter iw, Type tp, IndentedWriterContext context) {
			return WriteTypeStatic(iw, tp, context, IndentedWriterMemberOptions.OnlyStatic | IndentedWriterMemberOptions.AllowMethod);
		}

		/// <summary>
		/// 输出派生类及成员.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="basetype">Base Class type.</param>
		/// <param name="assembly">Assembly.</param>
		/// <param name="context">State Object. Can be null.</param>
		/// <returns>返回是否成功输出.</returns>
		public static bool WriteDerivedClass(IIndentedWriter iw, Type basetype, Assembly assembly, IndentedWriterContext context) {
			if (null == iw) return false;
			if (null == basetype) return false;
			//Type basetype = typeof(Calendar);
#if (NETFX_CORE)
			TypeInfo tiBase = basetype.GetTypeInfo();
#endif
			//Assembly assembly = basetype.Assembly;
			if (null == assembly) {
#if (NETFX_CORE)
				assembly = tiBase.Assembly;
#else
				assembly = basetype.Assembly;
#endif
			}
			if (!iw.Indent(basetype)) return false;
			iw.WriteLine("# DerivedClass of <{0}>:", basetype.FullName);
			int cnt = 0;
			IEnumerable<Type> lst = null;
#if (NETFX_CORE)
			lst = assembly.ExportedTypes;
#else
			lst = assembly.GetExportedTypes();
#endif
			foreach (Type tp in lst) {
				// check.
#if (NETFX_CORE)
				TypeInfo ti = tp.GetTypeInfo();
				if (!ti.IsAssignableFrom(tiBase)) continue;
#else
				if (tp.IsAbstract) continue;
				if (!tp.IsSubclassOf(basetype)) continue;
#endif
				// new.
				object ob = null;
				try {
					ob = Activator.CreateInstance(tp);
				}
				catch {
					// 忽略.
				}
				if (null == ob) continue;
				// write.
				iw.WriteLine("DerivedClass[{0}]:\t# <{1}>", cnt, tp.Name);
				WriteTypeStatic(iw, tp, context);
				IndentedObjectFunctor.CommonProc(iw, ob, context);
				++cnt;
			}
			iw.Unindent();
			return true;
		}

	}
}
