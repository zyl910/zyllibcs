using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace zyllibcs.text {

	/// <summary>
	/// 成员名称选项.
	/// </summary>
	[Flags]
	public enum MemberNameOptions {
		/// <summary>
		/// 默认.
		/// </summary>
		Default = 0,
		/// <summary>
		/// 类型参数的括号. 注意在有 <see cref="TypeParamSeparator"/> 标识时会自动有效.
		/// </summary>
		TypeParamBracket = 1,
		/// <summary>
		/// 类型参数的分隔符. 注意在有 <see cref="TypeParam"/>, <see cref="TypeParamActual"/> 标识时会自动有效.
		/// </summary>
		TypeParamSeparator = 2,
		/// <summary>
		/// 类型参数. 如 T 之类的占位符. 注意在有 <see cref="TypeParamActual"/> 标识时会自动有效.
		/// </summary>
		TypeParam = 4,
		/// <summary>
		/// 类型参数实参. 如 int 之类的实际类型.
		/// </summary>
		TypeParamActual = 8,
		/// <summary>
		/// 参数列表的括号. 注意在有 <see cref="ParamSeparator"/> 标识时会自动有效.
		/// </summary>
		ParamBracket = 0x10,
		/// <summary>
		/// 参数列表的分隔符. 注意在有 <see cref="ParamType"/>, <see cref="ParamName"/> 标识时会自动有效.
		/// </summary>
		ParamSeparator = 0x20,
		/// <summary>
		/// 参数类型.
		/// </summary>
		ParamType = 0x40,
		/// <summary>
		/// 参数名.
		/// </summary>
		ParamName = 0x80,
		/// <summary>
		/// 返回值类型.
		/// </summary>
		ResultType = 0x100,
		/// <summary>
		/// 带命名空间的全限定名, 仅对类型有效.
		/// </summary>
		FullName = 0x200,
		/// <summary>
		/// 类型参数采用全限定名.
		/// </summary>
		FullNameType = 0x400,
		/// <summary>
		/// 参数采用全限定名.
		/// </summary>
		FullNameParam = 0x800,
		/// <summary>
		/// 返回值采用全限定名.
		/// </summary>
		FullNameResult = 0x1000,
	}

	/// <summary>
	/// 成员信息格式化.
	/// </summary>
	public static class MemberInfoFormat {
		/// <summary>
		/// 规范化成员名称选项.
		/// </summary>
		/// <param name="options">选项.</param>
		/// <returns>返回规范后的结果.</returns>
		public static MemberNameOptions NormMemberNameOptions(MemberNameOptions options) {
			MemberNameOptions rt = options;
			if (0 != (options & MemberNameOptions.TypeParamActual)) rt |= MemberNameOptions.TypeParam;
			if (0 != (options & MemberNameOptions.TypeParam)) rt |= MemberNameOptions.TypeParamSeparator;
			if (0 != (options & MemberNameOptions.TypeParamSeparator)) rt |= MemberNameOptions.TypeParamBracket;
			if (0 != (options & (MemberNameOptions.ParamType | MemberNameOptions.ParamName))) rt |= MemberNameOptions.ParamSeparator;
			if (0 != (options & MemberNameOptions.ParamSeparator)) rt |= MemberNameOptions.ParamBracket;
			return rt;
		}

		/// <summary>
		/// 计算类型名称.
		/// </summary>
		/// <param name="mi">Type.</param>
		/// <param name="options">Options.</param>
		/// <returns>返回名称, 失败时返回 null.</returns>
		public static string GetMemberName(Type mi, MemberNameOptions options) {
			if (null == mi) return null;
			string rt = mi.Name;
			options = NormMemberNameOptions(options);
			if (0 != (options & MemberNameOptions.FullName)) rt = mi.FullName;
			//TODO: 支持全部的 MemberNameOptions . By Type.
			return rt;
		}

		/// <summary>
		/// 计算字段名称.
		/// </summary>
		/// <param name="mi">Field info.</param>
		/// <param name="options">Options.</param>
		/// <returns>返回名称, 失败时返回 null.</returns>
		public static string GetMemberName(FieldInfo mi, MemberNameOptions options) {
			if (null == mi) return null;
			string rt = mi.Name;
			options = NormMemberNameOptions(options);
			//TODO: 支持全部的 MemberNameOptions . By FieldInfo.
			return rt;
		}

		/// <summary>
		/// 计算属性名称.
		/// </summary>
		/// <param name="mi">Property info.</param>
		/// <param name="options">Options.</param>
		/// <returns>返回名称, 失败时返回 null.</returns>
		public static string GetMemberName(PropertyInfo mi, MemberNameOptions options) {
			if (null == mi) return null;
			string rt = mi.Name;
			options = NormMemberNameOptions(options);
			//TODO: 支持全部的 MemberNameOptions . By PropertyInfo.
			return rt;
		}

		/// <summary>
		/// 计算方法名称.
		/// </summary>
		/// <param name="mi">Method info.</param>
		/// <param name="options">Options.</param>
		/// <returns>返回名称, 失败时返回 null.</returns>
		public static string GetMemberName(MethodInfo mi, MemberNameOptions options) {
			if (null == mi) return null;
			string rt = mi.Name;
			options = NormMemberNameOptions(options);
			if (0 != (options & MemberNameOptions.ParamBracket)) rt = rt + "()";
			//TODO: 支持全部的 MemberNameOptions . By MethodInfo.
			return rt;
		}

		/// <summary>
		/// 计算事件名称.
		/// </summary>
		/// <param name="mi">Event info.</param>
		/// <param name="options">Options.</param>
		/// <returns>返回名称, 失败时返回 null.</returns>
		public static string GetMemberName(EventInfo mi, MemberNameOptions options) {
			if (null == mi) return null;
			string rt = mi.Name;
			options = NormMemberNameOptions(options);
			//TODO: 支持全部的 MemberNameOptions . By EventInfo.
			return rt;
		}

		/// <summary>
		/// 自动计算成员名称.
		/// </summary>
		/// <param name="mi">Member info.</param>
		/// <param name="options">Options.</param>
		/// <returns>返回名称, 失败时返回 null.</returns>
		public static string GetMemberNameAuto(MemberInfo mi, MemberNameOptions options) {
			if (null == mi) return null;
			string rt = null;
			if (false) {
			}
			else if (mi is FieldInfo) {
				rt = GetMemberName(mi as FieldInfo, options);
			}
			else if (mi is PropertyInfo) {
				rt = GetMemberName(mi as PropertyInfo, options);
			}
			else if (mi is MethodInfo) {
				rt = GetMemberName(mi as MethodInfo, options);
			}
			else if (mi is EventInfo) {
				rt = GetMemberName(mi as EventInfo, options);
			}
#if (!NETFX_CORE)
			else if (mi is Type) {
				rt = GetMemberName(mi as Type, options);
			}
#endif
			else {
				rt = mi.Name;
			}
			return rt;
		}
	}
}
