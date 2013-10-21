using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;

namespace zyllibcs.text {
	/// <summary>
	/// 带缩进输出对象的函数对象的选项.
	/// </summary>
	/// <remarks>总是不支持: 枚举类型.</remarks>
	public enum IndentedObjectFunctorOptions {
		/// <summary>
		/// 默认.
		/// </summary>
		Default = 0,
		/// <summary>
		/// 仅支持当前类型, 而不支持派生类型.
		/// </summary>
		OnlyType = 1,
		/// <summary>
		/// 仅支持当前类型的成员, 而不显示派生类型的成员.
		/// </summary>
		OnlyMember = 2,
		/// <summary>
		/// 允许简单类型. 如 <see cref="String"/>, <see cref="Decimal"/>, <see cref="DateTime"/>, <see cref="TimeSpan"/> .
		/// </summary>
		AllowSimple = 4,
		/// <summary>
		/// 不输出.
		/// </summary>
		NotWrite = 8,
		/// <summary>
		/// 不枚举集合或数组的条目.
		/// </summary>
		NoEnumerate = 0x10,
		/// <summary>
		/// 集合类型不使用简单格式, 而是详细输出每一个成员. 无此标志时, 默认会隐藏 Keys、Values 属性.
		/// </summary>
		EnumerateNoSimple = 0x10,
		/// <summary>
		/// 显示数组成员. 默认情况下不显示数组的成员, 只显示条目.
		/// </summary>
 		ArrayMember = 0x20,
	}

	/// <summary>
	/// 带缩进输出对象的函数子. 可将它的 <see cref="WriterObject"/> 方法 转为 <see cref="IndentedWriterObjectProc"/> 委托.
	/// </summary>
	/// <remarks>不支持的类型: Enum、Pointer、Primitive.</remarks>
	public class IndentedObjectFunctor {
		/// <summary>
		/// 公用函数子.
		/// </summary>
		private static readonly IndentedObjectFunctor CommonFunctor = new IndentedObjectFunctor(typeof(object));

		/// <summary>
		/// 公用输出过程.
		/// </summary>
		public static readonly IndentedWriterObjectProc CommonProc = CommonFunctor.WriterObject;

		/// <summary>
		/// 选项.
		/// </summary>
		private IndentedObjectFunctorOptions m_Options;

		/// <summary>
		/// 基本类型.
		/// </summary>
		private Type m_BaseType;

		/// <summary>
		/// 输出选项.
		/// </summary>
		IndentedWriterMemberOptions m_WriterOptions;

		/// <summary>
		/// 用户自定义数据.
		/// </summary>
		private object m_Tag;

		/// <summary>
		/// 处理成员时的事件.
		/// </summary>
		public event EventHandler<IndentedWriterMemberEventArgs> HandlerMember;

		/// <summary>
		/// 选项.
		/// </summary>
		public IndentedObjectFunctorOptions Options {
			get { return m_Options; }
			set { m_Options = value; }
		}

		/// <summary>
		/// 基本类型.
		/// </summary>
		public Type BaseType {
			get { return m_BaseType; }
			//set { m_BaseType = value; }
		}

		/// <summary>
		/// 输出选项.
		/// </summary>
		public IndentedWriterMemberOptions WriterOptions {
			get { return m_WriterOptions; }
			set { m_WriterOptions = value; }
		}

		/// <summary>
		/// 用户自定义数据.
		/// </summary>
		public object Tag {
			get { return m_Tag; }
			set { m_Tag = value; }
		}

		/// <summary>
		/// 构造 IndentedWriterObjectFunctor 对象, 拥有 类型,选项,绑定,输出选项,输出过程集,自定义数据 等参数.
		/// </summary>
		/// <param name="basetype">基本类型.</param>
		/// <param name="options">选项.</param>
		/// <param name="writeroptions">输出选项.</param>
		/// <param name="memberprocs">成员的输出过程集.</param>
		/// <param name="tag">用户自定义数据.</param>
		public IndentedObjectFunctor(Type basetype, IndentedObjectFunctorOptions options, IndentedWriterMemberOptions writeroptions, IEnumerable<IndentedWriterObjectProc> memberprocs, object tag)
			: base() {
			m_BaseType = basetype;
			m_Options = options;
			m_WriterOptions = writeroptions;
			//m_MemberProcs = memberprocs;
			m_Tag = tag;
		}

