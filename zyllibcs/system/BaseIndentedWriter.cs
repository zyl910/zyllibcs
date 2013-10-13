using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace zyllibcs.system {
	public abstract class BaseIndentedWriter : IIndentedWriter, IDisposable {
		/// <summary>
		/// 默认的缩进字符.
		/// </summary>
		public const char DefaultIndentChar = '\t';

		/// <summary>
		/// 默认的为空时的字符串.
		/// </summary>
		public static readonly string DefaultShowNullString = "(null)";

		/// <summary>
		/// 缩进级别,
		/// </summary>
		private int m_IndentLevel = 0;

		/// <summary>
		/// 需要缩进.
		/// </summary>
		private bool m_NeedIndent = true;

		/// <summary>
		/// 历史缩进集.
		/// </summary>
		private readonly ArrayList m_IndentHistory = new ArrayList();

		/// <summary>
		/// 析构.
		/// </summary>
		~BaseIndentedWriter() {
			Dispose(false);
		}

		/// <summary>
		/// 释放资源.
		/// </summary>
		/// <param name="disposing">是否释放托管资源.</param>
		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				// Release managed resources
				IndentLevel = 0;
			}
			// Release unmanaged resources
		}

		/// <summary>
		/// 历史缩进集中存在该对象.
		/// </summary>
		/// <param name="obj">预检测的对象.</param>
		/// <returns></returns>
		public bool ExistIndentHistory(object obj) {
			return m_IndentHistory.Contains(obj);
		}

		/// <summary>
		/// 设置历史缩进集的条目数量.
		/// </summary>
		/// <param name="count">新数量.</param>
		protected void SetIndentHistoryCount(int count) {
			if (count < 0) return;
			while (m_IndentHistory.Count < count) m_IndentHistory.Add(null);
			if (m_IndentHistory.Count > count) {
				m_IndentHistory.RemoveRange(count, m_IndentHistory.Count - count);
			}
		}

		/// <summary>
		/// 换行字符串.
		/// </summary>
		public virtual string NewLine {
			get { return Environment.NewLine; }
		}

		/// <summary>
		/// 核心输出, 参数为字符型.
		/// </summary>
		/// <param name="value">value.</param>
		protected virtual void CoreWrite(char value) {
			CoreWrite(value.ToString());
		}

		/// <summary>
		/// 核心输出, 参数为字符串型.
		/// </summary>
		/// <param name="value">value.</param>
		protected abstract void CoreWrite(string value);

		/// <summary>
		/// 设置需要缩进.
		/// </summary>
		protected void SetNeedIndent() {
			m_NeedIndent = true;
		}

		/// <summary>
		/// 处理需要缩进.
		/// </summary>
		protected void ProcessNeedIndent() {
			if (!m_NeedIndent) return;
			m_NeedIndent = false;
			WriteIndent();
		}

		/// <summary>
		/// 输出缩进.
		/// </summary>
		public virtual void WriteIndent() {
			int n = m_IndentLevel;
			if (n <= 0) return;
			for (int i = 0; i < n; ++i) {
				CoreWrite(DefaultIndentChar);
			}
		}

		#region IIndentedWriter

		/// <summary>
		/// 获取或设置缩进级别。
		/// </summary>
		public virtual int IndentLevel {
			get {
				return m_IndentLevel;
			}
			set {
				m_IndentLevel = value;
				SetIndentHistoryCount(m_IndentLevel);
			}
		}

		/// <summary>
		/// 将当前的 <see cref="IndentLevel"/> 增加 1。
		/// </summary>
		/// <param name="obj">新级别的相关对象. 可设为null.</param>
		/// <returns>是否成功增加一级. 若之前级别存在 <paramref name="obj"/>, 便返回 false.</returns>
		public virtual bool Indent(object obj) {
			Interlocked.Increment(ref m_IndentLevel);
			return true;
		}

		/// <summary>
		/// 将当前的 <see cref="IndentLevel"/> 减少 1。
		/// </summary>
		public virtual void Unindent() {
			int n = Interlocked.Decrement(ref m_IndentLevel);
			SetIndentHistoryCount(n);
		}

		public virtual void Write(char value) {
			ProcessNeedIndent();
			CoreWrite(value);
		}

		public virtual void Write(string value) {
			ProcessNeedIndent();
			CoreWrite(value);
		}

		public virtual void Write(object value) {
			ProcessNeedIndent();
			if (null == value) {
				Write(DefaultShowNullString);
			}
			else {
				Write(value.ToString());
			}
		}

		public virtual void Write(string format, params object[] args) {
			ProcessNeedIndent();
			Write(string.Format(format, args));
		}

		public virtual void WriteLine() {
			ProcessNeedIndent();
			CoreWrite(NewLine);
			SetNeedIndent();
		}

		public virtual void WriteLine(char value) {
			ProcessNeedIndent();
			Write(value);
			WriteLine();
		}

		public virtual void WriteLine(string value) {
			ProcessNeedIndent();
			Write(value);
			WriteLine();
		}

		public virtual void WriteLine(object value) {
			ProcessNeedIndent();
			Write(value);
			WriteLine();
		}

		public virtual void WriteLine(string format, params object[] args) {
			ProcessNeedIndent();
			Write(format, args);
			WriteLine();
		}

		#endregion	// #region IIndentedWriter.


		#region IDisposable

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
