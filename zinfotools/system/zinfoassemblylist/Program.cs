using System;
//using System.Collections.Generic;
//using System.Text;
using zyllibcs.text;

namespace zinfoassemblylist {
	/// <summary>
	/// Assembly List infos.
	/// </summary>
	class Program {
		/// <summary>
		/// main.
		/// </summary>
		/// <param name="args">args</param>
		static void Main(string[] args) {
			IIndentedWriter iw = new TextIndentedWriter(Console.Out);
			MyInfo.outl_main(iw, null, null);
		}
	}
}
