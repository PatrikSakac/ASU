using System;
using System.Collections.Generic;
using System.IO;

namespace ASU
{
    class Bridge
    {
        private static readonly string FILENAME = "../../Bridge/bridge.txt";
        static void Main(string[] args)
        {
            GraphVertex[] graph; Edge[] edges; int[] removeEdges;
            ReadInput(out graph, out edges, out removeEdges);

            int componentsCount = Calculate(graph);
            int[] output = new int[removeEdges.Length + 1];
            output[removeEdges.Length] = componentsCount;
            for ( int i = removeEdges.Length - 1; i >= 0; i-- )
            {
                var edge = edges[removeEdges[i]];
                if ( graph[edge.start].componentIndex.index != graph[edge.end].componentIndex.index )
                {
                    MergeComponents(graph, graph[edge.start].componentIndex, graph[edge.end].componentIndex);                    
                    componentsCount--;
                }

                output[i] = componentsCount;
            }

            for ( int i = 0; i < output.Length; i++ )
                Console.WriteLine(output[i]);
        }

        static void MergeComponents(GraphVertex[] graph, ComponentIndex comp1, ComponentIndex comp2)
        {
            if ( comp1.verts.Count > comp2.verts.Count )
                SetComponentAllVerts(graph, comp1, comp2);
            else
                SetComponentAllVerts(graph, comp2, comp1);
        }

        static void SetComponentAllVerts(GraphVertex[] graph, ComponentIndex comp1, ComponentIndex comp2)
        {
            foreach ( int j in comp2.verts )
            {
                graph[j].componentIndex = comp1;
                comp1.verts.Add(j);
            }
            comp1.index = Math.Min(comp1.index, comp2.index);
        }

        static void RemoveEdge(ref GraphVertex[] graph, Edge edge)
        {
            graph[edge.start].neighbours.Remove(edge.end);
            graph[edge.end].neighbours.Remove(edge.start);
        }

        static void AddEdge(ref GraphVertex[] graph, Edge edge)
        {
            if ( graph[edge.start] == null) 
                graph[edge.start] = new GraphVertex();
            graph[edge.start].neighbours.Add(edge.end);

            if ( graph[edge.end] == null )
                graph[edge.end] = new GraphVertex();
            graph[edge.end].neighbours.Add(edge.start);
        }
        

        static void ReadInput(out GraphVertex[] graph, out Edge[] edges, out int[] removeEdges)
        {
            TextReader stream;
            if ( File.Exists(FILENAME) )
            {
                var fileStream = File.OpenRead(FILENAME);
                stream = new StreamReader(fileStream);
            }
            else
                stream = Console.In;

            var nc = stream.ReadLine().Split(' ');
            int n = int.Parse(nc[0]);
            int c = int.Parse(nc[1]);
            graph = new GraphVertex[n + 1];
            edges = new Edge[c + 1];

            for ( int i = 1; i <= c; i++ )
            {
                var edgeInput = stream.ReadLine().Split(' ');
                edges[i] = new Edge() { start = int.Parse(edgeInput[0]), end = int.Parse(edgeInput[1]) };

                AddEdge(ref graph, edges[i]);
            }

            int m = int.Parse(stream.ReadLine());
            var removeInput = stream.ReadLine().Split(' ');
            removeEdges = new int[m];
            for ( int i = 0; i < m; i++ )
            {
                removeEdges[i] = int.Parse(removeInput[i]);
                RemoveEdge(ref graph, edges[removeEdges[i]]);
            }

        }

        static int Calculate(GraphVertex[] graph)
        {
            bool[] visited = new bool[graph.Length + 1];
            int result = 0;
            for ( int i = 1;i < graph.Length;i++ )
            {
                if ( visited[i] == false )
                {
                    ComponentIndex ci = new ComponentIndex() { index = result };
                    dfs(graph, i, ref visited, ref ci);
                    result++;
                }
            }
            return result - 1;
        }

        static void dfs(GraphVertex[] graph, int i, ref bool[] visited, ref ComponentIndex ci)
        {
            visited[i] = true;
            if ( graph[i] == null )
                graph[i] = new GraphVertex();
            graph[i].componentIndex = ci;
            graph[i].componentIndex.verts.Add(i);

            foreach ( int j in graph[i].neighbours )
                if ( !visited[j] )
                    dfs(graph, j, ref visited, ref ci);
        }
    }

    public class ComponentIndex
    {
        public int index;
        public List<int> verts = new List<int>();
    }

    public class GraphVertex
    {
        public ComponentIndex componentIndex;
        public List<int> neighbours = new List<int>();
    }
    public struct Edge
    {
        public int start;   //vertex
        public int end;     //vertex
    }
}
