using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class TreeRunner
    {

        public static int displayMenuGetInt(List<string> menu)
        {
            bool valid = false;
            int retval = -1;
            while (!valid)
            {
                foreach (var s in menu)
                {
                    Console.Write(s + "\n");
                }
                Console.Write(">");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid Input");
                }
                else if (Int32.TryParse(input, out retval))
                {
                    if (retval <= 0 || retval > menu.Count)
                    {
                        Console.WriteLine("Invalid Input");
                    }
                    else { valid = true; }
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
            return retval;
        }

    }
}
