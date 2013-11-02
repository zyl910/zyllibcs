using System;
using System.Collections.Generic;
using System.Text;

namespace zyllibcs.system {
	/// <summary>
	/// 类型名比较器.
	/// </summary>
	public class TypeNameComparer : IComparer<Type> {
		/// <summary>
		/// 默认字符串比较器.
		/// </summary>
		public static readonly StringComparer DefaultStringComparer = StringComparer.OrdinalIgnoreCase;

		/// <summary>
		/// 默认实例.
		/// </summary>
		public static readonly TypeNameComparer Default = new TypeNameComparer();

		#region IComparer<Type> 成员

		/// <summary>
		/// 比较.
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <returns>比较结果.</returns>
		public int Compare(Type x, Type y) {
			//throw new Exception("The method or operation is not implemented.");
			//if (x == y) return 0;
			if (object.ReferenceEquals(x, y)) return 0;
			if (null == x) return -1;	// null 最小.
			if (null == y) return 1;	// null 最小.
			return DefaultStringComparer.Compare(x.FullName, y.FullName);
		}

		#endregion
	}

}
