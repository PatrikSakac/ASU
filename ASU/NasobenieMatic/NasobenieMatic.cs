using System;
using System.IO;

namespace ASU
{
    class NasobenieMatic
    {

        private static readonly string FILENAME = "../../NasobenieMatic/matice.txt";
        static void Main(string[] args)
        {
            int[] input = ReadInput();
            Console.WriteLine(Calculate(input));
        }

        static int[] ReadInput()
        {
            string[] input;
            if ( File.Exists(FILENAME) )
            {
                using ( var fileStream = File.OpenRead(FILENAME) )
                {
                    using ( var streamReader = new StreamReader(fileStream) )
                    {
                        streamReader.ReadLine();
                        input = streamReader.ReadLine().Split(' ');
                    }
                }
            }
            else
            {
                Console.ReadLine();
                input = Console.ReadLine().Split(' ');
            }
            int[] size = new int[input.Length];
            for ( int j = 0; j < input.Length; j++ )
                int.TryParse(input[j], out size[j]);
            return size;
        }

        public static int Calculate(int[] p)
        {
            int n = p.Length - 1;
            long[,] m = new long[n + 1,n + 1];

            for ( int i = n - 1; i >= 1; i-- )
            {
                for ( int j = i + 1; j <= n; j++ )
                {
                    long min = long.MaxValue;
                    long pij = p[j] * p[i - 1];
                    for ( int k = i; k < j; k++ )
                    {
                        long value = (m[i,k] + m[k + 1,j] + p[k] * pij);
                        if ( min > value )
                            min = value;
                    }
                    m[i,j] = min;
                }
            }
            /*for (int i = 0; i < m.length; i++)
                System.out.println(Arrays.toString(m[i]));*/
            return (int)(m[1,n] % 99991);
        }

    }
}
