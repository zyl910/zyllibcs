using System;
using System.Collections.Generic;
using System.Text;
using zyllibcs.text;

namespace zinfotools {
	/// <summary>
	/// ��Ϣ����. ��һ����̬������.
	/// </summary>
	public static class InfoData {
		/// <summary>
		/// �������������.
		/// </summary>
		public static readonly KeyValuePair<string, IndentedWriterObjectProc>[] NameProcs = new KeyValuePair<string, IndentedWriterObjectProc>[] {
			new KeyValuePair<string, IndentedWriterObjectProc>("system/zinfoenvironment", zinfoenvironment.MyInfo.outl_main),
			new KeyValuePair<string, IndentedWriterObjectProc>("system/zinfoassemblylist", zinfoassemblylist.MyInfo.outl_main),
			new KeyValuePair<string, IndentedWriterObjectProc>("system/zinfoassemblylist_full", zinfoassemblylist.MyInfo.outl_main_full),
			new KeyValuePair<string, IndentedWriterObjectProc>("system/zinfobase", zinfobase.MyInfo.outl_main),
			new KeyValuePair<string, IndentedWriterObjectProc>("system/zinfoculture", zinfoculture.MyInfo.outl_main),
#if (!NETFX_CORE && !NETFX_PORTABLE)
			new KeyValuePair<string, IndentedWriterObjectProc>("system/zinfodraw", zinfodraw.MyInfo.outl_main),
			new KeyValuePair<string, IndentedWriterObjectProc>("system/zinfodraw_full", zinfodraw.MyInfo.outl_main_full),
			new KeyValuePair<string, IndentedWriterObjectProc>("winform/zinfowinform", zinfowinform.MyInfo.outl_main),
#endif
		};

		/// <summary>
		/// Ĭ������.
		/// </summary>
		public const string DefaultName = "system/zinfoenvironment";

		/// <summary>
		/// Ĭ�������� <see cref="NameProcs"/> �е�����.
		/// </summary>
		public static readonly int DefaultNameIndex = 0;

	}
}
