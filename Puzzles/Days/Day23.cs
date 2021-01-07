using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public static class Day23
    {
        public static string _Input { get; set; }
        public static int _Loops { get; set; }

        public static Dictionary<int, int> _Cups = new Dictionary<int, int>();
        public static void RunDay()
        {
            _Input = "389125467";
            _Loops = 10000000;

            var stringArray = _Input.ToArray();
            List<int> array = new List<int>();
            for (int i = 0; i < stringArray.Length; i++)
            {
                array.Add(int.Parse(stringArray[i].ToString()));
            }

            for (int i = 0; i < array.Count; i++)
            {
                _Cups[array[i]] = i;
            }

            for (int i = 10; i <= 1000000; i++)
            {
                _Cups[i] = i - 1;
            }

            int currentIndex = 0;

            for (int i = 0; i < _Loops; i++)
            {
                int currentKey = _Cups.FirstOrDefault(c => c.Value == currentIndex).Key;

                Console.WriteLine($"-- move {i + 1} --");
                foreach (var c in _Cups.OrderBy(obj => obj.Value))
                {
                    //Console.Write($"{c.Key} ");
                }
                //Console.WriteLine();
                //Console.WriteLine($"current: {currentKey}");

                #region pick
                //Console.Write("pick up: ");
                Dictionary<int, int> toMove = new Dictionary<int, int>();
                var pickIndex = currentIndex + 1;
                for (int j = 0; j < 3; j++)
                {
                    if (pickIndex > 1000000 - 1) pickIndex = 0;

                    ////Console.Write($" {array[1]}");
                    var next = _Cups.FirstOrDefault(c => c.Value == pickIndex);
                    toMove.Add(next.Key, next.Value);
                    pickIndex++;
                    //_Cups.Remove(next.Key);
                }
                foreach (var x in toMove)
                {
                    //Console.Write($"{x.Key} ");
                }
                //Console.WriteLine();
                #endregion

                #region find dest
                int minus = 1;
                int destinationKey = 0;
                int destinationValueIndex = 0;
                while (currentKey - minus >= _Cups.Keys.Min())
                {
                    if (_Cups.ContainsKey(currentKey - minus) &&
                        toMove.ContainsKey(currentKey - minus) == false)
                    {
                        destinationKey = currentKey - minus;
                        destinationValueIndex = _Cups[destinationKey];
                        break;
                    }
                    minus++;
                }

                if (destinationKey == 0)
                {
                    var temp = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(_Cups.Keys));
                    foreach (var p in toMove.Keys)
                    {
                        temp.Remove(p);
                    }

                    destinationKey = temp.Max();
                    destinationValueIndex = _Cups[destinationKey];
                }
                #endregion

                //Console.WriteLine($"destination: {destinationKey}");
                //Console.WriteLine();

                int newIdx = 4;
                var list = _Cups.Where(c => c.Key != destinationKey && toMove.ContainsKey(c.Key) == false).OrderBy(k => k.Value).ToList();
                foreach (var kvp in list.Where(k => k.Value > destinationValueIndex))
                {
                    _Cups[kvp.Key] = newIdx++;
                }
                foreach (var kvp in list.Where(k => k.Value < destinationValueIndex))
                {
                    _Cups[kvp.Key] = newIdx++;
                }

                _Cups[destinationKey] = 0;
                _Cups[toMove.First().Key] = 1;
                toMove.Remove(toMove.First().Key);
                _Cups[toMove.First().Key] = 2;
                toMove.Remove(toMove.First().Key);
                _Cups[toMove.First().Key] = 3;
                toMove.Remove(toMove.First().Key);

                currentIndex = _Cups[currentKey] + 1;
                if (currentIndex > 1000000 - 1) currentIndex = 0;
                //var tempDestKey = destinationKey;
                //var tempDestIndexValue = _Cups[destinationKey];

                //int add = tempDestIndexValue + 1;
                //int c0 = tempDestIndexValue + 1;
                //var max = _Cups.Values.Max();
                //var min = _Cups.Values.Min();

                //while (c0 != tempDestIndexValue)
                //{
                //    var cup = _Cups.FirstOrDefault(obj => obj.Value == c0).Key;
                //    _Cups[cup] = add++;

                //    c0++;
                //    if (c0 == max) c0 = min;
                //}

                ////for (int c = toMove.Last().Value + 1; c <= destinationValueIndex; c++)
                ////{
                ////    var cup = _Cups.FirstOrDefault(obj => obj.Value == c).Key;
                ////    _Cups[cup] = add++;
                ////}

                //_Cups[toMove.First().Key] = tempDestIndexValue + 1;
                //toMove.Remove(toMove.First().Key);
                //_Cups[toMove.First().Key] = tempDestIndexValue + 2;
                //toMove.Remove(toMove.First().Key);
                //_Cups[toMove.First().Key] = tempDestIndexValue + 3;
                //toMove.Remove(toMove.First().Key);

                //int k = 0;
                //for (int j = destination + 1; j <= destination + 3; j++)
                //{
                //    array.Insert(j, toMove[k++]);
                //}

                //current++;
            }

            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();

            //string final = "";
            int idx = _Cups[1];
            Console.WriteLine(_Cups.Single(c => c.Value == idx + 1).Key);
            Console.WriteLine(_Cups.Single(c => c.Value == idx + 2).Key);

            //for (int j = 0; j < array.Count - 1; j++)
            //{
            //    final += array[idx];
            //    idx++;
            //    if (idx > array.Count - 1)
            //    {
            //        idx = 0;
            //    }
            //}

            ////Console.WriteLine(final);
        }
    }
}
