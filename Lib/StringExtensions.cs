using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode {

    public static class StringExtensions {
        public static string StripMargin(this string st, string margin = "|") {
            return string.Join("\n",
                from line in st.SplitLine()
                select Regex.Replace(line, @"^\s*"+Regex.Escape(margin), "")
            );
        }

        public static string Indent(this string st, int l) {
            return string.Join("\n" + new string(' ', l),
                from line in st.SplitLine()
                select Regex.Replace(line, @"^\s*\|", "")
            );
        }

        public static string[] SplitLine(this string st)
            => Regex.Split(st, "\r?\n");

        public static string[] Split2Lines(this string st)
            => Regex.Split(st, "\r?\n\r?\n");
    }
}
