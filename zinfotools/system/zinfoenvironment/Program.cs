using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using zyllibcs.system;

namespace zinfoenvironment {
	/// <summary>
	/// System.Environment infos.
	/// </summary>
	class Program {
		/// <summary>
		/// 格式化_字符串的字符数组.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">obj.</param>
		public static string format_String_Chars(string obj) {
			StringBuilder sb = new StringBuilder();
			if (null == obj) {
				sb.Append(string.Format("len={0}", 0));
			}
			else {
				sb.Append(string.Format("len={0}", obj.Length));
				sb.Append(", chars={");
				foreach (char ch in obj) {
					sb.Append(string.Format(" {0:X4}", (int)ch));
				}
				sb.Append(" }");
			}
			return sb.ToString();
		}

		/// <summary>
		/// 输出多行_字符串的字符数组.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">obj.</param>
		public static bool outl_String_Chars(IIndentedWriter iw, string obj) {
			if (null == obj) return false;
			if (!(obj is String)) return false;
			String ob = obj as String;
			if (null == iw) return true;
			//
			if (!iw.Indent(obj)) return false;
			iw.WriteLine("# <String>");
			if (null == ob) {
				iw.WriteLine("Length:\t{0}", 0);
			}
			else {
				iw.WriteLine("Length:\t{0}", ob.Length);
				iw.Write("Chars:\t");
				foreach (char ch in ob) {
					iw.Write("{0:X4} ", (int)ch);
				}
				iw.WriteLine();
			}
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// 输出多行_版本.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">obj.</param>
		public static bool outl_Version(IIndentedWriter iw, object obj) {
			if (null == obj) return false;
			Version ob = obj as Version;
			if (null == ob) return false;
			if (null == iw) return true;
			//
			if (!iw.Indent(obj)) return false;
			IndentedWriterValueOptions iwvo = IndentedWriterValueOptions.Default;
			Type tp = obj.GetType();
			iw.WriteLine("# <{0}>", tp.FullName);
			// propertys.
			//IndentedWriterUtil.WriteLineValue(iw, "Major", ob.Major, iwvo);
			//IndentedWriterUtil.WriteLineValue(iw, "Minor", ob.Minor, iwvo);
			//IndentedWriterUtil.WriteLineValue(iw, "Build", ob.Build, iwvo);
			//IndentedWriterUtil.WriteLineValue(iw, "Revision", ob.Revision, iwvo);
			//IndentedWriterUtil.WriteLineValue(iw, "MajorRevision", ob.MajorRevision, iwvo);
			//IndentedWriterUtil.WriteLineValue(iw, "MinorRevision", ob.MinorRevision, iwvo);
			//foreach (PropertyInfo pi in tp.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public)) {
			//    if (pi.CanRead && pi.GetIndexParameters().Length<=0) {
			//        object value = pi.GetValue(obj, null);
			//        IndentedWriterUtil.WriteLineValue(iw, pi.Name, value, iwvo);
			//    }
			//}
			IndentedWriterUtil.ForEachMember(iw, obj, null, IndentedWriterUtil.PublicInstance, IndentedWriterMemberOptions.Default, null, null, null);
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// 输出多行_操作系统.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		/// <param name="obj">obj.</param>
		public static bool outl_OperatingSystem(IIndentedWriter iw, OperatingSystem obj) {
			if (null == obj) return false;
			iw.Indent(null);
			iw.WriteLine("# <System.OperatingSystem>");
			// propertys.
			iw.WriteLine("Platform:\t{0}\t# 0x{1:X}, {1}", obj.Platform, (int)obj.Platform);
			iw.WriteLine("ServicePack:\t{0}", obj.ServicePack);
			iw.WriteLine("Version:\t{0}", obj.Version); outl_Version(iw, obj.Version);
			iw.WriteLine("VersionString:\t{0}", obj.VersionString);
			iw.Unindent();
			return true;
		}

		/// <summary>
		/// 输出多行_环境.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		public static void outl_Environment(IIndentedWriter iw) {
			iw.Indent(null);
			iw.WriteLine("# <System.Environment>");

			// propertys.
			iw.WriteLine("CommandLine:\t{0}", Environment.CommandLine);
			iw.WriteLine("CurrentDirectory:\t{0}", Environment.CurrentDirectory);
			iw.WriteLine("ExitCode:\t{0}", Environment.ExitCode);
			iw.WriteLine("HasShutdownStarted:\t{0}", Environment.HasShutdownStarted);
			iw.WriteLine("MachineName:\t{0}", Environment.MachineName);
			//iw.WriteLine("NewLine:\t", Environment.NewLine.Length); outl_String_Chars(iw, Environment.NewLine);
			iw.WriteLine("NewLine:\t{0}", format_String_Chars(Environment.NewLine));
			iw.WriteLine("OSVersion:\t{0}", Environment.OSVersion); outl_OperatingSystem(iw, Environment.OSVersion);
			iw.WriteLine("ProcessorCount:\t{0}\t# 0x{0:X}", Environment.ProcessorCount);
			iw.WriteLine("SystemDirectory:\t{0}", Environment.SystemDirectory);
			iw.WriteLine("TickCount:\t{0}\t# 0x{0:X}", Environment.TickCount);
			iw.WriteLine("UserDomainName:\t{0}", Environment.UserDomainName);
			iw.WriteLine("UserInteractive:\t{0}", Environment.UserInteractive);
			iw.WriteLine("UserName:\t{0}", Environment.UserName);
			iw.WriteLine("Version:\t{0}", Environment.Version); outl_Version(iw, Environment.Version);
			iw.WriteLine("WorkingSet:\t{0}\t# 0x{0:X}", Environment.WorkingSet);

			// GetFolderPath.
			iw.WriteLine("GetFolderPath:");
			iw.Indent(null);
			foreach (Environment.SpecialFolder p in Enum.GetValues(typeof(Environment.SpecialFolder))) {
				iw.Write("{0}\t{1}\t", (int)p, p);
				try {
					iw.Write(Environment.GetFolderPath(p));
				}
				catch {
				}
				iw.WriteLine();
			}
			iw.Unindent();

			// GetLogicalDrives.
			iw.WriteLine("GetLogicalDrives:\t{0}", string.Join(", ", Environment.GetLogicalDrives()));

			// GetEnvironmentVariables.
			iw.WriteLine("GetEnvironmentVariables:");
			iw.Indent(null);
			IDictionary environmentVariables = Environment.GetEnvironmentVariables();
			//foreach (DictionaryEntry de in inenvironmentVariables) {
			//    iw.WriteLine("{0}:\t{1}", de.Key, de.Value);
			//}
			ArrayList lst = new ArrayList(environmentVariables.Keys);
			lst.Sort();
			foreach (object k in lst) {
				iw.WriteLine("{0}:\t{1}", k, environmentVariables[k]);
			}
			iw.Unindent();

			// done.
			iw.Unindent();
		}

		/// <summary>
		/// 输出多行_主函数.
		/// </summary>
		/// <param name="iw">带缩进输出者.</param>
		public static void outl_main(IIndentedWriter iw) {
			// test.
			//iw.WriteLine("Hello!");
			//for (int i = 0; i < 3; ++i) {
			//    iw.WriteLine(i);
			//    iw.Indent();
			//}
			//iw.IndentLevel = 0;
			//IndentedWriterObjectLnProc lwo = outl_Version;
			//IndentedWriterObjectLnProc[] lwos = new IndentedWriterObjectLnProc[] { lwo };
			//IEnumerable<IndentedWriterObjectLnProc> lwoe = lwos;
			//foreach (IndentedWriterObjectLnProc p in lwoe) {
			//    p(iw, "");
			//}
			//lwo(iw, "");
			//lwo.Invoke(iw, "");
			IndentedWriterUtil.WriteLineValue(iw, "enum", IndentedWriterValueOptions.ExistName, IndentedWriterValueOptions.Default);
			IndentedWriterUtil.WriteLineValue(iw, "char", 'A', IndentedWriterValueOptions.Default);
			IndentedWriterUtil.WriteLineValue(iw, "int", 255, IndentedWriterValueOptions.Default);
			// root.
			iw.WriteLine("root:");
			//decimal? dec = new decimal(1);
			//IndentedWriterUtil.WriteLineValue(iw, "dec", dec, IndentedWriterValueOptions.Default);
			//IndentedObjectFunctor.CommonProc(iw, dec);
			// Environment.
			IndentedObjectFunctor.CommonProc(iw, Environment.OSVersion);
			//outl_Environment(iw);
		}

		static void Main(string[] args) {
			//Console.WriteLine("Hello!");
			IIndentedWriter iw = new TextIndentedWriter(Console.Out);
			// main
			outl_main(iw);
		}
	}
}
