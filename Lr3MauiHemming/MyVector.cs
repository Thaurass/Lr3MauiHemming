namespace Lr3MauiHemming
{
    internal class MyVector
    {
        private double[] _values;
        public int Size => _values.Length;

        public double this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public MyVector(int size) { _values = new double[size]; }

        public MyVector(params double[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            _values = values;
        }
        public static MyVector GenVector(int a)
        {
            double[] GenV = new double[a];
            for (int i = 0; i < a; i++)
                GenV[i] = 0;
            return new MyVector(GenV);
        }
        public static int Sum(MyVector a)
        {
            int result = 0;
            for (int i = 0; i < a.Size; i++)
                result += (int)a[i];
            return result % 2;
        }
        public override string ToString() { return $"{string.Join(" ", _values)}".Replace(" ", ""); }
        public string PrintVector() { return $"{string.Join("    ", _values)}"; }
    }
}
