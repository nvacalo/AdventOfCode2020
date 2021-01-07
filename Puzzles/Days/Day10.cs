using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public static class Day10
    {
        public static void RunDay()
        {
            var numbers = GetInput();
            //var numbers = GetTest();
            //var numbers = GetTest2();

            numbers.Add(0);//the default one

            var ordered = numbers.OrderBy(n => n).ToList();

            int dif1 = 0;
            int dif2 = 0;
            int dif3 = 1;//plus the one extra in the end

            for (int i = 1; i < ordered.Count; i++)
            {
                if(ordered[i] - ordered[i-1] == 1)
                {
                    dif1++;
                }
                else if(ordered[i] - ordered[i-1] == 2)
                {
                    dif2++;
                }
                else if(ordered[i] - ordered[i-1] == 3)
                {
                    dif3++;
                }
            }

            Console.WriteLine($"dif1: {dif1}");
            Console.WriteLine($"dif2: {dif2}");
            Console.WriteLine($"dif3: {dif3}");
          
            Console.WriteLine($"result: {dif1 * dif3}");
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
