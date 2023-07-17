using System;
using System.Collections.Generic;
using System.IO;

namespace BT2.a
{
    class AdjacencyMatrix
    {
        private int n;
        private int[,] a;
        public int getN()
        {
            return n;
        }
        public int[,] getA()
        {
            return a;
        }
        //Đọc dữ liệu từ file
        public void ReadAdjacencyMatrix(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("This file does not exist.");
                Environment.Exit(1);
            }

            string[] lines = File.ReadAllLines(filename);
            n = Int32.Parse(lines[0]);

            string[] str = lines[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            a = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                string[] tokens = lines[i + 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = Int32.Parse(tokens[j]);
                }
            }
        }

        public void ShowAdjacencyMatrix()
        {
            Console.WriteLine($"n = {n}");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(a[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }

    class EDGE
    {
        //ham khoi tao 3 tham so
        public EDGE(int s, int e, int weight)
        {
            S = s;
            E = e;
            Weight = weight;
        }
        //Khoi tao ko tham so
        public EDGE()
        {
        }

        public int S
        {
            get; set;
        }
        public int E
        {
            get; set;
        }
        public int Weight
        {
            get; set;
        }
    }
    class Tree
    {
        public static EDGE[] Prim(AdjacencyMatrix g, int source)
        {
            //get g info
            int n = g.getN();
            int[,] a = g.getA();
            //bien can thiet
            EDGE[] T = new EDGE[n - 1];
            int nT = 0;
            bool[] marked = new bool[n];
            for (int i = 0; i < marked.Length; i++)
            {
                marked[i] = false;
            }
            marked[source] = true;
            while (nT < n - 1)
            {

                EDGE edgeMin = new EDGE();
                int nMinWeight = -1;
                for (int i = 0; i < n; i++)
                {
                    if (marked[i] == true)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            //chỉ chọn cạnh có trọng số để so sánh với minWeight
                            if (!marked[j] && a[i, j] != 0)
                            {
                                if (nMinWeight < 0 || nMinWeight > a[i, j])
                                {
                                    nMinWeight = a[i, j];
                                    edgeMin.S = i;
                                    edgeMin.E = j;
                                    edgeMin.Weight = nMinWeight;
                                }
                            }
                        }

                    }
                }
                T[nT++] = edgeMin;
                marked[edgeMin.E] = true;
            }
            return T;
        }


        //--------------Kruskal-----------------//

        public static EDGE[] Kruskal(AdjacencyMatrix g)
        {
            //get g info
            int n = g.getN();
            int[,] a = g.getA();
            //bien can thiet
            List<EDGE> T = new List<EDGE>();
            int nT = T.Count;
            //khởi tạo list cạnh
            EDGE[] temp = InitListEdges(g);

            //sắp xếp tăng dần theo trọng số
            SortListEdge(temp);

            //Collection
            List<EDGE> Edges = new List<EDGE>();
            for (int i = 0; i < temp.Length; i++)
            {
                Edges.Add(temp[i]);
            }

            int[] parent = new int[n];
            //khởi tạo node cha của mỗi đỉnh là chính nó
            for (int i = 0; i < parent.Length; i++)
            {
                parent[i] = i;
            }
            while (nT < n - 1)
            {
                //lay phan tu dau tien cua list
                //co the de Queue
                EDGE edge = Edges[0];
                Edges.Remove(edge);
                //check circle
                int x_set = Find(parent, edge.S);
                int y_set = Find(parent, edge.E);
                if (x_set == y_set)
                {
                    //ko làm gì cả vì tạo thành circle
                }
                else
                {
                    //add vào list T
                    T.Add(edge);
                    nT++;
                    //hội 2 tập lại
                    Union(parent, x_set, y_set);
                }
            }
            //chuyen sang EDGE[]
            EDGE[] E = new EDGE[T.Count];
            for (int i = 0; i < E.Length; i++)
            {
                E[i] = T[i];
            }
            return E;
        }

        //--------Find-Union---------//
        //tìm node cha của đỉnh i
        public static int Find(int[] parent, int vertex)
        {
            if (parent[vertex] != vertex)
            {
                return Find(parent, parent[vertex]);
            }
            //cải tiến nén đường đi
            return vertex;
        }
        //hợp 2 tập đỉnh với nhau
        //nếu 2 node cha của 2 đỉnh i,j bằng nhau thì ko làm gì cả
        //ngược lại gán node cha của a = b
        public static void Union(int[] parent, int i, int j)
        {

            int a = Find(parent, i);
            int b = Find(parent, j);
            int countA = 0;
            int countB = 0;
            for (int x = 0; x < parent.Length; x++)
            {
                if (parent[x] == a)
                {
                    countA++;
                }
                if (parent[x] == b)
                {
                    countB++;
                }
            }
            if (countA < countB)
            {
                parent[a] = b;
            }
            else
            {
                parent[b] = a;
            }
        }
        //đếm số cạnh của đồ thị
        public static int CountEdges(AdjacencyMatrix g)
        {
            int n = g.getN();
            int[,] a = g.getA();
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (a[i, j] > 0)
                    {
                        count++;
                    }
                }
            }
            return count % 2 == 0 ? count / 2 : count / 2 + 1;
        }
        public static EDGE[] InitListEdges(AdjacencyMatrix g)
        {
            int n = g.getN();
            int[,] a = g.getA();
            EDGE[] edges = new EDGE[CountEdges(g)];
            int index = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (a[i, j] > 0)
                    {
                        EDGE edge = new EDGE();
                        edge.S = i;
                        edge.E = j;
                        edge.Weight = a[i, j];
                        edges[index] = edge;
                        index++;
                    }
                }
            }
            return edges;
        }
        public static void SortListEdge(EDGE[] T)
        {
            for (int i = 0; i < T.Length; i++)
            {
                for (int j = i + 1; j < T.Length; j++)
                {
                    if (T[i].Weight > T[j].Weight)
                    {
                        EDGE temp = T[i];
                        T[i] = T[j];
                        T[j] = temp;
                    }
                }
            }
        }
        public static void PrintTree(EDGE[] T)
        {
            int totalWeight = 0;
            Console.WriteLine("Tap canh cua cay khung:");
            foreach (var item in T)
            {
                totalWeight += item.Weight;
                Console.WriteLine($"{item.S}-{item.E}: {item.Weight}");
            }
            Console.WriteLine($"Trong so cua cay khung: {totalWeight}");
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            string path = @"D:\HUS\LyThuyetDoThi\BT\4\21880159-21880002\SOURCE\Bai2.a\input.txt";
            AdjacencyMatrix g = new AdjacencyMatrix();
            g.ReadAdjacencyMatrix(path);
            // mặc định source bằng đỉnh 0
            EDGE[] T = Tree.Prim(g, 0);
            //EDGE[] T1 = Tree.Kruskal(g);
            // Tree.SortListEdge(T);
            Console.WriteLine("Prime: ");
            Tree.PrintTree(T);
            // Console.WriteLine("Kruskal: ");
            //Tree.PrintTree(T1);
        }
    }
}
