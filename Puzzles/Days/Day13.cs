using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public static class Day13
    {
        public static void RunDay()
        {
            Dictionary<int, List<long>> schedule = new Dictionary<int, List<long>>();

            //int timestamp = 939;
            //var busesString = "7,13,x,x,59,x,31,19";

            long timestamp = 1000066;
            var busesString = "13,x,x,41,x,x,x,37,x,x,x,x,x,659,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,19,x,x,x,23,x,x,x,x,x,29,x,409,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,17";

            var buses = busesString.Split(',').Where(s => s != "x").ToList().ConvertAll<int>(x => Convert(x));

            var max = buses.Max();
            var myDepart = Int64.MaxValue;
            var myBus = 0;

            foreach (int bus in buses)
            {
                schedule[bus] = new List<long>();
                for (long dep = 0; dep <= timestamp + max; dep += bus)
                {
                    if (dep >= timestamp)
                    {
                        schedule[bus].Add(dep);
                        if (dep < myDepart)
                        {
                            myDepart = dep;
                            myBus = bus;
                        }
                    }
                }
            }

            Console.WriteLine(myBus * (myDepart - timestamp));
        }

        private static int Convert(string x)
        {
            return Int32.Parse(x);
        }
    }
}
