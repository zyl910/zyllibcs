using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace zyllibcs.system {

	/// <summary>
	/// ȡ��Gac�б�ʱ��ѡ��.
	/// </summary>
	[Flags]
	public enum GacGetListOptions {
		/// <summary>
		/// Ĭ��.
		/// </summary>
		Default = 0,
		/// <summary>
		/// �Զ�����.
		/// </summary>
		AutoSort = 1,
		/// <summary>
		/// ʧ��ʱ���ˣ����׳��쳣����ö��GAC��ʧ��ʱö�ٵ�ǰӦ�ó������еĳ��򼯣���ʧ��ʱ����������򼯣������ʧ��ʱ����0��Ԫ�ص��б�.
		/// </summary>
		Fallback = 2,
	}

	/// <summary>
	/// ָʾ��ȫ�ֳ��򼯻����е� IAssemblyCacheItem ��ʾ�ĳ��򼯵�Դ��
	/// </summary>
	public enum ASM_CACHE_FLAGS : int {
		/// <summary>
		/// ʹ�� Ngen.exe ö��Ԥ�ȱ���ĳ��򼯵Ļ��档
		/// </summary>
		ASM_CACHE_ZAP = 0x01,
		/// <summary>
		/// ö��ȫ�ֳ��򼯻��档
		/// </summary>
		ASM_CACHE_GAC = 0x02,
		/// <summary>
		/// ö���Ѿ�������Ҫ���صĳ��򼯻��Ѿ�������Ӱ���Ƶĳ��򼯡�
		/// </summary>
		ASM_CACHE_DOWNLOAD = 0x04,
		/// <summary>
		/// ���ڵ��� GetCachePath �������������У�ASM_CACHE_ROOT �������塣
		/// </summary>
		ASM_CACHE_ROOT = 0x08
	}

#if (!NETFX_PORTABLE)
	/// <summary>
	/// ָʾ��ͨ�� IAssemblyName::GetDisplayName ������������ʾ���Ƶĳ��򼯵İ汾���ڲ��汾�������ԡ�ǩ���ȡ�
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
	/// �ṩ������ʹ�ó���Ψһ��ʶ�ķ�����
	/// </summary>
	[ComImport]
	[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAssemblyName {
		/// <summary>
		/// ����ָ���� PropertyId �����õ����Ե�ֵ��
		/// </summary>
		/// <param name="PropertyId"></param>
		/// <param name="pvProperty"></param>
		/// <param name="cbProperty"></param>
		void SetProperty(
			/* [in] */ int PropertyId,
			/* [in] */ IntPtr pvProperty,
			/* [in] */ int cbProperty);

		/// <summary>
		/// ��ȡһ��ָ�룬��ָ��ָ��ָ���� PropertyId ���õ����ԡ�
		/// </summary>
		/// <param name="PropertyId"></param>
		/// <param name="pvProperty"></param>
		/// <param name="pcbProperty"></param>
		void GetProperty(
			/* [in] */ int PropertyId,
			/* [out] */ IntPtr pvProperty,
			/* [out][in] */ out int pcbProperty);

		/// <summary>
		/// ����� IAssemblyName ����������������������֮ǰ�ͷ���Դ��ִ���������������
		/// </summary>
		void Finalize();

		/// <summary>
		/// ��ȡ�� IAssemblyName �������õĳ��򼯵Ŀɶ����ơ�
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
		/// ��ȡ�� IAssemblyName �������õĳ��򼯵ļ򵥡�δ�������ơ�
		/// </summary>
		/// <param name="lpcwBuffer"></param>
		/// <param name="pwzName"></param>
		void GetName(
			/* [out][in] */ ref int lpcwBuffer,
			/* [out] */ StringBuilder pwzName);

		/// <summary>
		/// ��ȡ�� IAssemblyName ���������õĳ��򼯵İ汾��Ϣ��
		/// </summary>
		/// <param name="pdwVersionHi"></param>
		/// <param name="pdwVersionLow"></param>
		void GetVersion(
			/* [out] */ out Int32 pdwVersionHi,
			/* [out] */ out Int32 pdwVersionLow);

		/// <summary>
		/// ����ָ���ıȽϱ�־��ȷ��ָ���� IAssemblyName �����Ƿ���� IAssemblyName ��ͬ��
		/// </summary>
		/// <param name="pName"></param>
		/// <param name="dwCmpFlags"></param>
		void IsEqual(
			/* [in] */ IAssemblyName pName,
			/* [in] */ int dwCmpFlags);

		/// <summary>
		/// ������ IAssemblyName �����ǳ������
		/// </summary>
		/// <param name="pName"></param>
		void Clone(
			/* [out] */out IAssemblyName pName);

	}
#endif

	/// <summary>
	/// GAC(Global Assembly Cache) ������.
	/// </summary>
	public static class GacUtil {
		/// <summary>
		/// Ĭ���ַ�������������.
		/// </summary>
		public static readonly int DefaultStringBufferSize = 0x1000;

#if (!NETFX_PORTABLE)
		/// <summary>
		/// ��ȡһ��ָ�룬��ָ��ָ�� IAssemblyEnum ʵ������ʵ����ö�پ���ָ�� IAssemblyName �ĳ����еĶ���
		/// </summary>
		/// <param name="pEnum">ָ��һ���ڴ�λ�õ�ָ�룬���ڴ�λ�ð�������� IAssemblyEnum ָ�롣</param>
		/// <param name="pUnkReserved">���������ڽ�����չ��pUnkReserved ����Ϊ null ���á�</param>
		/// <param name="pName">����ĳ��򼯵� IAssemblyName������������ɸѡö�١�������Ϊ null����ö��ȫ�ֳ��򼯻����е����г��򼯡�</param>
		/// <param name="dwFlags">�����޸�ö������Ϊ�ı�־���˲������ð��� ASM_CACHE_FLAGS ö���е�һλ��</param>
		/// <param name="pvReserved">���������ڽ�����չ��pvReserved ����Ϊ null ���á�</param>
		/// <returns>HRESULT</returns>
		[DllImport("fusion.dll")]
		internal static extern IntPtr CreateAssemblyEnum(out IAssemblyEnum pEnum, IntPtr pUnkReserved, IntPtr pName, ASM_CACHE_FLAGS dwFlags, IntPtr pvReserved);

		/// <summary>
		/// ö�� <see cref="IAssemblyName"/> ��Ϣ.
		/// </summary>
		/// <param name="dwCacheFlags">�����־.</param>
		/// <returns>���� <see cref="IEnumerable&lt;IAssemblyName&gt;"/>. </returns>
		/// <exception cref="System.DllNotFoundException">��mono�Ȼ������п����Ҳ���dll.</exception>
		public static IEnumerable<IAssemblyName> EnumerateIAssemblyName(ASM_CACHE_FLAGS dwCacheFlags) {
			IAssemblyEnum gacEnum;
			//����GAC���򼯵�ö����
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
						////��ȡ������ʾ����
						//asm.GetName(ref ccbuf, sbuf);
						////��ӡ��������
						//Console.WriteLine(sbuf.ToString());
						//�ͷ�COM����
						Marshal.ReleaseComObject(asm);
						asm = null;
						//ö����һ��
						gacEnum.GetNextAssembly(IntPtr.Zero, out asm, 0);
					}
				}
				finally {
					Marshal.ReleaseComObject(gacEnum);
				}
			}
		}

		/// <summary>
		/// ��GAC��ö�� <see cref="IAssemblyName"/> ��Ϣ.
		/// </summary>
		/// <returns>���� <see cref="IEnumerable&lt;IAssemblyName&gt;"/>. </returns>
		/// <exception cref="System.DllNotFoundException">��mono�Ȼ������п����Ҳ���dll.</exception>
		public static IEnumerable<IAssemblyName> GacEnumerateIAssemblyName() {
			return EnumerateIAssemblyName(ASM_CACHE_FLAGS.ASM_CACHE_GAC);
		}

		/// <summary>
		/// ö�ٳ�������.
		/// </summary>
		/// <param name="dwCacheFlags">�����־.</param>
		/// <param name="dwDisplayFlags">��ʾ��־.</param>
		/// <returns>���� <see cref="IEnumerable&lt;String&gt;"/>. </returns>
		/// <exception cref="System.DllNotFoundException">��mono�Ȼ������п����Ҳ���dll.</exception>
		public static IEnumerable<String> EnumerateAssemblyName(ASM_CACHE_FLAGS dwCacheFlags, ASM_DISPLAY_FLAGS dwDisplayFlags) {
			foreach (IAssemblyName asm in EnumerateIAssemblyName(dwCacheFlags)) {
				int ccbuf = DefaultStringBufferSize;
				StringBuilder sbuf = new StringBuilder(ccbuf);
				asm.GetDisplayName(sbuf, ref ccbuf, dwDisplayFlags);
				yield return sbuf.ToString();
			}
		}

		/// <summary>
		/// ��GAC��ö�ٳ�������.
		/// </summary>
		/// <param name="dwDisplayFlags">��ʾ��־.</param>
		/// <returns>���� <see cref="IEnumerable&lt;String&gt;"/>. </returns>
		/// <exception cref="System.DllNotFoundException">��mono�Ȼ������п����Ҳ���dll.</exception>
		public static IEnumerable<String> GacEnumerateAssemblyName(ASM_DISPLAY_FLAGS dwDisplayFlags) {
			return EnumerateAssemblyName(ASM_CACHE_FLAGS.ASM_CACHE_GAC, dwDisplayFlags);
		}
