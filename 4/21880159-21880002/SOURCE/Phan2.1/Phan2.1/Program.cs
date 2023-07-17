using System;
using System.IO;

namespace Phan2._1
{
    class Program
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
            public static void dijkstra(AdjacencyMatrix g, int source)
            {
                int n = g.getN();
                int[,] a = g.getA();

                int[] prev = new int[n];
                //danh sách các đỉnh đã duyệt và danh sách khoảng cách ban đầu
                bool[] visitedVertex = new bool[n];
                int[] distance = new int[n];
                //khỏi tạo 2 danh sách trên
                //ban đầu chưa có đỉnh nào được duyệt và khoảng cách là vô cùng
                for (int i = 0; i < n; i++)
                {
                    prev[i] = -1;
                    visitedVertex[i] = false;
                    distance[i] = Int16.MaxValue;
                }
                //khoảng cách đỉnh bắt đầu bằng 0;
                distance[source] = 0;
                //duyệt qua tất cả các đỉnh
                for (int i = 0; i < n; i++)
                {
                    //tìm đỉnh có độ đường đi ngắn nhất trong lần duyền hiệt tại
                    int k = findVertex(distance, visitedVertex);
                    visitedVertex[k] = true;
                    //cập nhật lại khoảng cách tất cả các đỉnh
                    for (int j = 0; j < n; j++)
                    {
                        //khoảng cách đường đi mới mà nhỏ hơn khoảng cách cũ thì cập nhập
                        if (!visitedVertex[j] && a[k, j] != 0 && (distance[k] + a[k, j] < distance[j]))
                        {
                            distance[j] = distance[k] + a[k, j];
                            prev[k] = j;
                        }
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"Khaong cah tu {source} den cac dinh la:");
                    Console.WriteLine($"{i}: {distance[i]}");
                }
            }
            public static void dijkstra2(AdjacencyMatrix g, int source, int destination)
            {
                int n = g.getN();
                int[,] a = g.getA();

                int[] prev = new int[Int16.MaxValue];
                //danh sách các đỉnh đã duyệt và danh sách khoảng cách ban đầu
                bool[] visitedVertex = new bool[n];
                int[] distance = new int[n];
                //khỏi tạo 2 danh sách trên
                //ban đầu chưa có đỉnh nào được duyệt và khoảng cách là vô cùng
                for (int i = 0; i < n; i++)
                {
                    visitedVertex[i] = false;
                    distance[i] = Int16.MaxValue;
                }
                prev[source] = source;
                //khoảng cách đỉnh bắt đầu bằng 0;
                distance[source] = 0;
                //duyệt qua tất cả các đỉnh
                for (int i = 0; i < n; i++)
                {
                    //tìm đỉnh có độ đường đi ngắn nhất trong lần duyền hiệt tại
                    int k = findVertex(distance, visitedVertex);
                    visitedVertex[k] = true;
                    //cập nhật lại khoảng cách tất cả các đỉnh
                    for (int j = 0; j < n; j++)
                    {
                        //khoảng cách đường đi mới mà nhỏ hơn khoảng cách cũ thì cập nhập
                        if (!visitedVertex[j] && a[k, j] != 0 && (distance[k] + a[k, j] < distance[j]))
                        {
                            distance[j] = distance[k] + a[k, j];
                            prev[k] = j;
                        }
                    }
                    if (visitedVertex[destination])
                    {
                        break;
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"Khaong cah tu {source} den cac dinh la:");
                    Console.WriteLine($"{i}: {distance[i]}");
                }

            }
            private static int findVertex(int[] distance, bool[] visitedVertex)
            {
                int minDistance = Int16.MaxValue;//lưu khoảng cách
                int minDistanceVertex = -1;//lưu đỉnh chứa khoảng cách đó
                //tìm ra khoảng cách nhỏ nhất trong số các đỉnh đã được duyệt
                for (int i = 0; i < distance.Length; i++)
                {
                    if (!visitedVertex[i] && distance[i] < minDistance)
                    {
                        minDistance = distance[i];
                        minDistanceVertex = i;
                    }
                }
                return minDistanceVertex;
            }
        }
        static void Main(string[] args)
        {
            string path = @"D:\HUS\LyThuyetDoThi\BT\4\21880159-21880002\SOURCE\Phan2.1\Phan2.1\input.txt";
            AdjacencyMatrix g = new AdjacencyMatrix();
            g.ReadAdjacencyMatrix(path);
            Dijkstra.dijkstra(g, 0);
        }
    }
}
