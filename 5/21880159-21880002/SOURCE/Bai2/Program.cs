using System;
using System.Collections.Generic;
using System.IO;

namespace Bai2
{
    public class AdjacencyMatrix
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
    public class Dijkstra
    {
        //vo cuc
        const int INF = 99999;

        public void dijkstra(int[,] a)
        {
            //check trong so am
            Console.WriteLine("Thuat toan Dijkstra");
            if (checkPositiveCost(a))
            {
                Console.WriteLine("Do thi co trong so am!");
                return;
            }

            //khoi tao bien 
            int count = a.GetLength(0);
            int src;
            int des;
            do
            {
                Console.Write("Nhap dinh bat dau: ");
                src = int.Parse(Console.ReadLine());
                if (src >= 0 && src < count)
                {
                    Console.Write("Nhap dinh ket thuc: ");
                    des = int.Parse(Console.ReadLine());
                    if (des >= 0 && des < count)
                    {
                        break;
                    }
                }
            } while (true);

            bool[] visitedVertex = new bool[count];
            int[] dist = new int[count];
            int[] prev = new int[count];

            //khoi tao
            for (int i = 0; i < count; i++)
            {
                prev[i] = -1;
                visitedVertex[i] = false;
                dist[i] = INF;
            }

            prev[src] = src;
            dist[src] = 0;

            for (int i = 0; i < count; i++)
            {
                int u = findMinDistance(dist, visitedVertex);
                if (u == -1)
                {
                    Console.WriteLine($"Khong co duong di tu {src} den {des}.");
                    return;
                }
                Console.WriteLine(u);
                //thoat khi da duyet den dinh des
                if (u == des)
                {
                    break;
                }
                visitedVertex[u] = true;

                //cap nhat dis va prev
                for (int v = 0; v < count; v++)
                {

                    if (!visitedVertex[v] && a[u, v] != 0 && dist[u] + a[u, v] < dist[v])
                    {
                        dist[v] = dist[u] + a[u, v];
                        prev[v] = u;

                    }
                }
            }
            //in ra ket qua
            //chi phi
            int cost = dist[des];
            //tao List path
            List<int> path = new List<int>();
            do
            {
                path.Add(des);
                des = prev[des];
            } while (src != des);

            //them src vao path
            path.Add(src);
            //dao nguoc path
            path.Reverse();
            //in path
            Console.WriteLine("Duong di ngan nhat: ");
            for (int i = 0; i < path.Count - 1; i++)
            {
                Console.Write(path[i] + " -> ");
            }
            Console.WriteLine(path[path.Count - 1]);
            Console.WriteLine($"Chi phi duong di ngan nhat: {cost}");
        }
        private int findMinDistance(int[] dist, bool[] visitedVertex)
        {
            int minDist = INF;
            int minDistVertex = -1;
            for (int i = 0; i < dist.Length; i++)
            {
                if (!visitedVertex[i] && dist[i] < minDist)
                {
                    minDist = dist[i];
                    minDistVertex = i;
                }
            }
            return minDistVertex;
        }
        private bool checkPositiveCost(int[,] a)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                {
                    if (a[i, j] < 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    public class BellmanFord
    {
        const int INF = 99999;
        public void bellmanFord(int[,] a, int src)
        {
            Console.WriteLine("Thuat toan Bellman-Ford");
            int n = a.GetLength(0);
            int[] minCost = new int[n];
            int[] prev = new int[n];
            //khoi tao
            for (int i = 0; i < n; i++)
            {
                minCost[i] = INF;
                prev[i] = -1;
            }
            minCost[src] = 0;

            //cap nhat moi canh n-1 lan
            for (int i = 1; i < n - 1; i++)
            {
                for (int v = 0; v < n; v++)
                {
                    List<int> V = findConnection(a, v);
                    foreach (var x in V)
                    {
                        if (minCost[x] > minCost[v] + a[v, x])
                        {
                            minCost[x] = minCost[v] + a[v, x];
                            prev[x] = v;
                        }
                    }
                }
                //Console.WriteLine("Buoc " + i);
                //Console.WriteLine("Min Cost " + i);
                for (int k = 0; k < minCost.Length; k++)
                {
                    if (minCost[k] == 99999)
                    {
                        Console.Write("INF" + " ");
                    }
                    else
                    {
                        Console.Write(minCost[k] + " ");
                    }
                }
                Console.WriteLine();
                // Console.WriteLine("Prev " + i);
                for (int l = 0; l < prev.Length; l++)
                {
                    Console.Write(prev[l] + " ");
                }
                Console.WriteLine();
                //kiem tra chu trinh am
            }

            bool check = false;
            for (int i = 0; i < n; i++)
            {
                List<int> V = findConnection(a, i);
                foreach (var x in V)
                {
                    if (minCost[x] > minCost[i] + a[i, x])
                    {
                        check = true;
                    }
                }
            }
            //xuat ket qua
            if (check)
            {
                Console.WriteLine("Do thi co mach am.");
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (minCost[i] == INF)
                    {
                        Console.WriteLine($"Duong di ngan nhat tu {src} den {i}: Khong co duong di");
                    }
                    else if (i != src)
                    {
                        Console.Write($"Duong di ngan nhat tu {src} den {i}: ");
                        List<int> path = new List<int>();
                        int des = i;
                        do
                        {
                            path.Add(des);
                            des = prev[des];
                        } while (src != des);
                        path.Add(src);
                        path.Reverse();
                        for (int j = 0; j < path.Count; j++)
                        {
                            if (j == path.Count - 1)
                            {
                                Console.Write(path[j]);
                                break;
                            }
                            Console.Write(path[j] + " -> ");
                        }
                        Console.WriteLine();
                        Console.WriteLine("Chi phi duong di ngan nhat: " + minCost[i]);
                    }
                }
            }
        }
        //Tim dinh ket noi voi v
        private List<int> findConnection(int[,] a, int v)
        {
            //tap dinh
            List<int> V = new List<int>();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                if (a[v, i] != 0)
                {
                    V.Add(i);
                }
            }
            return V;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\HUS\LyThuyetDoThi\BT\5\21880159-21880002\SOURCE\Bai2\input.txt";
            AdjacencyMatrix g = new AdjacencyMatrix();
            g.ReadAdjacencyMatrix(path);
            //Dijkstra T = new Dijkstra();
            //T.dijkstra(g.getA());
            BellmanFord G = new BellmanFord();
            G.bellmanFord(g.getA(), 0);
        }
    }
}
