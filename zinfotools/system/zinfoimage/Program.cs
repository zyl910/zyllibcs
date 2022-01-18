using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zyllibcs.text;

namespace zinfoimage {
    class Program {
        static void Main(string[] args) {
            IIndentedWriter iw = new TextIndentedWriter(Console.Out);
            if (args.Length >= 1) {
                MyInfo.FilePath = args[0];
            }
            MyInfo.outl_main(iw, null, null);
        }
    }
}
