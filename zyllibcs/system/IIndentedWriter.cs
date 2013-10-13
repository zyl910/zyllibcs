using System;
using System.Text;


[assembly: CLSCompliant(true)]

namespace zyllibcs.system {
	/// <summary>
	/// 带缩进输出者.
	/// </summary>
	public interface IIndentedWriter {
		/// <summary>
		/// 获取或设置缩进级别。
		/// </summary>
		int IndentLevel { get; set; }

		/// <summary>
		/// 将当前的 <see cref="IndentLevel"/> 增加 1。
		/// </summary>
		/// <param name="obj">新级别的相关对象. 可设为null.</param>
		/// <returns>是否成功增加一级. 若之前级别存在 <paramref name="obj"/>, 便返回 false.</returns>
		bool Indent(object obj);

		/// <summary>
		/// 将当前的 <see cref="IndentLevel"/> 减少 1。
		/// </summary>
		void Unindent();

		void Write(char value);
		void Write(string value);
		void Write(object value);
		void Write(string format, params Object[] args);

		void WriteLine();
		void WriteLine(char value);
		void WriteLine(string value);
		void WriteLine(object value);
		void WriteLine(string format, params Object[] args);

	}
}
