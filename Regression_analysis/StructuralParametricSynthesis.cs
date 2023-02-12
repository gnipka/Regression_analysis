using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regression_analysis
{
    class GausMethod
    {
        public uint RowCount;
        public uint ColumCount;
        public double[][] Matrix { get; set; }
        public double[] RightPart { get; set; }
        public double[] Answer { get; set; }

        public GausMethod(uint Row, uint Colum)
        {
            RightPart = new double[Row];
            Answer = new double[Row];
            Matrix = new double[Row][];
            for (int i = 0; i < Row; i++)
                Matrix[i] = new double[Colum];
            RowCount = Row;
            ColumCount = Colum;

            //обнулим массив
            for (int i = 0; i < Row; i++)
            {
                Answer[i] = 0;
                RightPart[i] = 0;
                for (int j = 0; j < Colum; j++)
                    Matrix[i][j] = 0;
            }
        }

        private void SortRows(int SortIndex)
        {

            double MaxElement = Matrix[SortIndex][SortIndex];
            int MaxElementIndex = SortIndex;
            for (int i = SortIndex + 1; i < RowCount; i++)
            {
                if (Matrix[i][SortIndex] > MaxElement)
                {
                    MaxElement = Matrix[i][SortIndex];
                    MaxElementIndex = i;
                }
            }

            //теперь найден максимальный элемент ставим его на верхнее место
            if (MaxElementIndex > SortIndex)//если это не первый элемент
            {
                double Temp;

                Temp = RightPart[MaxElementIndex];
                RightPart[MaxElementIndex] = RightPart[SortIndex];
                RightPart[SortIndex] = Temp;

                for (int i = 0; i < ColumCount; i++)
                {
                    Temp = Matrix[MaxElementIndex][i];
                    Matrix[MaxElementIndex][i] = Matrix[SortIndex][i];
                    Matrix[SortIndex][i] = Temp;
                }
            }
        }

        public int SolveMatrix()
        {
            if (RowCount != ColumCount)
                return 1; //нет решения

            for (int i = 0; i < RowCount - 1; i++)
            {
                SortRows(i);
                for (int j = i + 1; j < RowCount; j++)
                {
                    if (Matrix[i][i] != 0) //если главный элемент не 0, то производим вычисления
                    {
                        double MultElement = Matrix[j][i] / Matrix[i][i];
                        for (int k = i; k < ColumCount; k++)
                            Matrix[j][k] -= Matrix[i][k] * MultElement;
                        RightPart[j] -= RightPart[i] * MultElement;
                    }
                    //для нулевого главного элемента просто пропускаем данный шаг
                }
            }

            //ищем решение
            for (int i = (int)(RowCount - 1); i >= 0; i--)
            {
                Answer[i] = RightPart[i];

                for (int j = (int)(RowCount - 1); j > i; j--)
                    Answer[i] -= Matrix[i][j] * Answer[j];

                if (Matrix[i][i] == 0)
                    if (RightPart[i] == 0)
                        return 2; //множество решений
                    else
                        return 1; //нет решения

                Answer[i] /= Matrix[i][i];
            }
            return 0;
        }



        public override String ToString()
        {
            String S = "";
            for (int i = 0; i < RowCount; i++)
            {
                S += "\r\n";
                for (int j = 0; j < ColumCount; j++)
                {
                    S += Matrix[i][j].ToString("F04") + "\t";
                }

                S += "\t" + Answer[i].ToString("F08");
                S += "\t" + RightPart[i].ToString("F04");
            }
            return S;
        }
    }

    internal static class StructuralParametricSynthesis
    {
        /// <summary>
        /// Проверка на корреляцию фактора и отклика
        /// </summary>
        /// <returns></returns>
        public static double TryCorrelation(double[] X, double[] Y)
        {
            double meanX1 = X.Sum()/X.Length;
            double meanX2 = Y.Sum()/Y.Length;

            double[] deviationX1 = new double[X.Length];
            double[] deviationX2 = new double[Y.Length];

            for (int i = 0; i < deviationX1.Length; i++)
            {
                deviationX1[i] = meanX1 - X[i];
                deviationX2[i] = meanX2 - Y[i];
            }

            double sumDeviation = 0;

            for (int i = 0; i < deviationX1.Length; i++)
            {
                sumDeviation += deviationX1[i] * deviationX2[i];
            }

            double sqrDeviationX1 = 0;
            double sqrDeviationX2 = 0;

            for (int i = 0; i < deviationX1.Length; i++)
            {
                sqrDeviationX1 += deviationX1[i] * deviationX1[i];
                sqrDeviationX2 += deviationX2[i] * deviationX2[i];
            }

            double sqrDeviation = Math.Sqrt(sqrDeviationX1 * sqrDeviationX2);

            double coefCorrelation = sumDeviation / sqrDeviation; // коэффициент корреляции Пирсона
                                                                  //CoefCorrelationAuto = Correlation.Pearson(_X1, _X2);

            return coefCorrelation;            
        }

        public static Polynom MethodGauss(double[,] a, double[] b, Polynom polynom)
        {
            double s = 0;
            int n = b.Length;
            double[] x = new double[n];

            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = k + 1; j < n; j++)
                    {
                        a[i, j] = a[i, j] - a[k, j] * (a[i, k] / a[k, k]);
                    }
                    b[i] = b[i] - b[k] * a[i, k] / a[k, k];
                }
            }
            for (int k = n - 1; k >= 0; k--)
            {
                s = 0;
                for (int j = k + 1; j < n; j++)
                    s = s + a[k, j] * x[j];
                polynom.Equation[k].CoefA = (b[k] - s) / a[k, k];
            }
            return polynom;
        }

        static double CalcSR(double[] y, double[] yr, int mi)
        {
            double sum = 0;

            for (int i = 0; i < y.Length; i++)
            {
                sum += (y[i ]- yr[i]) * (y[i] - yr[i]);
            }
            return 1/(y.Length - (mi + 1)) * sum;
        }

        static Polynom EmpiricalModel(double[] currentFactor, double[] Y)
        {
            Polynom polynom = new Polynom(0);
            double srLast = 0;
            double srCurrent = 0;
            double[] yr = new double[Y.Length];

            while (true)
            {
                polynom.AddPolynom();

                List<Polynom> polynoms = new List<Polynom>();

                // Создаем систему уравнений Гаусса
                for (int i = 0; i < polynom.Equation.Count(); i++)
                {
                    var currentPolynom = new Polynom(i);
                    for (int j = 0; j < polynom.Equation.Count(); j++)
                    {
                        currentPolynom.AddPolynom();
                    }
                    polynoms.Add(currentPolynom);
                }

                // Расчет внутренних сумм
                double[][] A = new double[polynom.Equation.Count()][];
                double[] B = new double[polynom.Equation.Count()];

                for (int i = 0; i < polynom.Equation.Count(); i++)
                {
                    A[i] = new double[polynom.Equation.Count()];

                    for (int j = 0; j < polynom.Equation.Count(); j++)
                    {

                        A[i][j] = Math.Pow(currentFactor.Sum(), polynoms[i].Equation[j].Degree);
                    }
                }

                for (int i = 0; i < polynom.Equation.Count(); i++)
                {
                    B[i] = Math.Pow(Y.Sum(), polynoms[i].Equation[0].Degree);
                }

                for (int i = 0; i < polynom.Equation.Count(); i++)
                {
                    for (int j = 0; j < Y.Length; j++)
                    {
                        B[i] += Y[j] * Math.Pow(currentFactor[j], polynoms[i].Equation[0].Degree);
                    }
                }

                GausMethod gaus = new GausMethod((uint)B.Length, (uint)B.Length);
                gaus.RightPart = B;
                gaus.Matrix = A;
                gaus.SolveMatrix();

                for(int i = 0; i < polynom.Equation.Count; i++)
                {
                    polynom.Equation[i].CoefA = gaus.Answer[i];
                }

                //polynom = MethodGauss(A, B, polynom);

                for (int i = 0; i < Y.Length; i++)
                {
                    for (int j = 0; j < polynom.Equation.Count(); j++)
                    {
                        yr[i] += polynom.Equation[j].CoefA * Math.Pow(currentFactor[i], polynom.Equation[j].Degree);
                    }
                }

                if (polynom.Equation.Count() == 1)
                {
                    srCurrent = CalcSR(Y, yr, polynom.Equation.Count);
                }
                else
                {
                    srLast = srCurrent;
                    srCurrent = CalcSR(Y, yr, polynom.Equation.Count);

                    if (srLast < srCurrent)
                        return polynom;
                }


            }
        }

        public static double Synthesis(double[] X1, double[] X2, double[] Y, out Polynom polynom1, out Polynom polynom2)
        {
            double correlationX1 = TryCorrelation(X1, Y);
            double correlationX2 = TryCorrelation(X2, Y);
            double[] factor1;
            double[] factor2;

            if (correlationX1 > correlationX2)
            {
                factor1 = X1;
                factor2 = X2;
            }
            else
            {
                factor1 = X2;
                factor2 = X1;
            }

            polynom1 = EmpiricalModel(factor1, Y);   
            polynom2 = EmpiricalModel(factor2, Y);

            double yr1 = 0;
            double yr2 = 0;
            double sum = 0;
            

            for (int i = 0; i < Y.Length; i++)
            {
                for (int j = 0; j < polynom1.Equation.Count(); j++)
                {
                    yr1 += polynom1.Equation[j].CoefA * Math.Pow(factor1[i], polynom1.Equation[j].Degree);
                }

                for (int j = 0; j < polynom2.Equation.Count(); j++)
                {
                    yr2 += polynom2.Equation[j].CoefA * Math.Pow(factor2[i], polynom2.Equation[j].Degree);
                }

                sum += Y[i] / (yr1 * yr2);
                yr1 = 0;
                yr2 = 0;
            }

            return ((1.00/(double)Y.Length) * (double)sum);
        }
    }
}
