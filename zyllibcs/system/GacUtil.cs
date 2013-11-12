using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace zyllibcs.system {

	/// <summary>
	/// 取得Gac列表时的选项.
	/// </summary>
	[Flags]
	public enum GacGetListOptions {
		/// <summary>
		/// 默认.
		/// </summary>
		Default = 0,
		/// <summary>
		/// 自动排序.
		/// </summary>
		AutoSort = 1,
		/// <summary>
		/// 失败时回退，不抛出异常。先枚举GAC，失败时枚举当前应用程序域中的程序集，再失败时返回自身程序集，最后仍失败时返回0个元素的列表.
		/// </summary>
		Fallback = 2,
	}

	/// <summary>
	/// 指示由全局程序集缓存中的 IAssemblyCacheItem 表示的程序集的源。
	/// </summary>
	public enum ASM_CACHE_FLAGS : int {
		/// <summary>
		/// 使用 Ngen.exe 枚举预先编译的程序集的缓存。
		/// </summary>
		ASM_CACHE_ZAP = 0x01,
		/// <summary>
		/// 枚举全局程序集缓存。
		/// </summary>
		ASM_CACHE_GAC = 0x02,
		/// <summary>
		/// 枚举已经根据需要下载的程序集或已经进行了影像复制的程序集。
		/// </summary>
		ASM_CACHE_DOWNLOAD = 0x04,
		/// <summary>
		/// 仅在调用 GetCachePath 函数的上下文中，ASM_CACHE_ROOT 才有意义。
		/// </summary>
		ASM_CACHE_ROOT = 0x08
	}

#if (!NETFX_PORTABLE)
	/// <summary>
	/// 指示将通过 IAssemblyName::GetDisplayName 方法检索其显示名称的程序集的版本、内部版本、区域性、签名等。
	/// </summary>
	/// <remarks>http://msdn.microsoft.com/en-us/library/ms232947(v=vs.110).aspx</remarks>
	public enum ASM_DISPLAY_FLAGS : int {
		/// <summary>
		/// VERSION
		/// </summary>
		ASM_DISPLAYF_VERSION = 0x01,
		/// <summary>
		/// CULTURE
		/// </summary>
		ASM_DISPLAYF_CULTURE = 0x02,
		/// <summary>
		/// PUBLIC_KEY_TOKEN
		/// </summary>
		ASM_DISPLAYF_PUBLIC_KEY_TOKEN = 0x04,
		/// <summary>
		/// PUBLIC_KEY
		/// </summary>
		ASM_DISPLAYF_PUBLIC_KEY = 0x08,
		/// <summary>
		/// CUSTOM
		/// </summary>
		ASM_DISPLAYF_CUSTOM = 0x10,
		/// <summary>
		/// PROCESSORARCHITECTURE
		/// </summary>
		ASM_DISPLAYF_PROCESSORARCHITECTURE = 0x20,
		/// <summary>
		/// LANGUAGEID
		/// </summary>
		ASM_DISPLAYF_LANGUAGEID = 0x40,
		/// <summary>
		/// RETARGET
		/// </summary>
		ASM_DISPLAYF_RETARGET = 0x80,
		/// <summary>
		/// CONFIG_MASK
		/// </summary>
		ASM_DISPLAYF_CONFIG_MASK = 0x100,
		/// <summary>
		/// MVID
		/// </summary>
		ASM_DISPLAYF_MVID = 0x200,
		/// <summary>
		/// ASM_DISPLAYF_FULL reflects any changes made to the version of the <see cref="IAssemblyName"/> object. Do not assume that the returned value is immutable.
		/// </summary>
		ASM_DISPLAYF_FULL =
						  ASM_DISPLAYF_VERSION |
						  ASM_DISPLAYF_CULTURE |
						  ASM_DISPLAYF_PUBLIC_KEY_TOKEN |
						  ASM_DISPLAYF_RETARGET |
						  ASM_DISPLAYF_PROCESSORARCHITECTURE
	}

	[ComImport]
	[Guid("21b8916c-f28e-11d2-a473-00c04f8ef448")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAssemblyEnum {

		IntPtr GetNextAssembly(
			/* [in] */ IntPtr pvReserved,
			/* [out] */ out IAssemblyName ppName,
			/* [in] */ uint dwFlags);

		void Reset();

		void Clone(
			/* [out] */ out IAssemblyEnum ppEnum);

	}

	/// <summary>
	/// 提供描述和使用程序集唯一标识的方法。
	/// </summary>
	[ComImport]
	[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAssemblyName {
		/// <summary>
		/// 设置指定的 PropertyId 所引用的属性的值。
		/// </summary>
		/// <param name="PropertyId"></param>
		/// <param name="pvProperty"></param>
		/// <param name="cbProperty"></param>
		void SetProperty(
			/* [in] */ int PropertyId,
			/* [in] */ IntPtr pvProperty,
			/* [in] */ int cbProperty);

		/// <summary>
		/// 获取一个指针，该指针指向指定的 PropertyId 引用的属性。
		/// </summary>
		/// <param name="PropertyId"></param>
		/// <param name="pvProperty"></param>
		/// <param name="pcbProperty"></param>
		void GetProperty(
			/* [in] */ int PropertyId,
			/* [out] */ IntPtr pvProperty,
			/* [out][in] */ out int pcbProperty);

		/// <summary>
		/// 允许此 IAssemblyName 对象在其析构函数被调用之前释放资源并执行其他清理操作。
		/// </summary>
		void Finalize();

		/// <summary>
		/// 获取此 IAssemblyName 对象引用的程序集的可读名称。
		/// </summary>
		/// <param name="szDisplayName"></param>
		/// <param name="pccDisplayName"></param>
		/// <param name="dwDisplayFlags"></param>
		void GetDisplayName(
			/* [out] */ StringBuilder szDisplayName,
			/* [out][in] */ ref int pccDisplayName,
			/* [in] */ ASM_DISPLAY_FLAGS dwDisplayFlags);

		/// <summary>
		/// Reserved
		/// </summary>
		/// <param name="refIID"></param>
		/// <param name="pUnkReserved1"></param>
		/// <param name="pUnkReserved2"></param>
		/// <param name="szReserved"></param>
		/// <param name="llReserved"></param>
		/// <param name="pvReserved"></param>
		/// <param name="cbReserved"></param>
		/// <param name="ppReserved"></param>
		void Reserved(
			/* [in] */ ref Guid refIID,
			/* [in] */ IntPtr pUnkReserved1,
			/* [in] */ IntPtr pUnkReserved2,
			/* [in] */ string szReserved,
			/* [in] */ Int64 llReserved,
			/* [in] */ IntPtr pvReserved,
			/* [in] */ int cbReserved,
			/* [out] */ IntPtr ppReserved);

		/// <summary>
		/// 获取此 IAssemblyName 对象引用的程序集的简单、未加密名称。
		/// </summary>
		/// <param name="lpcwBuffer"></param>
		/// <param name="pwzName"></param>
		void GetName(
			/* [out][in] */ ref int lpcwBuffer,
			/* [out] */ StringBuilder pwzName);

		/// <summary>
		/// 获取此 IAssemblyName 对象所引用的程序集的版本信息。
		/// </summary>
		/// <param name="pdwVersionHi"></param>
		/// <param name="pdwVersionLow"></param>
		void GetVersion(
			/* [out] */ out Int32 pdwVersionHi,
			/* [out] */ out Int32 pdwVersionLow);

		/// <summary>
		/// 根据指定的比较标志，确定指定的 IAssemblyName 对象是否与此 IAssemblyName 相同。
		/// </summary>
		/// <param name="pName"></param>
		/// <param name="dwCmpFlags"></param>
		void IsEqual(
			/* [in] */ IAssemblyName pName,
			/* [in] */ int dwCmpFlags);

		/// <summary>
		/// 创建此 IAssemblyName 对象的浅表副本。
		/// </summary>
		/// <param name="pName"></param>
		void Clone(
			/* [out] */out IAssemblyName pName);

	}
#endif

	/// <summary>
	/// GAC(Global Assembly Cache) 工具类.
	/// </summary>
	public static class GacUtil {
		/// <summary>
		/// 默认字符串缓冲区长度.
		/// </summary>
		public static readonly int DefaultStringBufferSize = 0x1000;

#if (!NETFX_PORTABLE)
		/// <summary>
		/// 获取一个指针，该指针指向 IAssemblyEnum 实例，该实例可枚举具有指定 IAssemblyName 的程序集中的对象。
		/// </summary>
		/// <param name="pEnum">指向一个内存位置的指针，该内存位置包含请求的 IAssemblyEnum 指针。</param>
		/// <param name="pUnkReserved">保留以用于将来扩展。pUnkReserved 必须为 null 引用。</param>
		/// <param name="pName">请求的程序集的 IAssemblyName。此名称用于筛选枚举。它可以为 null，以枚举全局程序集缓存中的所有程序集。</param>
		/// <param name="dwFlags">用于修改枚举数行为的标志。此参数正好包含 ASM_CACHE_FLAGS 枚举中的一位。</param>
		/// <param name="pvReserved">保留以用于将来扩展。pvReserved 必须为 null 引用。</param>
		/// <returns>HRESULT</returns>
		[DllImport("fusion.dll")]
		internal static extern IntPtr CreateAssemblyEnum(out IAssemblyEnum pEnum, IntPtr pUnkReserved, IntPtr pName, ASM_CACHE_FLAGS dwFlags, IntPtr pvReserved);

		/// <summary>
		/// 枚举 <see cref="IAssemblyName"/> 信息.
		/// </summary>
		/// <param name="dwCacheFlags">缓存标志.</param>
		/// <returns>返回 <see cref="IEnumerable&lt;IAssemblyName&gt;"/>. </returns>
		/// <exception cref="System.DllNotFoundException">在mono等环境下有可能找不到dll.</exception>
		public static IEnumerable<IAssemblyName> EnumerateIAssemblyName(ASM_CACHE_FLAGS dwCacheFlags) {
			IAssemblyEnum gacEnum;
			//创建GAC程序集的枚举器
			CreateAssemblyEnum(out gacEnum, IntPtr.Zero, IntPtr.Zero, dwCacheFlags, IntPtr.Zero);
			if (null!=gacEnum)
			{
				try {
					IAssemblyName asm;
					gacEnum.GetNextAssembly(IntPtr.Zero, out asm, 0);
					while (asm != null) {
						yield return asm;
						//StringBuilder sbuf = new StringBuilder(1024);
						//uint ccbuf = 1024;
						////获取程序集显示名称
						//asm.GetName(ref ccbuf, sbuf);
						////打印程序集名称
						//Console.WriteLine(sbuf.ToString());
						//释放COM对象
						Marshal.ReleaseComObject(asm);
						asm = null;
						//枚举下一个
						gacEnum.GetNextAssembly(IntPtr.Zero, out asm, 0);
					}
				}
				finally {
					Marshal.ReleaseComObject(gacEnum);
				}
			}
		}

		/// <summary>
		/// 在GAC中枚举 <see cref="IAssemblyName"/> 信息.
		/// </summary>
		/// <returns>返回 <see cref="IEnumerable&lt;IAssemblyName&gt;"/>. </returns>
		/// <exception cref="System.DllNotFoundException">在mono等环境下有可能找不到dll.</exception>
		public static IEnumerable<IAssemblyName> GacEnumerateIAssemblyName() {
			return EnumerateIAssemblyName(ASM_CACHE_FLAGS.ASM_CACHE_GAC);
		}

		/// <summary>
		/// 枚举程序集名称.
		/// </summary>
		/// <param name="dwCacheFlags">缓存标志.</param>
		/// <param name="dwDisplayFlags">显示标志.</param>
		/// <returns>返回 <see cref="IEnumerable&lt;String&gt;"/>. </returns>
		/// <exception cref="System.DllNotFoundException">在mono等环境下有可能找不到dll.</exception>
		public static IEnumerable<String> EnumerateAssemblyName(ASM_CACHE_FLAGS dwCacheFlags, ASM_DISPLAY_FLAGS dwDisplayFlags) {
			foreach (IAssemblyName asm in EnumerateIAssemblyName(dwCacheFlags)) {
				int ccbuf = DefaultStringBufferSize;
				StringBuilder sbuf = new StringBuilder(ccbuf);
				asm.GetDisplayName(sbuf, ref ccbuf, dwDisplayFlags);
				yield return sbuf.ToString();
			}
		}

		/// <summary>
		/// 在GAC中枚举程序集名称.
		/// </summary>
		/// <param name="dwDisplayFlags">显示标志.</param>
		/// <returns>返回 <see cref="IEnumerable&lt;String&gt;"/>. </returns>
		/// <exception cref="System.DllNotFoundException">在mono等环境下有可能找不到dll.</exception>
		public static IEnumerable<String> GacEnumerateAssemblyName(ASM_DISPLAY_FLAGS dwDisplayFlags) {
			return EnumerateAssemblyName(ASM_CACHE_FLAGS.ASM_CACHE_GAC, dwDisplayFlags);
		}
#endif

		/// <summary>
		/// 取得程序集名称列表.
		/// </summary>
		/// <param name="dwCacheFlags">缓存标志.</param>
		/// <param name="dwDisplayFlags">显示标志.</param>
		/// <param name="issort">是否排序.</param>
		/// <returns>返回程序集名称列表.</returns>
		/// <exception cref="System.DllNotFoundException">当 <paramref name="listoptions"/> 没有 <see cref="GacGetListOptions.Fallback"/> 标志时 , 在mono等环境下有可能找不到dll.</exception>
		public static List<String> GetAssemblyNameList(ASM_CACHE_FLAGS dwCacheFlags, ASM_DISPLAY_FLAGS dwDisplayFlags, GacGetListOptions listoptions) {
			List<String> lst = new List<string>();
			// get list.
			bool isok = false;
#if (!NETFX_PORTABLE)
			try {
				foreach (string str in EnumerateAssemblyName(dwCacheFlags, dwDisplayFlags)) {
					lst.Add(str);
				}
				isok = true;
			}
			catch {
				if (0==(listoptions & GacGetListOptions.Fallback)) {
					// 没有标识, 重抛异常.
					throw;
				}
				else {
					// 忽略.
				}
			}
#endif
			if (0 != (listoptions & GacGetListOptions.Fallback) && dwCacheFlags == ASM_CACHE_FLAGS.ASM_CACHE_GAC) {
				// 尝试枚举当前应用程序域.
#if (!NETFX_CORE)
				if (!isok && lst.Count == 0) {
					try {
						foreach (Assembly p in AppDomain.CurrentDomain.GetAssemblies()) {
							lst.Add(p.FullName);
						}
						isok = true;
					}
					catch {
						// 忽略.
					}
				}
#endif
				// 尝试所在程序集.
				if (!isok && lst.Count == 0) {
					Type tp = typeof(GacUtil);
#if (NETFX_CORE)
					TypeInfo ti = tp.GetTypeInfo();
					lst.Add(ti.Assembly.FullName);
					isok = true;
#else
					lst.Add(tp.Assembly.FullName);
					isok = true;
#endif
				}
			}
			// sort.
			if (0!=(listoptions & GacGetListOptions.AutoSort)) {
				lst.Sort(StringComparer.OrdinalIgnoreCase);
			}
			return lst;
		}

		/// <summary>
		/// 在Gac中取得程序集名称列表.
		/// </summary>
		/// <param name="dwDisplayFlags">显示标志.</param>
		/// <param name="issort">是否排序.</param>
		/// <returns>返回程序集名称列表.</returns>
		/// <exception cref="System.DllNotFoundException">在mono等环境下有可能找不到dll.</exception>
		public static List<String> GacGetAssemblyNameList(ASM_DISPLAY_FLAGS dwDisplayFlags, GacGetListOptions listoptions) {
			return GetAssemblyNameList(ASM_CACHE_FLAGS.ASM_CACHE_GAC, dwDisplayFlags, listoptions);
		}

	}

}