		/// <summary>
		/// 构造 IndentedWriterObjectFunctor 对象, 拥有 类型,选项 等参数.
		/// </summary>
		/// <param name="basetype">基本类型.</param>
		/// <param name="options">选项.</param>
		public IndentedObjectFunctor(Type basetype, IndentedObjectFunctorOptions options)
			: this(basetype, options, IndentedWriterMemberOptions.Default, null, null) {
		}

		/// <summary>
		/// 构造 IndentedWriterObjectFunctor 对象, 拥有 类型 等参数.
		/// </summary>
		/// <param name="basetype">基本类型.</param>
		public IndentedObjectFunctor(Type basetype)
			: this(basetype, IndentedObjectFunctorOptions.Default) {
		}

		/// <summary>
		/// 取得类型名称.
		/// </summary>
		/// <param name="tp">type.</param>
		/// <returns>返回名称.</returns>
		/// <remarks>默认返回 <c>MemberInfoFormat.GetMemberName(tp, IndentedWriterUtil.DefaultTypeNameOption)</c>. </remarks>
		public static string GetTypeName(Type tp) {
			return MemberInfoFormat.GetMemberName(tp, IndentedWriterUtil.DefaultTypeNameOption);
		}

		/// <summary>
		/// 输出对象.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. If <paramref name="obj"/> is null, result alway is false.</param>
		/// <param name="context">Context.</param>
		/// <returns>当<paramref name="iw"/>为null时, 返回是否支持输出. 否则返回是否成功输出.</returns>
		public bool WriterObject(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			Type tp;
			bool showBaseName;
			// check.
			if (!WriterObject_CheckObject(iw, obj, context, out tp, out showBaseName)) return false;
			if (null == iw) return true;
			// write.
			return WriterObject_Write(iw, obj, context, tp, showBaseName);
		}

		/// <summary>
		/// 输出对象_检查对象.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object.</param>
		/// <param name="context">Context.</param>
		/// <param name="tp">返回 类型.</param>
		/// <param name="showBaseName">返回 是否显示基类类型的名称.</param>
		/// <returns>检查是否支持此对象的输出, 支持的话返回true, 否则返回false.</returns>
		public virtual bool WriterObject_CheckObject(IIndentedWriter iw, object obj, IndentedWriterContext context, out Type tp, out bool showBaseName) {
			// check.
			tp = obj.GetType();
			showBaseName = false;	// 是否显示基类类型的名称.
			if (null == obj) return false;
			if (null == m_BaseType) return false;
#if NETFX_CORE
			TypeInfo ti = tp.GetTypeInfo();
			if (ti.IsEnum) return false;
			if (ti.IsPointer) return false;
			if (ti.IsPrimitive) return false;
#else
			if (tp.IsEnum) return false;
			if (tp.IsPointer) return false;
			if (tp.IsPrimitive) return false;
#endif
			if ((m_Options & IndentedObjectFunctorOptions.OnlyType) != 0) {
				if (!m_BaseType.Equals(tp)) return false;
			}
			else {
#if NETFX_CORE
				if (!m_BaseType.GetTypeInfo().IsAssignableFrom(ti)) return false;
#else
				if (!m_BaseType.IsInstanceOfType(obj)) return false;
#endif
				if ((m_Options & IndentedObjectFunctorOptions.OnlyMember) != 0) {
					tp = m_BaseType;
				}
				else {
					showBaseName = !m_BaseType.Equals(tp) && !m_BaseType.Equals(typeof(object));
				}
			}
			if ((m_Options & IndentedObjectFunctorOptions.AllowSimple) == 0) {
				if (IndentedWriterUtil.IsSimpleType(tp)) return false;
			}
			return true;
		}

		/// <summary>
		/// 输出对象_输出.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object.</param>
		/// <param name="context">Context.</param>
		/// <param name="tp">类型.</param>
		/// <param name="showBaseName">是否显示基类类型的名称.</param>
		/// <returns>检查是否支持此对象的输出, 支持的话返回true, 否则返回false.</returns>
		public virtual bool WriterObject_Write(IIndentedWriter iw, object obj, IndentedWriterContext context, Type tp, bool showBaseName) {
			// write.
			if ((m_Options & IndentedObjectFunctorOptions.NotWrite) != 0) return true;
			if (!iw.Indent(obj)) return false;
			//if (null != context) {
			//    foreach (KeyValuePair<Type, object> p in context.TypeOwners) {
			//        iw.Write(p.Key.FullName);
			//        iw.Write('/');
			//    }
			//    iw.WriteLine();
			//}
			bool needtitle = true;
			bool showmember = true;	// 显示成员.
			if (0 == (m_Options & IndentedObjectFunctorOptions.ArrayMember) && tp.IsArray) showmember = false;
			if (showmember) {
				WriterObject_WriteMember(iw, obj, context, tp, showBaseName, ref needtitle);
			}
			else {
				WriterObject_WriteTitle(iw, obj, context, tp, showBaseName);
			}
			WriterObject_WriteEnumerate(iw, obj, context, tp);
			iw.Unindent();
			return !needtitle;
		}

