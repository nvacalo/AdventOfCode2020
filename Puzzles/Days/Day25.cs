using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{

    public static class Day25
    {
        public static void RunDay()
        {
            decimal cardPublicKey = 14788856;//5764801;
            decimal doorsPublicKey = 19316454;//17807724;

            decimal card = 1;
            int cardLoops = 0;
            while (card != cardPublicKey)
            {
                card = card * 7;
                card = card % 20201227;
                cardLoops++;
            }

            decimal door = 1;
            int doorLoops = 0;
            while (door != doorsPublicKey)
            {
                door = door * 7;
                door = door % 20201227;
                doorLoops++;
            }

            decimal encryption = 1;
            for (int i = 0; i < doorLoops; i++)
            {
                encryption = encryption * cardPublicKey;
                encryption = encryption % 20201227;
            }

            Console.WriteLine(cardLoops);
            Console.WriteLine(doorLoops);
            Console.WriteLine(encryption);
        }
    }
}
