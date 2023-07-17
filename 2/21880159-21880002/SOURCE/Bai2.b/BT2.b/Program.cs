using System;
using System.Collections.Generic;
using System.IO;

namespace BT2.b
{
    class AdjacencyMatrix
    {
        public int n;
        public int[,] a;
        public List<int> verticesVisited_Copy;
        public List<int> verticesVisited_BFS = new();
        public List<int> paths;
        public Queue<int> queue = new();
        public int start;
        public int goal;
        List<List<int>> AdjList = new();


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
            BFS(this);
            //RemoveOnlyInVertices();
            //FindShrotPath_BFS(this);
            CheckConnection();
            Console.WriteLine("Danh sach dinh da duyen theo thu tu:");
            foreach (var item in verticesVisited_BFS)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            if (verticesVisited_BFS.Contains(goal) == false)
            {
                Console.WriteLine("Khong co duong di");
            }
            else
            {
                Console.WriteLine("Duong di kieu nghich:");
                foreach (var item in verticesVisited_Copy)
                {
                    if (item == start)
                    {
                        Console.Write(item);
                        break;
                    }
                    Console.Write(item + " <- ");
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

        public void BFS(AdjacencyMatrix g)
        {
            int start = g.start;
            bool[] visited = new bool[n];
            visited[start] = true;
            queue.Enqueue(start);
            while (queue.Count != 0)
            {

                start = queue.Dequeue();
                g.verticesVisited_BFS.Add(start);
                if (start == goal)
                {
                    break;
                }
                for (int i = 0; i < n; i++)
                {
                    if (g.a[start, i] == 1 && !visited[i])
                    {
                        visited[i] = true;
                        queue.Enqueue(i);
                    }
                }
            }
            verticesVisited_Copy = new(verticesVisited_BFS);
            verticesVisited_Copy.Reverse();
        }

        //Tao danh sach ma moi phan tu cua danh sach chua cac cach ke tuong ung
        public void CreatAdjList()
        {
            for (int i = 0; i < n; i++)
            {
                List<int> temp = new();
                for (int j = 0; j < n; j++)
                {

                    if (a[j, i] == 1)
                    {
                        temp.Add(j);
                    }
                }
                AdjList.Add(temp);
            }
        }
        //Kiem tra ket noi giua cac dinh, neu dinh truoc ko ket noi dinh sau -> bo dinh sau
        public void CheckConnection()
        {
            CreatAdjList();
            //paths = new(verticesVisited_Copy);
            for (int i = 0; i < verticesVisited_Copy.Count - 1; i++)
            {
                while (!AdjList[verticesVisited_Copy[i]].Contains(verticesVisited_Copy[i + 1]))
                {
                    verticesVisited_Copy.Remove(verticesVisited_Copy[i + 1]);
                }
            }
        }
        ////remove vertex has only in Degree on verticesVisited_BFS
        //public void RemoveOnlyInVertices()
        //{
        //    verticesVisited_Copy = new(verticesVisited_BFS);
        //    int dem = 0;
        //    for (int i = 0; i < verticesVisited_Copy.Count; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            if (a[verticesVisited_Copy[i], j] == 1)
        //            {
        //                dem++;
        //            }
        //        }
        //        if (dem == 0)
        //        {
        //            verticesVisited_Copy.Remove(verticesVisited_Copy[i]);
        //        }
        //        dem = 0;
        //    }
        //    RemoveCircle();
        //}
        //public void RemoveCircle()
        //{
        //    for (int i = 0; i < verticesVisited_Copy.Count; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {

        //            if (a[verticesVisited_Copy[i], j] == 1)
        //            {

        //                if (verticesVisited_Copy.Contains(j) && verticesVisited_Copy.IndexOf(j) < i)
        //                {
        //                    verticesVisited_Copy.Remove(verticesVisited_Copy[i]);
        //                }

        //            }
        //        }
        //    }
        //}
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\HUS\LyThuyetDoThi\BT\2\BT2.b\input.txt";
            AdjacencyMatrix g = new AdjacencyMatrix();
            //Doc thong tin do thi tu file
            g.ReadAdjacencyMatrix(path);

            //In thong tin cua do thi
            //g.ShowGraphInfo();

            //In ra ket qua
            g.ShowResults();

        }
    }
}
