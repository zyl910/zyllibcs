using System;
//using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace zyllibcs.text {
	/// <summary>
	/// C风格字符串转义模式.
	/// </summary>
	public enum StringEscapeCMode {
		/// <summary>
		/// 默认.
		/// </summary>
		Default = 0,
		/// <summary>
		/// 自动括号. 转义时，仅在遇到必须转义时才加上引号，否则不会有引号.
		/// </summary>
		QuoteAuto = 0,
		/// <summary>
		/// 有括号. 转义时，总是加上引号. 解转义时，仅解转义引号内的内容.
		/// </summary>
		QuoteYes = 1,
		/// <summary>
		/// 无括号. 转义时，不会加上引号. 解转义时，假定已经在引号内.
		/// </summary>
		QuoteNo = 2,
		/// <summary>
		/// 不转码. 返回原始文本.
		/// </summary>
		NoEscape = 3,
		/// <summary>
		/// 引号掩码. 一般多用于转义时.
		/// </summary>
		QuoteMask = 3,
		/// <summary>
		/// 自动切换. 解转义时，若遇到单独的引号便自动在 原始文本、解转义 这两种模式下切换.
		/// </summary>
		SwitchAuto = 0,
		/// <summary>
		/// 切换为原始文本. 解转义时，若遇到单独的引号，便转为原始文本直接输出剩余内容.
		/// </summary>
		SwitchRaw = 4,
		/// <summary>
		/// 忽略切换. 解转义时，若遇到单独的引号，便输出这个引号，然后依旧对剩余数据进行解转义.
		/// </summary>
		SwitchIgnore = 8,
		/// <summary>
		/// 切换时中断. 解转义时，若遇到单独的引号，便直接返回，不处理剩余数据.
		/// </summary>
		SwitchBreak = 0xC,
		/// <summary>
		/// 切换掩码. 仅用于解转义时.
		/// </summary>
		SwitchMask = 0xC,
	}

	/// <summary>
	/// C-style escape (C风格字符串转义).
	/// </summary>
	/// <remarks>
	/// 规则有:
	/// <para>\\: '\'.</para>
	/// <para>\": '"'.</para>
	/// <para>\': '''.</para>
	/// <para>\0.</para>
	/// <para>\a.</para>
	/// <para>\b.</para>
	/// <para>\f.</para>
	/// <para>\n.</para>
	/// <para>\r.</para>
	/// <para>\t.</para>
	/// <para>\v.</para>
	/// <para>\uxxxx: 16位unicode字符.</para>
	/// <para>\Uyyyyyyyy: 32位unicode字符.</para>
	/// <para>未来考虑支持:</para>
	/// <para>\000: 8进制字符.</para>
	/// <para>\0x00: 16进制字符.</para>
	/// </remarks>
	public class StringEscapeC {
		/// <summary>
		/// 保持原值的转义字符.
		/// </summary>
		public static readonly string EscapeKeepChars = "\\\"\'";

		/// <summary>
		/// 命名的转义字符.
		/// </summary>
		public static readonly string EscapeNameChars = "0abfnrtv";

		/// <summary>
		/// 16位unicode字符转义.
		/// </summary>
		public static readonly string EscapeUnicode16Chars = "u";

		/// <summary>
		/// 32位unicode字符转义.
		/// </summary>
		public static readonly string EscapeUnicode32Chars = "U";

		/// <summary>
		/// 解转义字符串.
		/// </summary>
		/// <param name="src">源字符串.</param>
		/// <param name="mode">模式.</param>
		/// <returns>返回该字符串的解转义.</returns>
		public static string UnescapeString(string src, StringEscapeCMode mode) {
			if (string.IsNullOrEmpty(src)) return src;
			Debug.Assert(mode == StringEscapeCMode.Default, "Now only support StringEscapeCMode.Default!");	//TODO: 支持更多的C转义的解码.
			StringBuilder sb = new StringBuilder(src.Length);
			char[] arr = src.ToCharArray();
			bool bQuoteIn = false;	// 已经进入括号内.
			bool bBackslash = false;	// 前一个是反斜杠.
			long posBackslash = 0;	// 反斜杠的位置.
			for(int i=0; i<arr.Length; ++i) {
				bool bOutChar = true;	// 输出当前字符.
				char ch = arr[i];
				if (bQuoteIn) {
					if (bBackslash) {
						switch (ch) {
							case '\\':
							case '\"':
							case '\'':
								// 输出ch.
								break;
							case '0': ch = '\0'; break;
							case 'a': case 'A': ch = '\a'; break;
							case 'b': case 'B': ch = '\b'; break;
							case 'f': case 'F': ch = '\f'; break;
							case 'n': case 'N': ch = '\n'; break;
							case 'r': case 'R': ch = '\r'; break;
							case 't': case 'T': ch = '\t'; break;
							case 'v': case 'V': ch = '\v'; break;
							case 'u':
							case 'U':
								Debug.Assert(false, "Now not support.");	//TODO: 实现unicode转码.
								sb.Append('\\');
								break;
							default:
								// 无效转义，补上前面的反斜杠.
								sb.Append('\\');
								break;
						}
						if (bOutChar) bBackslash = false;
					}
					else {
						switch (ch) {
							case '\"':
								bQuoteIn = false;
								bBackslash = false;
								bOutChar = false;
								break;
							case '\\':
								bBackslash = true;
								posBackslash = i;
								bOutChar = false;
								break;
							default:
								//bOutChar = true;
								break;
						}
					}
				}
				else {
					if ('"' == ch) {
						bQuoteIn = true;
						bBackslash = false;
						bOutChar = false;
					}
					else {
						//bOutChar = true;
					}
				}
				if (bOutChar) sb.Append(ch);
			}
			if (bBackslash) {
				sb.Append(src.Substring((int)posBackslash));
			}
			return sb.ToString();
		}

		/// <summary>
		/// 解转义字符串.
		/// </summary>
		/// <param name="src">源字符串.</param>
		/// <returns>返回该字符串的解转义.</returns>
		public static string UnescapeString(string src) {
			return UnescapeString(src, StringEscapeCMode.Default);
		}

		// checkNeedEscape: 检测是否需要转义.

		/// <summary>
		/// C转码字符，仅返回会转码的字符串.
		/// </summary>
		/// <param name="ch">字符.</param>
		/// <returns>返回该字符的转义. 不需转码的会返回 null .</returns>
		private static string EscapeChar_Only(char ch) {
			string rt = null;
			switch (ch) {
				case '\0': rt = "\\0"; break;
				case '\a': rt = "\\a"; break;
				case '\b': rt = "\\b"; break;
				case '\f': rt = "\\f"; break;
				case '\n': rt = "\\n"; break;
				case '\r': rt = "\\r"; break;
				case '\t': rt = "\\t"; break;
				case '\v': rt = "\\v"; break;
				default:
					if (char.IsControl(ch)) {
						rt = string.Format("\\u{0:X4}", (int)ch);
					}
					break;
			}
			return rt;
		}

		/// <summary>
		/// C转码字符(字符串版)，仅返回会转码的字符串.
		/// </summary>
		/// <param name="ch">字符.</param>
		/// <returns>返回该字符的转义. 不需转码的会返回 null .</returns>
		private static string EscapeChar_OnlyS(char ch) {
			string rt = null;
			switch (ch) {
				case '\0': rt = "\\0"; break;
				case '\a': rt = "\\a"; break;
				case '\b': rt = "\\b"; break;
				case '\f': rt = "\\f"; break;
				case '\n': rt = "\\n"; break;
				case '\r': rt = "\\r"; break;
				case '\t': rt = "\\t"; break;
				case '\v': rt = "\\v"; break;
				case '\'': rt = "\\\'"; break;
				case '\"': rt = "\\\""; break;
				default:
					if (char.IsControl(ch)) {
						rt = string.Format("\\u{0:X4}", (int)ch);
					}
					break;
			}
			return rt;
		}

		/// <summary>
		/// C转码字符，根据 模式 参数.
		/// </summary>
		/// <param name="ch">字符.</param>
		/// <param name="mode">模式.</param>
		/// <returns>返回该字符的转义.</returns>
		public static string EscapeChar(char ch, StringEscapeCMode mode) {
			if ((mode & StringEscapeCMode.QuoteMask) == StringEscapeCMode.NoEscape) return ch.ToString();
			string rt = EscapeChar_Only(ch);
			bool hasquote = ((mode & StringEscapeCMode.QuoteMask) == StringEscapeCMode.QuoteYes);
			if ((mode & StringEscapeCMode.QuoteMask) == StringEscapeCMode.QuoteAuto && null != rt) hasquote = true;
			if (null == rt) rt = ch.ToString();
			if (hasquote) rt = string.Format("'{0}'", rt);
			return rt;
		}

		/// <summary>
		/// C转码字符，使用默认参数(自动增加引号).
		/// </summary>
		/// <param name="ch">字符.</param>
		/// <returns>返回该字符的转义.</returns>
		public static string EscapeChar(char ch) {
			return EscapeChar(ch, StringEscapeCMode.Default);
		}

		/// <summary>
		/// C转码字符串，根据 模式 参数.
		/// </summary>
		/// <param name="src">源字符串.</param>
		/// <param name="mode">模式.</param>
		/// <returns>返回该字符串的转义.</returns>
		public static string EscapeString(string src, StringEscapeCMode mode) {
			if (string.IsNullOrEmpty(src)) return src;
			if ((mode & StringEscapeCMode.QuoteMask) == StringEscapeCMode.NoEscape) return src;
			StringBuilder sb = new StringBuilder();
			bool hasescape = false;
			foreach (char ch in src) {
				string s = EscapeChar_OnlyS(ch);
				if (null != s) {
					sb.Append(s);
					hasescape = true;
				}
				else {
					sb.Append(ch);
				}
			}
			bool hasquote = ((mode & StringEscapeCMode.QuoteMask) == StringEscapeCMode.QuoteYes);
			if ((mode & StringEscapeCMode.QuoteMask) == StringEscapeCMode.QuoteAuto && hasescape) hasquote = true;
			if (!hasescape && !hasquote) return src;
			if (hasquote) {
				sb.Insert(0, "\"");
				sb.Append('"');
			}
			return sb.ToString();
		}

		/// <summary>
		/// C转码字符串，使用默认参数(自动增加引号).
		/// </summary>
		/// <param name="src">源字符串.</param>
		/// <returns>返回该字符串的转义.</returns>
		public static string EscapeString(string src) {
			return EscapeString(src, StringEscapeCMode.Default);
		}

	}
}
