using System;
using System.IO;
using System.Linq;

namespace ASU
{
    class Minimum
    {
        private static readonly string FILENAME = "../../Minimum/minimum.txt";

        static void Main(string[] args)
        {
            long[] numbers; int n;

            var stream = ReadInput(out numbers, out n);

            Commands(stream,
                (x, y) => Set(ref numbers, n, x, y),
                (x, y) => Console.WriteLine(Min(numbers, n, x, y)));
            
        }

        static TextReader ReadInput(out long[] numbers, out int n)
        {
            TextReader result;
            if ( File.Exists(FILENAME) )
            {
                var fileStream = File.OpenRead(FILENAME);
                result = new StreamReader(fileStream);
            }
            else
                result = Console.In;

            int.TryParse(result.ReadLine(), out n);
            int l = 1;
            while ( l < n + 2 ) l = l * 2;

            n = l;
            var s = result.ReadLine().Split(' ');
            numbers = Enumerable.Repeat(long.MaxValue, 2 * n).ToArray();

            for ( int i = 0; i < s.Length; i++ )
            {
                long tmp;
                long.TryParse(s[i], out tmp);
                Set(ref numbers, n, i + 1, tmp);
            }


            return result;
        }

        static void Commands(TextReader stream, Action<int, long> first, Action<int, int> second)
        {
            while ( true )
            {
                var cmd = stream.ReadLine().Split(' ');
                int x;

                switch ( cmd[0] )
                {
                    case "0":
                        return;
                    case "1":
                        long y1;
                        int.TryParse(cmd[1], out x);
                        long.TryParse(cmd[2], out y1);
                        first(x, y1);
                        break;
                    case "2":
                        int y2;
                        int.TryParse(cmd[1], out x);
                        int.TryParse(cmd[2], out y2);
                        second(x, y2);
                        break;
                }
            }
        }
        static void Set(ref long[] numbers, int n, int index, long value)
        {
            int k = n + index - 1;
            if ( k <= 0 || k >= numbers.Length ) return;

            numbers[k] = value;
            k = k / 2;

            while ( k > 0 )
            {
                numbers[k] = Math.Min(numbers[2 * k], numbers[2 * k + 1]);
                k = k / 2;
            }
        }

        static long Min(long[] numbers, int n, int indexFrom, int indexTo)
        {
            int a = n + indexFrom - 1;
            int b = n + indexTo;
            long result = numbers[a];
            while ( b - a > 1 )
            {
                if ( a % 2 == 0 ) result = Math.Min(result, numbers[a + 1]);
                if ( b % 2 == 1 ) result = Math.Min(result, numbers[b - 1]);
                a = a / 2; b = b / 2;
            }

            return result;
        }
    }
}
