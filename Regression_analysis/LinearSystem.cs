using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regression_analysis
{
    public class LinearSystem
    {
        int n;//размерность системы
        private double[,] c;//коэффициенты
        private double[] b;//свободные члены
        double[] x;//неизвестные

        public LinearSystem(double[,] initial_c, double[] initial_b)
        {
            n = initial_b.Length;
            c = new double[n, n];
            c = initial_c;
            b = new double[n];
            initial_b.CopyTo(b, 0);
            x = new double[n];
            GaussSolve();
        }

        public double[] xVector
        {
            get
            {
                return x;
            }
        }

        private void GaussSolve()
        {
            GaussForwardStroke();
            GaussBackwardStoke();
        }

        private void GaussForwardStroke()//прямой ход
        {
            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = k + 1; j < n; j++)
                    {
                        c[i, j] = c[i, j] - c[k, j] * (c[i, k] / c[k, k]);
                    }
                    b[i] = b[i] - b[k] * c[i, k] / c[k, k];
                }
            }
        }

        private void GaussBackwardStoke()
        {
            double s = 0;
            for (int k = n - 1; k >= 0; k--)
            {
                s = 0;
                for (int j = k + 1; j < n; j++)
                    s = s + c[k, j] * x[j];
                x[k] = (b[k] - s) / c[k, k];
            }
        }
    }
}
