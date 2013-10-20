using System;
using System.Collections.Generic;
using System.Text;

namespace zyllibcs.text {
	/// <summary>
	/// 带格式输出时的环境.
	/// </summary>
	public class IndentedWriterContext {

		/// <summary>
		/// [原参数]成员选项.
		/// </summary>
		private IndentedWriterMemberOptions m_MemberOptions = IndentedWriterMemberOptions.Default;

		/// <summary>
		/// 输出过程表.
		/// </summary>
		IEnumerable<IndentedWriterObjectProc> m_Procs;

		/// <summary>
		/// 用户自定义数据.
		/// </summary>
		private object m_Tag;

		/// <summary>
		/// 成员选项. 输出成员时( <see cref="IndentedWriterUtil.ForEachMember"/> )会将此属性与 options 参数做或运算.
		/// </summary>
		public IndentedWriterMemberOptions MemberOptions {
			get { return m_MemberOptions; }
			set { m_MemberOptions = value; }
		}

		/// <summary>
		/// 输出过程表. 输出成员时( <see cref="IndentedWriterUtil.ForEachMember"/> )会优先在该表格中查找匹配的输出过程.
		/// </summary>
		public IEnumerable<IndentedWriterObjectProc> Procs {
			get { return m_Procs; }
			set { m_Procs = value; }
		}

		/// <summary>
		/// 用户自定义数据.
		/// </summary>
		public object Tag {
			get { return m_Tag; }
			set { m_Tag = value; }
		}

		/// <summary>
		/// 构造 IndentedWriterContext 对象.
		/// </summary>
		public IndentedWriterContext()
			: base() {
		}
	}
}
