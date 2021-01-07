﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public class Seat2
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public string symbol { get; set; }
        public List<int> Neighbors { get; set; }

        public void Occupy() => symbol = "#";
        public void Empty() => symbol = "L";
        public void Floor() => symbol = ".";

        public bool isEmpty() => symbol == "L";
        public bool isOccupied() => symbol == "#";
        public bool isFloor() => symbol == ".";

        public int NumberOfOccupiedNeighbors(Dictionary<int, Seat2> seats)
        {
            int count = 0;
            foreach (var neighborId in Neighbors)
            {
                if (seats[neighborId].isOccupied())
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class Day11_2
    {
        public static Dictionary<int, Seat2> seats = new Dictionary<int, Seat2>();

        public static void RunDay()
        {
            var lines = GetInput();
            //var lines = GetTest();

            int idx = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    Seat2 seat = new Seat2();
                    seat.Id = ++idx;
                    seat.X = i;
                    seat.Y = j;

                    if (lines[i][j] == 'L')
                    {
                        seat.Empty();
                    }
                    else if (lines[i][j] == '.')
                    {
                        seat.Floor();
                    }

                    seats[seat.Id] = seat;
                }
            }

            foreach (var kvp in seats)
            {
                Seat2 seat = kvp.Value;
                seat.Neighbors = new List<int>();

                int r = 1;
                var n = seats.Values.Where(s => s.X == seat.X - r && s.Y == seat.Y - r).FirstOrDefault();
                while(n != null && n.isFloor())
                {
                    r++;
                    n = seats.Values.Where(s => s.X == seat.X - r && s.Y == seat.Y - r).FirstOrDefault();
                }
                if (n != null)
                {
                    seat.Neighbors.Add(n.Id);
                }

                r = 1;
                n = seats.Values.Where(s => s.X == seat.X && s.Y == seat.Y - r).FirstOrDefault();
                while(n != null && n.isFloor())
                {
                    r++;
                    n = seats.Values.Where(s => s.X == seat.X && s.Y == seat.Y - r).FirstOrDefault();
                }
                if (n != null)
                {
                    seat.Neighbors.Add(n.Id);
                }

                r = 1;
                n = seats.Values.Where(s => s.X == seat.X + r && s.Y == seat.Y - r).FirstOrDefault();
                while(n != null && n.isFloor())
                {
                    r++;
                    n = seats.Values.Where(s => s.X == seat.X + r && s.Y == seat.Y - r).FirstOrDefault();
                }
                if (n != null)
                {
                    seat.Neighbors.Add(n.Id);
                }

                r = 1;
                n = seats.Values.Where(s => s.X == seat.X - r && s.Y == seat.Y).FirstOrDefault();
                while(n != null && n.isFloor())
                {
                    r++;
                    n = seats.Values.Where(s => s.X == seat.X - r && s.Y == seat.Y).FirstOrDefault();
                }
                if (n != null)
                {
                    seat.Neighbors.Add(n.Id);
                }

                r = 1;
                n = seats.Values.Where(s => s.X == seat.X + r && s.Y == seat.Y).FirstOrDefault();
                while(n != null && n.isFloor())
                {
                    r++;
                    n = seats.Values.Where(s => s.X == seat.X + r && s.Y == seat.Y).FirstOrDefault();
                }
                if (n != null)
                {
                    seat.Neighbors.Add(n.Id);
                }

                r = 1;
                n = seats.Values.Where(s => s.X == seat.X - r && s.Y == seat.Y + r).FirstOrDefault();
                while(n != null && n.isFloor())
                {
                    r++;
                    n = seats.Values.Where(s => s.X == seat.X - r && s.Y == seat.Y + r).FirstOrDefault();
                }
                if (n != null)
                {
                    seat.Neighbors.Add(n.Id);
                }

                r = 1;
                n = seats.Values.Where(s => s.X == seat.X && s.Y == seat.Y + r).FirstOrDefault();
                while(n != null && n.isFloor())
                {
                    r++;
                    n = seats.Values.Where(s => s.X == seat.X && s.Y == seat.Y + r).FirstOrDefault();
                }
                if (n != null)
                {
                    seat.Neighbors.Add(n.Id);
                }

                r = 1;
                n = seats.Values.Where(s => s.X == seat.X + r && s.Y == seat.Y + r).FirstOrDefault();
                while(n != null && n.isFloor())
                {
                    r++;
                    n = seats.Values.Where(s => s.X == seat.X + r && s.Y == seat.Y + r).FirstOrDefault();
                }
                if (n != null)
                {
                    seat.Neighbors.Add(n.Id);
                }
            }

            bool cont = true;
            while (cont)
            {
                cont = false;
                var previousSeats = JsonConvert.DeserializeObject<Dictionary<int, Seat2>>(JsonConvert.SerializeObject(seats));

                foreach (var kvp in seats)
                {
                    if (kvp.Value.isFloor() == false)
                    {
                        if (kvp.Value.isEmpty() && kvp.Value.NumberOfOccupiedNeighbors(previousSeats) == 0)
                        {
                            cont = true;
                            kvp.Value.Occupy();
                        }
                        else if (kvp.Value.isOccupied() && kvp.Value.NumberOfOccupiedNeighbors(previousSeats) > 4)
                        {
                            cont = true;
                            kvp.Value.Empty();
                        }
                    }
                }
            }

            int count = seats.Where(kvp => kvp.Value.isOccupied()).Count();
            Console.WriteLine(count);
        }

        private static List<string> GetTest()
        {
            return new List<string>()
            {
"L.LL.LL.LL",
"LLLLLLL.LL",
"L.L.L..L..",
"LLLL.LL.LL",
"L.LL.LL.LL",
"L.LLLLL.LL",
"..L.L.....",
"LLLLLLLLLL",
"L.LLLLLL.L",
"L.LLLLL.LL",
            };
        }

        private static List<string> GetInput()
        {
            return new List<string>()
        {
"LLLLLL.LLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LL",
"LLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.L.LLLLLL.LL.LLLLLL",
"LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLL.LLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLL..LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLLL.LLLLL.LL.LLLLLLLLLLLLLL.LL.LLLLLLLLL",
"LLLLLL.LLLLL..LLLLL.LLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.L.LLLLL.L",
"LLLLLL.LLLLLL.LLL.L.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLL.LLLLLL..LLLLLLLL.LLLLLLLL.LLLLLLLLLLLLLL.LLL",
".LLLLLLLLLLLL.LLL.L.LLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLLL.LLLLLL.L.L.LLLLLLLLLLLLL.L.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL..LLLLLLLLLL..LLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLLLLL.LLL.LLLL..LLLLLLLL",
"..........LL....L..LL..L.....L...L....L............L.LL...L.......L.L.LL...L.L...L....L.L....LLL..",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLL.L.LLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLL.LL..LLLLLLL.LLLLLLLLLL",
"LLLLLLLLLLLLL.LLLLL.LLLLLLL.LLL.L.LLLLLLLLLLLLLL.LLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLLLLLLL.LLL",
".LLLLL..LL.LL..LLL.LLLLLLLL.LLLLL.LLLLL.LLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLL",
"L.LLLL.LLL.LLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLL.LLL.LLL.LLLLLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLL",
"LLLLLL.LLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLL.L.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLL.LLLLLLL.LLLLLL.L.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"...LLLLL.......L.LL..L..LL...LL..L.....L...L...LLL.L.L.....L.L....L.L......LL..L.L.L..LL.L.....LL.",
"LLLLLLLLLLL.L.LLL.LLLLLLLLL.LLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLL.L.LL.LLLLLLLLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLLLLLLL.LLL.LLL.LLLLLLLL.LLLLLLLL..LLLLLLLLLLLLLL.LL",
"LLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLL.LLLLL.L.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLL..LL.L.LLLLLLL.LLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLL",
"L.LLLL.LLLLLL.LLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLL.LLLL.LL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
".LLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLL.L.LLL.LLLLLLLL.LLL..LLLLLLL.LL.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.L.LLLLLL.LLLLLLLLLLLLLLLLL.",
"L.LL..LLL.L.........L.L....L...LL.L......L..LL.......L.....L.LL.....L..L.L.....L.....L.L...L.L....",
"LLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLL..LLLL..LLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLL",
"LLL.L..LLLL.L.LLLLLLLLLLLLL.LLLLL.LLLLL.L.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL..LLLLLLLL",
"LLLLLL.LLLLLL.LLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLL..LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL",
"LL.LLLLLLLLLLLLLLL..LLLLLLLLLLLL..LLLLLLLLL.LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLL",
"LLLLLL..LLLLLLLLLLLLLLLLL.L.LLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"..L...L...L..L..LL...L..........L.....L......L....L.L..L.L.L..L.LL....L..L...L...L......L.....L..L",
"LLLLLLLLLLLL..LLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLL.LL.LLLLLLLLLLLLL.L.LLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLL.L.LLLLL.LLLLLLLLLLLLLLLLL..LLLLLLL.LLLLLLLLL",
"LLLLLLLLLLLLL.LLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLL.LLLLLL.LLLLLLLLLL.LLLLLLLLL",
"LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLL.LLLL.LLLLLLLL.LLLLL.LLLLLLLLLL.L",
"LLLLLLLLLLLLL.L.LLL.LLLLLLL.LLLLL.LLLLL.LLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLLLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLL..LLLLLLLLL",
"LLL..L..LLLL.LLL.L......L...LL.L..L.L...L...LL....L.L.L.L................L....L..L...L......LLL.L.",
"LLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLL..LLLLL.LLLLLLLL.LLLL.LLLLLLL.LLLLLLLL..LLLLLLL.LLLLLLLLLLLLLLLLLL",
"LLLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLL.LL.LLLLLLLL.LLLLLL.LLLLLLLLLL.LLLLLLLLL",
".LLLLL.LLLLLL.LLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLL.LLLL",
"LLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLL..LLLLLLLL.LLLLLLLL.LLLLLLLL.L.LLLLLLL",
"LLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLL.LL.LLLLLLLLLLLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLL.LL.LLLLL.LLLLL.LLLLLLLL..LLL.LLLLL.L.LLLLLLLLLLLLLLLLL.LLL.LLLL.LLLLLLLLL",
"LL.LLL.LLLLLL.LLLLL.LLLLLLLLLLLLL.LL.LLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLL.L.LL.LLLLLLLLLLL.LLLLLL",
"LLLLLLLLLLLLL.LLLLL..LLLLLLLLLLLLLLLLLL.L.LLLLLL.LLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLL",
".LLLLL..L.L.L...L...L..LL.L.L.LL.L...L.L.L...................LL..L...L......L..L.........L.....LL.",
"LLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLLL.LLLL.L.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLLL.L.LLLL.LLLLL.LLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLLLL.LLLL.LLL.LL..LLLL.",
".LLLLL.LLLLLLLLLLLL.LLLL.LLLLLLLL.LLLLLLLLLLLLLL.L.LLLL.LLLLLLLLLLLLLL.LLLLLLL..LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLL...LLLLL.LLLLLLL.LLLL.LLLLLL..LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
"LLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLL.LLL.LLLLLLLLL.LLLLLLL.LLLLLLLL..L.LLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LL.LLLLLLLLLLLLLL.",
"LLL.LL.LLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL",
"LLLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLLLL.LLLLLLLL.LL.LLLL.LLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL",
"LLLLLL.LLL..L.LLLLL.LLLLLLLLLLLLLLLLL.L.LLLLLLLLLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"L.LL.L.L...L.LL.L.LLL...L.......LLL.L.LL..LLLL.L.L...L..LL.L..LL..L.L..LLLL.L.L..L.L....L....L...L",
"LLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLL.LLLL.L.LLLLLLL",
"LLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLL.LL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL",
"LLLLLL.L.LLLL.LLLLL.LLL.LLL.LLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLL..LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLL.LLLLLLLL.LLLLL..LLLLLLLLLLLL.LL.LL.LLLLLLLLLLLLL.LLLLLLL.LLLLLLLL.L.LLLLLL.LLLLLLLLLLLLLLLLLL",
"LLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLL..LLLLLLL.LL.LLLLLL",
"LLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLL.LLLL..LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLL.LLLLL",
"L.L..L................L..LL.....LL...L............L....LL..LL.LLL.L.............L..L...L.....LLL..",
"LLLLLLLLLLLLLLLL.LL.LLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLL.LLLLLL.LLLLLLL.LLLLL.LLL.LLLLLLLLLL.LLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.",
"LLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLL.LL.LL.LLLLLLLL.LLLL.LLLLLLL.LL.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL",
"LLLL.LL.LLLLL.LLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLL..LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLL.LLL.LLLL.LLLLLL.L.LLLLLLLL.LLLLLLLLL",
"L.L..L...LL.L.....LL..LL.L.....L.L..L....L..L.L...L.LLLL....L...LL......L..L....LL..L..L........LL",
"LLLLLLLLLLLLLLLLLLL.LLLL.LL.LLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLL.LLLL.LLLLLLLL.LLLLL.LLL",
"LLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLL.LL.LLLLLL.LLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL",
"LLLLLL.LLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLL..LLLLLLL.L.LL.LLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLL..LLLLLLLLL",
"LLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLL.LLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLLLLLLLLLLLLL.LL.LLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLL.L.LLLLLLLLLLLLL.LLLLLLLLL.L.LLLLLLLL.LLLL.LLLLLLL.LLLLLLLL.LLLLLLL..LLLLLLLL.LLLLLLLLL",
"LLLLL..LLLL.L.LLLLL.LLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLL..LLLLLLLL.LLLLLLLLL",
"L.LLLLLLLLLLLLLLLLL.LLLLLLL.LL.LL.LLLLL.LLLLLLLLLLLLL.LL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LL.L......L......L...L..LLL.....L.L..L.L...L......L..L..L.....L...LL.L.LL.L.LLL....L.LLL....L..L..",
"LLLLLLLLLL.LLLLLLLLLL.LL.LL.LLLLLLLLLLL.L..LLLLL.LLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLL.LLLLL.LLLLL.LL..LLL.LLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL..LL.LL.LLLLL.LLLLLLL.L.LLLLLLLLL.LLLLLLLL.LLLLLLLL.LLL.LLL.LLLL.LLLLLLLLLLLLLLLL..LLLLLLLLL",
"LLLLLLLLLLL.LLLLLLL.LLL.LLLLLLLLL.LLLLL.LLLLLLLL.LLLLLL.LLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLLLLLLL.L.LLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.L.LLLLLLLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL..LLLL.LLLLLLLLLLLLLLLL.LL.LLLL.LLL.LLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.L.L.LLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLL.LLLLLLLLLL.LLLLLLLLLLLLLLLLLLL.LL.LLLLLLLLL.LLLLLLL.LLLLLL",
"L..L.LL......LLL....LLLL.......L.L..L..L....LLLLL..LLLLL..L..L.L.L........LLL...LL.L......LLL.....",
"LLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLL..LLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LL.LLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLL.LLLL.LLLLLLLL..LLLLLL.LLLLL.LLLL...LLLLL.L.LLLL.LLLLLLL..LLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLL",
"LLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLL.LLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLL.LLL",
"LLLLLLLLLLLLL.LLLLL.L.LLLLL.LLLLLLLLLLL.LLLLLLLLLL.LL.LLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLL",
"LLLLL.LLLLLLLLLLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL",
"LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL.L.LLLLLL.LLLLLLLLLLLLLLLLL.",
"LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL",
        };
        }
    }
}
