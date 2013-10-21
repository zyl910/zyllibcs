using System;
using System.Text;


[assembly: CLSCompliant(true)]

namespace zyllibcs.text {
	/// <summary>
	/// 带缩进输出对象的过程.
	/// </summary>
	/// <param name="iw">带缩进输出者.</param>
	/// <param name="obj">object. If <paramref name="obj"/> is null, result alway is false.</param>
	/// <param name="context">State Object. Can be null.</param>
	/// <returns>当<paramref name="iw"/>为null时, 返回是否支持输出. 其他情况则返回是否成功输出.</returns>
	public delegate bool IndentedWriterObjectProc(IIndentedWriter iw, object obj, IndentedWriterContext context);

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

		/// <summary>
		/// Write a char.
		/// </summary>
		/// <param name="value">value.</param>
		void Write(char value);

		/// <summary>
		/// Write a string.
		/// </summary>
		/// <param name="value">value.</param>
		void Write(string value);

		/// <summary>
		/// Write a object.
		/// </summary>
		/// <param name="value">value.</param>
		void Write(object value);

		/// <summary>
		/// Write formated string.
		/// </summary>
		/// <param name="format">format.</param>
		/// <param name="args">args.</param>
		void Write(string format, params Object[] args);

		/// <summary>
		/// Write NewLine.
		/// </summary>
		void WriteLine();

		/// <summary>
		/// Write a char and NewLine.
		/// </summary>
		/// <param name="value">value.</param>
		void WriteLine(char value);

		/// <summary>
		/// Write a string and NewLine.
		/// </summary>
		/// <param name="value">value.</param>
		void WriteLine(string value);

		/// <summary>
		/// Write a object and NewLine.
		/// </summary>
		/// <param name="value">value.</param>
		void WriteLine(object value);

		/// <summary>
		/// Write formated string and NewLine.
		/// </summary>
		/// <param name="format">format.</param>
		/// <param name="args">args.</param>
		void WriteLine(string format, params Object[] args);

	}
}
