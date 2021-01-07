using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzles
{
    public static class Day13_2
    {
        static Dictionary<long, long> _buses = new Dictionary<long, long>();

        public static void RunDay()
        {
            ulong r1 = 624236161987028390;
            ulong r2 = 164700069547;

            ulong r3 = r1 * r2;

            Console.WriteLine(r2);
            Console.WriteLine(r1);
            Console.WriteLine(r3);
            Console.WriteLine(long.MaxValue);
            Console.WriteLine(ulong.MaxValue);
        
            //var busesString = "3,5,x,x,7";
            //var busesString = "7,13,x,x,59,x,31,19";
            //var busesString = "67,7,59,61";
            //var busesString = "67,x,7,59,61";
            //var busesString = "67,7,x,59,61";
            //var busesString = "1789,37,47,1889";
            var busesString = "13,x,x,41,x,x,x,37,x,x,x,x,x,659,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,19,x,x,x,23,x,x,x,x,x,29,x,409,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,17";

            var buses = busesString.Split(',').ToList();

            long c = 0;
            long meetTime = 1;
            foreach (var bus in buses)
            {
                if (bus != "x")
                {
                    var b = Int64.Parse(bus);
                    _buses[b] = c;
                    meetTime *= b;
                }
                c++;
            }

            Console.WriteLine($"multiply all gives: {meetTime}");

            while (_buses.Count > 1)
            {
                var a = _buses.ElementAt(0);
                var b = _buses.ElementAt(1);

                long gcd;
                long solution = Find_any_solution(a.Key, -b.Key, b.Value - a.Value, out gcd);

                long lcm = FindLCM(a.Key, b.Key);
                long dist = MathMod(solution * a.Key, lcm);// (solution * a.Key) % lcm;
                if (dist < 0) dist += lcm;

                long newBus = lcm;

                _buses.Remove(a.Key);
                _buses.Remove(b.Key);
                _buses[newBus] = dist + a.Value;

                //_buses = _buses.OrderBy(k => k.Value).ToDictionary(x => x.Key, x => x.Value);
            }

            Console.WriteLine($"last bus is: {_buses.First().Key} @ {_buses.First().Value}");
            Console.WriteLine($"difference is: {_buses.First().Key - _buses.First().Value}");
            Console.WriteLine($"must be: 939490236001473");
        }
        
        static long MathMod(long a, long b)
        {
            return (Math.Abs(a * b) + a) % b;
        }

        public static long Find_any_solution(long a, long b, long c, out long g)
        {
            long x0, y0;
            g = Gcd(Math.Abs(a), Math.Abs(b), out x0, out y0);

            if (c % g != 0)
            {
                return 0;
            }

            x0 = x0 * (c / g);
            y0 = y0 * (c / g);

            if (a < 0) x0 = -x0;
            if (b < 0) y0 = -y0;

            return x0;
        }

        public static long Gcd(long a, long b, out long x, out long y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }

            long x1 = 0;
            long y1 = 0;

            long d = Gcd(b, a % b, out x1, out y1);
            x = y1;
            y = x1 - y1 * (a / b);

            return d;
        }

        public static long FindLCM(long a, long b)
        {
            long num1, num2;
            if (a > b)
            {
                num1 = a;
                num2 = b;
            }
            else
            {
                num1 = b;
                num2 = a;
            }

            for (long i = 1; i <= num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }

            return num2;
        }

    }
}
