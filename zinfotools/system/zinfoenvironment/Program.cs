using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using System.Reflection;
using zyllibcs.text;

namespace zinfoenvironment {
	/// <summary>
	/// System.Environment infos.
	/// </summary>
	class Program {
		/// <summary>
		/// Main.
		/// </summary>
		/// <param name="args">args.</param>
		static void Main(string[] args) {
			//Console.WriteLine("Hello!");
			IIndentedWriter iw = new TextIndentedWriter(Console.Out);
			// main
			MyInfo.outl_main(iw, null, null);
		}

	}
}
