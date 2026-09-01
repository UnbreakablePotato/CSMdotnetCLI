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
    }
}
