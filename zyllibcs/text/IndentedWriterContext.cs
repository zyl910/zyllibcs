using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace zyllibcs.text {
	/// <summary>
	/// 带格式输出时的环境.
	/// </summary>
	public class IndentedWriterContext {

		/// <summary>
		/// 成员选项.
		/// </summary>
		private IndentedWriterMemberOptions m_MemberOptions = IndentedWriterMemberOptions.Default;

		/// <summary>
		/// 输出过程表.
		/// </summary>
		private IEnumerable<IndentedWriterObjectProc> m_Procs;

		/// <summary>
		/// 开始枚举成员前.
		/// </summary>
		private IndentedWriterForEachMemberNotify m_ForEachMemberBegin;

		/// <summary>
		/// 枚举成员结束后.
		/// </summary>
		private IndentedWriterForEachMemberNotify m_ForEachMemberEnd;

		/// <summary>
		/// 每层的类型对象信息.
		/// </summary>
		private readonly List<KeyValuePair<Type, object>> m_TypeOwners = new List<KeyValuePair<Type, object>>();


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
		/// 开始枚举成员前.
		/// </summary>
		public IndentedWriterForEachMemberNotify ForEachMemberBegin {
			get { return m_ForEachMemberBegin; }
			set { m_ForEachMemberBegin = value; }
		}

		/// <summary>
		/// 枚举成员结束后.
		/// </summary>
		public IndentedWriterForEachMemberNotify ForEachMemberEnd {
			get { return m_ForEachMemberEnd; }
			set { m_ForEachMemberEnd = value; }
		}

		/// <summary>
		/// 每层的类型对象信息.
		/// </summary>
		public List<KeyValuePair<Type, object>> TypeOwners {
			get { return m_TypeOwners; }
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

		/// <summary>
		/// 通知开始枚举成员.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="owner">欲查询成员的对象.</param>
		/// <param name="tp">类型.</param>
		/// <param name="bindingAttr">绑定标志.</param>
		/// <param name="options">成员选项. </param>
		/// <param name="handle">每个成员的处理过程. </param>
		/// <param name="context">环境对象. </param>
		/// <returns>若在开始枚举成员之前, 返回值表示是否允许枚举. 其他时候忽略.</returns>
		public bool NotifyForEachMemberBegin(IIndentedWriter iw, object owner, Type tp, BindingFlags bindingAttr, IndentedWriterMemberOptions options, EventHandler<IndentedWriterMemberEventArgs> handle, IndentedWriterContext context) {
			bool rt = true;
			m_TypeOwners.Add(new KeyValuePair<Type, object>(tp, owner));
			IndentedWriterForEachMemberNotify p = ForEachMemberBegin;
			if (null != p) rt = p(iw, owner, tp, bindingAttr, options, handle, context);
			return rt;
		}

		/// <summary>
		/// 通知枚举成员结束.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="owner">欲查询成员的对象.</param>
		/// <param name="tp">类型.</param>
		/// <param name="bindingAttr">绑定标志.</param>
		/// <param name="options">成员选项. </param>
		/// <param name="handle">每个成员的处理过程. </param>
		/// <param name="context">环境对象. </param>
		/// <returns>若在开始枚举成员之前, 返回值表示是否允许枚举. 其他时候忽略.</returns>
		public bool NotifyForEachMemberEnd(IIndentedWriter iw, object owner, Type tp, BindingFlags bindingAttr, IndentedWriterMemberOptions options, EventHandler<IndentedWriterMemberEventArgs> handle, IndentedWriterContext context) {
			bool rt = true;
			IndentedWriterForEachMemberNotify p = ForEachMemberEnd;
			if (null != p) rt = p(iw, owner, tp, bindingAttr, options, handle, context);
			if (m_TypeOwners.Count > 0) m_TypeOwners.RemoveAt(m_TypeOwners.Count - 1);
			return rt;
		}

	}
}
