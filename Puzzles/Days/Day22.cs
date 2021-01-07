using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public static class Day22
    {
        public static Queue<int> Player1 = new Queue<int>();
        public static Queue<int> Player2 = new Queue<int>();
        public static void RunDay()
        {
            //var input = GetTestInput();
            var input = GetInput();

            bool first = true;
            foreach (var num in input)
            {
                if (num == "")
                {
                    first = false;
                }
                else
                {
                    if (first)
                    {
                        Player1.Enqueue(int.Parse(num));
                    }
                    else
                    {
                        Player2.Enqueue(int.Parse(num));
                    }
                }
            }

            int win = PlayGame(Player1, Player2);

            int points = 0;
            int mult = 1;
            var reverse = win == 1 ? Player1.Reverse() : Player2.Reverse();
            foreach (var num in reverse)
            {
                points += mult++ * num;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(points);
        }

        public static int _Games = 0;
        private static int PlayGame(Queue<int> Player1, Queue<int> Player2)
        {
            _Games++;

            Dictionary<int, Queue<int>> player1Snapshots = new Dictionary<int, Queue<int>>();
            Dictionary<int, Queue<int>> player2Snapshots = new Dictionary<int, Queue<int>>();

            int round = 1;

            while (true)
            {
                //Console.WriteLine($"Game {_Games} Round {round}!");
                //Console.Write("Player1:");
                //foreach (var num in Player1)
                //{
                //    Console.Write($"{num},");
                //}
                //Console.WriteLine();
                //Console.Write("Player2:");
                //foreach (var num in Player2)
                //{
                //    Console.Write($"{num},");
                //}
                //Console.WriteLine();

                List<int> sameRounds1 = player1Snapshots.Where(s => s.Value.Equal(Player1)).Select(kvp => kvp.Key).ToList();
                List<int> sameRounds2 = player2Snapshots.Where(s => s.Value.Equal(Player2)).Select(kvp => kvp.Key).ToList();

                if (sameRounds1.Any(r => r != round - 1 && sameRounds2.Contains(r)))
                {
                    //Console.WriteLine("Prevent --> Player1 wins!");
                    return 1;
                }
                else
                {
                    int p1 = Player1.Dequeue();
                    int p2 = Player2.Dequeue();

                    if (Player1.Count >= p1 && Player2.Count >= p2)
                    {
                        int w = PlayGame(Clone(Player1).Take(p1), Clone(Player2).Take(p2));
                        if (w == 1)
                        {
                            //Console.WriteLine("Player1 wins!");
                            Player1.Enqueue(p1);
                            Player1.Enqueue(p2);
                        }
                        else
                        {
                            //Console.WriteLine("Player2 wins!");
                            Player2.Enqueue(p2);
                            Player2.Enqueue(p1);
                        }
                    }
                    else
                    {
                        if (p1 > p2)
                        {
                            //Console.WriteLine("Player1 wins!");
                            Player1.Enqueue(p1);
                            Player1.Enqueue(p2);
                        }
                        else
                        {
                            //Console.WriteLine("Player2 wins!");
                            Player2.Enqueue(p2);
                            Player2.Enqueue(p1);
                        }
                    }
                }

                player1Snapshots[round] = Clone(Player1);
                player2Snapshots[round] = Clone(Player2);

                //Console.WriteLine();

                if (Player1.Count == 0)
                {
                    return 2;
                }
                else if (Player2.Count == 0)
                {
                    return 1;
                }

                round++;
            }
        }

        private static Queue<int> Clone(Queue<int> cards)
        {
            return JsonConvert.DeserializeObject<Queue<int>>(JsonConvert.SerializeObject(cards));
        }

        public static List<string> GetTestInput()
        {
            return new List<string>()
            {
            "9",
"2",
"6",
"3",
"1",
"",
"5",
"8",
"4",
"7",
"10",
            };
        }

        public static List<string> GetInput()
        {
            return new List<string>()
            {
              "31",
"24",
"5",
"33",
"7",
"12",
"30",
"22",
"48",
"14",
"16",
"26",
"18",
"45",
"4",
"42",
"25",
"20",
"46",
"21",
"40",
"38",
"34",
"17",
"50",
"",
"1",
"3",
"41",
"8",
"37",
"35",
"28",
"39",
"43",
"29",
"10",
"27",
"11",
"36",
"49",
"32",
"2",
"23",
"19",
"9",
"13",
"15",
"47",
"6",
"44",
            };
        }
    }

    public static class QueueExtensions
    {
        public static Queue<T> Take<T>(this Queue<T> queue, int chunkSize)
        {
            Queue<T> newQueue = new Queue<T>();

            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
            {
                newQueue.Enqueue(queue.Dequeue());
            }

            return newQueue;
        }

        public static bool Equal<T>(this IEnumerable<T> queue, IEnumerable<T> other)
        {
            foreach (var i in queue)
            {
                if (other.Contains(i) == false)
                {
                    return false;
                }
            }

            foreach (var i in other)
            {
                if (queue.Contains(i) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
