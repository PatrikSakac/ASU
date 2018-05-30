using System;
using System.IO;
using System.Linq;

namespace ASU
{
    class Stafeta
    {
        private static readonly string FILENAME = "../../Stafeta/stafeta.txt";
        static void Main(string[] args)
        {
            int[] a = null, b = null, c = null;
            int n;
            ReadInput(ref a, ref b, ref c, out n);

            int[] results = new int[6];
            results[0] = Calculate(a, b, c, n);
            results[1] = Calculate(a, c, b, n);
            results[2] = Calculate(c, a, b, n);
            results[3] = Calculate(c, b, a, n);
            results[4] = Calculate(b, a, c, n);
            results[5] = Calculate(b, c, a, n);
            Console.WriteLine(results.Min());
        }

        static void ReadInput(ref int[] a, ref int[] b, ref int[] c,out int n)
        {
            n = 0;
            string[][] input = new string[3][];
            if ( File.Exists(FILENAME) )
            {
                using ( var fileStream = File.OpenRead(FILENAME) )
                {
                    using ( var streamReader = new StreamReader(fileStream) )
                    {
                        streamReader.ReadLine();
                        for ( int i = 0; i < 3; i++ )
                            input[i] = streamReader.ReadLine().Split(' ');
                    }
                }
            }
            else
            {
                Console.ReadLine();
                for ( int i = 0; i < 3; i++ )
                    input[i] = Console.ReadLine().Split(' ');
            }
            if ( input.Length != 3 ) return;

            n = input[0].Length;

            if ( n == 0 ) return;

            a = new int[n];
            b = new int[n];
            c = new int[n];

            int.TryParse(input[0][0], out a[0]);
            int.TryParse(input[1][0], out b[0]);
            int.TryParse(input[2][0], out c[0]);

            for ( int i = 1; i < n; i++ )
            {
                int.TryParse(input[0][i], out a[i]);
                a[i] += a[i - 1];
                int.TryParse(input[1][i], out b[i]);
                b[i] += b[i - 1];
                int.TryParse(input[2][i], out c[i]);
                c[i] += c[i - 1];
            }
        }

        static int Calculate(int[] a,int[] b,int[] c, int n)
        {
            int i = 1;
            int result = int.MaxValue;
            for ( int j = 2; j < n - 1; j++ )
            {
                if ( a[i] + b[j] - b[i] > a[j - 1] + b[j] - b[j - 1] )
                    i = j - 1;
                result = Math.Min(result, a[i] + b[j] - b[i] + c[n - 1] - c[j]);
            }

            return result;
        }
    }
}
