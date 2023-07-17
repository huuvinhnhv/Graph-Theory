using System;
using System.Collections.Generic;
using System.IO;

namespace BTTuan02
{
    class AdjacencyMatrix
    {
        public int n;
        public int[,] a;
        public List<int> verticesVisited_DFS = new();
        public List<int> verticesVisited_BFS = new();
        public List<int> paths = new();
        public Queue<int> queue = new();
        public int start;
        public int goal;

        //Đọc dữ liệu từ file
        public bool ReadAdjacencyMatrix(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("This file does not exist.");
                Environment.Exit(1);
            }

            string[] lines = File.ReadAllLines(filename);
            n = Int32.Parse(lines[0]);

            string[] str = lines[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            start = Int32.Parse(str[0]);
            goal = Int32.Parse(str[1]);

            a = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                string[] tokens = lines[i + 2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = Int32.Parse(tokens[j]);
                }
            }
            return true;
        }

        //Kiểm tra đồ thị vô hướng
        private bool IsUndirectedGraph(AdjacencyMatrix g)
        {
            // Ý tưởng: kiểm tra ma trận kề của đồ thị có đối xứng?
            int i, j;
            bool isSymmetric = true;
            for (i = 0; i < g.n && isSymmetric; ++i)
            {
                for (j = i + 1; (j < g.n) && (g.a[i, j] == g.a[j, i]); ++j) ;
                if (j < g.n)
                    isSymmetric = false;
            }
            return isSymmetric;
        }

        public void ShowResults()
        {
            FindShrotPath_DFS(this, start, goal);
            Console.WriteLine("Danh sach dinh da duyen theo thu tu:");
            foreach (var item in verticesVisited_DFS)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            if (paths.Contains(-1))
            {
                Console.WriteLine("Khong co duogn di");
            }
            else
            {


                Console.WriteLine("Duong di kieu thuan:");
                foreach (var item in paths)
                {
                    if (item == goal)
                    {
                        Console.Write(item);
                        break;
                    }
                    Console.Write(item + " -> ");
                }
            }
        }
        public void ShowGraphInfo()
        {
            Console.WriteLine($"n = {n}");
            Console.WriteLine($"start = {start}");
            Console.WriteLine($"goal = {goal}");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(a[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            bool check = IsUndirectedGraph(this);
            if (check)
            {
                Console.WriteLine("Do Thi Vo Huong");
            }
            else
            {
                Console.WriteLine("Do Thi Co Huong");
            }
        }
        public void DFS_Recursive(AdjacencyMatrix g, int start, bool[] visited)
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
        public void BFS(AdjacencyMatrix g, int start)
        {
            bool[] visited = new bool[n];
            g.verticesVisited_BFS.Add(start);
            visited[start] = true;
            queue.Enqueue(start);
            while (queue.Count != 0)
            {
                start = queue.Dequeue();
                g.verticesVisited_BFS.Add(start);
                Console.Write(start + " ");

                for (int i = 0; i < n; i++)
                {
                    if (g.a[start, i] == 1 && !visited[i])
                    {
                        visited[i] = true;
                        queue.Enqueue(i);
                    }
                }
            }

        }
        public List<int> FindShrotPath_BFS(AdjacencyMatrix g)
        {
            BFS(g, start);
            if (start == goal)
            {
                paths.Add(start);
                return paths;
            }
            for (int i = 0; i < verticesVisited_DFS.Count; i++)
            {
                if (a[verticesVisited_BFS[i], goal] == 1)
                {
                    paths.Add(verticesVisited_BFS[i]);
                    break;
                }
                if ((i == verticesVisited_BFS.Count - 1) && a[verticesVisited_BFS[i], goal] != 1)
                {
                    paths.Add(-1);
                    return paths;
                }
                paths.Add(verticesVisited_DFS[i]);
            }
            paths.Add(goal);
            return paths;
        }
        public List<int> FindShrotPath_DFS(AdjacencyMatrix g, int start, int goal)
        {
            DFS_Recursive(g, start, new bool[g.n]);
            if (start == goal)
            {
                paths.Add(start);
                return paths;
            }
            for (int i = 0; i < verticesVisited_DFS.Count; i++)
            {
                if (a[verticesVisited_DFS[i], goal] == 1)
                {
                    paths.Add(verticesVisited_DFS[i]);
                    break;
                }
                if ((i == verticesVisited_DFS.Count - 1) && a[verticesVisited_DFS[i], goal] != 1)
                {
                    paths.Add(-1);
                    return paths;
                }
                paths.Add(verticesVisited_DFS[i]);
            }
            paths.Add(goal);
            return paths;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\HUS\LyThuyetDoThi\BT\2\BTTuan02\input.txt";
            AdjacencyMatrix g = new AdjacencyMatrix();
            //Doc thong tin do thi tu file
            g.ReadAdjacencyMatrix(path);
            g.BFS(g, g.start);
            //In thong tin cua do thi
            //g.ShowGraphInfo();

            //In ra ket qua
            //g.ShowResults();

        }
    }

}
