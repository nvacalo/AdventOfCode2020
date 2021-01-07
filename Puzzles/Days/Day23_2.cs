using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public class Cup
    {
        public int Number { get; set; }
        public Cup Next { get; set; }
    }

    public static class Day23_2
    {
        public static string _Input { get; set; }
        public static int _Loops { get; set; }

        public static Dictionary<int, Cup> _Cups = new Dictionary<int, Cup>();
        public static void RunDay()
        {
            _Input = "467528193";
            _Loops = 10000000;

            var stringArray = _Input.ToArray();
            List<int> array = new List<int>();
            for (int i = 0; i < stringArray.Length; i++)
            {
                var val = int.Parse(stringArray[i].ToString());
                array.Add(val);
                _Cups[val] = new Cup { Number = val, Next = null };
            }

            for (int i = 10; i <= 1000000; i++)
            {
                array.Add(i);
            }

            _Cups[1000000] = new Cup { Number = array[1000000 - 1] };
            for (int i = 999999; i > 9; i--)
            {
                _Cups[array[i - 1]] = new Cup { Number = array[i - 1], Next = _Cups[i + 1] };
            }
            _Cups[1000000].Next = _Cups[int.Parse(stringArray[0].ToString())];

            for (int i = 1; i < 9; i++)
            {
                _Cups.ElementAt(i - 1).Value.Next = _Cups.ElementAt(i).Value;
            }
            _Cups[int.Parse(stringArray[8].ToString())].Next = _Cups[10];

            //for (int i = 0; i < _Cups.Count - 1; i++)
            //{
            //    _Cups.ElementAt(i).Value.Next = _Cups.ElementAt(i + 1).Value;
            //}
            //_Cups.Last().Value.Next = _Cups.First().Value;

            Cup currentCup = _Cups[int.Parse(stringArray[0].ToString())];
            for (int i = 1; i <= _Loops; i++)
            {
                //Console.WriteLine(i);
                Cup destination = null;
                int val = currentCup.Number - 1;
                while (destination == null)
                {
                    destination = val <= 0 ? null : _Cups[val];

                    if (destination == null ||
                        val == currentCup.Number ||
                        val == currentCup.Next.Number ||
                        val == currentCup.Next.Next.Number ||
                        val == currentCup.Next.Next.Next.Number)
                    {
                        destination = null;
                        val--;
                        if (val < 1)
                        {
                            var max = _Cups.Where(m => m.Key != currentCup.Number &&
                                                   m.Key != currentCup.Next.Number &&
                                                   m.Key != currentCup.Next.Next.Number &&
                                                   m.Key != currentCup.Next.Next.Next.Number)
                                           .Select(c => c.Key).Max();
                            destination = _Cups.Single(c => c.Key == max).Value;
                        }
                    }
                }

                var lastNexttemp = currentCup.Next.Next.Next.Next.Number;

                currentCup.Next.Next.Next.Next = _Cups[destination.Next.Number];
                destination.Next = _Cups[currentCup.Next.Number];

                currentCup.Next = _Cups[lastNexttemp];

                currentCup = currentCup.Next;
            }

            var first = _Cups[1];
            Console.WriteLine(first.Next.Number);
            Console.WriteLine(first.Next.Next.Number);

            Console.WriteLine(first.Next.Number * first.Next.Next.Number);

        }
    }
}
