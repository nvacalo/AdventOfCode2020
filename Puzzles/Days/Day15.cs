using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public class Number
    {
        public int Last { get; set; }
        public int BeforeLast { get; set; }
    }

    public static class Day15
    {
        static Dictionary<int, Number> _memory = new Dictionary<int, Number>();

        //static string input = "0,3,6";
        static string input = "1,12,0,20,8,16";
        public static void RunDay()
        {
            List<int> numbers = input.Split(",").ToList().ConvertAll(n => Convert(n));

            var previous = 0;
            for (int i = 1; i <= 30000000; i++)
            {
                var current = 0;
                if (i <= numbers.Count)
                {
                    current = numbers[i - 1];
                }
                else
                {
                    if (_memory[previous].BeforeLast == 0)
                    {
                        current = 0;
                    }
                    else
                    {
                        current = _memory[previous].Last - _memory[previous].BeforeLast;
                    }
                }

                if (_memory.ContainsKey(current))
                {
                    _memory[current] = new Number { BeforeLast = _memory[current].Last, Last = i };
                }
                else
                {
                    _memory[current] = new Number { BeforeLast = 0, Last = i };
                }

                previous = current;
            }

            Console.WriteLine(previous);
        }

        private static int Convert(string n)
        {
            return Int32.Parse(n);
        }
    }
}
