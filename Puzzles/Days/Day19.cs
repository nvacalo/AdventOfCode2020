﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public static class Day19
    {
        public class Rule
        {
            public int Number { get; set; }
            public string Value { get; set; }
            public List<int> Rules1 { get; set; }
            public List<int> Rules2 { get; set; }
        }

        public static Dictionary<int, Rule> _Rules = new Dictionary<int, Rule>();
        public static List<string> _Invalid = new List<string>();
        public static void RunDay()
        {
            //var rulesInput = GetRulesTestInput();
            var rulesInput = GetRulesInput();

            //var input = GetTestInput();
            var input = GetInput();

            //rulesInput.Add("11: 42 31 | 42 11 31");
            //rulesInput.Add("8: 42 | 42 8");
            //rulesInput.Add("11: 42 31 | 42 42 31 31");
            //rulesInput.Add("11: 42 31 | 42 42 42 31 31 31");
            //rulesInput.Add("11: 42 31 | 42 42 42 42 31 31 31 31");
            //rulesInput.Add("11: 42 31 | 42 42 42 42 42 31 31 31 31 31");

            int count = 0;

            Rule rule;
            foreach (var line in rulesInput)
            {
                rule = new Rule();

                var parts = line.Split(":");

                rule.Number = int.Parse(parts[0]);

                if (parts[1].Contains("|"))
                {
                    var subParts = parts[1].Split("|");

                    rule.Rules1 = Parse(subParts[0]);
                    rule.Rules2 = Parse(subParts[1]);
                }
                else
                {
                    if (parts[1].Contains("'"))
                    {
                        rule.Value = parts[1].Replace(" ", "").Replace("'", "");
                    }
                    else
                    {
                        rule.Rules1 = Parse(parts[1]);
                    }
                }

                _Rules[rule.Number] = rule;
            }

            List<string> list = new List<string>();
            foreach (var inp in input)
            {
                if (Check(inp))
                {
                    list.Add(inp);
                    count++;
                }
            }

            Console.WriteLine("Invalid");
            foreach (var inp in _Invalid.Distinct())
            {
                Console.WriteLine(inp);
            }

            Console.WriteLine("Correct");
            foreach (var inp in list)
            {
                Console.WriteLine(inp);
            }

            Console.WriteLine();
            Console.WriteLine(count);
            Console.WriteLine();
            Console.WriteLine(x);
            Console.WriteLine();
            Console.WriteLine(y);
        }

        public static int x = 0;
        public static int y = 0;
        private static bool Check(string input)
        {
            bool outoflen = false;

            int rule42 = 0;
            int rule31 = 0;

            int idx = 0;
            while (true)
            {
                if (IsValid(input, _Rules[42], ref idx) == false)
                {
                    break;
                }
                else
                {
                    idx++;
                    rule42++;
                    if (idx >= input.Length)
                    {
                        return false;
                    }
                }
            }

            while (true)
            {
                if (IsValid(input, _Rules[31], ref idx) == false)
                {
                    break;
                }
                else
                {
                    idx++;
                    rule31++;
                    if (idx >= input.Length)
                    {
                        outoflen = true;
                        break;
                    }
                }
            }

            if (outoflen && rule42 > rule31)
            {
                x++;
            }
            else if (rule42 == rule31 + 1)
            {
                y++;
            }

            return outoflen && rule42 > rule31 || rule42 == rule31 + 1;

            //var rule = _Rules[0];

            //int idx = 0;

            //var valid = IsValid(input, rule, ref idx);

            //return valid && idx == input.Length - 1;
        }

        private static bool IsValid(string input, Rule rule, ref int idx)
        {
            //Console.WriteLine("~~~~~~~~~~~");
            //Console.WriteLine($"{input} idx: {idx} on rule {rule.Number}");

            if (string.IsNullOrEmpty(rule.Value))
            {
                bool valid1 = true;
                int i = idx;
                foreach (var subRule in rule.Rules1)
                {
                    //Console.WriteLine($"on rule {rule.Number}");
                    valid1 = IsValid(input, _Rules[subRule], ref i);
                    i++;
                    if (valid1 == false) break;
                }

                if (valid1 == false && rule.Rules2 != null && rule.Rules2.Count > 0)
                {
                    bool valid2 = true;
                    int j = idx;
                    foreach (var subRule in rule.Rules2)
                    {
                        //Console.WriteLine($"on rule {rule.Number}");
                        valid2 = IsValid(input, _Rules[subRule], ref j);
                        j++;
                        if (valid2 == false) break;
                    }

                    if (valid2) idx = j - 1;
                    //Console.WriteLine($"Rule {rule.Number} is {valid2} continue at {idx}");
                    return valid2;
                }
                else
                {
                    if (valid1) idx = i - 1;
                    //Console.WriteLine($"Rule {rule.Number} is {valid1} continue at {idx}");
                    return valid1;
                }
            }
            else
            {
                if (idx > input.Length - 1)
                {
                    _Invalid.Add($"{input}");
                    return true;
                }
                var f = input[idx].ToString() == rule.Value;
                //Console.WriteLine($"Equal OR Not, Rule {rule.Number} is {f}");
                return f;
            }
        }

        private static List<int> Parse(string first)
        {
            var parts = first.Split(" ");
            var rules = new List<int>();

            foreach (var rule in parts.ToList().Where(p => p != ""))
            {
                rules.Add(int.Parse(rule));
            }

            return rules;
        }

        public static List<string> GetRulesTestInput()
        {
            return new List<string>()
            {
"10: 23 14 | 28 1",
"11: 42 31",
"5: 1 14 | 15 1",
"19: 14 1 | 14 14",
"12: 24 14 | 19 1",
"16: 15 1 | 14 14",
"6: 14 14 | 1 14",
"2: 1 24 | 14 4",
"0: 8 11",
"13: 14 3 | 1 12",
"15: 1 | 14",
"17: 14 2 | 1 7",
"23: 25 1 | 22 14",
"28: 16 1",
"4: 1 1",
"20: 14 14 | 1 15",
"3: 5 14 | 16 1",
"21: 14 1 | 1 14",
"25: 1 1 | 1 14",
"22: 14 14",
"8: 42",
"26: 14 22 | 1 20",
"18: 15 15",
"7: 14 5 | 1 21",
"24: 14 1",
"42: 9 14 | 10 1",
"9: 14 27 | 1 26",
"14: 'b'",
"27: 1 6 | 14 18",
"1: 'a'",
"31: 14 17 | 1 13",
            };
        }

        //abbbb babbbaaaababbaabbbbabababbbabbbbbbabaaaa

        public static List<string> GetRulesInput()
        {
            return new List<string>()
            {
               "124: 72 26 | 58 91",
"76: 72 90 | 58 73",
"89: 58 135 | 72 25",
"43: 58 30 | 72 98",
"130: 58 58",
"87: 135 72 | 100 58",
"24: 72 5 | 58 78",
"61: 84 72 | 71 58",
"8: 42",
"68: 49 72 | 91 58",
"103: 126 58 | 124 72",
"132: 58 17 | 72 37",
"75: 72 89 | 58 50",
"99: 72 72 | 58 58",
"28: 58 19 | 72 65",
"2: 9 72 | 67 58",
"113: 58 100 | 72 130",
"66: 129 58 | 70 72",
"74: 72 116 | 58 56",
"45: 72 82 | 58 38",
"70: 137 58 | 12 72",
"78: 72 58 | 58 58",
"115: 78 58 | 27 72",
"48: 58 121 | 72 95",
"23: 120 58 | 93 72",
"44: 58 25 | 72 130",
"17: 58 27 | 72 91",
"65: 102 72 | 46 58",
"19: 72 84 | 58 35",
"79: 29 72 | 127 58",
"13: 58 88 | 72 136",
"6: 15 72 | 130 58",
"81: 58 72",
"100: 117 72 | 58 58",
"116: 78 72 | 49 58",
"136: 20 72 | 50 58",
"97: 53 72 | 85 58",
"107: 58 27 | 72 5",
"0: 8 11",
"49: 58 72 | 72 117",
"88: 105 58 | 21 72",
"83: 69 72 | 5 58",
"112: 58 32 | 72 2",
"47: 62 72 | 118 58",
"59: 58 134 | 72 101",
"60: 132 72 | 103 58",
"55: 58 44 | 72 92",
"67: 72 15 | 58 100",
"96: 81 72 | 135 58",
"121: 25 58 | 26 72",
"39: 58 81 | 72 25",
"82: 81 58",
"27: 72 72 | 117 58",
"85: 58 119 | 72 68",
"53: 72 39 | 58 80",
"91: 72 58 | 72 72",
"129: 58 133 | 72 3",
"18: 58 69 | 72 25",
"120: 110 58 | 36 72",
"114: 58 87 | 72 16",
"34: 72 40 | 58 45",
"29: 58 123 | 72 59",
"36: 15 117",
"133: 58 107 | 72 113",
"109: 58 58 | 58 72",
"46: 100 117",
"41: 109 58 | 91 72",
"26: 72 72 | 58 72",
"72: 'a'",
"21: 58 78 | 72 130",
"117: 58 | 72",
"111: 58 5 | 72 81",
"95: 58 27 | 72 109",
"4: 97 72 | 28 58",
"135: 58 117 | 72 58",
"37: 58 27",
"1: 58 99 | 72 25",
"52: 130 58 | 109 72",
"106: 5 72 | 99 58",
"12: 111 58 | 68 72",
"42: 94 58 | 79 72",
"11: 42 31",
"77: 125 72 | 86 58",
"98: 58 91 | 72 109",
"62: 58 122 | 72 52",
"119: 109 72 | 135 58",
"108: 15 72 | 25 58",
"10: 78 58 | 15 72",
"5: 72 58",
"3: 72 33 | 58 37",
"57: 58 51 | 72 7",
"7: 72 43 | 58 74",
"38: 58 91 | 72 15",
"20: 26 58 | 130 72",
"22: 58 99 | 72 15",
"105: 15 72 | 99 58",
"25: 72 72",
"35: 72 99 | 58 91",
"56: 78 72 | 99 58",
"63: 114 72 | 55 58",
"86: 23 72 | 13 58",
"54: 47 58 | 112 72",
"50: 72 5 | 58 99",
"126: 58 81 | 72 26",
"127: 63 72 | 60 58",
"9: 72 27 | 58 15",
"137: 10 58 | 96 72",
"15: 117 117",
"128: 66 58 | 57 72",
"58: 'b'",
"14: 18 72 | 116 58",
"92: 58 25 | 72 100",
"40: 83 58 | 131 72",
"31: 77 58 | 128 72",
"69: 58 72 | 72 58",
"125: 34 58 | 76 72",
"110: 117 99",
"94: 72 4 | 58 54",
"102: 72 25 | 58 109",
"30: 58 91 | 72 5",
"84: 99 117",
"93: 50 72 | 6 58",
"80: 72 100 | 58 91",
"64: 58 5 | 72 130",
"51: 58 61 | 72 48",
"122: 5 58 | 25 72",
"16: 58 130 | 72 78",
"90: 22 58 | 106 72",
"131: 58 91 | 72 130",
"123: 14 72 | 75 58",
"33: 58 15 | 72 26",
"71: 27 72 | 69 58",
"73: 104 58 | 24 72",
"32: 72 126 | 58 41",
"134: 72 1 | 58 35",
"101: 72 84 | 58 115",
"104: 130 58",
"118: 64 58 | 108 72",
            };
        }

        public static List<string> GetTestInput()
        {
            return new List<string>()
            {
              "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa",
"bbabbbbaabaabba",
"babbbbaabbbbbabbbbbbaabaaabaaa",
"aaabbbbbbaaaabaababaabababbabaaabbababababaaa",
"bbbbbbbaaaabbbbaaabbabaaa",
"bbbababbbbaaaaaaaabbababaaababaabab",
"ababaaaaaabaaab",
"ababaaaaabbbaba",
"baabbaaaabbaaaababbaababb",
"abbbbabbbbaaaababbbbbbaaaababb",
"aaaaabbaabaaaaababaa",
"aaaabbaaaabbaaa",
"aaaabbaabbaaaaaaabbbabbbaaabbaabaaa",
"babaaabbbaaabaababbaabababaaab",
"aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba",
            };
        }

        public static List<string> GetInput()
        {
            return new List<string>()
            {
                "baabababbaababbbaabbbbab",
"baabababbbabaabaabbbbbbb",
"bbbabaaababbbbaaababbabbaabbabba",
"bbbbbbaaaababbaaaababaaabbaaababaaababba",
"bbabbaaaabbbabaaaabbbbbbabbbbbaa",
"aaaabaaabbbabbaaaabbbbbabaabbbaabbbaaaabbababbbb",
"bbbabababbbbbbbbbbbbabbbababbaba",
"aabbbbaaabbabbabaaaababaabbbabbbbabbaaabaaaaabbbbaaababbabbbbbbabbbbaaababbbbaab",
"bbababbaaabbbbbbabaaaabbbbbbabba",
"babaaaabbaaabbbabaaaaabb",
"bbbabaaabaaabbbabababbaaabbbbbabababababaaaaaaba",
"bbaabaabbbaaaabaabaaabaa",
"bbaabababbaaabbbaaaaabaabababbbbbaaabbaa",
"babbbbaaaaaaaabbabbabbba",
"bbbbbbabababbabbbbbbbbba",
"bbaaababbaaabbbbaababbbababaaaabaabbbaaaabbbabbabaaabbab",
"bbaaaabaaabbbaaabbaaabaa",
"baabbbaababbbbaaaaabbbbbbaaababbbbbbaaba",
"abbabbbabaaaaaaabbabaaabaaabababbabaaabaaabbbaaa",
"aababaaababbbaabababbabbabbbabbbbaaaababaabbabab",
"ababbbbaaaaaaaabababaabb",
"ababbbaaaaaabbabbaaabbabbabbaabbaabaaaab",
"bbaabaabaaabbaaabbbbbbbb",
"babaababbbbbbaaabbababba",
"bbaabbaaaaaabbbbabbbabbbabaabbabababaaba",
"bbbbbaaababbabaabbbaabbaaabbbbababbabaabaaaaaabaaabababaaaaaaaabaaaaababbbbbaabababbabbb",
"aabbbabababbaaabbbbaaaabbabbbbbbabaabaab",
"abbbabbaabbbababbbababbabaabaaabbababbab",
"baaaabaaaaabbababbaaaaaabababbbbabbbbababaaaaaaaabbbabab",
"aabaabababbabbbbbaabbaabbbaabbba",
"aaaabbbbabbabbabbaabbaabbaaaababababaaaabaabaabbabbbbbaabaaaaaba",
"bbbaaaabbaabbaababbabbbaabaababbbbaabbbbbbaabaaaabbbbaabaaaaabba",
"babbaabbaaaabbababaaabbabababbaaabbbababbbbbaabb",
"aabbbbaaabbbabbbabbababa",
"aaabaabaabbaaaabababaaabababbaabbabaaabbabaababaabbbaaab",
"aabbbabbbaababbbbbaaabaaaabaabaabaabaabbbaabbbaabbbaabba",
"bbaabbaababbbaabaaaaaabbbbbbbbba",
"ababbabbabbbaabbabaababaaaaaaaaabbbaabaabbbabbba",
"bbbabbabbabbbabbabbaaaaabbabbbabbbaaababababbabbbaaaaaaaaaababaaaabaabaaabbbababbbbaaaab",
"babaaaabababaaaabbabaaaababaababbabaabbabaabaaabaabbabba",
"aaabaabaabbbaabbbbbbabbababbabab",
"baababbbbabaabbaabbbaaaaabbabbbabbaabaabbababbaabbabbbbaabbbbbbb",
"ababbaababbabbbbbaababaaababbaba",
"baabbbabaaabbaababaaaabbbabbbaaaaaaaabaaaabbbbabababaabbabaaaabb",
"bbbaaababbbbbbaabbbbabbb",
"bbbaaaabaabaababaaabbaaaabbabbaaaaaaabbbaaabbabbbbabbaababbaaababaaabbaa",
"aabbbabaaaaaabbbabbabbba",
"aaaaaaabababbbabaabbbbba",
"aaaaaaaaababbbaababababbbbbabbbbabababaaabababaabbbbbaba",
"bbaaaaabababaababbbaabbababbaaababbabababbababbaabbababbabaababaaabababbbbbbaaba",
"bbaaaabbaaaaaaaababbaaaa",
"aaaaabaababaababaaababaa",
"babaabbbbaaabbbabbabbbbbbaaabbababaaabaababbaaaa",
"bbbbbbaabababbaaaaabbbbabaabbaba",
"abaaabbbaababbaaaababbbbbbaaaaababbabaaabbbaababaaababaa",
"abbabbbabaaabaabaabbbabbbabbbabaababaaaaaaaaaaabbbbabaaaabbababbbaaaaaaababbabbbabbbbaab",
"aababbaaaaabaababbbaaabababbababbbbaabbbbabaabbbabaabbaa",
"bbaabababbbbaababbaaabbabaaaaabbbabababa",
"aabbbabaabbbaabaaabbbabaabaababababbaaba",
"bbabbaabbbbbbbbabbbbbaaaaabbaaabbbbabbbbbaaabbabbbbbbbbbbbbbbaababbaaabbaaaababbbbbbaaab",
"abbbababbbaaaaaabbbabbabbbbabbabbbabaabb",
"aababbaaabbbaabbababbaaa",
"abbabbbbbbbbaababaaabbbb",
"aaabbbaaaaabbbabbabbaaaa",
"ababbbbbabbabaabaaaaabaaabbabbaabbbbbaaa",
"aaaabbbabbbbababbbababbaabbbaabbaaababba",
"aaabbbabbbaabaababbbaaabbaaaababaabbabaababbabaa",
"baaaabaabaaabaabababaaba",
"abbbabbbabbbabbaaaabbaab",
"bbabbabbbaaaababbbaaabbaabbabbbbaabbbaaaabbbaabbaabbababababaaba",
"abbabbaaaababababaabbbaa",
"ababbbaabaaabbbabbbabbab",
"bbaaabbbbaaaaaababbbaababbabbbaabbaaaabb",
"bbabbabbaabbababbaabbabaaaabaaaaabababaaaaabbaab",
"bbaababaaaabbbaaaabbbbabbbabaabb",
"bbababbaabaabbbababaababaabaaaab",
"bababaaaaaaabbbabbbbaabaabbbabaababbbabbababaaba",
"babbaabbbbbbababbbabbbbbbaaaaaabbabbabaaaaabbabb",
"babbbabbabaaabbabbbbaaaa",
"aaaabbabbbababbabbaaaabaaaabbbaa",
"abbbaabaaababaaaabbabbbbbbbaaaaabbabaabbababbaaabbbbbbbabaabaabb",
"abbbabaaababaaaaaabaababbbababbaaabbabaabbbbaaabbbbbbaab",
"babbaabbbbbaababbaaabbbbabbaaaaa",
"babbbbbbbbbabaaabaabbbba",
"bbbaaabaabaaaaabbaabbababbaabbabbabbababbabbbabaaabbabab",
"aabbbaaaababbaabbbabaaab",
"bbbbbbaaababbbaabaababba",
"bbabbbbbbbabaababbbabbba",
"babbabaababbbaaababbabababbbaabbbbaaabbbaaaabbababaababb",
"ababaaaaaaaaaaabbbaaaabbbaabbbbb",
"abbbaaaaaaaaabbaababaabb",
"baabaabaaabbbabaabbabbabaabbbbba",
"abaaabbbbbabbbbbbbbaabbbbbaaaabbbbbaabbbbbabbbababbaabaa",
"aaabbbaaababbbbabbbbabbabaabaabb",
"abbabbbbbababaaababbabababbabbbbbaaaaaabbbaabaaaaabaaabb",
"ababbabbaabbaaabaaababbb",
"bbbabaaaabaaaaabbaabbaabbbaababaaaaaababaaabaaabbbbbabaa",
"babbbababbbbbbabaaabaabababbbbbbbaaabbbb",
"aababaaaababbbaaababbbaababbbaaabbbaabbaaababbba",
"ababbbbaaabbbaaaaabaababaaaaaaaaabbababababbaaba",
"aabbaaababbabbaaaaabbbaa",
"bbbbaaabbaabbababaabbbabbbabaaabbbabbabababaabab",
"babaababbbbbabababbabbbbbbbbaaaa",
"abbbabaaabaaabbbaaabbbaaaaabbabaabbabbaaaabbaabaaabaaabaabbbbabbbabaaabbaababbaabbabaabb",
"aaabbbbaabbbababababbabbabaaabbabaabaabb",
"aaabbbaabbbbababbbaababb",
"baabababbbbabbbabbaabbababbbaaabaaababbb",
"bbbabaaaabaaaaabbbababab",
"baaaaabbbabbabbbbbaaaabaababbbbaabaabbabbbaaabbaababaaba",
"abbaaaabaaabbaaababbabababbbaababbbbbbaabbbbbaaa",
"baabbbabaababaaaabbababa",
"aabbbabbabaaaaaabbbbbaba",
"aabbbbaaaaaaababbaabbbaababbabbbaaababbbabaabbbbbabbaaaa",
"aabbaabbbabaaaabbbabbaab",
"aabbaaaaabababbbabaaaaaabbbababbaababababbaabbabaaabaaaababbbbab",
"bababbaaababaaaabbbbbbbb",
"aaaaababbbaaaabbbbaabbab",
"baabbaaaaabbbaaabaaababa",
"bbbabbbabbaabaaabbbbaabbbbabbbaabbbaaaababbababbbaabababbaabaaab",
"bbababababbababbbaaabbaaabbabababbbbbbaabaababaa",
"aababbaabaaaabbbabababaa",
"aaabbaaabbbaaaabbbbbaaaa",
"bbabbabbabbaaaababbababa",
"bbbabaaaaaabbaaaaabbabbbaaabbabaabbabaaa",
"aaaaaaaabbaababaaabbbbaabaabababbbbbbaab",
"babbaabaababaabaabababaabbbbbbba",
"aababaaaababbbbbbabbabababababaaabbbaaab",
"baabbbaabbbbbbbbabbaabbb",
"baababbbabbabbabaaaaaababaaabaaa",
"ababbbababbbbbabbaabbbaa",
"bbaababaaabbbbaaababaaababbababa",
"bbabbaaaabbabbbbaaabaaab",
"abbbabbabbabaabaaaaaaabbabbbbbbbbbbbbaaa",
"abbaaabbbbaaabbbbaababbbaaaabaab",
"bbaabababaaabbbaabbaabbb",
"ababbabbaaabaabaaaaaaaabababaaababaaabbbaabbabaabbbbbaba",
"aababababaaaababbabaaaababaaabbbbaaaaaba",
"babaaabaabbbbbbbabbbbbbbbaababaabbaaabaa",
"bababbbabaabbbabaaabbbaaaaabaababaabbbaabbbbbbba",
"ababbbabbbaaaabaabaabaaa",
"ababbbbaaabbaaabaababbbbaaaaaaaaabbababb",
"bbaabaabaaaabbabaaaaaaabbabbaaaaabbbbbbb",
"aabaaabbababbbabaaaabaabbbaababb",
"babbbbabaaaabababaababbabaaaabbabbabaaabaaabbbbbbbaabbbb",
"babbaababbbbbbbabbabaabbbbabbaaaababaaaa",
"baabaabababbbbabbaaabaaaabbbaabaaabaabbabbbbabbbabaaaabababaaaabbbbbbabb",
"bbbabaabaabbbabbbaabbaabaabaabba",
"babbbababbabbabbabbbaabaaaaabbbbbabaabbbabaaaabbbaaabaaa",
"aaabbbaababbbaaaaaabbbbaaaaaabbbaaaabbaaaababbbabbbabbba",
"babbaabbababbbbaaababbaabbaaaaaa",
"abaababaabbbabaaaaabbaba",
"abbabbabaabaaaaaaaabbaba",
"bbbbbbaaababbbbaabbaaabbaababbbaaaababbb",
"abbaaabbbbbbababaabaaaaaaaaaababbbbabbaa",
"abbbabaaaabbaabbaaabababbbbbaaaa",
"bbbababbaababbbbbbaaabbbbaabbababaaaaabb",
"abbaababaabbbabbabababab",
"ababbaababaaabbbbababaaaaabababaaaabaabb",
"aaaaababbabbabaababbabba",
"aabbaababbaabaaaaaabbabababaaaaa",
"bbabbabbaabaabbbbababaaabbbababbababaabb",
"babaaababbababbabbbbbbaabbbaaaaabbaaaabbbaaaaabb",
"aaabaabaaaaaabbbaaaabababaaabbbbbbbbabbbbaababababaabbbaabaaabaabbabbbba",
"baabababbabaaaabababaaabbaabbbba",
"abaaaaaababbbaababbbbbba",
"baabbaaabaabbbabbbabbbaa",
"ababbbbbabaababaaabbabba",
"abbaaabbbabbaaabbbbbbaaa",
"ababaaaaaabbaabbbbbaaaaaabaabbbabbbabaabbababbabbbbbaaababbbbaab",
"aabaaaaaaaaabaaabababbbaababaabaaaaabbababaabbababbaaaabbbabaaba",
"babaabababababbbaaabbbaaaabaaaabaaaabbbbaaababaaaababbaabaabaaabababaabb",
"ababbaabbbabbaababbababa",
"ababaaabbbaaabaabbbababbbaabaaabaabaaabb",
"aabbbabaaabaabababaaabbaaaabbabbaaababbb",
"baaabbabbbaaaababaaabaabbaaaabbbabbaaaabaaaabaabbaaabaaa",
"baaaababbaabbbbaaaaabbba",
"bbababbaaabaababbbbbbaaa",
"bbbaaabaabaaabbabaabaaaa",
"aabbbbaabaababbaaaabbaab",
"ababababbbbabbbaabbbbabbbaaababb",
"abbaaabbbaaaabbabbbbbaba",
"aababbaaaaaaababbabbaaba",
"abbaaabababbaabaaaaaaabaabbaabaabaabaaababaaaaba",
"abaabbbababbbbaabbbbaabb",
"abaaabbaaaaabbbbaaabaaaa",
"bababbbbbaabbbbbbababbababbbbaba",
"abbbabbbabbaaaabbabbbaaaaaabaabababbbbbaabaaabab",
"ababbbbbbabbbbabaaaaabba",
"baaabbabbaaaaaaaaabbbbaabbbbbaba",
"aabaaabbabbaabbabaabaaabbabbababaaabbaabaababaab",
"abbbbabbaaabbaaaabbbbbaaababbbbbbbbaaabbbabbaaab",
"ababbaabbbbabaaaabaabbaa",
"abababbababbabaabbaaaabaaaaaaabbaaaaaaabaababbababababbbaababbba",
"aaaaababbaababaaabbbaaababbbbaabbbbabbbaababbaaa",
"bbaaaaabbabbaabbbbbbbbba",
"ababbabbabaaabbbaabababa",
"bababbbaabbaaaaabbbbbaab",
"abaaaaaabababaaabaabaabaaaaabaaa",
"abaaabbaaababaaabbbbbabaababbbbbababaabababbbbbaabbbabbbabbaaabaababbaabbaabbabb",
"ababaaaabbaabaaaabbabbba",
"bbbabaaabbbabaaaabaaaaabbababbaaabababbb",
"bbabbabbbaaaaaabbbbbbbbb",
"bbbbbbaabbaabaabaaabbabb",
"bbbabaaabbabbabbaabbabba",
"aabaababaaaaaaaaaabbbabaabbbabbaabbbbaabbbbbaaaa",
"abbaaabbaaabbbbaaababbab",
"abaabababbbabaabbaaaabaaaaabbabbabaabaaa",
"babbbaaabaabbaabbbbaabbbabbbabbababbbababbbabbaa",
"aaaaaabbbbbaaababbaababb",
"abbaaabbbbbbabbabbaabbab",
"bbbbabbaabaababaaaaabababaaaabba",
"baaababbbbaaabaaaabababbbbaabbbbbbababab",
"babbbbababbbabbbaaabaaab",
"bbaaabbbbbbaabaaaabbbbbbababbbabaaababbabbbbbaba",
"bbabababbababbbabbbbbaabbaababbbbbaababaaabbabbababbaaaaababbbbabaaabaabbbbabbaaaabbbaba",
"bbaabbbababbbbbbbaaabababbaaabaabbababbaabaaabaababbbaabbbbbbbba",
"baaaabababbaaaaabbabbbbabbbababb",
"aababbbbbabbaabbbbaaaaabaabbabbabaaababa",
"babbbbaaababbbabbbbbabababaaaaaaabbaaaba",
"babbaabbabbabbabaaabbbaabababaaababbabbaaababaabbabbabbb",
"bbaabaabaabaabbbababaabb",
"aaaabbbababbababbabbabaaaaabbbbabbbababbbbbabbbbbaaaaabb",
"aabbbbaababbbbababbbbabbbaaabaaa",
"aaabbbbbabbbaaabbaababaa",
"babaaaabaabbbaaabbaabbaa",
"aaaaabbbaaabbbbabaaabbabbabbbababbabbbab",
"bbbbaababaabababbabbabbb",
"abbaababbbaaaababbbaaabaaabbabaa",
"ababbaabaaabbbabbaabaaab",
"baababbbabaaaaaaabbbabbabbabbabaaababbba",
"baaabbbaaabbbaaababbabaabbbbbaba",
"bababbbbababbbbababaababbaabbbaabaaabbbbababaababbbbabbbbabaaababababbaababbbbaa",
"abbaaaaabbbbbbababbbbaaa",
"bbaabbababbbbaaaabaabababaaaabbababababbbbababbb",
"abbbabbaabbaaabbaaabbaba",
"aaaabbbababbbbabaababaab",
"bbaabbaaabaabbbababaaaabbbaabbbabbbabbab",
"babbbbbbbbababbaabaababb",
"bbbbabbbaaabbbbbaaaabaabaababaababaabbbb",
"babbbabbaababbaabaaaaabb",
"abbbabbbaabbbabbbbaaaaba",
"babababbaabbabbaaabbbbab",
"baaaabaaaaabaabaabbaaaba",
"ababbaaabbbabbbbabababbabbbbabbb",
"babaabababaaabbabababbaaaabaabaa",
"bababbaaabbabbabaaababaa",
"baaaabaaaabbbbbbabbabbba",
"aabbaababaabbbabbabbbbbbbabbbabbaaaaabba",
"abbaabbbabbbbbaabbaabbbbbbbbaababababbabababbaab",
"ababbbbbabaaaaabbbbaaababbbaabbbaaabaaaabaaaaababaabbbaa",
"babbababababaaaaaababbab",
"babaaabbabaaaaaabbbbaaaa",
"bbbbaaaabbbabbbabaabaabbbbbabbab",
"bababbaaabbbaababababaaaabbabbbbbbbaababbabbabba",
"ababbbababaaaaababbabaaa",
"bbbbaababababaaaababaabb",
"baaaabbbaabababababbabba",
"aabbbabbaaabbbabbbaaaaabbbabbaabaaaaaaaabbaaaabaabbaabaa",
"aabaababaababbabbbbaababaabbbbabaaabbbbbaaabaaabbababbab",
"abbabaabbbabbbaaabbaabba",
"aababaaaaababaaabbaaaabbabbbbbaaabababaa",
"baabbaababbbbaabbabbbaaaabbabaaaaaaabbbabbbaaabaaaabaaabbbaabbaaabbaabbbaabbabaaaaaabaab",
"babbbabbbaababbbabbabaaa",
"aaababababbbaabbabbabbba",
"babbbbbbaaabbaaabbbbbaab",
"abbbaabbaaaaabbbbbabbbab",
"bbaabababaabbbabbabababa",
"bbabbbbabbbaaaabbaabaabaaabbbabbbbbababb",
"abbabbabbbbbaabaababbbbbbbababbaabbbbbbaaabaaabbbbaabbba",
"bababaabaababaaabbabbbbb",
"babbaabbbbaabaabbbaabbbabaabbaaabbbbaaabaabbabba",
"bbabbaabababbbbabbbabaaa",
"baabbaabaaaaabaabbaaabab",
"aaabababbabbbaaabaaaabbaaaabbbaabaaaabbbabbbbbabaaaabaababbbbaab",
"baabbbabaabababaaaaabaabaaaaabaaaaaaabbaaabbbaabaabbbbaabaaaabba",
"bbaabbaaaabbbaaaabaaabab",
"baaaaaaaaabbbababaaaaabbaabbbbbabbaaabab",
"aabbaabbabbbaaaabbbabbaaabaabbbbababbaba",
"abaabbbaaaabbbaabbaaaabbbaabbbbb",
"bbbbabbbaaabaabbaaaaabba",
"babbabaabaaababaabaabbabbbaaaaaa",
"abbbbaababbbaabbabaaabbbbaaaaababaaabababbbbbaab",
"bbaaaabbbbbbaaabbababaabaaabbbaababbbbabaaabbababbbababaaaaaabbbbbababba",
"baaaabbbbbbbabbabbaababb",
"bbaaabbaabbbaaaabaabbaba",
"abaaaaabbbbbaabaaababbbbbbbaaaab",
"aaaabbbaababaaabbbbbabbabbaaaabbaababababbbbbababaabbababbbbaaaabbbaaabb",
"abaaabbbbbbaaaaabaabbbaa",
"aabaababbbabbbbaaaabbbaabbaaaaabaaabaabb",
"bababaabbababaabbbaaabaa",
"baaaaaabbabaabbbbabaaabbbbabaabb",
"bbabbbbbabbabbababbaaabbbabbababbaabbbbbbbbabbaa",
"aabbbbbaabbbabbabababbbaaabbbbbaaaabaaab",
"bbbabaabaabbbabababbabaababbbbbababbaaaa",
"aaaabababaaababbbbbbbababbaabbbbbbbbaabbaababbbbaaababaaabbbaababaabbabbbbbbbbbb",
"aaaabaaabbabbabaabbbaaab",
"aaaabaabbbbaaaabbaaabaabbababababbbaabaaababbabaaabbabaa",
"bbbabaaabaaaabbaabbbbaab",
"abbabbbbbbabaaaababbbaabbaaaaababaabbabbbbbaababbabaaaaa",
"abbabbaaaaaabababaaabbabaaaaabaabbbaabbaababaaba",
"aababbbabaababaaabbbaabbbbbbabaaaabbaababababbaaaaaaababbbbbbabbaababbbaaababbaabbbbabab",
"abbaaaabbbaaaaabbaabbaaaaabbabbbbbbabaabaabbabbbbbbabbbaabbababb",
"baababbaababbabbbaabbaba",
"bbababbaabbaababaaababbabbaaaaabbbabbabaabbbbababbabbbabababaaaa",
"aabbbbababababbaabaabbaababbbbba",
"abaaaabbaaabbaaabbabbbaa",
"abbaaabbabaabbbabababaabbbbaabba",
"ababaaababaaabbbabaaabaa",
"aabbaabaaabbbbaabaabaaab",
"aabaabbbabbaaaaaabbbbabaaabaabbaaaababbbaabbabbabbbbaaaaabbaabaa",
"baaabaabbaaaaaaabaaaaaabbababaaaabbbababbbbbaaaa",
"aababbbabbabaaabbababaaaabaabaaaabbbaaba",
"abbabbbabbbabaaabbabababbbabaaaabbaaaaaaaaabababbbabbbab",
"bbbabaaaaaaabbabababbaabaaaaabaaabaabbbabbbabbbabaababbbbbababbbaaaaabbb",
"baabababaaaaabbbbbbabbbb",
"abbaaabbaaabaabaabaabaab",
"abbbbbabbabaaababbabaabbbbabaaaaabbbbaabbaaabababaabaaaabababbbabbabbbaaabbaaabbababbabb",
"ababaaababaabbbabbbbaabb",
"bbaababaabababbaabbbabbbbbabbabaabaabbaa",
"babaabbbababbbbabaaaababbbaaaabababababaabababab",
"bbababbabbbbababaabbabaa",
"bbaaaabababaababaaaabbbaabaaaaabbaabbbabbbbaabba",
"bbabaababbbabaabaabaababababbbaabbbbbaabbbabbaba",
"bbababbabbabaaabaaaabbbb",
"baababbbbbaabbaaabaabaaa",
"abaaaaaaabbaaaababaabaabbbbbaabbbaaababb",
"abaaaabbbabbabababbbbaaa",
"abbbababaabbaabbbaaabbaabbbabbbbaabbbbbabbbbaabb",
"aabbaabbbbabbaaaaabbbbba",
"bbbbabbaabaaaabbbaaababbabbababa",
"babbbabaaabaaaaaabbbbbabaabbabab",
"baaaaaababbbababbbbababa",
"baabbaabaabaaaaabaaabaabaaaabaabaabbbbba",
"aabaabbbbabbababaaabaabaaabaabaa",
"aabbaabbbabbabbbbababaababaaaaabbbaababaabbabbaabbabaaabbbabbaab",
"bbabbaabbabaaabaabbbaabbbbbaaaaaaaaabbaaaabaabaa",
"babbaaabbabaabbbaababbba",
"aaaaabbbbbaaabbaabbabbaabababbbbaaababba",
"babbbaaaabaaabbabaaabbabbbabbaabbbababbb",
"babbbabbbabbbbaabababbaaabababbbbbaababbbbaabaaabbbbaabb",
"bbabbbbbaabbbabbbabaabbbabababaa",
"abbbabbabaaabbbbaaaabaaa",
"babbbbaabaaaaaababaabbbb",
"aaaababababbbaabbbbaabbbbabbaaabbbaaaaaa",
"abbbaabbbababaaabaaaaabb",
"bbaabbaabbaaabbabbaaabbaabaabaabbaaabbbb",
"bababbababbabbbbabababbaaaabaabaaabaaababbaabaabbbabbabbaabbabbbaaaaaaababaaabba",
"abbababaaaabaaabbaabbaba",
"baaababbbabaaabaaaaabaaaabaaaaababbbbbbbbabbabbbabbbabaabaababbabaaabbaaaaabaaabaabaaabababbbaab",
"bbaaabbbabbabbaaabababab",
"aabababaabbabbbbbbababbaaaaabbbbabbbbbba",
"aaaaaabbaaabababaababaaabaaababb",
"abbabaabbbbaaaaaabbaaaba",
"abbbababaaabaababaaaaaaababaaaababababbbaabbbbba",
"abbabbaababbbbababbaaaabbbbaabbbbabbbabbbabbabba",
"bbbaabaaaaabbbaababbaabbbbbabaabbaaababaaaaababb",
"bbaaaaababaabaaabaaabbaaabbaababbbbaaaabbbbbbaba",
"bbaaaabbabbbbbbbaaaaaabbabbbbbabaababbaabaaabaabbbabaaabbaaabbbabbbabaabaaaabbaaaabaaabb",
"abbabbaabbaaabbbbabbbbaabbaabaababaabaaaabbbaaababaabaabaaabaaab",
"abbbabaaaabbaaabbbbabbaa",
"baabaaaabaaaabbaabaaaaab",
"aabbbabaabaaabbbbabbabba",
"bbababbabbbaabbbbbbaaaabbaaabbabbbaabaaa",
"aaaabbbbbaaaababaabababababaabba",
"aabbbabababbbaaaaaabbbbaaaabaaaa",
"abbaaaabbababbbaabbabbabababaababbbaaabb",
"baaaaaabbbabaaaaabbaabbb",
"aaaaaaabbbaaaabaabaaabaa",
"aabbbbbbbbabbaabababaaba",
"bbbbbbaaaaabbaaaabbaabbb",
"bbaaabbbbabaaabbbaaaabaaaabbaabababbbabbabaaaabaabababbb",
"abbbbbabaabbbabbbbbababb",
"abaababaabbbaabbabbbaababbababbbbbbbbbba",
"aabbbabaaaabbaaabbaabbaaabbbbaaa",
"aabbabbbaaaabbabababbaba",
"aaabbbbababbbaaababaabbbbaabbaba",
"aabbbababbabbaaaabbaaaaaaaaaaaaabbbaabbaaabababb",
"ababbbabbabaaababbaabbbb",
"aabbbabbbaabbbabababbabbabbbabbabaaabbbabaabbbbb",
"abbbabbaaabbbababbabbbbbbbbbabaabaaaaabbabbbbababbbababb",
"baabaababaaaababbababbbb",
"bbaaaaaabaabbbaaaaaabbbaaabbbbabbbaaabbbbbbbbbbbabbbbaaababaabaaaaaaaaabbbabaababaabaabb",
"aababbbbabaaababbbabbaaaaaaaaabbaabbbbbaaababababababaababbaabababababbbabbbababbbaabaab",
"bbabaabaaabbbbbbbababbaa",
"aaababbabbbbababaababaabbbababaabbbabaababaaaabbaabbbaaa",
"bbababbaaabbbaaaabbaabbb",
"aabbbabaababbbaabbbabaaaaaaabbbabbbbbbaaabbababaabbaaabaabaabbaa",
"ababbbaaaabaabbbbbbbbbbb",
"abbbabaababbabaababaabba",
"aaabbbaababbabaabbbbaababbabbbab",
"bbaaaababaaabbbabbaababbababbababaaabaaa",
"bbaaabbaabbaababaaaaabbbbaabbbaaabbbbaaa",
"aaaaabababbbaaaabbbbbbabbabbbaababbabbaabbbbaabb",
"aaabbaaaabbbabaabbbbbabbbbaababbbbbbbbbabbaaaabaabbbabbbaaaabbabbbabbaaaaaaaabbbbbabaaba",
"babbbaabbaabaabaaaaabbbabbbbaabaaaabaabbbbabaabb",
"abbabbabbabaaabaabbaabaa",
"abaaaabbaaaaaaabbaaabbbabbaaabaa",
"bbbbbbaababbababababbaba",
"bbaaabbaabbbabbbabbaabba",
"babaabbbaabaababaabaabba",
"ababbbabbabbbaaabababbab",
"bbaabbaaaaaaaaabbaabaaaa",
"abbbbbabbbbabaaaaaabbbbabbbbbbbabaabbbaaaababbab",
"bbbbaababaaabbabababaaba",
"babbabaababbaaabaababbaa",
"babbbaaaababaaababbbabaaabbbabaaabbbbbba",
"bbbaabaababbababbbabaabb",
"aababbbbabbabbbbababbbabbbabbababbababab",
"baaaabbbbaaabbabbaabaabaaababbaaaaaaaabbbaabbbaaaabaaabb",
"aaaaaaaaababbbbbbbbaaaaabababaaabbbbbaaa",
"aaaaabababbabaabbabbabaaaaaabaab",
"ababbbabbabbbaabbaaababb",
"abaaabbababaaaababbabbaaabaaabaa",
"babbabbbbabaaabbaabbbbaaabbbabaababbbbbbabaabbabaaabbbba",
"aaaaabbbabbabbbbbbaaaaaa",
"aaababbabbabaaabbabbabbb",
"babaabbbabbbabbbaabbbbbbbaabbaabaababbab",
"aaabbaabbaabbbbaaaaabbabbbaaaabbbaabababaaabbbabbababaaabbaababb",
"babbbbabababbabbbababbab",
"bababbbbbaabaaabaabaaababbaabaabaaaaaaabbbababaaaaabaaba",
"aaaaaaaabaaaabbbbbaaabaa",
"bbabbbbbaabaaaaaaaaaabaaabaabababbbbbbaaaaababbaaaababbb",
"aaabababaababbababbbbbaabbaabbba",
"baababababaaabbbbaaabbbb",
"ababbbabbaababbbbaaaaaababbbaabbaaaaabaabaabaaaa",
"babbaabbaabbaabbbaaaabbbababbbbbbabbbabbaaaababbbaaabaaaaababbaaaaababbbbbbbabbaaaababaa",
"aaaaaabbbbabaababbbaaababbababaa",
"babbbbaababbaabbabbbbaba",
"bbaabbaaaaaabbbbabababbbbbabbbbbbaaabaaaabbabbbabbabbaaaaaaababa",
"ababbbaababbbaaaaabaabaa",
"baaaabbbbbbbabbaabbabaaa",
            };
        }
    }
}
