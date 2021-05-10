using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotepad {
    class MyNotepadWindowDesignData {
        /// <summary>
        /// text
        /// </summary>
        public string Text { set; get; } = "テストデータです";

        /// <summary>
        /// list of text
        /// </summary>
        public List<string> TextList { set; get; } = new List<string>() {
            { "aaaaa" },
            { "bbbb" }
        };
    }
}
