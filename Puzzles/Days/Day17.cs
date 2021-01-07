using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public class Cube
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }
        public char V { get; set; }
        public bool New { get; set; }

        public Cube(int x, int y, int z, int w, char v, bool n)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
            V = v;
            New = n;
        }

        public char NewValue(List<Cube> previousState)
        {
            int activeNeighbors = 0;
            foreach (var c in previousState)
            {
                if (c.V == '#' && this.IsNeighbuor(c))
                {
                    activeNeighbors++;
                }
            }

            if (this.V == '#')
            {
                if (activeNeighbors != 2 && activeNeighbors != 3)
                {
                    return '.';
                }
            }
            else
            {
                if (activeNeighbors == 3)
                {
                    return '#';
                }
            }

            return this.V;
        }

        private bool IsNeighbuor(Cube cube)
        {
            if (cube.X == X && cube.Y == Y  && cube.Z == Z && cube.W == W) return false;

            if ((Math.Abs(cube.X - X) < 2) &&
                 (Math.Abs(cube.Y - Y) < 2) &&
                 (Math.Abs(cube.Z - Z) < 2) &&
                 (Math.Abs(cube.W - W) < 2))
                return true;

            return false;
        }
    }

    public static class Day17
    {
        public static Dictionary<(int, int, int, int), Cube> _PreviousState = new Dictionary<(int, int, int, int), Cube>();

        public static void RunDay()
        {
            //var input = GetTestInput();
            var input = GetInput();

            int w = 0;
            int z = 0;
            int x = 0;

            foreach (var line in input)
            {
                int y = 0;
                foreach (var ch in line)
                {
                    Cube cube = new Cube(x, y, z, w, ch, true);
                    _PreviousState[(x, y, z, w)] = cube;
                    y++;
                }
                x++;
            }

            for (int i = 0; i < 6; i++)
            {
                AddNeighbors();

                var temp = JsonConvert.DeserializeObject<List<Cube>>(JsonConvert.SerializeObject(_PreviousState.Values));

                foreach (var cube in temp)
                {
                    var newValue = cube.NewValue(temp);
                    _PreviousState[(cube.X, cube.Y, cube.Z, cube.W)] = new Cube(cube.X, cube.Y, cube.Z, cube.W, newValue, cube.New);
                }

                //for (int k = -1; k <= 1; k++)
                //{
                //    Console.WriteLine(k);
                //    Console.WriteLine($"{_PreviousState[(0,0,k)].V}{_PreviousState[(0,1,k)].V}{_PreviousState[(0,2,k)].V}");
                //    Console.WriteLine($"{_PreviousState[(1,0,k)].V}{_PreviousState[(1,1,k)].V}{_PreviousState[(1,2,k)].V}");
                //    Console.WriteLine($"{_PreviousState[(2,0,k)].V}{_PreviousState[(2,1,k)].V}{_PreviousState[(2,2,k)].V}");
                //}
            }

            int count = 0;
            foreach (var cube in _PreviousState.Values)
            {
                if (cube.V == '#') count++;
            }

            Console.WriteLine(count);
        }

        private static void AddNeighbors()
        {
            Console.WriteLine($"Adding Neighbors to {_PreviousState.Count} cubes");
            Console.WriteLine($"New {_PreviousState.Values.Count(c => c.New)} cubes");
            Console.WriteLine($"Old {_PreviousState.Values.Count(c => c.New == false)} cubes");
            Console.WriteLine();

            int sad = 0;
            int add = 0;

            var temp = JsonConvert.DeserializeObject<List<Cube>>(JsonConvert.SerializeObject(_PreviousState.Values));

            foreach (var cube in temp.Where(c => c.New))
            {
                for (var x = cube.X - 1; x <= cube.X + 1; x++)
                {
                    for (var y = cube.Y - 1; y <= cube.Y + 1; y++)
                    {
                        for (var z = cube.Z - 1; z <= cube.Z + 1; z++)
                        {
                            for (var w = cube.W - 1; w <= cube.W + 1; w++)
                            {
                                if (_PreviousState.ContainsKey((x, y, z, w)) == false)
                                {
                                    add++;
                                    _PreviousState[(x, y, z, w)] = new Cube(x, y, z, w, '.', true);
                                }
                                else
                                {
                                    sad++;
                                }
                            }
                        }
                    }
                }

                _PreviousState[(cube.X, cube.Y, cube.Z, cube.W)].New = false;
            }

            Console.WriteLine("Add: " + add);
            Console.WriteLine("Sad: " + sad);
            Console.WriteLine();
        }

        public static List<string> GetTestInput()
        {
            return new List<string>()
            {
                ".#.",
                "..#",
                "###",
            };
        }

        public static List<string> GetInput()
        {
            return new List<string>()
            {
                ".##..#.#",
                "##.#...#",
                "##.#.##.",
                "..#..###",
                "####.#..",
                "...##..#",
                "#.#####.",
                "#.#.##.#",
            };
        }
    }
}
