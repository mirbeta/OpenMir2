namespace ConsoleApp1
{
    public static class HighPerfArrayExt
    {
        #region Sum

        public static int Sum(this HighPerfArray<int> data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data.Memory.Span[i];
            }
            return sum;
        }

        public static float Sum(this HighPerfArray<float> data)
        {
            float sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data.Memory.Span[i];
            }
            return sum;
        }

        public static double Sum(this HighPerfArray<double> data)
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data.Memory.Span[i];
            }
            return sum;
        }

        #endregion

        #region Max

        public static int Max(this HighPerfArray<int> data)
        {
            int max = data.Memory.Span[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (max < data.Memory.Span[i])
                    max = data.Memory.Span[i];
            }
            return max;
        }

        public static float Max(this HighPerfArray<float> data)
        {
            float max = data.Memory.Span[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (max < data.Memory.Span[i])
                    max = data.Memory.Span[i];
            }
            return max;
        }

        public static double Max(this HighPerfArray<double> data)
        {
            double max = data.Memory.Span[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (max < data.Memory.Span[i])
                    max = data.Memory.Span[i];
            }
            return max;
        }

        #endregion

        #region Min

        public static int Min(this HighPerfArray<int> data)
        {
            int min = data.Memory.Span[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (min > data.Memory.Span[i])
                    min = data.Memory.Span[i];
            }
            return min;
        }

        public static float Min(this HighPerfArray<float> data)
        {
            float min = data.Memory.Span[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (min > data.Memory.Span[i])
                    min = data.Memory.Span[i];
            }
            return min;
        }

        public static double Min(this HighPerfArray<double> data)
        {
            double min = data.Memory.Span[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (min > data.Memory.Span[i])
                    min = data.Memory.Span[i];
            }
            return min;
        }

        #endregion

        #region Average

        public static int Average(this HighPerfArray<int> data)
        {
            return data.Sum() / data.Length;
        }

        public static float Average(this HighPerfArray<float> data)
        {
            return data.Sum() / data.Length;
        }

        public static double Average(this HighPerfArray<double> data)
        {
            return data.Sum() / data.Length;
        }

        #endregion

    }
}