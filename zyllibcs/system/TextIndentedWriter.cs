using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace zyllibcs.system {
	/// <summary>
	/// 文本流的带缩进输出者.
	/// </summary>
	public class TextIndentedWriter : BaseIndentedWriter {
		/// <summary>
		/// 基础文字输出者.
		/// </summary>
		private TextWriter m_BaseTextWriter;

		/// <summary>
		/// 基础文字输出者.
		/// </summary>
		public TextWriter BaseTextWriter {
			get { return m_BaseTextWriter; }
			set { m_BaseTextWriter = value; }
		}

		/// <summary>
		/// 构造 TextIndentedWriter 对象, 具有 textwriter 参数.
		/// </summary>
		/// <param name="textwriter">基础文字输出者.</param>
		public TextIndentedWriter(TextWriter textwriter)
			: base() {
			m_BaseTextWriter = textwriter;
		}

		/// <summary>
		/// 构造 TextIndentedWriter 对象, 使用默认参数.
		/// </summary>
		public TextIndentedWriter()
			: this(null) {
		}

		/// <summary>
		/// 释放资源.
		/// </summary>
		/// <param name="disposing">是否释放托管资源.</param>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				// Release managed resources
			}
			// Release unmanaged resources
			if (null != m_BaseTextWriter) {
				(m_BaseTextWriter as IDisposable).Dispose();
				m_BaseTextWriter = null;
			}
			base.Dispose(disposing);
		}

		#region BaseIndentedWriter

		/// <summary>
		/// 换行字符串.
		/// </summary>
		public override string NewLine {
			get {
				if (null!=m_BaseTextWriter) return m_BaseTextWriter.NewLine;
				return Environment.NewLine;
			}
		}

		protected override void CoreWrite(char value) {
			if (null == m_BaseTextWriter) throw new NullReferenceException("BaseTextWriter is null!");
			m_BaseTextWriter.Write(value);
		}

		protected override void CoreWrite(string value) {
			if (null == m_BaseTextWriter) throw new NullReferenceException("BaseTextWriter is null!");
			m_BaseTextWriter.Write(value);
		}

		#endregion	// #region BaseIndentedWriter
	}
}
