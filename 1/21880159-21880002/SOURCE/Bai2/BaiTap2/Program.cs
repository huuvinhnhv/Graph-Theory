using System;
using System.IO;

namespace BaiTap2
{
    class AdjacencyMatrix
    {
        public int n;
        public int[,] a;

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
            a = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                string[] tokens = lines[i + 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = Int32.Parse(tokens[j]);
                }
            }
            return true;
        }

        //In ra ma trận kề
        public void ShowAdjacencyMatrix()
        {
            Console.WriteLine(KiemTraDoThiDayDu(this));
            Console.WriteLine(KiemTraDoThiChinhQuy(this));
            Console.WriteLine(KiemTraDoThiVong(this));
        }


        //Xác định bậc của đỉnh đồ thị vô hướng
        private int[] CountDegrees(AdjacencyMatrix g)
        {
            int[] degrees = new int[g.n]; // Mảng chứa bậc của các đỉnh
            for (int i = 0; i < g.n; i++)
            {
                int count = 0;
                for (int j = 0; j < g.n; j++)
                    if (g.a[i, j] != 0)
                    {
                        count += g.a[i, j];
                        if (i == j) // xet truong hop canh khuyen
                            count += g.a[i, i];
                    }
                degrees[i] = count;
            }
            return degrees;
        }

        private string KiemTraDoThiDayDu(AdjacencyMatrix g)
        {
            bool check = true;
            int[] degrees = CountDegrees(g);
            for (int i = 0; i < g.n - 1; i++)
            {
                if (degrees[i] != degrees[i + 1])
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                if (degrees[0] == g.n - 1)
                {
                    return $"Day la do thi day du K{g.n}";
                }
            }
            return "Day khong phai la do thi day du";
        }
        //Kiem tra do thi ching quy
        private string KiemTraDoThiChinhQuy(AdjacencyMatrix g)
        {
            bool check = true;
            int[] degrees = CountDegrees(g);
            for (int i = 0; i < g.n - 1; i++)
            {
                if (degrees[i] != degrees[i + 1])
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                return $"Day la do thi {degrees[0]}-chinh quy";
            }
            return "Day khong phai la do thi chinh quy";
        }
        //Kiem tra do thi vong
        private string KiemTraDoThiVong(AdjacencyMatrix g)
        {
            int[] degrees = CountDegrees(g);
            if (degrees[0] != 2)
            {
                return "Day khong phai la do thi vong";
            }

            if (g.a[0, g.n - 1] != g.a[0, 1])
            {
                return "Day khong phai la do thi vong";
            }
            if (g.a[g.n - 1, g.n - 2] != g.a[g.n - 1, 0])
            {
                return "Day khong phai la do thi vong";
            }
            for (int i = 1; i < g.n - 1; i++)
            {
                if (g.a[i, i + 1] != g.a[i, i - 1])
                {
                    return "Day khong phai la do thi vong";
                }
            }
            return $"Day la do thi vong C{g.n}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\HUS\LyThuyetDoThi\BT\1\21880159-21880002\SOURCE\Bai2\input.txt";
            AdjacencyMatrix g = new AdjacencyMatrix();
            g.ReadAdjacencyMatrix(path);
            g.ShowAdjacencyMatrix();
        }
    }
}
