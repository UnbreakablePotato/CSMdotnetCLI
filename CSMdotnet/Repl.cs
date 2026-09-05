using System;
using System.Collections.Generic;
using System.Text;

namespace CSMdotnet
{
    internal class Repl
    {
        public static string[] cleanInput(string input)
        {
            string[] res;
            res = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return res;
        }

        public static string[] cleanMatchIDS(List<string>? matches)
        {
            string[] res = new string [matches.Count];

            for (var i = 0; i < matches.Count; i++)
            {
                res[i] = matches[i].Replace("[", "");
                res[i] = matches[i].Replace("]", "");
                res[i] = matches[i].Replace("\"", "");
            }

            return res;
        }
    }
}
