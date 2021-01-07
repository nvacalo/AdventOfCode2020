using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public static class Day10_2
    {
        public static Dictionary<int, long> _combinations = new Dictionary<int, long>();
        public static Dictionary<int, List<int>> _validIndexers = new Dictionary<int, List<int>>();
        public static List<int> _excluded = new List<int>();
        public static List<long> _ordered = new List<long>();
        public static long _max;
        public static long _count = 0;

        public static void RunDay()
        {
            var numbers = GetInput();
            //var numbers = GetTest();
            //var numbers = GetTest2();

            numbers.Add(0);//the default one

            _ordered = numbers.OrderBy(n => n).ToList();

            _max = _ordered.Last() + 3;
            _ordered.Add(_max);//the last one

            int i = 0;
            var _count = CalculateCombinations(i);

            Console.WriteLine($"result: {_count}");
        }

        private static long CalculateCombinations(int i)
        {
            if (_combinations.ContainsKey(i))
            {
                return _combinations[i];
            }
            else
            {
                long combs = 0;
                var validIndexes = GetValidAdapterIndexes(i);
                if (validIndexes.Count == 0 && _ordered[i] == _max)
                {
                    return 1;
                }
                else
                {
                    foreach (var validIndexer in validIndexes)
                    {
                        combs += CalculateCombinations(validIndexer);
                    }

                    _combinations[i] = combs;
                    return _combinations[i];
                }
            }
        }

        private static List<int> GetValidAdapterIndexes(int idx)
        {
            if (_validIndexers.ContainsKey(idx))
            {
                return _validIndexers[idx];
            }
            else
            {
                List<int> validIndexes = new List<int>();

                int j = idx + 1;
                while (j < _ordered.Count && _ordered[j] <= _ordered[idx] + 3)
                {
                    if (_excluded.Contains(j) == false)
                    {
                        validIndexes.Add(j);
                    }
                    j++;

                }

                _validIndexers[idx] = validIndexes;
                return validIndexes;
            }
        }

        private static List<long> GetTest()
        {
            return new List<long>()
            {
16,
10,
15,
5,
1,
11,
7,
19,
6,
12,
4,
            };
        }

        private static List<long> GetTest2()
        {
            return new List<long>()
            {
28,
33,
18,
42,
31,
14,
46,
20,
48,
47,
24,
23,
49,
45,
19,
38,
39,
11,
1,
32,
25,
35,
8,
17,
7,
9,
4,
2,
34,
10,
3,
            };
        }

        private static List<long> GetInput()
        {
            return new List<long>()
        {
153,
17,
45,
57,
16,
147,
39,
121,
75,
70,
85,
134,
128,
115,
51,
139,
44,
65,
119,
168,
122,
72,
105,
31,
103,
89,
154,
114,
55,
25,
48,
38,
132,
157,
84,
71,
113,
143,
83,
64,
109,
129,
120,
100,
151,
79,
125,
22,
161,
167,
19,
26,
118,
142,
4,
158,
11,
35,
56,
18,
40,
7,
150,
99,
54,
152,
60,
27,
164,
78,
47,
82,
63,
46,
91,
32,
135,
3,
108,
10,
159,
127,
69,
110,
126,
133,
28,
15,
104,
138,
160,
98,
90,
144,
1,
2,
92,
41,
86,
66,
95,
12,
        };
        }
    }
}
