using System;
using System.Collections.Generic;
using System.IO;

namespace DoAn1
{
    class Graph
    {
        public int n;
        public List<int>[] adjList;
        public int[,] a;
        public List<int>[] adjListT;
        public int[,] p;
        public List<int> verticesVisited_DFS = new();
        public List<List<int>> SCCs = new();
        public List<int> SCCTemp = new();
        static void Main(string[] args)
        {
            string path = @"D:\HUS\LyThuyetDoThi\BT\3 - DoAn1\21880159-21880002\SOURCE\DoAn1\input.txt";
            Graph g = new Graph();
            g.ReadGraphFromFile(path);
            //g.ShowGraphInfo();
            g.GetSCC();
            g.PrintResult();
        }
        //in ket qua
        //xác định loại liên thông
        public void PrintResult()
        {
            int check = ConnectedClassification();
            switch (check)
            {
                case 0:
                    Console.WriteLine("Do thi lien thong yeu");
                    break;
                case 1:
                    Console.WriteLine("Do thi lien thong manh");
                    break;
                case 2:
                    Console.WriteLine("Do thi khong lien thong");
                    break;
                case 3:
                    Console.WriteLine("Do thi lien thong tung phan");
                    break;
            }
            for (int i = 0; i < SCCs.Count; i++)
            {
                Console.Write($"Thanh phan lien thong manh {i + 1}: ");
                for (int j = 0; j < SCCs[i].Count; j++)
                {
                    if (j == SCCs[i].Count - 1)
                    {
                        Console.Write(SCCs[i][j]);
                    }
                    else
                    {
                        Console.Write(SCCs[i][j] + ", ");
                    }
                }
                Console.WriteLine();
            }

        }

        //Đọc dữ liệu từ file tạo a[,], p[,], adjList
        //đọc danh sách kề từ file txt
        public bool ReadGraphFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("This file does not exist.");
                Environment.Exit(1);
            }

            string[] lines = File.ReadAllLines(filename);
            n = Int32.Parse(lines[0]);

            a = new int[n, n];
            p = new int[n, n];

            adjList = new List<int>[n];
            for (int i = 0; i < adjList.Length; i++)
            {
                adjList[i] = new List<int>();
            }

            //chuyển string[] đọc từ file để tạo ra a, p, adjList
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = 0;
                }
                List<int> temp = new();
                string[] tokens = lines[i + 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 1; j <= int.Parse(tokens[0]); j++)
                {
                    temp.Add(int.Parse(tokens[j]));
                    a[i, int.Parse(tokens[j])] = 1;
                }
                foreach (var item in temp)
                {
                    adjList[i].Add(item);
                }
            }

            //tạo path matrix
            for (int i = 0; i < n; i++)
            {
                bool[] visited = new bool[n];
                DFS_Recursive(this, i, visited);
                foreach (var item in verticesVisited_DFS)
                {
                    p[i, item] = 1;

                }
                verticesVisited_DFS.Clear();
            }
            return true;
        }
        //kiem tra tính liên thông
        //ma tran duong di tat ca phan tu == 1 => lien thong manh
        //ma tran duong di doi xung nhau qua duong cheo => ko lien thong
        //ma tran duong di co tam giac tren hoac tam giac duoi == 1 => loen thong tung phan
        //con lai la lien thong yeu
        public int ConnectedClassification()
        {
            int dem = 0;//lien thong manh
            int dem1 = 0;//khong lien thong
            int dem2 = 0;//lien thong 1 phan
            int dem3 = 0;//lien thong 1 phan
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (p[i, j] == 1)
                    {
                        dem++;
                    }
                    if (p[i, j] == p[j, i])
                    {
                        dem1++;
                    }
                    if (j > i && p[i, j] == 1)
                    {
                        dem2++;
                    }
                    if (j < i && p[i, j] == 1)
                    {
                        dem3++;
                    }
                }
            }
            if (dem == n * n)
            {
                return 1;
            }

            if (dem1 == n * n)
            {
                return 2;
            }
            //Console.WriteLine("Dem 2: " + dem2);
            if (dem3 == (n * n - n) / 2 || dem2 == (n * n - n) / 2)
            {
                return 3;
            }
            return 0;//lien thong yeu
        }
        public void FillStack(int s, bool[] visited, Stack<int> stack)
        {
            visited[s] = true;

            foreach (var i in adjList[s])
            {
                if (!visited[i])
                    FillStack(i, visited, stack);
            }
            stack.Push(s);
        }
        //đảo hướng đồ thị
        public void ReverseGraph()
        {
            adjListT = new List<int>[n];
            for (int i = 0; i < adjListT.Length; i++)
            {
                adjListT[i] = new List<int>();
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < adjList[i].Count; j++)
                {
                    adjListT[adjList[i][j]].Add(i);
                }
            }
            adjList = adjListT;
        }

        //tìm strongly connected components
        public void GetSCC()
        {
            Stack<int> stack = new Stack<int>();

            bool[] visitedVertices = new bool[n];
            for (int i = 0; i < n; i++)
            {
                visitedVertices[i] = false;
            }
            for (int i = 0; i < n; i++)
                if (visitedVertices[i] == false)
                    FillStack(i, visitedVertices, stack);

            ReverseGraph();

            for (int i = 0; i < n; i++)
                visitedVertices[i] = false;

            while (stack.Count != 0)
            {
                int s = stack.Pop();
                if (visitedVertices[s] == false)
                {
                    DFS_SCC(s, visitedVertices);
                    int temp = SCCTemp[0];
                    SCCTemp.RemoveAt(0);
                    SCCTemp.Add(temp);
                    SCCTemp.Reverse();
                    SCCs.Add(SCCTemp);
                    SCCTemp = new();
                }
            }
        }

        //ham DFS duyet danh sach ke adjList
        void DFS_SCC(int s, bool[] visitedVertices)
        {
            visitedVertices[s] = true;
            SCCTemp.Add(s);

            foreach (var item in adjList[s])
            {
                if (!visitedVertices[item])
                {
                    DFS_SCC(item, visitedVertices);
                }
            }
        }

        //ham DFS duyet ma tran ke
        public void DFS_Recursive(Graph g, int start, bool[] visited)
        {
            g.verticesVisited_DFS.Add(start);
            visited[start] = true;
            for (int i = 0; i < n; i++)
            {
                if (g.a[start, i] == 1 && !visited[i])
                {
                    DFS_Recursive(g, i, visited);
                }
            }
        }

        public void ShowGraphInfo()
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
            Console.WriteLine("Path matrix:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(p[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
