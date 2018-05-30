using System;
using System.IO;
using System.Linq;

namespace ASU
{
    class Karty
    {
        private static readonly string FILENAME = "../../Karty/karty.txt";

        static void Main(string[] args)
        {
            long[] numbers; int treeN, n, h;

            ReadInput(out numbers, out treeN, out n, out h);

            if ( n == 1 )
            {
                Console.WriteLine("=");
                return;
            }

            if ( numbers[treeN] >= numbers[treeN + 1] ) Console.Write(">"); else Console.Write("=");
            for ( int i = 1; i < n - 1; i++ )
                Calculate(numbers, i + treeN, treeN, h);
            if ( numbers[treeN + n - 1] >= numbers[treeN + n - 2] ) Console.WriteLine("<"); else Console.WriteLine("=");
            
        }
        static void ReadInput(out long[] numbers, out int treeN, out int n, out int h)
        {
            TextReader stream;
            if ( File.Exists(FILENAME) )
            {
                var fileStream = File.OpenRead(FILENAME);
                stream = new StreamReader(fileStream);
            }
            else
                stream = Console.In;

            int.TryParse(stream.ReadLine(), out n);
            h = (int)Math.Log(n + 2, 2) + 1;
            treeN = Power(h);
            var s = stream.ReadLine().Split(' ');
            numbers = Enumerable.Repeat(long.MaxValue, 2 * treeN).ToArray();

            for ( int i = 0; i < s.Length; i++ )
            {
                long tmp;
                long.TryParse(s[i], out tmp);
                Set(ref numbers, treeN, i + 1, tmp);
            }
            
        }

        private static int[] cachePower;
        static int Power(int p)
        {
            if ( p < 0 ) return 0;
            if ( cachePower == null || p >= cachePower.Length )
            {
                cachePower = new int[p + 1];
                cachePower[0] = 1;
                for ( int i = 1; i < cachePower.Length; i++ )
                    cachePower[i] = cachePower[i - 1] * 2;
            }

            return cachePower[p];
        }

        static void Set(ref long[] numbers, int n, int index, long value)
        {
            int k = n + index - 1;
            if ( k <= 0 || k >= numbers.Length ) return;

            numbers[k] = value;
            k = k / 2;

            while ( k > 0 )
            {
                numbers[k] = Math.Max(numbers[2 * k], numbers[2 * k + 1]);
                k = k / 2;
            }
        }

        static void Calculate(long[] numbers, int index, int n, int h)
        {
            int left = 0;
            int right = 0;
            int k = index;
            int reverseH = 1;

            while ( numbers[index] == numbers[k / 2] )
            {
                if ( k % 2 == 0 )
                    right += Power(reverseH - 1);
                else
                    left += Power(reverseH - 1);

                k /= 2;
                reverseH++;
            }

            int down = Power(h - reverseH + 1);
            int up = Power(h - reverseH + 2);
            
            if ( k - 1 >= down ) left += SubTree(numbers, k - 1, reverseH, numbers[k], false, h);
            if ( k + 1 < up ) right += SubTree(numbers, k + 1, reverseH, numbers[k], true, h);
            

            if ( left == right )
                Console.Write("=");
            else if ( left > right )
                Console.Write("<");
            else
                Console.Write(">");
        }

        static int SubTree(long[] numbers, int index, int reverseH, long limit, bool leftShift, int h)
        {
            int tmp;
            if ( numbers[index] > limit )
                //down
                while ( numbers[index] > limit )
                {
                    reverseH--;
                    tmp = index * 2;
                    if ( tmp >= numbers.Length - 1 ) break;
                    index = leftShift ? tmp : tmp + 1;                    
                }

            else
                //up
                while ( numbers[index / 2] <= limit )
                {
                    index /= 2;
                    reverseH++;
                }

            index = leftShift ? index + 1 : index - 1;
            int result = Power(reverseH - 1);
            if ( index >= Power(h - reverseH + 1) && index < Power(h - reverseH + 2) )
                result += SubTree(numbers, index, reverseH, limit, leftShift, h);
            return result;
        }
    }
}
