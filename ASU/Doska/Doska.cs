using System;
using System.Collections.Generic;
using System.IO;

namespace ASU
{
    class Doska
    {
        private static readonly string FILENAME = "../../Doska/doska.txt";

        static void Main(string[] args)
        {
            var sortedList = ReadInput();
            
            var distance = MinDistance(sortedList);
            Console.WriteLine(distance);
        }

        static List<P> ReadInput()
        {
            TextReader stream;
            if ( File.Exists(FILENAME) )
            {
                var fileStream = File.OpenRead(FILENAME);
                stream = new StreamReader(fileStream);
            }
            else
                stream = Console.In;

            var n = int.Parse(stream.ReadLine());
            var list = new List<P>(n);
            for ( int i = 0; i < n; i++ )
            {
                var positions = stream.ReadLine().Split(' ');
                var point = new P() { X = int.Parse(positions[0]), Y = int.Parse(positions[1]) };
                list.Add(point);
            }

            list.Sort(XComparer.XCompare);

            return list;
        }

        static double MinDistance(List<P> points)
        {
            var crtMinDist = double.MaxValue;            
            var leftMostCandidateIndex = 0;
         
            var candidates = new SortedSet<P>(new YComparer());
            foreach ( var current in points )
            {
                while ( current.X - points[leftMostCandidateIndex].X > crtMinDist )
                {
                    candidates.Remove(points[leftMostCandidateIndex]);
                    leftMostCandidateIndex++;
                }
                
                var head = new P { X = current.X, Y = checked(current.Y - crtMinDist) };
                var tail = new P { X = current.X, Y = checked(current.Y + crtMinDist) };
              
                var subset = candidates.GetViewBetween(head, tail);
                foreach ( var point in subset )
                {
                    var distance = current.SqrDistance(point);
                    if ( distance < 0 ) throw new ApplicationException("number overflow");
                    
                    if ( distance < crtMinDist )
                        crtMinDist = distance;
                    
                }
                
                candidates.Add(current);
            }

            return Math.Sqrt(crtMinDist);
        }


    }

    public class P : IComparable
    {
        private static int counter;
        public int Id { get; private set; }
        public double X { get; set; }
        public double Y { get; set; }
        public P()
        {
            Id = ++counter;
        }
        public override string ToString() { return string.Format("Id: {0}; X: {1}; Y: {2}", Id, X, Y); }
        public override bool Equals(object obj)
        {
            var other = obj as P;
            if ( other == null ) { return false; }
            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public int CompareTo(object obj)
        {
            var o = obj as P;
            if ( o == null ) return -1;

            return XComparer.XCompare(this, o); // default
        }

        public double SqrDistance(P p)
        {
            var dx = p.X - X;
            var dy = p.Y - Y;

            var dist = (dx * dx) + (dy * dy);

            return dist;
        }
    }

    public class YComparer : IComparer<P>
    {
        public int Compare(P p1, P p2)
        {
            return YCompare(p1, p2);
        }

        public static int YCompare(P p1, P p2)
        {
            if ( p1.Y < p2.Y ) return -1;
            if ( p1.Y > p2.Y ) return 1;
            if ( p1.X < p2.X ) return -1;
            if ( p1.X > p2.X ) return 1;
            return 0;
        }
    }
    public class XComparer : IComparer<P>
    {
        public int Compare(P p1, P p2)
        {
            return XCompare(p1, p2);
        }

        public static int XCompare(P p1, P p2)
        {
            if ( p1.X < p2.X ) return -1;
            if ( p1.X > p2.X ) return 1;
            if ( p1.Y < p2.Y ) return -1;
            if ( p1.Y > p2.Y ) return 1;
            return 0;
        }
    }



}

