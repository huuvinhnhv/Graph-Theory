using System;
using System.IO;

namespace BaiTap1
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
            Console.WriteLine(n);
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
            Console.WriteLine($"So dinh cua do thi: {DemSoDinh(this)}");
            Console.WriteLine($"So canh cua do thi: {DemSoCanh(this)}");
            Console.WriteLine($"So cap dinh xuat hien canh boi: {DemSoDinhCanhBoi(this)}");
            Console.WriteLine($"So canh khuyen: {DemSoCanhKhuyen(this)}");
            Console.WriteLine($"So dinh treo: {DemSoDinhTreo(this, CountDegrees(this))}");
            Console.WriteLine($"So dinh co lap: {CountIsolatedVertices(this, CountDegrees(this))}");
            if (!check)
            {
                int[,] result = DemSoBacDinhDoThiCoHuong(this);
                Console.WriteLine("(Bac vao - Bac ra) cua tung dinh:");
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"{i}(");
                    for (int j = 0; j < 2; j++)
                    {
                        Console.Write($"{result[j, i]}");
                        if (j == 1)
                        {
                            continue;
                        }
                        Console.Write("-");
                    }
                    Console.Write(") ");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Bac cua tung dinh:");
                int[] degrees = CountDegrees(this);
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"{i}({degrees[i]}) ");
                }
                Console.WriteLine();
            }
            Console.WriteLine(XacDinhLoaiDoThi(this));
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

        //Kiểm tra cạnh khuyên
        private bool IsGraphHasNoLoops(AdjacencyMatrix g)
        {
            // kiểm tra các giá trị a[0][0], a[1][1], … xem có giá
            // trị khác 0 hay không, nếu có thì đồ thị có khuyên
            for (int i = 0; i < g.n && g.a[i, i] == 0; i++)
                if (i < g.n)
                    return false;
            return true;
        }
        //Đếm số cạnh khuyên
        private int DemSoCanhKhuyen(AdjacencyMatrix g)
        {
            int count = 0;
            for (int i = 0; i < g.n; i++)
                if (g.a[i, i] > 0)
                {
                    count++;
                }
            return count;
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

        //Xác định bậc của đỉnh đồ thị có hướng
        private int[,] DemSoBacDinhDoThiCoHuong(AdjacencyMatrix g)
        {
            int[] dOut = new int[n];
            int[] dIn = new int[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    dOut[i] += a[i, j];
                    dIn[i] += a[j, i];
                }
            }

            int[,] result = new int[2, n];
            for (int i = 0; i < n; i++)
            {
                result[0, i] = dIn[i];
                result[1, i] = dOut[i];
            }
            return result;
        }

        //Đếm số lượng đỉnh cô lập do thi vo huong
        private int CountIsolatedVertices(AdjacencyMatrix g, int[] degrees)
        {
            // đếm số giá trị degrees[0], degrees[1], … bằng 0
            int count = 0;
            for (int i = 0; i < g.n; i++)
                if (degrees[i] == 0)
                    count = count + 1;
            return count;
        }

        //DemSoDinhTreo
        private int DemSoDinhTreo(AdjacencyMatrix g, int[] degrees)
        {
            int count = 0;
            for (int i = 0; i < g.n; i++)
                if (degrees[i] == 1)
                    count = count + 1;
            return count;
        }

        //Đếm số đỉnh của đồ thị
        private int DemSoDinh(AdjacencyMatrix g)
        {
            return g.n;
        }

        //Đếm số cạnh của đồ thị
        private int DemSoCanh(AdjacencyMatrix g)
        {
            int count = 0;
            int count1 = 0;
            for (int i = 0; i < g.n; i++)
            {
                for (int j = 0; j < g.n; j++)
                {
                    count += g.a[i, j];
                    count1 += g.a[i, j];
                }
                if (g.a[i, i] > 0)
                {
                    count1 = count1 - g.a[i, i];
                    count1 = count1 + g.a[i, i] * 2;
                }
            }
            //Đồ thị vô hướng thì có số cạnh bằng count/2
            if (g.IsUndirectedGraph(g))
            {
                return count1 / 2;
            }
            return count;
        }
        //Đếm số cặp đỉnh xuất hiện cạnh bội
        private int DemSoDinhCanhBoi(AdjacencyMatrix g)
        {
            //Đếm a[i,j] có giá trị >0 lưu vào count
            int count = 0;

            for (int i = 0; i < g.n; i++)
            {
                for (int j = 0; j < g.n; j++)
                {
                    if (g.a[i, j] > 1)
                    {
                        count++;
                    }
                }
            }
            if (IsUndirectedGraph(this))
            {
                return count / 2;
            }
            return 0;
        }

        //Xác định loại đồ thị
        private string XacDinhLoaiDoThi(AdjacencyMatrix g)
        {
            bool kiemTraCanhKhuyen = g.IsGraphHasNoLoops(g);
            bool kiemTraDoThiVoHuong = g.IsUndirectedGraph(g);
            int soDinhCanhBoi = g.DemSoDinhCanhBoi(g);
            if (kiemTraDoThiVoHuong)
            {
                if (kiemTraCanhKhuyen)
                {
                    return "Gia Do Thi";
                }
                if (soDinhCanhBoi > 0)
                {
                    return "Da Do Thi";
                }
            }
            else
            {
                if (soDinhCanhBoi > 0)
                {
                    return "Da Do Thi Co Huong";
                }
                return "Do Thi Co Huong";
            }
            return "Don Do Thi";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\HUS\LyThuyetDoThi\BT\1\21880159-21880002\SOURCE\Bai1\input.txt";
            AdjacencyMatrix g = new AdjacencyMatrix();
            g.ReadAdjacencyMatrix(path);
            g.ShowAdjacencyMatrix();
        }
    }
}
