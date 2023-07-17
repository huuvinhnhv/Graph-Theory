using System;
using System.Collections.Generic;
using System.IO;

namespace BT2.a
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
            Console.WriteLine("Cau 2a: ");
            Console.WriteLine("Danh sach dinh da duyen theo thu tu:");
            foreach (var item in verticesVisited_DFS)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            if (verticesVisited_DFS.Contains(goal) == false)
            {
                Console.WriteLine("Khong co duong di");
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
            //kiem tra do thi co huong
            bool check = IsUndirectedGraph(this);
            if (check)
            {
                Console.WriteLine("\n-----------------------------");
                Console.WriteLine("Cau 2c: ");
                List<List<int>> ThanhPhanLienThong = LienThong();
                Console.WriteLine("So thanh phan lien thong: " + ThanhPhanLienThong.Count);
                for (int i = 0; i < ThanhPhanLienThong.Count; i++)
                {
                    Console.Write("Thanh phan lien thong thu " + (i + 1) + ": ");
                    foreach (var item in ThanhPhanLienThong[i])
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
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
        //DFS dung de quy
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
        //Tim thanh phan lien thong
        public List<List<int>> LienThong()
        {
            List<List<int>> result = new();
            List<int> temp;
            temp = DFS(0);
            temp.Sort();
            result.Add(temp);
            int dem = temp.Count;
            for (int i = 1; i < n; i++)
            {
                List<int> temp2 = new();

                if (!temp.Contains(i))
                {
                    temp2 = DFS(i);
                    temp2.Sort();
                    dem += temp2.Count;
                    result.Add(temp2);
                    if (dem == n)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        //DFS ko dung de quy
        public List<int> DFS(int s)
        {
            List<int> list = new();
            bool[] visited = new bool[n];
            Stack<int> stack = new();
            stack.Push(s);
            visited[s] = true;
            list.Add(s);
            while (stack.Count > 0)
            {
                s = stack.Peek();
                stack.Pop();
                if (visited[s] == false)
                {
                    list.Add(s);
                    visited[s] = true;
                }
                for (int i = n - 1; i > 0; i--)
                {
                    if (a[s, i] == 1 && !visited[i])
                    {
                        stack.Push(i);
                    }
                }
            }
            return list;
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
            string path = @"D:\HUS\HK1\LyThuyetDoThi\BT\2\Bai2.a-c\input.txt";
            AdjacencyMatrix g = new AdjacencyMatrix();
            //Doc thong tin do thi tu file
            g.ReadAdjacencyMatrix(path);

            //In thong tin cua do thi
            //g.ShowGraphInfo();

            //In ra ket qua
            g.ShowResults();
            Console.ReadLine();

        }
    }
}
