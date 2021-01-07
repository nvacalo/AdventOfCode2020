using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Color { get; set; }
    }

    public static class Day24
    {
        public static Dictionary<string, Tile> _Tiles = new Dictionary<string, Tile>();
        public static void RunDay()
        {
            //var input = GetTestInput();
            var input = GetInput();

            foreach (var inp in input)
            {
                string directions = inp;
                int x = 0;
                int y = 0;
                int flips = 0;
                //Console.WriteLine();
                //Console.WriteLine(inp);
                while (directions != "")
                {
                    if (directions.StartsWith("e"))
                    {
                        y -= 2;
                        directions = directions.Remove(0, 1);
                    }
                    else if (directions.StartsWith("se"))
                    {
                        y -= 1;
                        x += 1;
                        directions = directions.Remove(0, 2);
                    }
                    else if (directions.StartsWith("sw"))
                    {
                        y += 1;
                        x += 1;
                        directions = directions.Remove(0, 2);
                    }
                    else if (directions.StartsWith("w"))
                    {
                        y += 2;
                        directions = directions.Remove(0, 1);
                    }
                    else if (directions.StartsWith("nw"))
                    {
                        x -= 1;
                        y += 1;
                        directions = directions.Remove(0, 2);
                    }
                    else if (directions.StartsWith("ne"))
                    {
                        x -= 1;
                        y -= 1;
                        directions = directions.Remove(0, 2);
                    }

                    //Console.WriteLine($"x: {x}, y: {y}");
                }

                Tile tile = null;
                if (_Tiles.ContainsKey($"{x},{y}"))
                {
                    tile = _Tiles[$"{x},{y}"];
                }

                if (tile == null)
                {
                    tile = new Tile { X = x, Y = y, Color = 0 };
                    _Tiles[$"{x},{y}"] = tile;
                }
                else
                {
                    if (tile.Color == 1) tile.Color = 0;
                    else tile.Color = 1;
                }
            }

            //Console.WriteLine(_Tiles.Count(t => t.Value.Color == 0));

            for (int day = 1; day <= 100; day++)
            {
                List<string> toFliped = new List<string>();

                foreach (var tile in JsonConvert.DeserializeObject<List<Tile>>(JsonConvert.SerializeObject(_Tiles.Values)))
                {
                    AddNeighbors(tile);
                }

                foreach (var tile in JsonConvert.DeserializeObject<List<Tile>>(JsonConvert.SerializeObject(_Tiles.Values)))
                {
                    if (MustFlipped(tile))
                    {
                        toFliped.Add($"{tile.X},{tile.Y}");
                    }
                }

                foreach (var toFlip in toFliped)
                {
                    var tile = _Tiles[toFlip];
                    if (tile.Color == 1) tile.Color = 0;
                    else tile.Color = 1;
                }

                Console.WriteLine($"day {day}: {_Tiles.Count(t => t.Value.Color == 0)}");
            }
        }

        private static void AddNeighbors(Tile tile)
        {
            GetNeihbor(tile.X, tile.Y - 2);
            GetNeihbor(tile.X + 1, tile.Y - 1);
            GetNeihbor(tile.X + 1, tile.Y + 1);
            GetNeihbor(tile.X, tile.Y + 2);
            GetNeihbor(tile.X - 1, tile.Y + 1);
            GetNeihbor(tile.X - 1, tile.Y - 1);
        }

        private static bool MustFlipped(Tile tile)
        {
            int black = 0;
            int white = 0;

            var neih = GetNeihbor(tile.X, tile.Y - 2);
            if (neih.Color == 1) white++;
            else black++;

            neih = GetNeihbor(tile.X + 1, tile.Y - 1);
            if (neih.Color == 1) white++;
            else black++;

            neih = GetNeihbor(tile.X + 1, tile.Y + 1);
            if (neih.Color == 1) white++;
            else black++;

            neih = GetNeihbor(tile.X, tile.Y + 2);
            if (neih.Color == 1) white++;
            else black++;

            neih = GetNeihbor(tile.X - 1, tile.Y + 1);
            if (neih.Color == 1) white++;
            else black++;

            neih = GetNeihbor(tile.X - 1, tile.Y - 1);
            if (neih.Color == 1) white++;
            else black++;

            if (tile.Color == 0 && (black == 0 || black > 2))
            {
                return true;
            }
            else if (tile.Color == 1 && black == 2)
            {
                return true;
            }

            return false;
        }

        private static Tile GetNeihbor(int x, int y)
        {
            string neihCode = $"{x},{y}";
            Tile neih = null;
            if (_Tiles.ContainsKey(neihCode))
            {
                neih = _Tiles[neihCode];
            }

            if (neih == null)
            {
                neih = new Tile { X = x, Y = y, Color = 1 };
                _Tiles[neihCode] = neih;
            }

            return neih;
        }

        public static List<string> GetTestInput()
        {
            return new List<string>()
            {
           "sesenwnenenewseeswwswswwnenewsewsw",
"neeenesenwnwwswnenewnwwsewnenwseswesw",
"seswneswswsenwwnwse",
"nwnwneseeswswnenewneswwnewseswneseene",
"swweswneswnenwsewnwneneseenw",
"eesenwseswswnenwswnwnwsewwnwsene",
"sewnenenenesenwsewnenwwwse",
"wenwwweseeeweswwwnwwe",
"wsweesenenewnwwnwsenewsenwwsesesenwne",
"neeswseenwwswnwswswnw",
"nenwswwsewswnenenewsenwsenwnesesenew",
"enewnwewneswsewnwswenweswnenwsenwsw",
"sweneswneswneneenwnewenewwneswswnese",
"swwesenesewenwneswnwwneseswwne",
"enesenwswwswneneswsenwnewswseenwsese",
"wnwnesenesenenwwnenwsewesewsesesew",
"nenewswnwewswnenesenwnesewesw",
"eneswnwswnwsenenwnwnwwseeswneewsenese",
"neswnwewnwnwseenwseesewsenwsweewe",
"wseweeenwnesenwwwswnew",
            };
        }

        public static List<string> GetInput()
        {
            return new List<string>()
            {
              "enwwsenweswweseseswnenewwswseswenw",
"nwnwneswwnenwnwnwenenwwnesenenwsenenee",
"eneswwenwneeswneenenw",
"swwswswwwswswsweeswww",
"swewnwneeewnewwwswwnwwswswnwww",
"seswnwenesenwsenenwwenwsenwswnwseswnw",
"nwswwenwwwswwnwnewswnwnwwwswnee",
"seswseswseseseseeseseseswnwsesw",
"swswseswseswswnweneswswseswse",
"wenwswwswwwswwnwsesewwenwseswwne",
"nenewswsenenenenenwnenesewswnenenewe",
"eeneneswswnenwnenenwnwnwnwnwnwswenwnesw",
"nwsweneeeeweenenwneeseeesenesw",
"nwnewsesesesesesesewsweseenwsesesese",
"nenwneenenwneneneswsenenenenewnenwnenewse",
"wswnwseseneswswswenewswneswenw",
"wwswsewswswwwnewnwwswswewsewwnw",
"nwnwnwsenenwnwnwneenwwnwnwswwnwnwenwnw",
"seenweeswenwwseeeeeesweesese",
"neswwewnewwwnewseswwwwwnwseswnew",
"swseseesweenwswnwswswnwsesewseswene",
"nwsenenenwsenenenesenewneswwnenenenesenene",
"ewnenwneswsenenwnwsenewsenwwnenwnesesene",
"sesenenwswsesesweseseswnwsesenesenewe",
"wwenwnewewswwswwswwsewwenww",
"neweeseseswswwweenwnenwweeneenew",
"neseneseneneneneneswnenwswnewnenenenene",
"wswwwnwewwwnwneswswsewewwweww",
"swseseswwnwswswswnwnew",
"eseseesewnwseseseewseneseesesenwsw",
"swsweswswnesenwswsesw",
"seeeswnenweenewseeesweeenenewsw",
"swnwsewesewnenwswneweneswseenwsew",
"wsesweswnwswswnesweswwwswswswswsww",
"wseenwnwswneewnwswsweenenwnwswneew",
"seswnwseeeseseseseseswswneswseesenwsew",
"nwnwnenwnwwnwsenwenwneneneswnwnwnw",
"swseeseswswnwswsesesenwsesenwswnwnw",
"nwsewnwwnwwewwneswnwenenwseneswsw",
"wwsewswenwwwnweenwnwnenwswswwnw",
"wneeneeneeeeseenesewneneeswee",
"wnwenwswneneenwswnenesenwnenwwneswnwse",
"sewsenwseswsewewswneswnwneseseswseee",
"wnwwwswnwnwnwwsenwneww",
"swswnwweseeswneswswswswswnwswswswswswne",
"neeneneeeneeeswenwnenenwswneneswe",
"wneeseeeseeseswsenwweeeeseee",
"nenweesenesenewwwsweswnwnwweswne",
"senewseswsewswneseesenwsenenwsesenwsw",
"ewenwnwseeeenwesenwnewswwswe",
"nwnwnesesewnwnwnewneenwnwnwnwnwswnwne",
"enwwewnwnesenenenwnwnenenweneseswwswne",
"nwswnenwswnwwnwseswnwnwewnwwwnenew",
"wweeswweneseewnewwwewwewse",
"swseseswswseseneseswneswseswneswseswnwsw",
"wnwnwnesenwwnwnwnwwsewnwswnenwwnw",
"sewsenwswsenewneneewnwswwnesw",
"wswseneeswsenwsesenwwneswnenwewsesesene",
"nwnwnwwnwnwnwenwsenwwnwenwnww",
"esenweeswseseenwe",
"nwnenwwnwnwneweswswwnwsweesewsew",
"swwswseeseseneseseeswswwswnesesenwwswsw",
"seseesewseseeeswnwseseenwseesesenwsese",
"wwnwwwswwwwwnewww",
"swseenewseneneeeswswnwswsenwswenwsene",
"swwswwseewnwnenwwnewwnenwsewwnw",
"nwnwenwswnwnwnenwnwwnenwsenenenwnwwsenw",
"nwenwwnwnwnwnenwnwneswnwnwnwseenwnwnw",
"seswnenenewwneeneneesesewenwenwse",
"nenenenewwnenesee",
"senwwwwenwwnwwwnesenwnwsenwnwnwnw",
"senwneswwewwseswwnwswesewnenwwwww",
"nenenenenenwneneewneeneswneswsewene",
"swseneswswwswswnwswnewswseseseswswswswswne",
"wwenwewnwwnwnwnwwwwswneswsewnww",
"swnwnwwswswseswsenweneseswswswwswsee",
"wnwswsenwwwswsweswwswswswewwww",
"neeeswneeeeeeeeneeneeswwenesw",
"newwwnewwwnwnwsewnwswsew",
"enwswnwnesenewwneenenwnesw",
"eneewwneswnwneeneeswe",
"seseesesesesesesesenwsesenwesewswnenew",
"wseswswseswnesweswswswsw",
"seeeeeenweeeeweseenenenewwe",
"nwsenewsenweswsewwnwenwwenwse",
"nwwnwnwwnenwwswsewsenenwnwnwewwse",
"eeneeweeeeeenee",
"seswneneewswswswswswswswswneswwwwsw",
"senwnewnwnwenwnwnwnwnwnwnwnwnw",
"wwsenwnwwenwwwneewwnwswseneswe",
"nenenenenenwnenenesenewneneewnwseewsw",
"wsenwswswnwnwnwnwnenwwnwnwwnwneeenw",
"neeenwsewsesweeeenweneswenenwswwe",
"nwwnwenwwnwnenweswwenwesenwnwwnw",
"seneswnwwswwswwswwwwwnewswneswe",
"eeneeweweeeenee",
"wneseseweneseeewsesenesenwwwwnwse",
"swwnweseswneesesenewseseeswswnwwswne",
"swnwsenwsewseseseswnwseneseswswswswene",
"swwseswswseseneseswseswseswswneneswnwsw",
"sesesenweswswsweswsewseseseseswsw",
"seswnwewwwsewnwwnenwwnwwnwnwnwnw",
"enweewseeseenweneseseseenweeee",
"wwwwwwswwwwneswswsww",
"sweswnwnwsewswsewnwswwwwneseneesw",
"nwseswnwnenwsewnwenw",
"nwnesenewnesweenenenwneneneneenewsene",
"swswnwwswswseswenwswswwswswe",
"nenwnenwnwnwnwwnwenenwswnesewnenwnwe",
"swseeseswweswswswseneswswswswswswww",
"neswnwnenenwnwneneneneneeswnwnenenwneesw",
"nenwnwesewnwsenenenwnwswwnwnwnesene",
"seeeseswewnwneseseenwweeeseneesw",
"swswswnenwswnwneswseswesewnwseswsenwswsw",
"senesewseseseseseseeswesenwsewsenesese",
"nwnwnwwnwnwnwnwsenwnewnwnwww",
"neswneseeseneswwwnewswwwwwwswwsww",
"nweesesenwesesesenweeeseseeseswse",
"neneeneswneswneenesenwwnwsenwneswwnene",
"enewewswnwnweesweswseeeeeenwe",
"eswseeseneeseseseseeee",
"sesesewsesewseesenwsesesenwswesenweswse",
"nwnwswnesweenwswneseeseswneewnenenenwsw",
"nwseswswswseeewswswwenwne",
"swneewswnwswswswseswwseeeswswswneswwsw",
"eneneeseneewneneenenesenwnewnenene",
"nwsweseswswswswswswswsw",
"nwnenwnwwwnwseneswnwenwnwswswwweswnee",
"nenwnwwnwwesenwseswenenwwnesewnww",
"sesenwseeweeenweseeeeeenwesee",
"eeswwesewseseeeesewnw",
"nwenwnwenwnwnwnwswnwwnwewnwnwswswnw",
"neweswnwsweneneweneeneneswnenesenwsw",
"seseseswswsesenesewsesese",
"seeeeeewesenwenenwewsenwseee",
"eneneweewwneeneneesene",
"swnwnweeswswsweesenwsewseswswnwswsewse",
"neneseneswneneneneswneneswneneneenenwnee",
"nenewnenewwneneneneneeseneneneseneswse",
"nwswsweeeseeewwnwswnwswnwne",
"newwnwsewswwwnewwwsewwwewww",
"wwwwwnwwwwswwenwnwnwnwwesweew",
"nwneneneseneeeneswswneweseeeneene",
"enesesenwnwseseewneswswseesweseseene",
"wwnwenewwwnwnwnwsenwnwwwnenwnwsw",
"swwwewwweww",
"wnewwwseeesewsenwnwnwnwswnwewwnw",
"eswnwnesenwnwnwseseswnwswseseewswsew",
"swneneseswnenwnenwenenweswneenwnenwne",
"wwnewnewsewsww",
"nwwenwnwneseseewwwnesenwswswswnenw",
"seenwnwswwsenwseswwsewneswseseesee",
"eseeseeswseeseeesenenewese",
"eseseseseeseeseseswesesenenwnewesw",
"neeswnwswsweeewnweesweswnewnwene",
"newwnwnwneswwwenwwwsewwwsenww",
"nenwswnwnwnwswnwneseewswnwnwnee",
"sesewswwswwsewswnwnwnew",
"sweswswnwnwnweswswsesw",
"senesenweneneswsewnenwnenenenenenwenee",
"enwwnwswnwneenwnwnwnwswnwnwseswnw",
"newswnwnwswewneneneneswwwseneneesene",
"seswsesesewswneseseseseewswswsenwsese",
"newnwnenwnenenwsenwseneneneswnwnenwnene",
"nwnwnenwnwnwnwnwswnwnwnwwwewnweswnw",
"nwnenwnwnenenwnwwsenenenwnweswnene",
"nwwnenwnenenesenewseneneneswnwenwnwnenw",
"nwnwnwnenwnwwnwsenewnenwsenwnwnwnenwnwsw",
"swsenwswswswswneswswswsweswswswsewswnw",
"wwswwswsweswnwswwnwewwsewwwew",
"sewwnenwnwswwwnwwewnwwnwnenwneswnw",
"ewneeenweneeseeeswenw",
"nwnwneeswneseewneneneseeeeee",
"wsweswewswwnewswnwwwnwnwswwesewe",
"enwnwnwsenwnwnenwneswnwsenenwneswnwnwnw",
"nenesenewnenwswnewwsesewswsenewsww",
"wnenenenenenenenenenenenesenwnesesewnene",
"wswnwwwwnwnwewnwsew",
"nesenwwswseenwseswnesesesenweseseseese",
"nwewnenenwswsenwnewnwneneesenwnwwnwnwne",
"nwnwnwwnwneneswenenwnwnwsenwneswneeswne",
"seseseseeeseswnwsenwswneseeeewsese",
"sewnenweswneeswenesenwenwwswwneswnw",
"wsesesesewsesesesesesesenwseseeeew",
"seseeeesesesweewewwesewwnew",
"nwnwswswenwnenenenwnwnenwnwweneswnenwe",
"neeneneenwneneneneesweswneseneneew",
"weewwwwwnewwwwwsewewswnw",
"swseseenweeeeeeee",
"seseseeseseseneseseeswseee",
"nwewnwneenenenwnwnwwswsewnwsenenwne",
"nenwnesenewnenenenenenenenesenenw",
"seeesweenwsesese",
"nenwnenwsesenenenenenwwnene",
"eseneseswnwsewesesenwseseeeeswsese",
"swseseesweseneswneesesesenwsee",
"swnenewwswwwwwnwwewnewswwsesw",
"eswnwsesenwsesenwenwnesesesweswwnwnenw",
"nesenwneseseswswseseswswseweswneswswsese",
"eseswsewnwnewenwseneeeseenesw",
"eneeeneeneweneewneneeneewsee",
"sewesesewseswseseseseseswnesenwneseswsw",
"eneesenwneeseenenwwseneeneeee",
"neweneswnenweeneeseeneewnesenenwe",
"wsesewswwwwnwenweswnwneneswnww",
"eswnwenwwweswwsewswswnwsenwwsww",
"wnewnwwsenenwnwnwnwnesenenwsesewnene",
"seswnwwwseneswnenewwnwesesewne",
"eenwesweweeneeeeweeee",
"neneenewswwseenenwnenenwnwnesenenenene",
"nwsenwnwsesenwseseseeseswseseseseseswsese",
"sesenwseswewswswsenweswnwswswseenesese",
"swsewswsenwswwnwsweenwnwseseseeswwe",
"seeseswswneseseswwsesenwswsesesenesese",
"sweneswnenwswswseswseswswseewnwnenwesw",
"wswseswwnewwwswswswswsewswswnwwenw",
"seneenweswnwswneeseseeswsesenwesesee",
"wwenwsewwwwwww",
"swseswswwseseseseeswsw",
"wswneneswswnwwseswwwswswewwsewesw",
"esesweeeesenenenwnwnew",
"sesesenweseseeseseseeesee",
"esewseseseseswsesenesewsewswsesesee",
"swwswwwswwnwwswwsenewneswswsenwwe",
"seseswsesesesesewsesenesenesesenwseswsenw",
"senwwwwenwseneswseneewnwnwenewww",
"seswsenwsweseswseseneeseeseenwseww",
"seseswwneneswswwwswnwneswswswsewnwswwsw",
"eweewneeenewwseewewseene",
"neeneswnenenenenenene",
"wnenwwwwnenwswwwswsenwwenwweww",
"wnenwnenenesweeneneenenenwneenwsese",
"neewsenenesesenweseseswsweswne",
"neneeneneeeeneesw",
"eseneswseenwswneesweseeseeesenwnwee",
"wwsesenwneswneseseneswnwseseneseswsene",
"wewsenwewwwnwwwwwwwwesene",
"wsewnwnwwseeswwsw",
"swwswwswswswwswsweswwsw",
"eeeseeneswnwseseneseenwesweswnwswswne",
"eseswwsenenesesewsesesesesese",
"eseeswesesesesewseenwsesewnwsesesese",
"newwwwwsewswnwnwneseswwnwswwwwne",
"nwesenwnwwswwweweewwwneswwsw",
"eeeeeeeeeweese",
"eeneewweswweeneenwseseseeswnenw",
"swewswwwwnwsweswswswneewseswwww",
"swwswwsweswwswwnesw",
"seeseseneesewseseswnwswsesweseneseese",
"swswswewneseseseseseswnwswnesesesenwse",
"weseeeeeeeee",
"swswsewseseeswsesw",
"seseseswnweneswsenenenwwwswswsenweeese",
"swnenwneswenwnwsenwenenenenenenewswnene",
"nenewwwwwswweswewewnwwsewnwse",
"senwewswswnwenwswseneeswswse",
"eseeeweweeeeenwsweneeeneseee",
"enwseeeseeneeweseeseeeeswenwe",
"nwnwsenwnwnwnwsenwnw",
"swswwswswweewwwsww",
"seeeeeeewsee",
"ewwwnwwnwwsenesewneswswswswswseew",
"nesenwwnwnwseswswnwneswnwnwnwnwnwwnenw",
"nenwsweseswseewneswwseeenwnenese",
"wswswswswswswswwswnese",
"swnenwswswnwswnesenwnwseewseneseenwnene",
"nwsesenesenwnwswesenwnenenesewwnwnwnw",
"nenwswswwewnwseswwwswneewnewese",
"enenenenewwneneesesenenewenenenesew",
"nwnwsenwsenwnenwnesenenenwwesenwwsenesww",
"nwsenwswswsesesenesewswsesesesesesenene",
"eeneweenwenwenwsesesweseeswsenw",
"seseseswswswswswnwwesweswneswnw",
"seseswsesesenwseswswsee",
"nwwnwnewwwwwwwewsewnwswnww",
"seswnewswnwseneseneswseseeswswsenesewse",
"sewswneswnwswswswswneweswswswswswswswsw",
"wenesenenenwnwnwnew",
"nenwnwnwnwnwwweswwenwnwnwnwswnw",
"seneneswneneneswnwsenwnesenw",
"wswswnwsweswnwsesewsenwnenewsewsene",
"eswnwseeseewseenwe",
"neenwsweswnenewsenwnwswne",
"wnwewnwwnwswwnwwseewsenwnesenwnw",
"nwneneenewsesweseneeneneewsenwnenene",
"sesewsesenewswseseenwseneswswsesesese",
"wwwsewwnwwnesenwwnwnesewwwnwnww",
"seseseseswnewseswnewne",
"neswsweneeneneeneswenwnenenewneenene",
"wsesesesesesesenwseeseseeseseneswwne",
"swwenwnwnwenwsenwwwne",
"swnenenenenenewnenenwneesenwswnenenenene",
"wswneseneswenewswswnwswneewenwweswe",
"nenewseenwsenesenwswwsewwnwewsewnw",
"wseeseeweneswsenweseneesenenwswwsw",
"neswswneswsewwswwsewsweeneswwnwsw",
"sesenwswnwseswseeswswseswneseseseswwswsw",
"seswneseneswwnwnwnesenwseenw",
"eesesewsewnwnenenenwnweseswwwnee",
"wneenewnenenwneneseneesenesenwwwsenese",
"seseseseseeweseswsesewseseesesenwe",
"nwenwwenwnwnwwnenwnwnwsenw",
"eseeeneeseneswewseseeesesenweesw",
"swsewswswnwsewnene",
"nwnenwneswnwnwnenenenwnwnenenw",
"eeeseeewseeeseeesee",
"swnenwswnwnwnenwnenwnwnwnwnwenwnwenwnesw",
"eeseseeswwneeeesesesesesenewsee",
"swnenenenesenenwnesenenwnenesenenenenewne",
"neewwwwewwesewwswwnwwwne",
"nwsewnwwswwnwwwwswneneswneswnwnwnew",
"wswswseeneswwwwsweswswswwnenwesw",
"nwswewnwsenwswnenweewnwnwenwsw",
"seeenwweseeeeenenweeesenw",
"swnwwswswswswwswswneswswneesw",
"sweseneneswwswswewswwwnenwseneswnese",
"eneenwewseeeeesenwseeeseseswsee",
"nwnewnwnwsenwnwwneesenwnwnenewnw",
"newswswwswwsewsewwnene",
"eneseswenewwneseneneneneswneneneswnene",
"nweneeseneeeesesweeneeenwewnee",
"seswsewnesenenwnenwswwwwnweweswnw",
"nwneseneswswseseseswnewneswseseseesene",
"eeewseswweswenwesenwsewswswnenwne",
"nwnwnwnwwnewnwswnwnwsenwnwnwnewenwnw",
"seseswswsweswwseswswsweswwsesesenwnesw",
"neeswnwswswswseswswwneswswswseseswswsesw",
"swswseeneswswswwswswswnwnwseswenwew",
"swswswseswsweswswewswnwseseswswswnwnee",
"nwwwnwnenwneneeeenwswnenw",
"senenwswnwneewsewswwweneeeeesesw",
"nwswenwsweseneesesewnwwnwseweenenese",
"swesenwseeneseeseswswseswseeseenwnwse",
"wneseweweneneswsenwnenenenenwnenwnew",
"swswswswswswswswnwswwwwswse",
"neeseseenwsweneneeesenwweewwew",
"swswswsweeswewnwwewsweswnenwswswnw",
"enwwneneswnenenwnwnenenenwwneenenenese",
"nwwwenwwenwwwnwnwnwnwseswnwwnww",
"sweswwwsweweswswweswwnwswswnenwese",
"neseseseseseseseseseseseswsese",
"nwsesenwneseseseseswseese",
"swneswnwwwwenwwwe",
"nwnwnwnwnewnwwwnwnwnenenwsenwsesenwnww",
"nwwswneseseswsenweswswswneswwswwswswsw",
"neenwsenenenenenenwswswnenenwsenenene",
"newnwnwnwnwwnweswwewsewenwwnwnwnwnw",
            };
        }
    }
}
