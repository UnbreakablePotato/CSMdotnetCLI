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

        public static string[] cleanMatchIDS(string[] matches)
        {
            string[] res = new string [matches.Length];

            for (var i = 0; i < matches.Length; i++)
            {
                res[i] = matches[i].Replace("[", "");
                res[i] = matches[i].Replace("]", "");
                res[i] = matches[i].Replace("\"", "");
            }

            return res;
        }
    }
}
