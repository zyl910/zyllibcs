using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace zyllibcs.text {
	/// <summary>
	/// 带格式输出成员时的事件.
	/// </summary>
	/// <remarks>
	/// 该事件有3种处理策略:
	/// <para>1. 默认。不修改任何属性，默认处理.</para>
	/// <para>2. 完全自定义。将 <see cref="IsCancel"/> 设为true，阻止默认处理，由事件处理者负责输出（或不输出）.</para>
	/// <para>3. 半自定义。调整本类的属性（如 <see cref="ValueOptions"/>,<see cref="AppendComment"/>,<see cref="WriteProc"/> ），然后再交由默认处理.</para>
	/// <para></para>
	/// <para>可通过 描述中有“[原参数]”的属性 获得 <see cref="IndentedWriterUtil.ForEachMember"/> 的参数, 在事件中不要更改这些属性. </para>
	/// </remarks>
	public class IndentedWriterMemberEventArgs : EventArgs {
		/// <summary>
		/// 是否取消当前成员的默认处理. (推荐修改) .
		/// </summary>
		private bool m_IsCancel;

		/// <summary>
		/// 是否取消所有操作，直接退出. (可修改) .
		/// </summary>
		private bool m_IsCancelAll;

		/// <summary>
		/// 是否存在默认处理.
		/// </summary>
		private bool m_HasDefault;

		/// <summary>
		/// 成员信息.
		/// </summary>
		private MemberInfo m_MemberInfo;

		/// <summary>
		/// 成员名. (可修改) .
		/// </summary>
		private string m_MemberName;

		/// <summary>
		/// 成员值.
		/// </summary>
		private object m_Value;

		/// <summary>
		/// 值的输出选项. (推荐修改) .
		/// </summary>
		private IndentedWriterValueOptions m_ValueOptions;

		/// <summary>
		/// 追加注释. (推荐修改) .
		/// </summary>
		string m_AppendComment;

		/// <summary>
		/// 值的详细输出过程. (推荐修改) .
		/// </summary>
		private IndentedWriterObjectProc m_WriteProc;

		/// <summary>
		/// [原参数]带缩进输出者.
		/// </summary>
		private IIndentedWriter m_IndentedWriter;

		/// <summary>
		/// [原参数]欲查询成员的对象.
		/// </summary>
		private object m_Owner;

		/// <summary>
		/// [原参数]欲查询成员的对象的类型.
		/// </summary>
		private Type m_OwnerType;

		/// <summary>
		/// [原参数]成员选项.
		/// </summary>
		private IndentedWriterMemberOptions m_MemberOptions;

		///// <summary>
		///// [原参数]输出过程的集合.
		///// </summary>
		//IEnumerable<IndentedWriterObjectProc> m_Procs;

		/// <summary>
		/// Context Object.
		/// </summary>
		private IndentedWriterContext m_Context;

		/// <summary>
		/// 是否取消当前成员的默认处理. (推荐修改) .
		/// </summary>
		public bool IsCancel {
			get { return m_IsCancel; }
			set { m_IsCancel = value; }
		}

		/// <summary>
		/// 是否取消所有操作，直接退出. (可修改) .
		/// </summary>
		public bool IsCancelAll {
			get { return m_IsCancelAll; }
			set { m_IsCancelAll = value; }
		}

		/// <summary>
		/// 是否存在默认处理.
		/// </summary>
		public bool HasDefault {
			get { return m_HasDefault; }
			set { m_HasDefault = value; }
		}

		/// <summary>
		/// 成员信息.
		/// </summary>
		public MemberInfo MemberInfo {
			get { return m_MemberInfo; }
			set { m_MemberInfo = value; }
		}

		/// <summary>
		/// 成员名. (可修改) .
		/// </summary>
		public string MemberName {
			get { return m_MemberName; }
			set { m_MemberName = value; }
		}

		/// <summary>
		/// 成员值.
		/// </summary>
		public object Value {
			get { return m_Value; }
			set { m_Value = value; }
		}

		/// <summary>
		/// 值的输出选项. (推荐修改) .
		/// </summary>
		public IndentedWriterValueOptions ValueOptions {
			get { return m_ValueOptions; }
			set { m_ValueOptions = value; }
		}

		/// <summary>
		/// 追加注释. (推荐修改) .
		/// </summary>
		public string AppendComment {
			get { return m_AppendComment; }
			set { m_AppendComment = value; }
		}

		/// <summary>
		/// 值的详细输出过程. (推荐修改) .
		/// </summary>
		public IndentedWriterObjectProc WriteProc {
			get { return m_WriteProc; }
			set { m_WriteProc = value; }
		}

		/// <summary>
		/// [原参数]带缩进输出者.
		/// </summary>
		public IIndentedWriter IndentedWriter {
			get { return m_IndentedWriter; }
			set { m_IndentedWriter = value; }
		}

		/// <summary>
		/// [原参数]欲查询成员的对象.
		/// </summary>
		public object Owner {
			get { return m_Owner; }
			set { m_Owner = value; }
		}

		/// <summary>
		/// [原参数]欲查询成员的对象的类型.
		/// </summary>
		public Type OwnerType {
			get { return m_OwnerType; }
			set { m_OwnerType = value; }
		}

		/// <summary>
		/// [原参数]成员选项.
		/// </summary>
		public IndentedWriterMemberOptions MemberOptions {
			get { return m_MemberOptions; }
			set { m_MemberOptions = value; }
		}

		///// <summary>
		///// [原参数]输出过程的集合.
		///// </summary>
		//public IEnumerable<IndentedWriterObjectProc> Procs {
		//    get { return m_Procs; }
		//    set { m_Procs = value; }
		//}

		/// <summary>
		/// Context Object.
		/// </summary>
		public IndentedWriterContext Context {
			get { return m_Context; }
			set { m_Context = value; }
		}

		/// <summary>
		/// 构造 IndentedWriterMemberEventArgs 对象.
		/// </summary>
		public IndentedWriterMemberEventArgs()
			: base() {
		}

	}
}
