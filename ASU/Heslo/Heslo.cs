using System;
using System.IO;
using System.Linq;

namespace ASU
{
    class Heslo
    {
        private static readonly string FILENAME = "../../Heslo/heslo1.txt";

        static void Main(string[] args)
        {
            string s;
            int n;
            ReadInput(out s, out n);
            int[] arr = Enumerable.Range(1, n).ToArray();
            using ( StringReader reader = new StringReader(s) )
                Calculate(reader, 0, n, ref arr);

            string[] result = new string[n];

            for ( int i = 0; i < n; i++ )
                result[arr[i] - 1] = (i + 1).ToString();

            Console.WriteLine(string.Join(" ", result));
        }
        static void ReadInput(out string s, out int n)
        {
            n = 0;
            s = null;
            string[] input = new string[2];
            if ( File.Exists(FILENAME) )
            {
                using ( var fileStream = File.OpenRead(FILENAME) )
                {
                    using ( var streamReader = new StreamReader(fileStream) )
                    {
                        input[0] = streamReader.ReadLine();
                        input[1] = streamReader.ReadLine();
                    }
                }
            }
            else
            {
                input[0] = Console.ReadLine();
                input[1] = Console.ReadLine();
            }

            int.TryParse(input[0], out n);
            if ( n == 0 ) return;
            s = input[1];
        }

        public static void Calculate(StringReader s, int from ,int to,ref int[] result)
        {
            int n = to - from;
            if ( n <= 1 )
                return;
            int mid = (from + to) / 2;
            Calculate(s, from, mid, ref result);
            Calculate(s, mid, to, ref result);
            Merge(s, from, to, ref result);
        }

        static void Merge(StringReader s, int from, int to, ref int[] result)
        {
            int len = to - from;
            int[] r = new int[len];
            int p1 = 0,p2 = len / 2;
            for ( int i = 0; i < len; i++ )
            {
                if ( p1 >= len / 2 )
                {
                    r[i] = result[from + p2];
                    p2++;
                    continue;
                }

                if ( p2 >= len )
                {
                    r[i] = result[from + p1];
                    p1++;
                    continue;
                }

                var ch = (char)s.Read();
                if ( ch == '+')
                {
                    r[i] = result[from + p1];
                    p1++;
                }
                else
                {
                    r[i] = result[from + p2];
                    p2++;
                }
            }

            for ( int i = 0; i < len; i++ )
                result[from + i] = r[i];
        }
    }
}
