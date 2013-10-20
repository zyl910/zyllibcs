using System;
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
		/// 基本绑定.
		/// </summary>
		private BindingFlags m_BaseBinding;

		/// <summary>
		/// 输出选项.
		/// </summary>
		IndentedWriterMemberOptions m_WriterOptions;

		/// <summary>
		/// 成员的输出过程集.
		/// </summary>
		private IEnumerable<IndentedWriterObjectProc> m_MemberProcs;

		///// <summary>
		///// 用户自定义数据.
		///// </summary>
		//private object m_Userdata;

		///// <summary>
		///// 输出成员信息时的处理过程.
		///// </summary>
		//private IndentedWriterHandleMemberProc m_MemberHandleProc;

		///// <summary>
		///// 当前输出者.
		///// </summary>
		//private IIndentedWriter m_CurrentWriter = null;

		///// <summary>
		///// 当前对象.
		///// </summary>
		//private object m_CurrentObject = null;

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
		/// 基本绑定.
		/// </summary>
		public BindingFlags BaseBinding {
			get { return m_BaseBinding; }
			set { m_BaseBinding = value; }
		}

		/// <summary>
		/// 输出选项.
		/// </summary>
		public IndentedWriterMemberOptions WriterOptions {
			get { return m_WriterOptions; }
			set { m_WriterOptions = value; }
		}

		/// <summary>
		/// 成员的输出过程集.
		/// </summary>
		public IEnumerable<IndentedWriterObjectProc> MemberProcs {
			get { return m_MemberProcs; }
			set { m_MemberProcs = value; }
		}

		///// <summary>
		///// 用户自定义数据.
		///// </summary>
		//public object Userdata {
		//    get { return m_Userdata; }
		//    set { m_Userdata = value; }
		//}

		///// <summary>
		///// 输出成员信息时的处理过程.
		///// </summary>
		//public IndentedWriterHandleMemberProc MemberHandleProc {
		//    get { return m_MemberHandleProc; }
		//    set { m_MemberHandleProc = value; }
		//}

		///// <summary>
		///// 当前输出者.
		///// </summary>
		//public IIndentedWriter CurrentWriter {
		//    get { return m_CurrentWriter; }
		//}

		///// <summary>
		///// 当前对象.
		///// </summary>
		//public object CurrentObject {
		//    get { return m_CurrentObject; }
		//}

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
		/// <param name="basebinding">基本绑定.</param>
		/// <param name="writeroptions">输出选项.</param>
		/// <param name="memberprocs">成员的输出过程集.</param>
		/// <param name="tag">用户自定义数据.</param>
		public IndentedObjectFunctor(Type basetype, IndentedObjectFunctorOptions options, BindingFlags basebinding, IndentedWriterMemberOptions writeroptions, IEnumerable<IndentedWriterObjectProc> memberprocs, object tag)
			: base() {
			m_BaseType = basetype;
			m_Options = options;
			m_BaseBinding = basebinding;
			m_WriterOptions = writeroptions;
			m_MemberProcs = memberprocs;
			m_Tag = tag;
		}

		/// <summary>
		/// 构造 IndentedWriterObjectFunctor 对象, 拥有 类型,选项,绑定 等参数.
		/// </summary>
		/// <param name="basetype">基本类型.</param>
		/// <param name="options">选项.</param>
		/// <param name="basebinding">基本绑定.</param>
		public IndentedObjectFunctor(Type basetype, IndentedObjectFunctorOptions options, BindingFlags basebinding)
			: this(basetype, options, basebinding, IndentedWriterMemberOptions.Default, null, null) {
		}

		/// <summary>
		/// 构造 IndentedWriterObjectFunctor 对象, 拥有 类型,选项 等参数.
		/// </summary>
		/// <param name="basetype">基本类型.</param>
		/// <param name="options">选项.</param>
		public IndentedObjectFunctor(Type basetype, IndentedObjectFunctorOptions options)
			: this(basetype, options, IndentedWriterUtil.PublicInstance) {
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
		/// <param name="context">State Object.</param>
		/// <returns>当<paramref name="iw"/>为null时, 返回是否支持输出. 否则返回是否成功输出.</returns>
		public bool WriterObject(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			return WriterObject_Core(iw, obj, context);
		}

		/// <summary>
		/// 输出对象_内部版.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">object. If <paramref name="obj"/> is null, result alway is false.</param>
		/// <param name="context">State Object.</param>
		/// <returns>当<paramref name="iw"/>为null时, 返回是否支持输出. 否则返回是否成功输出.</returns>
		protected virtual bool WriterObject_Core(IIndentedWriter iw, object obj, IndentedWriterContext context) {
			// check.
			if (null == obj) return false;
			if (null == m_BaseType) return false;
			Type tp = obj.GetType();
			bool showBaseName = false;	// 是否显示基类类型的名称.
			if (tp.IsEnum) return false;
			if (tp.IsPointer) return false;
			if (tp.IsPrimitive) return false;
			if ((m_Options & IndentedObjectFunctorOptions.OnlyType) != 0) {
				if (!m_BaseType.Equals(tp)) return false;
			}
			else {
				if (!m_BaseType.IsInstanceOfType(obj)) return false;
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
			if (null == iw) return true;
			// write.
			if ((m_Options & IndentedObjectFunctorOptions.NotWrite) != 0) return true;
			if (!iw.Indent(obj)) return false;
			bool needtitle = true;
			try {
				IndentedWriterUtil.ForEachMember(iw, obj, tp, m_BaseBinding, m_WriterOptions, m_MemberProcs, delegate(object sender, IndentedWriterMemberEventArgs e) {
					//Debug.WriteLine(string.Format("{0}: {1}", mi.Name, mi.MemberType));
					if (needtitle && null!=e && e.HasDefault) {
						// 仅当至少有一个成员, 才输出标题.
						needtitle = false;
						string title;
						if (showBaseName) title = string.Format("# <{0}>\tBase: {1}", GetTypeName(tp), GetTypeName(m_BaseType));
						else title = string.Format("# <{0}>", GetTypeName(tp));
						iw.WriteLine(title);
					}
					OnHandlerMember(this, e);
				}, context);
			}
			catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex);
			}
			iw.Unindent();
			return !needtitle;
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

		///// <summary>
		///// 当需要处理输出成员信息时.
		///// </summary>
		///// <param name="userdata">用户自定义对象.</param>
		///// <param name="mi">成员信息.</param>
		///// <param name="value">值.</param>
		///// <param name="writeproc">匹配的输出过程.</param>
		///// <param name="iwvo">输出数值的选项. 注意但不是字段或属性时, 其初始值不同.</param>
		///// <param name="isdefault">是否进行默认处理. 若不需要进行默认处理, 便返回false. 注意但不是字段或属性时, 其初始值不同.</param>
		///// <remarks>
		///// 若 <paramref name="isdefault"/> 为 true, 则默认会调用 <see cref="IndentedWriterUtil.WriteLineValue"/> 输出值的信息行, 再使用 <paramref name="writeproc"/> 输出值的详细内容 .
		///// 如果你想定制输出信息, 请将 <paramref name="isdefault"/> 设为 false, 并自行调用 <see cref="IndentedWriterUtil.WriteLineValue"/> 与 <see cref="IndentedWriterObjectProc"/> .
		///// 注意默认操作仅支持字段与(非索引化的)属性.
		///// </remarks>
		//protected virtual void OnHandleMemberProc(object userdata, MemberInfo mi, object value, ref IndentedWriterObjectProc writeproc, ref IndentedWriterValueOptions iwvo, ref bool isdefault) {
		//    IndentedWriterHandleMemberProc proc = m_MemberHandleProc;
		//    if (null != proc) proc(userdata, mi, value, ref writeproc, ref iwvo, ref isdefault);
		//}

	}
}
