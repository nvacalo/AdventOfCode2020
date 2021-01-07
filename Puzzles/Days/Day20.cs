using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public static class Day20
    {
        public class Tile
        {
            public int Number { get; set; }
            public List<string> Lines { get; set; }

            public long Up { get; set; }
            public long Down { get; set; }
            public long Right { get; set; }
            public long Left { get; set; }

            public void Rotate90()
            {
                int len = Lines.Count;

                List<string> newLines = new List<string>();

                List<char[]> array = new List<char[]>();
                for (int i = 0; i < len; i++)
                {
                    array.Add(new char[len]);
                }

                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < len; j++)
                    {
                        array[j][len - 1 - i] = Lines[i][j];
                    }
                }

                for (int i = 0; i < len; i++)
                {
                    newLines.Add(new string(array[i]));
                }

                Lines = newLines;
            }

            public void FlipVertical()
            {
                int len = Lines.Count;

                List<string> newLines = new List<string>();
                foreach (var line in Lines)
                {
                    var array = line.ToArray();

                    for (int idx = 0; idx < len / 2; idx++)
                    {
                        var temp = array[idx];
                        array[idx] = array[len - 1 - idx];
                        array[len - 1 - idx] = temp;
                    }

                    newLines.Add(new string(array));
                }

                Lines = newLines;
            }

            public void FlipHorizontal()
            {
                int len = Lines.Count;

                for (int idx = 0; idx < len / 2; idx++)
                {
                    var temp = Lines[idx];
                    Lines[idx] = Lines[len - 1 - idx];
                    Lines[len - 1 - idx] = temp;
                }
            }

            public Fit? Check(Tile tile)
            {
                #region Right
                if (Right == 0 && tile.Left == 0)
                {
                    bool right = true;
                    for (int i = 0; i <= 9; i++)
                    {
                        if (Lines[i][9] != tile.Lines[i][0])
                        {
                            right = false;
                            break;
                        }
                    }
                    if (right)
                    {
                        return Fit.Right;
                    }
                }

                #endregion

                #region Left
                if (Left == 0 && tile.Right == 0)
                {
                    bool left = true;
                    for (int i = 0; i <= 9; i++)
                    {
                        if (Lines[i][0] != tile.Lines[i][9])
                        {
                            left = false;
                            break;
                        }
                    }
                    if (left)
                    {
                        return Fit.Left;
                    }
                }
                #endregion

                #region Up
                if (Up == 0 && tile.Down == 0)
                {
                    bool up = true;
                    for (int i = 0; i <= 9; i++)
                    {
                        if (Lines[0][i] != tile.Lines[9][i])
                        {
                            up = false;
                            break;
                        }
                    }
                    if (up)
                    {
                        return Fit.Up;
                    }
                }
                #endregion

                #region Down
                if (Down == 0 && tile.Up == 0)
                {
                    bool down = true;
                    for (int i = 0; i <= 9; i++)
                    {
                        if (Lines[9][i] != tile.Lines[0][i])
                        {
                            down = false;
                            break;
                        }
                    }
                    if (down)
                    {
                        return Fit.Down;
                    }
                }
                #endregion

                return null;
            }
        }

        public enum Fit
        {
            Up,
            Down,
            Right,
            Left
        }

        public static List<Tile> _Tiles = new List<Tile>();
        public static Dictionary<long, Tile> _DoneTiles = new Dictionary<long, Tile>();

        public static List<string> _Monster = new List<string>() {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "};
        public static void RunDay()
        {
            //var input = GetTestInput();
            var input = GetInput();

            Tile t = new Tile();
            foreach (var line in input)
            {
                if (line == "")
                {
                    _Tiles.Add(t);
                    t = new Tile();
                }
                else if (line.Contains(":"))
                {
                    t.Lines = new List<string>();
                    t.Number = int.Parse(line.Split(" ")[1].Replace(":", ""));
                }
                else
                {
                    t.Lines.Add(line);
                }
            }

            _Tiles.Add(t);

            #region Test
            //var test = JsonConvert.DeserializeObject<Tile>(JsonConvert.SerializeObject(_Tiles[0]));
            //foreach (var line in test.Lines)
            //{
            //    Console.WriteLine(line);
            //}

            ////Console.WriteLine("Flip");
            ////test.Flip();
            ////foreach (var line in test.Lines)
            ////{
            ////    Console.WriteLine(line);
            ////}

            //Console.WriteLine("");
            //Console.WriteLine("Rotate90");
            //test = JsonConvert.DeserializeObject<Tile>(JsonConvert.SerializeObject(_Tiles[0]));
            //test.Rotate90();
            //foreach (var line in test.Lines)
            //{
            //    Console.WriteLine(line);
            //}

            //Console.WriteLine("");
            //Console.WriteLine("Rotate180");
            //test = JsonConvert.DeserializeObject<Tile>(JsonConvert.SerializeObject(_Tiles[0]));
            //test.Rotate180();
            //foreach (var line in test.Lines)
            //{
            //    Console.WriteLine(line);
            //}

            //Console.WriteLine("");
            //Console.WriteLine("Rotate270");
            //test = JsonConvert.DeserializeObject<Tile>(JsonConvert.SerializeObject(_Tiles[0]));
            //test.Rotate270();
            //foreach (var line in test.Lines)
            //{
            //    Console.WriteLine(line);
            //}
            #endregion

            _DoneTiles[_Tiles[0].Number] = _Tiles[0];
            _Tiles.RemoveAt(0);

            while (_Tiles.Count > 0)
            {
                foreach (var tile in _Tiles)
                {
                    Fit? result = null;
                    Tile done = null;
                    foreach (var doneTile in _DoneTiles.Values)
                    {
                        done = doneTile;

                        result = tile.Check(doneTile);
                        if (result != null) break;

                        tile.FlipVertical();
                        result = tile.Check(doneTile);
                        if (result != null) break;
                        tile.FlipVertical();//initial state 0

                        tile.FlipHorizontal();
                        result = tile.Check(doneTile);
                        if (result != null) break;
                        tile.FlipHorizontal();//intial state 0

                        tile.Rotate90();//90
                        result = tile.Check(doneTile);
                        if (result != null) break;

                        tile.FlipVertical();
                        result = tile.Check(doneTile);
                        if (result != null) break;
                        tile.FlipVertical();//initial state 90

                        tile.FlipHorizontal();
                        result = tile.Check(doneTile);
                        if (result != null) break;
                        tile.FlipHorizontal();//intial state 90

                        tile.Rotate90();//180
                        result = tile.Check(doneTile);
                        if (result != null) break;

                        tile.FlipVertical();
                        result = tile.Check(doneTile);
                        if (result != null) break;
                        tile.FlipVertical();//initial state 180

                        tile.FlipHorizontal();
                        result = tile.Check(doneTile);
                        if (result != null) break;
                        tile.FlipHorizontal();//intial state 180

                        tile.Rotate90();//270
                        result = tile.Check(doneTile);
                        if (result != null) break;

                        tile.FlipVertical();
                        result = tile.Check(doneTile);
                        if (result != null) break;
                        tile.FlipVertical();//initial state 270

                        tile.FlipHorizontal();
                        result = tile.Check(doneTile);
                        if (result != null) break;
                        tile.FlipHorizontal();//intial state 270

                        tile.Rotate90();//intial state 360 - 0
                    }

                    if (result != null)
                    {
                        UpdateTitles(tile, done, result.Value);

                        //Console.WriteLine("");
                        //Console.WriteLine(result.Value);
                        //Console.WriteLine(tile.Number);
                        //foreach (var line in tile.Lines)
                        //{
                        //    Console.WriteLine(line);
                        //}
                        //Console.WriteLine("");
                        //Console.WriteLine(done.Number);
                        //foreach (var line in done.Lines)
                        //{
                        //    Console.WriteLine(line);
                        //}

                        //Console.WriteLine($"tile {tile.Number} done with Up {tile.Up}, Down {tile.Down}, Left {tile.Left}, Right {tile.Right}");
                        _DoneTiles[tile.Number] = tile;
                    }
                }

                _Tiles.RemoveAll(t => _DoneTiles.ContainsKey(t.Number));
            }

            foreach (var done in _DoneTiles)
            {
                RefreshTitles(done.Value);
            }

            var upLeft = _DoneTiles.SingleOrDefault(d => d.Value.Left == 0 && d.Value.Up == 0);
            var upRight = _DoneTiles.SingleOrDefault(d => d.Value.Right == 0 && d.Value.Up == 0);
            var downLeft = _DoneTiles.SingleOrDefault(d => d.Value.Left == 0 && d.Value.Down == 0);
            var downRight = _DoneTiles.SingleOrDefault(d => d.Value.Right == 0 && d.Value.Down == 0);

            //Console.WriteLine($"{upLeft.Key} {upRight.Key} {downRight.Key} {downLeft.Key}");
            //Console.WriteLine(upLeft.Key * upRight.Key * downRight.Key * downLeft.Key);

            foreach (var tile in _DoneTiles.Values)
            {
                int idx = tile.Lines.Count;

                List<string> noBorders = new List<string>();

                for (int id = 1; id < idx - 1; id++)
                {
                    noBorders.Add(tile.Lines[id].Substring(0, idx - 1).Substring(1, idx - 2));
                }

                tile.Lines = noBorders;
            }

            Tile map = new Tile();
            map.Lines = new List<string>() { "", "", "", "", "", "", "", "" };

            Console.WriteLine();
            var currentTile = upLeft.Value;
            Tile firstInLine = currentTile;
            int row = 0;
            while (true)
            {
                Console.Write(currentTile.Number + " ");

                int idx = 0;
                foreach (var currentLine in currentTile.Lines)
                {
                    map.Lines[row + idx++] += currentLine;
                }

                if (currentTile.Right == 0)
                {
                    if (firstInLine.Down == 0) break;
                    Console.WriteLine();
                    row += 8;
                    map.Lines.AddRange(new List<string>() { "", "", "", "", "", "", "", "" });
                    currentTile = _DoneTiles[firstInLine.Down];
                    firstInLine = currentTile;
                }
                else
                {
                    currentTile = _DoneTiles[currentTile.Right];
                }
            }

            Console.WriteLine();

            map.Rotate90();
            map.Rotate90();
            //map.FlipVertical();
            //map.FlipHorizontal();

            int ci = -1;
            int cj = -1;

            int mi = 0;
            int startj = 0;

            int count = 0;
            int internalCount = 0;

            int i = 0;
            while (i < map.Lines.Count - 2)
            {
                int mj = 0;
                int j = startj;
                bool stop = false;

                while (j < map.Lines[i].Length)
                {
                    //Console.Write($"{i} {j} ({map.Lines[i][j]}) against {mi} {mj} ({_Monster[mi][mj]}): ");
                    if (_Monster[mi][mj] == ' ' || map.Lines[i][j] == _Monster[mi][mj])
                    {
                        if (ci == -1) ci = i;
                        if (cj == -1) cj = j;

                        if (_Monster[mi][mj] == '#')
                        {
                            internalCount++;
                        }

                        //Console.WriteLine($"matches '{_Monster[mi][mj]}'");
                        mj++;
                        if (mj == 20)
                        {
                            if (mi == 2)
                            {
                                Console.WriteLine("YES!");
                                count += internalCount;
                                stop = true;
                            }
                            else
                            {
                                mi++;
                                i++;
                                startj = j - 19;
                                break;
                                //thelw to j na 3ekinhsei kateu8eian katw apo to j pou prwto matcare me to monster
                            }
                        }

                        j++;

                        if (j == map.Lines[i].Length)
                        {
                            stop = true;
                        }
                    }
                    else
                    {
                        stop = true;
                    }

                    if (stop)
                    {
                        internalCount = 0;

                        //Console.WriteLine($"wrong");

                        i = ci;

                        if (j == map.Lines[i].Length)
                        {
                            i++;
                            startj = 0;
                        }
                        else
                        {
                            startj = cj + 1;
                        }

                        //Console.WriteLine($"................................start from {i} {startj}");

                        ci = -1;//rollback
                        cj = -1;//rollback

                        mi = 0;
                        mj = 0;

                        break;
                    }
                }
            }

            int kagela = 0;
            foreach (var line in map.Lines)
            {
                foreach (char c in line)
                {
                    if (c == '#') kagela++;
                }
            }

            Console.WriteLine($"{kagela} {count}");
            Console.WriteLine();
            Console.WriteLine(kagela - count);
        }

        private static void UpdateTitles(Tile check, Tile other, Fit fit)
        {
            if (fit == Fit.Right)
            {
                check.Right = other.Number;
                other.Left = check.Number;

                if (other.Up > 0)
                {
                    check.Up = _DoneTiles[other.Up].Left;
                    if (check.Up > 0) _DoneTiles[check.Up].Down = check.Number;
                }
                if (other.Down > 0)
                {
                    check.Down = _DoneTiles[other.Down].Left;
                    if (check.Down > 0) _DoneTiles[check.Down].Up = check.Number;
                }

                if (check.Up > 0 && _DoneTiles[check.Up].Left > 0)
                {
                    check.Left = _DoneTiles[_DoneTiles[check.Up].Left].Down;
                    if (check.Left > 0) _DoneTiles[check.Left].Right = check.Number;
                }
                if (check.Left == 0)
                {
                    if (check.Down > 0 && _DoneTiles[check.Down].Left > 0)
                    {
                        check.Left = _DoneTiles[_DoneTiles[check.Down].Left].Up;
                        if (check.Left > 0) _DoneTiles[check.Left].Right = check.Number;
                    }
                }
            }
            else if (fit == Fit.Left)
            {
                check.Left = other.Number;
                other.Right = check.Number;

                if (other.Up > 0)
                {
                    check.Up = _DoneTiles[other.Up].Right;
                    if (check.Up > 0) _DoneTiles[check.Up].Down = check.Number;
                }
                if (other.Down > 0)
                {
                    check.Down = _DoneTiles[other.Down].Right;
                    if (check.Down > 0) _DoneTiles[check.Down].Up = check.Number;
                }

                if (check.Up > 0 && _DoneTiles[check.Up].Right > 0)
                {
                    check.Right = _DoneTiles[_DoneTiles[check.Up].Right].Down;
                    if (check.Right > 0) _DoneTiles[check.Right].Left = check.Number;
                }
                if (check.Right == 0)
                {
                    if (check.Down > 0 && _DoneTiles[check.Down].Right > 0)
                    {
                        check.Right = _DoneTiles[_DoneTiles[check.Down].Right].Up;
                        if (check.Right > 0) _DoneTiles[check.Right].Left = check.Number;
                    }
                }
            }
            else if (fit == Fit.Up)
            {
                check.Up = other.Number;
                other.Down = check.Number;

                if (other.Right > 0)
                {
                    check.Right = _DoneTiles[other.Right].Down;
                    if (check.Right > 0) _DoneTiles[check.Right].Left = check.Number;
                }
                if (other.Left > 0)
                {
                    check.Left = _DoneTiles[other.Left].Down;
                    if (check.Left > 0) _DoneTiles[check.Left].Right = check.Number;
                }

                if (check.Right > 0 && _DoneTiles[check.Right].Down > 0)
                {
                    check.Down = _DoneTiles[_DoneTiles[check.Right].Down].Left;
                    if (check.Down > 0) _DoneTiles[check.Down].Up = check.Number;
                }

                if (check.Down == 0)
                {
                    if (check.Left > 0 && _DoneTiles[check.Left].Down > 0)
                    {
                        check.Down = _DoneTiles[_DoneTiles[check.Left].Down].Right;
                        if (check.Down > 0) _DoneTiles[check.Down].Up = check.Number;
                    }
                }
            }
            else if (fit == Fit.Down)
            {
                check.Down = other.Number;
                other.Up = check.Number;

                if (other.Right > 0)
                {
                    check.Right = _DoneTiles[other.Right].Up;
                    if (check.Right > 0) _DoneTiles[check.Right].Left = check.Number;
                }
                if (other.Left > 0)
                {
                    check.Left = _DoneTiles[other.Left].Up;
                    if (check.Left > 0) _DoneTiles[check.Left].Right = check.Number;
                }

                if (check.Right > 0 && _DoneTiles[check.Right].Up > 0)
                {
                    check.Up = _DoneTiles[_DoneTiles[check.Right].Up].Left;
                    if (check.Up > 0) _DoneTiles[check.Up].Down = check.Number;
                }
                if (check.Up == 0)
                {
                    if (check.Left > 0 && _DoneTiles[check.Left].Up > 0)
                    {
                        check.Up = _DoneTiles[_DoneTiles[check.Left].Up].Right;
                        if (check.Up > 0) _DoneTiles[check.Up].Down = check.Number;
                    }
                }
            }
        }

        private static void RefreshTitles(Tile check)
        {
            if (check.Up > 0 && _DoneTiles[check.Up].Left > 0)
            {
                check.Left = _DoneTiles[_DoneTiles[check.Up].Left].Down;
                if (check.Left > 0) _DoneTiles[check.Left].Right = check.Number;
            }
            if (check.Left == 0)
            {
                if (check.Down > 0 && _DoneTiles[check.Down].Left > 0)
                {
                    check.Left = _DoneTiles[_DoneTiles[check.Down].Left].Up;
                    if (check.Left > 0) _DoneTiles[check.Left].Right = check.Number;
                }
            }

            if (check.Up > 0 && _DoneTiles[check.Up].Right > 0)
            {
                check.Right = _DoneTiles[_DoneTiles[check.Up].Right].Down;
                if (check.Right > 0) _DoneTiles[check.Right].Left = check.Number;
            }
            if (check.Right == 0)
            {
                if (check.Down > 0 && _DoneTiles[check.Down].Right > 0)
                {
                    check.Right = _DoneTiles[_DoneTiles[check.Down].Right].Up;
                    if (check.Right > 0) _DoneTiles[check.Right].Left = check.Number;
                }
            }

            if (check.Right > 0 && _DoneTiles[check.Right].Down > 0)
            {
                check.Down = _DoneTiles[_DoneTiles[check.Right].Down].Left;
                if (check.Down > 0) _DoneTiles[check.Down].Up = check.Number;
            }

            if (check.Down == 0)
            {
                if (check.Left > 0 && _DoneTiles[check.Left].Down > 0)
                {
                    check.Down = _DoneTiles[_DoneTiles[check.Left].Down].Right;
                    if (check.Down > 0) _DoneTiles[check.Down].Up = check.Number;
                }
            }

            if (check.Right > 0 && _DoneTiles[check.Right].Up > 0)
            {
                check.Up = _DoneTiles[_DoneTiles[check.Right].Up].Left;
                if (check.Up > 0) _DoneTiles[check.Up].Down = check.Number;
            }
            if (check.Up == 0)
            {
                if (check.Left > 0 && _DoneTiles[check.Left].Up > 0)
                {
                    check.Up = _DoneTiles[_DoneTiles[check.Left].Up].Right;
                    if (check.Up > 0) _DoneTiles[check.Up].Down = check.Number;
                }
            }
        }

        public static List<string> GetTestInput()
        {
            return new List<string>()
            {
              "Tile 2311:",
"..##.#..#.",
"##..#.....",
"#...##..#.",
"####.#...#",
"##.##.###.",
"##...#.###",
".#.#.#..##",
"..#....#..",
"###...#.#.",
"..###..###",
"",
"Tile 1951:",
"#.##...##.",
"#.####...#",
".....#..##",
"#...######",
".##.#....#",
".###.#####",
"###.##.##.",
".###....#.",
"..#.#..#.#",
"#...##.#..",
"",
"Tile 1171:",
"####...##.",
"#..##.#..#",
"##.#..#.#.",
".###.####.",
"..###.####",
".##....##.",
".#...####.",
"#.##.####.",
"####..#...",
".....##...",
"",
"Tile 1427:",
"###.##.#..",
".#..#.##..",
".#.##.#..#",
"#.#.#.##.#",
"....#...##",
"...##..##.",
"...#.#####",
".#.####.#.",
"..#..###.#",
"..##.#..#.",
"",
"Tile 1489:",
"##.#.#....",
"..##...#..",
".##..##...",
"..#...#...",
"#####...#.",
"#..#.#.#.#",
"...#.#.#..",
"##.#...##.",
"..##.##.##",
"###.##.#..",
"",
"Tile 2473:",
"#....####.",
"#..#.##...",
"#.##..#...",
"######.#.#",
".#...#.#.#",
".#########",
".###.#..#.",
"########.#",
"##...##.#.",
"..###.#.#.",
"",
"Tile 2971:",
"..#.#....#",
"#...###...",
"#.#.###...",
"##.##..#..",
".#####..##",
".#..####.#",
"#..#.#..#.",
"..####.###",
"..#.#.###.",
"...#.#.#.#",
"",
"Tile 2729:",
"...#.#.#.#",
"####.#....",
"..#.#.....",
"....#..#.#",
".##..##.#.",
".#.####...",
"####.#.#..",
"##.####...",
"##..#.##..",
"#.##...##.",
"",
"Tile 3079:",
"#.#.#####.",
".#..######",
"..#.......",
"######....",
"####.#..#.",
".#...#.##.",
"#.#####.##",
"..#.###...",
"..#.......",
"..#.###...",
            };
        }

        public static List<string> GetInput()
        {
            return new List<string>()
            {
               "Tile 3001:",
"##...#..##",
".##...#.##",
"###.......",
"#...##..##",
"...#..#..#",
"#..#...#..",
"#......#..",
"....#.##..",
"#......#.#",
"..##.#...#",
"",
"Tile 2069:",
"###..##..#",
"....##.###",
"###..##..#",
"..#..#.#.#",
"#.........",
"##.#...#.#",
".........#",
"....#..#.#",
"#.........",
"###.#...#.",
"",
"Tile 3023:",
".##..##...",
"......##.#",
"#......#..",
"..#....#..",
"#.....#.#.",
"#.....#...",
"...#...#..",
"......####",
"....###..#",
".#.#.#...#",
"",
"Tile 3389:",
"....#..#.#",
"##...#....",
".....#....",
".....#..#.",
".#...##...",
"##..#...##",
"##.##..#..",
"#...##....",
".#....#...",
"..#.......",
"",
"Tile 2693:",
"#.#.###.#.",
"....#.....",
"...#.##.#.",
".###.....#",
".#..#..#..",
"#..#....#.",
"#.........",
"......#..#",
"#..#......",
"...#.###..",
"",
"Tile 1987:",
"####.#..#.",
"##.#.....#",
"#.#..###..",
".#.#..###.",
"#.....#...",
"#..#......",
".....##..#",
"....##....",
"#....#.#.#",
".###...#.#",
"",
"Tile 2113:",
"...#.###.#",
"#.#.##.#..",
".....##..#",
"#.#..#....",
"#........#",
"#.#.#.###.",
"#.##.###..",
"..#.###..#",
"......#.##",
"#..#.#...#",
"",
"Tile 1999:",
"#.####..##",
"#.##..#...",
".#..####..",
".#..#.#...",
"......#.#.",
".......#..",
".#........",
"##..#.#..#",
"......#...",
".#####..##",
"",
"Tile 1973:",
"#..#.###.#",
".......#.#",
"#..##....#",
"..#.#.....",
".##.....#.",
"#....#.#.#",
"...#.###.#",
"#......#.#",
"......#.##",
"#.#.#...#.",
"",
"Tile 1801:",
"######.#.#",
"#..###...#",
".#.##..#..",
".##..#.#.#",
"..........",
"..#...#.#.",
"..#......#",
"##.......#",
"#....#...#",
"#..#.#.#..",
"",
"Tile 3881:",
"#.###...#.",
"#......#..",
"#.....#..#",
"......#..#",
"#.......#.",
"#.#..#..#.",
"##....##..",
"#..#.#...#",
"#.#......#",
".#...#.###",
"",
"Tile 1823:",
"####..#.##",
"##.......#",
".......#..",
"....#.####",
".#.#.#..#.",
"#....#....",
"#.........",
"#..#......",
".####.#..#",
"#.###.#...",
"",
"Tile 1031:",
"..##.##.#.",
"#.#.......",
"#...#....#",
".....#.#.#",
"#.....#...",
"#.#.#.....",
"....#....#",
"#.........",
"..#.......",
"##.###.###",
"",
"Tile 1361:",
".#.####..#",
"##..#..#..",
".#........",
".##......#",
"#..#......",
".#.......#",
"#..#..#...",
"#..#......",
"#........#",
"#..##.#.#.",
"",
"Tile 1483:",
".###....##",
"...#....#.",
"..........",
"#..##....#",
"#......#.#",
"#.........",
"##.#......",
".....#.###",
"#...#.....",
"#..#.##..#",
"",
"Tile 2381:",
".#.#...##.",
"#...#.....",
"...#.#...#",
"...##.##..",
"...##.....",
"..##......",
"........##",
"#..##..##.",
"..##.#..#.",
"#####..#..",
"",
"Tile 2447:",
".##.##..#.",
".#....##.#",
".#..#..###",
"...##....#",
"#.....###.",
"......#..#",
"#.......##",
".......#..",
".....#.#.#",
".#.####.##",
"",
"Tile 2633:",
"..##.#...#",
".........#",
"#..##.....",
".#.#...###",
"..#....##.",
"#.#.#...##",
"#.##.....#",
"#........#",
"....#.....",
"..##..##.#",
"",
"Tile 1861:",
".###.###.#",
".##.#..#..",
"##..#..#.#",
"##..#..#.#",
".###..#.#.",
"..........",
".....#....",
"....###.##",
".#...#....",
".#.##..##.",
"",
"Tile 1453:",
"#....#...#",
"#..#.....#",
"..#..#...#",
"#.#..#...#",
"#.#......#",
"##..##...#",
"...#...#..",
"#..#...###",
"....#..#..",
"#.#.#.....",
"",
"Tile 3719:",
".#...#####",
".##..#....",
".#...#...#",
".......#.#",
"#....#...#",
"....###...",
"..#..##...",
"..#..##...",
"##......#.",
"#####.#..#",
"",
"Tile 3167:",
".......#..",
"#........#",
".#...#..##",
".#.#.#...#",
"#..#..##.#",
"#.........",
".#....#...",
"##..#....#",
"....#....#",
"#.###..#.#",
"",
"Tile 2521:",
".#.#.##...",
"#...#.#.#.",
"#........#",
".......#..",
"..#.......",
"#....#..##",
".#..#.#...",
"#.....##.#",
"....#.##.#",
"..#######.",
"",
"Tile 1601:",
"....##.#.#",
"...##...#.",
"#..##....#",
"#.###...#.",
"#.......##",
"..#....###",
"...#..#.##",
"....#..###",
"##..#..##.",
"#......#..",
"",
"Tile 1657:",
".##...#...",
"##.....#.#",
"##.#.##.#.",
"#.#...#..#",
".......#.#",
"........#.",
"#.#......#",
"##..#..#.#",
".#....#..#",
"..#..##.##",
"",
"Tile 3919:",
"##....#.##",
"....#..#.#",
"#..#.....#",
"#......#.#",
"#.....#...",
"#...#...#.",
"..###....#",
"#..##.....",
"..##.#..##",
"####..#..#",
"",
"Tile 3851:",
"###.#####.",
"##.......#",
"#..#.....#",
"#......###",
"......#.#.",
"#.##..##..",
"#...#....#",
".....##.#.",
"#...##.#..",
"........##",
"",
"Tile 3821:",
".###.#####",
".....#....",
"...##....#",
"##........",
"#.#..#....",
"#..#..##..",
"...#.###..",
"#....#....",
"#...#....#",
"..#..###..",
"",
"Tile 3499:",
"#######..#",
"#...##...#",
"##...#...#",
"......##.#",
"#.##......",
"#.........",
"...#..#..#",
".....#.#..",
"....##....",
".###...#.#",
"",
"Tile 3191:",
"##.#.#..#.",
"....##....",
"...#...#.#",
"...##..#..",
"#..##.....",
"#...#.#..#",
".#.#...#..",
"#####...#.",
"##.....##.",
"..###....#",
"",
"Tile 3137:",
".#...#....",
"##........",
"#..#...#..",
"#..#..#...",
"..#...#...",
".##......#",
"..#..###..",
"#.#..###..",
"#..#.#....",
".#......##",
"",
"Tile 2551:",
"#.#####.##",
"..#.#..#..",
"#.........",
"#..#.##.##",
"...###.##.",
"..####..##",
"#.#..#...#",
"#.....#...",
"#.#......#",
"#.##.#.###",
"",
"Tile 3217:",
"#.####...#",
".........#",
"..#......#",
"##.#..#.##",
"#..##..#..",
".#.#....##",
"#.#......#",
".........#",
".#.##.#..#",
"....##.#.#",
"",
"Tile 3491:",
"###.###..#",
".#........",
"..#....#..",
"...#..#..#",
"##.#....#.",
"#....#....",
"#.....#...",
".##......#",
".#.......#",
"#....#.##.",
"",
"Tile 3181:",
"#.#.#####.",
"##.#.....#",
"#..#.#..#.",
"##..#..#..",
"#..#....#.",
"........#.",
"#...##..##",
"#.##.##..#",
".#.#.#...#",
"#.#.###.#.",
"",
"Tile 2819:",
"..#.#.#.#.",
".....#.#..",
"..#.#..#..",
"#..#.#.#.#",
"#.#.#..##.",
"##.....##.",
"##......##",
"##.###.#..",
"...#...#.#",
"..#.#..#.#",
"",
"Tile 2857:",
"###.#....#",
"####.....#",
"#....#..#.",
"#....##...",
"...#.##...",
"#....#...#",
".#...#...#",
"##.......#",
"#..####.#.",
"...###.###",
"",
"Tile 1783:",
"#..##....#",
"#.....##.#",
"#......###",
"....######",
"...####.##",
"#.......#.",
"...#.....#",
"..........",
"#........#",
".#.##.##..",
"",
"Tile 2297:",
".###.####.",
".....##.##",
"...#...#..",
"##........",
".......##.",
"##.#.#.##.",
"##.#.#..#.",
"##........",
"...#.##..#",
".###...###",
"",
"Tile 3727:",
"...#..#.#.",
"###.##.#..",
"#.##..#.#.",
".#......#.",
"##........",
".#....####",
"#...#..#.#",
"########.#",
"....#####.",
"####.#.#.#",
"",
"Tile 2237:",
".####.##..",
"#....##...",
".......#..",
"#......#..",
"...#...#..",
".........#",
"#....#.#..",
"#...#.#...",
".........#",
"#####.....",
"",
"Tile 2281:",
"####..###.",
"#...#.#..#",
"#........#",
"#.#.#.##.#",
"#........#",
".###.#....",
"#.....#...",
"...##..#..",
"##......##",
"###.....#.",
"",
"Tile 1321:",
".....##..#",
".#.#......",
"#..#....##",
"....##....",
"#...#...#.",
"#.....#..#",
"..#.....##",
"..#.......",
".##...#...",
".#.###....",
"",
"Tile 1489:",
"##..#.....",
"..........",
"#........#",
"###......#",
"###......#",
"#....#....",
"..........",
"...#..#..#",
"....#....#",
"#.#.#....#",
"",
"Tile 2251:",
"#..#...#..",
"##..#....#",
"........#.",
"##.......#",
"##..#.#...",
"#..#.#...#",
"....##...#",
"....###.#.",
"#...#..##.",
"#..##.###.",
"",
"Tile 3467:",
"#.##...#.#",
"#...#.....",
"#.#....##.",
"###....#..",
"#..#####..",
"...#......",
"...#......",
".#.#.....#",
"#..#.....#",
".#.######.",
"",
"Tile 2677:",
".#....##.#",
"#.#.#.....",
"#.#....#.#",
".......#.#",
"###.#....#",
"#..##.....",
"#.###.....",
"#..###....",
"..........",
"#.#.#...##",
"",
"Tile 3457:",
".#.#.#.###",
"#..#..#...",
"###......#",
"#...#....#",
"#.#...####",
"#.......##",
"...#.##...",
"#......#.#",
"#...#.....",
".###.....#",
"",
"Tile 3307:",
"#....#.###",
"#....##.#.",
"..#.###...",
"##.##....#",
"...#......",
"....#.#..#",
"..#.....##",
".....#.#..",
"#...#..##.",
"#.#...#.#.",
"",
"Tile 3187:",
".###.#.#.#",
"..#..#...#",
"#.....#..#",
"#.......##",
"...#.#..#.",
"#........#",
".#.....#..",
".#...#.#.#",
"#.....#..#",
"......####",
"",
"Tile 2357:",
".#...##..#",
"..#.#...##",
"......#..#",
".#....#..#",
"#.....####",
"##.....##.",
"#.###.....",
"#......#.#",
"...##.#...",
".#....###.",
"",
"Tile 3529:",
".##...###.",
".......#..",
".......#.#",
".........#",
"##..#...#.",
"##..#..#.#",
"#....##.##",
".........#",
"#........#",
"###....#.#",
"",
"Tile 1069:",
"...##.....",
"##.#...#..",
"#..#.#...#",
"..#..#.#..",
"##.#..##.#",
"#.#..#.##.",
"#..#.....#",
"#..#.#....",
".........#",
".###..##..",
"",
"Tile 2393:",
".#.#.....#",
".......###",
"..........",
"..##...#..",
".....#....",
".........#",
"#.##.....#",
"...##..#..",
"..#...#..#",
"####..#...",
"",
"Tile 2389:",
".#####...#",
"#........#",
"#.#.......",
".#........",
"........#.",
"#.........",
".....#.#.#",
"...###...#",
"#.......#.",
"#####.####",
"",
"Tile 3673:",
"#######..#",
"#..#..##..",
"....#...#.",
"#....#....",
"...#..#.#.",
"#.#..##...",
"....###..#",
"#.....#..#",
"#........#",
".##..###..",
"",
"Tile 1907:",
".###......",
"#.....##..",
"#.#...#...",
"#....#####",
".....#..##",
"#.#......#",
"#....##...",
"#.....##.#",
"##.#.##...",
"....###.##",
"",
"Tile 2963:",
".#.######.",
"......#...",
"#.....##..",
".#.......#",
".....#....",
".#...#...#",
"#.#......#",
"###.......",
"###....##.",
"#..##.##..",
"",
"Tile 3583:",
".....##...",
"#.....#.##",
"##.......#",
".#.......#",
"#........#",
"#....#....",
".#.##....#",
"#.......##",
"#..####...",
"#....###..",
"",
"Tile 3251:",
".####.#...",
"###.#....#",
"....#.####",
"#.#...####",
".#........",
"##.#.....#",
"..##..#...",
".#...#.#.#",
"..#.....#.",
"#.###.##..",
"",
"Tile 2939:",
".###...#..",
".......#..",
"#...##.#.#",
".#.....###",
".#.##.....",
"#......#.#",
"#..####.##",
"##.##....#",
"....##...#",
"#.##.##.##",
"",
"Tile 2557:",
"..##.#.#..",
"#.#..#...#",
"..#......#",
"..#...#...",
"#.#...#.#.",
"#........#",
"##....####",
"#.#..#..#.",
"#.........",
".#.###.##.",
"",
"Tile 2753:",
".#.##....#",
"#......#.#",
"#........#",
"#.....#..#",
".....#...#",
".........#",
".##.#.....",
"....#.#..#",
"........##",
".##..####.",
"",
"Tile 2971:",
"...#.##..#",
"#..#....##",
".#.##.....",
"..........",
"........##",
"#..##.....",
"..##.....#",
"...#.#...#",
"#....#....",
"...#.#.#..",
"",
"Tile 2539:",
"#....#..#.",
"..###.....",
"###.#....#",
"........##",
"#..#..#...",
"..#.##.#.#",
"#..##...##",
"####......",
"#.#....##.",
"###.#.##.#",
"",
"Tile 1913:",
"##..#....#",
"#....#..##",
"#..##...##",
"#...##...#",
"##..#.....",
"##..#.....",
"....##...#",
"#..##.....",
"#..#..####",
"#..##..##.",
"",
"Tile 2153:",
"...#......",
"#.#.....#.",
"#.........",
".#........",
"..#......#",
"..##.#..#.",
"#.#..##.#.",
"##....##..",
"..#....#.#",
".###...#..",
"",
"Tile 1559:",
"..#...##..",
"...#....##",
"#....#..#.",
"##.##..#.#",
"..#..#.#..",
"#.#.#.....",
"..#......#",
"##........",
".#.......#",
".....##..#",
"",
"Tile 2999:",
"#...#..##.",
"##....#...",
"#....#..#.",
"....#...##",
"#...##..##",
"....##..##",
"##.##.....",
"#...#..#..",
"#...#....#",
"#...#.#..#",
"",
"Tile 2039:",
"#.##.##.#.",
".#....#..#",
".#.......#",
".....#....",
".#..##...#",
"..#....#.#",
"###..###.#",
"##...###..",
"#....#....",
"####....#.",
"",
"Tile 2129:",
"##.######.",
"#.#..#..##",
"#.###.....",
"#.....#.##",
"....#..###",
"..#......#",
"......#...",
"..#.......",
"##......#.",
"....####.#",
"",
"Tile 3407:",
"#..##.#..#",
"#.#...#...",
"#....#...#",
"#...#.#..#",
"#...#.....",
"..#...##.#",
"#..###...#",
"....#.....",
"..#.#.....",
"##....####",
"",
"Tile 1093:",
".##..#.###",
"#.....#.##",
".......#..",
".....#.#..",
"...#...##.",
"........##",
".##.......",
".#......##",
"..#.....##",
"....#####.",
"",
"Tile 3623:",
"##..###.##",
"..#......#",
"#.#..##.#.",
".....#....",
".#........",
".##.#..#..",
"#.#..#..#.",
"#.....#..#",
"...#..#..#",
"#.##....#.",
"",
"Tile 3931:",
".##..#.###",
"#....#....",
"#....##...",
"..##..###.",
"........#.",
".#.......#",
"#......#..",
".##.....##",
".####.....",
"##.##.###.",
"",
"Tile 3371:",
".#...#...#",
"#.#....#.#",
"#.#...#...",
".....#..#.",
"###.......",
"#........#",
"#.##.#..#.",
"#.........",
"#.........",
".#####...#",
"",
"Tile 1381:",
"####...#..",
"##........",
"#.......#.",
"#.#....#..",
"..#......#",
"..........",
"##........",
"##........",
".........#",
"#..##..#.#",
"",
"Tile 2011:",
".#.#...#..",
"#.....##.#",
"#.....##..",
"....#..#.#",
"##..#...#.",
".....#.#.#",
"#....##..#",
"....#.#.#.",
".......#..",
".#.###..##",
"",
"Tile 2341:",
"###....#..",
"#........#",
".#...#...#",
".........#",
"#......#.#",
"......#...",
"##....#..#",
"#....#..#.",
"###..#...#",
"#.#.##..#.",
"",
"Tile 3677:",
".#..###.##",
"...#..#.#.",
"#..#.##.#.",
".##.##..##",
"......#..#",
"#.....#..#",
"....#.....",
"#..##....#",
"#..#.#.###",
"...####.#.",
"",
"Tile 1373:",
"####.#...#",
".........#",
"##.......#",
"..........",
".........#",
"#...#...##",
"#.#..##.##",
"....#.....",
"......#...",
".#.####.##",
"",
"Tile 2591:",
"..##.....#",
"..#.......",
"##.##.#.##",
".#####..#.",
".....#..##",
"#..#.....#",
"........##",
"..#..#...#",
".#....##.#",
"#..#.####.",
"",
"Tile 2707:",
".#.#...#.#",
"......#..#",
"#...#.#..#",
"#.#....#..",
"#........#",
"#...#.....",
"....###..#",
"#.........",
"...#.....#",
"##..#..##.",
"",
"Tile 2917:",
"...#######",
".#...#....",
".#........",
".....#...#",
"..#...#.##",
"#.......##",
"#....##...",
".##.#..#..",
"#.##......",
".#..##...#",
"",
"Tile 3229:",
"..##.###..",
"....#.....",
"......##.#",
"#.###.#..#",
"#....###.#",
".........#",
"......#..#",
"....##.##.",
"#.#.##.#.#",
"#.#..##.#.",
"",
"Tile 2459:",
"..###.##..",
"#.......##",
"...#...###",
".........#",
"#..#....##",
"...##.....",
"##........",
"##..#....#",
"..##.....#",
"...##.##..",
"",
"Tile 3631:",
"..##.#..##",
".........#",
"..##......",
"....#.....",
"#.#.....##",
"#.........",
"###.......",
"#.........",
"#.#.#.#...",
"....#.##..",
"",
"Tile 3733:",
"..#..###.#",
".......###",
".....##.#.",
"##.....#..",
"##.#....#.",
"#.##...#.#",
"###.#.#..#",
"#.#..#...#",
"##...#..#.",
".#..#...#.",
"",
"Tile 2953:",
"##..#.#...",
"........#.",
"..#......#",
"#.....#...",
".........#",
"##.......#",
"#..#......",
"#.....#.#.",
".....#.###",
".###..#.##",
"",
"Tile 1153:",
"#.##.##.##",
"..####....",
".........#",
".#..#.....",
".....####.",
".#.#..##.#",
".......#..",
"#.........",
"....#..#..",
"...##.#..#",
"",
"Tile 3833:",
"####..#...",
"##.......#",
".........#",
".#........",
"##..#.###.",
"#.#..#.#..",
"#.#..##...",
"#........#",
"#...#.....",
"......##..",
"",
"Tile 1597:",
"#...#...##",
".....##..#",
"#.....#..#",
"#.#.#....#",
".###..##.#",
"##.......#",
".......#..",
"...#....##",
"...#......",
".#.##.#.#.",
"",
"Tile 3793:",
"#......#.#",
"#..#..#.#.",
"...#......",
"##......##",
"##..#..#..",
".#..#.....",
"..#.....#.",
".#....#...",
".....##...",
".#..#####.",
"",
"Tile 3373:",
".#.#.#....",
"##.#......",
"#...#....#",
".......###",
"#.#.#..#.#",
"####..####",
"..##......",
"#..#..#..#",
"........##",
"#.##..#.#.",
"",
"Tile 1439:",
"..#####...",
".#.....#.#",
"....#...##",
"..........",
"#....##..#",
"#....#...#",
"##........",
"#....##..#",
".#..##.###",
".####.#..#",
"",
"Tile 1867:",
"#..##....#",
"......#..#",
".#.##....#",
"#...#.....",
".#.....#..",
"..##...#..",
".#..##....",
"#..###.#..",
".#..#.#...",
"#..#.###..",
"",
"Tile 3343:",
"..###.....",
"..#..#..#.",
"#..####.#.",
"#.........",
".........#",
"#.#....##.",
".#..#....#",
"#....#...#",
".........#",
"#.....####",
"",
"Tile 2089:",
"....##.###",
"#..#.#.#..",
"##..#.....",
"#...#..#..",
"#......#.#",
".......##.",
"#.......##",
"#....##.##",
"#........#",
"###.#.#...",
"",
"Tile 1747:",
"...#.....#",
"#........#",
"........##",
"#......##.",
"#.........",
"#...##....",
"#...#.....",
"#.#.......",
"...#....#.",
"......####",
"",
"Tile 2383:",
".#.###...#",
"#.#....###",
"..###..#..",
"..##...#.#",
".......##.",
"##.....#..",
"##...#...#",
"#.........",
".......#..",
"##.##..#..",
"",
"Tile 2843:",
"###......#",
"..##......",
"...#....#.",
"#....#....",
"#........#",
"#.#.....#.",
"#.#.#.#...",
"......#.##",
"##..#..#.#",
"#..###.##.",
"",
"Tile 3593:",
"...###..##",
"###.#.#...",
".......#..",
"##....#.##",
"#.......#.",
"##..#....#",
"#....#....",
"...#......",
".#.#.....#",
".#.##..#..",
"",
"Tile 1699:",
"#.#.#....#",
".##.#.#...",
"#....#...#",
"#....#..##",
"...#....#.",
".....#...#",
"..##.#....",
".......#..",
"##...##..#",
"#.##.#....",
"",
"Tile 1579:",
"##.....###",
"#..#....#.",
"#..#.#..#.",
"#.......##",
"#.#....#..",
"..........",
"........##",
".#...#...#",
"#...#.#..#",
"####.###..",
"",
"Tile 1789:",
".#.#...#..",
"..........",
"##.......#",
"....##....",
"#.#.#.....",
"#........#",
"##.#....##",
"#...#.#...",
"....#..#.#",
"..#######.",
"",
"Tile 1091:",
"#.###.##..",
"#.##.#..#.",
"...#....#.",
"#....#....",
"....##....",
"##.......#",
"##....#...",
"#......#.#",
"##.......#",
"#...####..",
"",
"Tile 3607:",
"#.#.#.##..",
"#......#.#",
"........##",
".#.....#..",
".......#..",
"#.#..#..##",
".#..#.#..#",
"...#.#.#..",
"##..#...##",
"##.....#..",
"",
"Tile 1367:",
"#...##.##.",
"..........",
"......#.#.",
".#.#..#...",
"..#...#.#.",
"#.#......#",
".....#....",
"...#......",
"........##",
".#.#.####.",
"",
"Tile 1607:",
".##.#..#.#",
"#.........",
".......###",
"##....#..#",
"##.....##.",
"#.#.#.....",
"#.........",
"##...#....",
"#.........",
"##..##.#.#",
"",
"Tile 3739:",
"...#.##...",
"#........#",
"..#..#.#.#",
"#......#..",
"....#.....",
"##......##",
"##....#.##",
"#........#",
"..........",
"##.####..#",
"",
"Tile 3989:",
"#.##..##..",
".....#..##",
"#.........",
".#.##..#.#",
"..##.##...",
"........#.",
"#.........",
".##.....##",
"#.###..#.#",
"###...###.",
"",
"Tile 1723:",
"..#.#..###",
".....##.#.",
".##..##...",
"#..#.##...",
"..#....##.",
"...#.#...#",
".....#..#.",
"..#..#..#.",
"..#..#..##",
"#.###..##.",
"",
"Tile 2029:",
"##...##...",
".......#.#",
"....##.###",
"#..#......",
"##...#...#",
"#...#....#",
".#.......#",
"#..#..#.##",
"....##...#",
"#####.##.#",
"",
"Tile 3637:",
".####...#.",
".#.#..#..#",
"#..#..#.##",
".....##.#.",
"...#.#...#",
"#..#....##",
".....##.##",
"...#...#..",
"##....#..#",
"#.....#.#.",
"",
"Tile 3701:",
".#.#.#....",
"#.###....#",
"..#..#.###",
"#.....#.#.",
"........#.",
"#.....#...",
"#...##.###",
"####..#...",
"#.....#.#.",
"#..#....#.",
"",
"Tile 1087:",
".#..##...#",
"#...##....",
"...#......",
"....#.....",
"#.##....##",
"#....#....",
"#.##.....#",
".#..###.##",
"#..#.....#",
".#.#..#.##",
"",
"Tile 2617:",
"#.#..#...#",
"..###.##.#",
"##....####",
".........#",
".#....#...",
"......#.##",
".......##.",
"#...#....#",
"#.......##",
"#.#.#.#...",
"",
"Tile 3581:",
"...###.###",
"#....##...",
"......##.#",
"......##..",
"##....#..#",
".###....#.",
".#.#..#...",
"#..#..#..#",
".....#...#",
".###.#.#.#",
"",
"Tile 3697:",
"..####.###",
".#.##.....",
".#....#..#",
"..........",
".###......",
"#..#..#.##",
"#.#..#..##",
"#..##...#.",
"..#......#",
"###..#.#..",
"",
"Tile 2399:",
"##.###.###",
"#.........",
"#..#.##..#",
"##.#....##",
"#...#....#",
"..##..#...",
"..#..#..##",
"..##.#.#.#",
"...##...#.",
"..#....###",
"",
"Tile 2767:",
"###..#####",
".......#..",
"#........#",
"....#..###",
".#####..#.",
"#.#....#..",
"....###...",
"#........#",
"##........",
".....###..",
"",
"Tile 3463:",
"....#.####",
"#.#......#",
"..##.#....",
"#.#......#",
"#.##....##",
"#...#..#.#",
"#...#.####",
"##..#....#",
"#.#.....##",
"###...##..",
"",
"Tile 3163:",
".##..#.##.",
".........#",
"#.....#...",
"..#......#",
"#..#.....#",
"...#......",
"..#......#",
".#...#.#..",
".##..###..",
".#.###..#.",
"",
"Tile 3709:",
"##...##.##",
"#...#..#..",
"#..##.#.##",
"#..##....#",
"#.#..##..#",
"#........#",
"#..#......",
"##.###.##.",
"##.###...#",
"..##.##.##",
"",
"Tile 1571:",
"#.##.#.##.",
"##..#..#.#",
"..........",
"#.#.....#.",
"#.....##..",
"..#.#..#.#",
"###.....##",
".......###",
"...#......",
"#.#.#...#.",
"",
"Tile 1201:",
"#...###...",
"#........#",
"#.#......#",
"#..#...#.#",
"#.....#..#",
"##..#....#",
"#........#",
"......#...",
"....#..#..",
".##...##..",
"",
"Tile 3121:",
"..#.#.#..#",
".#..#.##.#",
"..###...#.",
"...##.....",
".....#....",
".#.....##.",
"..#.......",
"#.....#...",
"#.........",
"#..#....#.",
"",
"Tile 1049:",
"##...#.#.#",
"#...#.....",
".#...#...#",
"..#...#..#",
"......##.#",
"......####",
"#..#.....#",
"#.#.......",
"#.#..#..#.",
"##..#.##.#",
"",
"Tile 1997:",
"##.#.#.##.",
".......#.#",
"....#.#..#",
"..###.....",
"#.#####.#.",
".##...##.#",
"#.#...#..#",
"#.#.......",
"####......",
".#..##..##",
"",
"Tile 3329:",
".#.##.....",
"...#.#..#.",
"...#..##..",
"#.#....#.#",
"...#####..",
"#......#..",
"#.#..#..#.",
"...###..#.",
"#..##..#..",
".###.##.#.",
"",
"Tile 1433:",
"..#...##..",
"....#..#..",
".#...#....",
"..#.##....",
"....##..#.",
"#...#....#",
"#.#...#...",
".##.#...##",
"#..#.#.#..",
"##.##.##..",
"",
"Tile 1103:",
"#########.",
"###.#....#",
"..#.##..##",
"##.#..##..",
".....#....",
"#...#..#.#",
"#....#....",
"###.#....#",
".#..##.#.#",
"#.#.##.#..",
"",
"Tile 3359:",
"..#.##.#.#",
"#.........",
"#...#....#",
"##..##.##.",
".#......#.",
"##..#..#..",
"#........#",
".######...",
"#...##...#",
".....#...#",
"",
"Tile 2143:",
"..#.#####.",
"#.#.#....#",
"....#.#...",
"#....#....",
"..#......#",
".....#....",
"##..##...#",
".#.####.#.",
"..#.#....#",
"....#..#.#",
"",
"Tile 2003:",
"#.#..#.#..",
"......##.#",
"#.....#.#.",
"##...#.#..",
"#.........",
"##..#....#",
".#####.#.#",
"......#...",
"...#..#...",
".###.#####",
"",
"Tile 3461:",
"#...#...#.",
".#.#.#.#..",
".#.......#",
"....##.#.#",
"#...#.#..#",
"..........",
"..#.#..#..",
".#.#....##",
".........#",
"####..#.#.",
"",
"Tile 3067:",
"....###.#.",
".#.....#.#",
".....#..##",
"#.#.....##",
".##......#",
"#.#.......",
".##......#",
".....#....",
"....#.....",
".#.....#..",
"",
"Tile 2027:",
".##.#....#",
"#....#.#..",
".......##.",
".......##.",
".........#",
"..#.....##",
"..#......#",
".#.....#..",
".....#....",
"..#.#.#...",
"",
"Tile 1951:",
"#.#.#..#.#",
"##...#.#..",
".........#",
"#.####...#",
"#.....#..#",
"#..##....#",
"#...##...#",
"###......#",
"...#......",
"##....###.",
"",
"Tile 1129:",
"#..###.#.#",
"#...#.....",
"#..#..#...",
"...#......",
"#.#.#..#..",
"#....#..#.",
"#........#",
"#........#",
".....#....",
"..#.#####.",
"",
"Tile 3257:",
"####.####.",
"#...#...##",
"....##.#..",
"....#..#..",
"......#.##",
"#.....#..#",
"......##..",
"......#.##",
"##.....#.#",
".#.###..##",
"",
"Tile 1223:",
"#.###.#.#.",
"#..####..#",
".....#...#",
"##.#.##...",
"#.....#...",
"##..#..#..",
"...#.###..",
"...#..#...",
"##.....##.",
".###.##..#",
"",
"Tile 1709:",
"#.##......",
"#.#.......",
"......##..",
"......#...",
"#.#.......",
".##....#..",
".......#.#",
".#.#...#..",
".###...#.#",
"##.###.#.#",
"",
"Tile 1019:",
"...##.##..",
"..#.....#.",
"#......##.",
"..#....#.#",
"..#......#",
"#........#",
".##...#...",
".......#.#",
"#..#......",
"...#.#.#.#",
            };
        }
    }
}
