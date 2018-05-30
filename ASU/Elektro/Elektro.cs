using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASU
{
    class Elektro
    {
        private static readonly string FILENAME = "../../Elektro/elektro3.txt";
        static void Main(string[] args)
        {
            Dictionary<int, TreeNode<SocketData>> tree;int[] sockets; int n;
            ReadInput(out tree,out sockets, out n);

            int cabelSize;
            int cabelDistance = Calculate(tree, sockets, n, out cabelSize);
            Console.WriteLine(string.Format("{0} {1}", cabelSize, cabelDistance));
        }

        static void ReadInput(out Dictionary<int,TreeNode<SocketData>> tree,out int[] sockets,  out int n)
        {
            TextReader stream;
            if ( File.Exists(FILENAME) )
            {
                var fileStream = File.OpenRead(FILENAME);
                stream = new StreamReader(fileStream);
            }
            else
                stream = Console.In;

            var rooms = stream.ReadLine().Split(' ');
            sockets = new int[rooms.Length];
            tree = new Dictionary<int, TreeNode<SocketData>>();
            tree.Add(0,new TreeNode<SocketData>(new SocketData(), null));
            tree.Add(1, new TreeNode<SocketData>(new SocketData() { index = 1}, null)); //root

            int counter = 1;
            for ( int i = 0; i < sockets.Length; i++ )
            {
                sockets[i] = int.Parse(rooms[i]);
                for ( int j = 0; j < sockets[i]; j++ )
                {
                    counter++;
                    tree.Add(counter, new TreeNode<SocketData>(new SocketData() { index = counter }, null));
                }
            }
            
            n = counter - 1;
            
            for ( int i = 0; i < n; i++ )
            {
                var section = stream.ReadLine().Split(' ');
                int x = int.Parse(section[0]);
                int y = int.Parse(section[1]);
                int d = int.Parse(section[2]);

                tree[y].data.parentDistance = d;
                tree[x].AddChild(tree[y]);
            }
        }

        static int Calculate(Dictionary<int, TreeNode<SocketData>> tree, int[] sockets,int n,out int cabelSize)
        {
            int counter = 2;
            bool[] useSocket;
            int distance = 0;
            int[] cableSizes = new int[tree.Count];
            for ( int i = 0; i < sockets.Length; i++ )
            {
                useSocket = new bool[n + 2];
                for ( int j = 0; j < sockets[i]; j++ )
                {
                    var node = tree[counter];

                    while ( node != null && !useSocket[node.data.index] )
                    {
                        if ( node.parent != null && node.parent.data.index == 1 )
                            cableSizes[node.data.index]++;

                        useSocket[node.data.index] = true;
                        distance += node.data.parentDistance;
                        node = node.parent;
                    }
                    counter++;
                    
                }
            }
            cabelSize = cableSizes.Max();
            return distance;
        }
    }

    class SocketData
    {
        public int index;
        public int parentDistance;
    }
    
    class TreeNode<T>
    {
        public T data;
        public TreeNode<T> parent;

        private LinkedList<TreeNode<T>> children;
        public int ChildrenCount { get { return children.Count; } }

        public TreeNode(T data, TreeNode<T> parent)
        {
            this.data = data;
            this.parent = parent;
            children = new LinkedList<TreeNode<T>>();
        }

        public void AddChild(TreeNode<T> child)
        {
            child.parent = this;
            children.AddLast(child);
        }

        public TreeNode<T> GetChild(int i)
        {
            foreach ( TreeNode<T> n in children )
                if ( --i == 0 )
                    return n;
            return null;
        }
    }
}