		/// <summary>
		/// 输出对象_输出标题.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object.</param>
		/// <param name="context">Context.</param>
		/// <param name="tp">类型.</param>
		/// <param name="showBaseName">是否显示基类类型的名称.</param>
		public virtual void WriterObject_WriteTitle(IIndentedWriter iw, object obj, IndentedWriterContext context, Type tp, bool showBaseName) {
			string title;
			if (showBaseName) title = string.Format("# <{0}>\tBase: {1}", GetTypeName(tp), GetTypeName(m_BaseType));
			else title = string.Format("# <{0}>", GetTypeName(tp));
			iw.WriteLine(title);
		}

		/// <summary>
		/// 非 <see cref="IndentedObjectFunctorOptions.EnumerateNoSimple"/> 模式下默认会隐藏的属性名.
		/// </summary>
		public static readonly string[] EnumerateSimpleNames = new string[] {
			"Keys",
			"Values",
		};

		/// <summary>
		/// 输出对象_输出成员.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object.</param>
		/// <param name="context">Context.</param>
		/// <param name="tp">类型.</param>
		/// <param name="showBaseName">是否显示基类类型的名称.</param>
		/// <param name="needtitle">是否需要输出标题.</param>
		public virtual void WriterObject_WriteMember(IIndentedWriter iw, object obj, IndentedWriterContext context, Type tp, bool showBaseName, ref bool needtitle) {
			bool needtitle2 = needtitle;
			try {
				IndentedWriterUtil.ForEachMember(iw, obj, tp, m_WriterOptions, delegate(object sender, IndentedWriterMemberEventArgs e) {
					//Debug.WriteLine(string.Format("{0}: {1}", mi.Name, mi.MemberType));
					if (needtitle2 && null != e && e.HasDefault) {
						// 仅当至少有一个成员, 才输出标题.
						needtitle2 = false;
						WriterObject_WriteTitle(iw, obj, context, tp, showBaseName);
					}
					// EnumerateNoSimple.
					if (0 == (m_Options & IndentedObjectFunctorOptions.EnumerateNoSimple) && null!=e.MemberInfo) {
						PropertyInfo pi = e.MemberInfo as PropertyInfo;
						if (null != pi) {
							string name = pi.Name;
							foreach (string s in EnumerateSimpleNames) {
								if (IndentedWriterUtil.StringComparer.Equals(s, name)) {
									e.IsCancel = true;
									break;
								}
							}
						}
					}
					//
					OnHandlerMember(this, e);
				}, context);
			}
			catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex);
			}
			needtitle = needtitle2;
		}

		/// <summary>
		/// 输出对象_枚举条目.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object.</param>
		/// <param name="context">Context.</param>
		/// <param name="tp">类型.</param>
		public virtual void WriterObject_WriteEnumerate(IIndentedWriter iw, object obj, IndentedWriterContext context, Type tp) {
			if (0 != (m_Options & IndentedObjectFunctorOptions.NoEnumerate)) return;
			IEnumerable lst = obj as IEnumerable;
			if (null == lst) return;
			int i = 0;
			foreach (object p in lst) {
				string name = string.Format("[{0}]", i);
				IndentedWriterObjectProc proc = null;
				if (null != context) {
					proc = IndentedWriterUtil.LookupWriteProcAt(p, context, context.Procs);
				}
				if (null == proc && (m_WriterOptions & IndentedWriterMemberOptions.NoCommonProcs) == 0) {
					if (IndentedObjectFunctor.CommonProc(null, p, context)) proc = IndentedObjectFunctor.CommonProc;
				}
				IndentedWriterUtil.WriteLineValue(iw, name, p, IndentedWriterValueOptions.Default, null);
				if (null != proc) {
					proc(iw, p, context);
				}
				// next.
				++i;
			}
		}

		/// <summary>
		/// 当需要处理输出成员信息时.
		/// </summary>
		/// <param name="sender">发送者.</param>
		/// <param name="e">事件参数.</param>
		protected virtual void OnHandlerMember(object sender, IndentedWriterMemberEventArgs e) {
			EventHandler<IndentedWriterMemberEventArgs> handler = HandlerMember;
			if (null != handler) handler(sender, e);
		}

	}
}
