using static Lr3MauiHemming.MyVector;

namespace Lr3MauiHemming
{
    internal class Matrix
    {
        private MyVector[] _values;
        public int Size => _values.Length;

        public MyVector this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }
        public Matrix(int size) { _values = new MyVector[size]; }
        public Matrix(params MyVector[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            _values = values;
        }
        public static Matrix GenMatrix(int a, int b)
        {
            MyVector[] GenM = new MyVector[a];
            for (int i = 0; i < a; i++)
                GenM[i] = GenVector(b);
            return new Matrix(GenM);
        }
        public static Matrix operator *(Matrix A, Matrix B)
        {
            Matrix C = GenMatrix(A.Size, B[0].Size);
            for (int i = 0; i < A.Size; i++)
            {
                for (int j = 0; j < B[0].Size; j++)
                {
                    for (int k = 0; k < B.Size; k++)
                    {
                        C[i][j] += A[i][k] * B[k][j];
                    }

                    C[i][j] = C[i][j] % 2;
                }
            }
            return C;
        }
        public static Matrix T(Matrix C)
        {
            Matrix Result = GenMatrix(C[0].Size, C.Size);
            for (int i = 0; i < C.Size; i++)
            {
                for (int j = 0; j < C[0].Size; j++)
                    Result[j][i] = C[i][j];
            }
            return Result;
        }
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _values.Length; i++)
                s += _values[i].ToString();
            return $"{s}";
        }
        public string PrintMatrix()
        {
            string s = "";
            for (int i = 0; i < _values.Length; i++)
            {
                if (i == _values.Length - 1)
                {
                    s += _values[i].PrintVector();
                }
                else
                {
                    s += _values[i].PrintVector() + "\n";
                }
                
            }
                
            return $"{s}";
        }
    }
}