#endif

		/// <summary>
		/// ȡ�ó��������б�.
		/// </summary>
		/// <param name="dwCacheFlags">�����־.</param>
		/// <param name="dwDisplayFlags">��ʾ��־.</param>
		/// <param name="issort">�Ƿ�����.</param>
		/// <returns>���س��������б�.</returns>
		/// <exception cref="System.DllNotFoundException">�� <paramref name="listoptions"/> û�� <see cref="GacGetListOptions.Fallback"/> ��־ʱ , ��mono�Ȼ������п����Ҳ���dll.</exception>
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
					// û�б�ʶ, �����쳣.
					throw;
				}
				else {
					// ����.
				}
			}
#endif
			if (0 != (listoptions & GacGetListOptions.Fallback) && dwCacheFlags == ASM_CACHE_FLAGS.ASM_CACHE_GAC) {
				// ����ö�ٵ�ǰӦ�ó�����.
#if (!NETFX_CORE)
				if (!isok && lst.Count == 0) {
					try {
						foreach (Assembly p in AppDomain.CurrentDomain.GetAssemblies()) {
							lst.Add(p.FullName);
						}
						isok = true;
					}
					catch {
						// ����.
					}
				}
#endif
				// �������ڳ���.
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
		/// ��Gac��ȡ�ó��������б�.
		/// </summary>
		/// <param name="dwDisplayFlags">��ʾ��־.</param>
		/// <param name="issort">�Ƿ�����.</param>
		/// <returns>���س��������б�.</returns>
		/// <exception cref="System.DllNotFoundException">��mono�Ȼ������п����Ҳ���dll.</exception>
		public static List<String> GacGetAssemblyNameList(ASM_DISPLAY_FLAGS dwDisplayFlags, GacGetListOptions listoptions) {
			return GetAssemblyNameList(ASM_CACHE_FLAGS.ASM_CACHE_GAC, dwDisplayFlags, listoptions);
		}

	}

}
