using static Lr3MauiHemming.MyVector;
using static Lr3MauiHemming.Matrix;

namespace Lr3MauiHemming
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            InitializeMatrix();
        }

        Matrix H = new();
        Matrix G = new();
        Matrix Code = new();
        Matrix CorrectedCode = new();


        void Calculate_Haffman(object sender, EventArgs args)
        {
            if (symbol.Text != string.Empty)
            {
                byte_code.Text = ConvertToHexString(symbol.Text);
                MyVector[] byte4 = new MyVector[symbol.Text.Length * 4];
                dgvprint1(symbol.Text, byte4);
                dgvprint2(symbol.Text, byte4);
                Tb3(symbol.Text, byte4);
            }
            else
            {
                //MessageBox.Show("Данные не введены!");
            }
        }

        private void InitializeMatrix()
        {
            var h1 = new MyVector(1, 1, 0, 1, 1, 0, 0);
            var h2 = new MyVector(1, 0, 1, 1, 0, 1, 0);
            var h3 = new MyVector(0, 1, 1, 1, 0, 0, 1);
            H = new Matrix(h1, h2, h3);

            var g1 = new MyVector(1, 0, 0, 0, 1, 1, 0);
            var g2 = new MyVector(0, 1, 0, 0, 1, 0, 1);
            var g3 = new MyVector(0, 0, 1, 0, 0, 1, 1);
            var g4 = new MyVector(0, 0, 0, 1, 1, 1, 1);
            G = new Matrix(g1, g2, g3, g4);

            G_matrix.Text = "";
            G_matrix.Text += "Матрица G \n\n";
            G_matrix.Text += G.PrintMatrix();
            H_matrix.Text = "";
            H_matrix.Text += "Матрица H \n\n";
            H_matrix.Text += H.PrintMatrix();
            H_matrix.Text += "\n";
        }

        string ConvertToHexString(string s)
        {
            string result = "";

            for (int i = 0; i < s.Length; i++) 
            { 
                result += ($"{to2(Convert.ToString((long)s[i], 2))}"); 
            }

            return result;
        }

        static string to2(string s)
        {
            while (s.Length < 16)
                s = s.Insert(0, "0");
            s = s.Insert(4, " ");
            s = s.Insert(9, " ");
            s = s.Insert(14, " ");
            return s;
        }

        void dgvprint1(string s, MyVector[] byte4)
        {
            int t = 0;
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < 4 * 4; j++)
                {
                    t = (j / 4) + (i * 4);
                    if (j % 4 == 0)
                    {
                        byte4[t] = new MyVector
                            (Convert.ToInt32(to2(Convert.ToString((long)s[i], 2)).Replace(" ", "").Substring(j + 0, 1)),
                             Convert.ToInt32(to2(Convert.ToString((long)s[i], 2)).Replace(" ", "").Substring(j + 1, 1)),
                             Convert.ToInt32(to2(Convert.ToString((long)s[i], 2)).Replace(" ", "").Substring(j + 2, 1)),
                             Convert.ToInt32(to2(Convert.ToString((long)s[i], 2)).Replace(" ", "").Substring(j + 3, 1)));
                    }
                }
            }
        }

        void dgvprint2(string s, MyVector[] byte4)
        {
            Matrix C = new Matrix(byte4) * G;
            Code = GenMatrix(s.Length * 4, 8);

            int t = 0;
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < 7 * 4; j++)
                {
                    t = (j / 7) + (i * 4);
                    Code[t][j % 7] = C[t][j % 7];
                }
                Code[(i * 4) + 0][7] = Sum(C[(i * 4) + 0]);
                Code[(i * 4) + 1][7] = Sum(C[(i * 4) + 1]);
                Code[(i * 4) + 2][7] = Sum(C[(i * 4) + 2]);
                Code[(i * 4) + 3][7] = Sum(C[(i * 4) + 3]);
            }

            coded_message.Text = "";
            coded_message.Text += Code.PrintMatrix();
        }

        void Tb3(string s, MyVector[] byte4)
        {
            Matrix C2 = new Matrix(byte4) * G;
            resulting_matrix.Text = "";
            corrected_code.Text = "";
            int t = 0;

            for (int i = 0; i < s.Length * 4; i++)
            {
                C2[i] = Transfer(C2[i]);
                CorrectedCode = C2;
                int bit_2 = Sum(C2[i]);
                Matrix Temp = new Matrix(C2[i]);
                MyVector sindrom = (Temp * T(H))[0];
                if ((sindrom.ToString() == "000"))
                {
                    resulting_matrix.Text += "Нету ошибок";
                }
                //if (bit_2 == 1 && ((Temp * T(H))[0].ToString() == "000"))
                //{
                //    textBox3.Text += "Ошибка в бите четности";
                //    bit_2 = zero_one(bit_2);
                //}
                if (((Temp * T(H))[0].ToString() != "000"))
                {
                    t = find(H, sindrom);
                    resulting_matrix.Text += $"Ошибка в бите номер {t}";
                    CorrectedCode = C2;
                    CorrectedCode[i][t] = zero_one(C2[i][t]);
                }
                //if (bit_2 == 0 && ((Temp * T(H))[0].ToString() != "000"))
                //{
                //    textBox3.Text += "Более одной ошибки";
                //}
                resulting_matrix.Text += Environment.NewLine;
                resulting_matrix.Text += (Temp * T(H))[0].PrintVector();
                resulting_matrix.Text += Environment.NewLine;
                resulting_matrix.Text += C2[i].PrintVector();
                resulting_matrix.Text += Environment.NewLine;

                
                for (int num = 0; num < 16; num++)
                {
                    if (CorrectedCode[i].PrintVector()[num] != ' ')
                    {
                        corrected_code.Text += CorrectedCode[i].PrintVector()[num];
                    }
                }
                corrected_code.Text += " ";
            }
        }

        int find(Matrix H, MyVector B)
        {
            int math = 0;
            MyVector B2 = new MyVector();
            MyVector H2 = new MyVector();

            for (int i = 0; i < T(H).Size; i++)
            {
                B2 = B;
                H2 = T(H)[i];
                if (H2.ToString() == B2.ToString() && math == 0)
                    math = i;
            }
            return math;
        }

        MyVector Transfer(MyVector A)
        {
            Random r = new Random();
            int t = r.Next(1, 3);
            switch (t)
            {
                case 1:
                    int temp1 = r.Next(0, 7);
                    A[temp1] = zero_one(A[temp1]);
                    break;
                case 2:
                    //int temp2 = r.Next(0, 7);
                    //A[temp2] = zero_one(A[temp2]);
                    //temp2 = r.Next(0, 7);
                    //A[temp2] = zero_one(A[temp2]);
                    break;
                default:
                    break;
            }
            return A;
        }

        int zero_one(double num)
        {
            if (num == 0) return 1;
            else return 0;
        }
    }
}