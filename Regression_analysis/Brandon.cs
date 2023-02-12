using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regression_analysis
{
    public class Brandon
    {
        public int[] m = new int[2] { 1, 1 };//степени полинома
        public List<List<double>> a = new List<List<double>>();//коэф-ты
        List<List<double>> y_calc = new List<List<double>>();//рассчитанные значения выходного параметра
        public string[] regressEquation = new string[2];//уравнения регрессии
        double K;//общий коэф-т
        public string model;//общее уравнение
        public List<List<List<double>>> a_ = new List<List<List<double>>>();//коэф-ты на всех этапах
        public List<List<double>> resVarians = new List<List<double>>();//значения остаточной регрессии
        public Brandon(List<List<double>> x, List<double> y)
        {
            if (Math.Abs(StructuralParametricSynthesis.TryCorrelation(x[0].ToArray(), y.ToArray())) < Math.Abs(StructuralParametricSynthesis.TryCorrelation(x[1].ToArray(), y.ToArray())))//расчет коэф-тов парной корреляции
                x.Reverse();//ранжирование факторов

            List<double> y_ = new List<double>();
            y_.AddRange(y);

            for (int i = 0; i<2; i++)
            {
                a.Add(new List<double>());
                y_calc.Add(new List<double>());

                a_.Add(new List<List<double>>());
                List<double> y_calc_ = new List<double>();//значения прогнозируемых выходных параметров на предыдущем этапе
                resVarians.Add(new List<double>());

                while (true)
                {
                    a[i].AddRange(CalcCoeff(m[i], x[i], y_));//расчет коэф-тов полинома
                    a_[i].Add(new List<double>());
                    a_[i][m[i] - 1].AddRange(a[i]);
                    for (int j = 0; j < x[i].Count; ++j)
                        y_calc[i].Add(CalcY(x[i][j], a[i]));//расчет прогнозируемых выходных параметров

                    resVarians[i].Add(ResidualDisp(y_, y_calc[i], m[i]));//расчет остаточной дисперсии
                    if (m[i] == 1)
                        ++m[i];
                    else
                    {
                        if (resVarians[i][m[i] - 1] > resVarians[i][m[i] - 2])
                        {
                            //возвращаемся к предыдущей степене
                            --m[i];
                            a[i].Clear();
                            a[i].AddRange(a_[i][m[i] - 1]);
                            y_calc[i].Clear();
                            y_calc[i].AddRange(y_calc_);
                            break;
                        }
                        else ++m[i];
                    }
                    y_calc_.Clear();
                    y_calc_.AddRange(y_calc[i]);
                    a[i].Clear();
                    y_calc[i].Clear();
                }
                regressEquation[i] = GetRegressEquation(a[i]);

                for (int j = 0; j < y_.Count; ++j)//пересчет выхода
                    y_[j] = y_[j] / y_calc[i][j];

            }
            model = GetModel(y);
        }

        private List<double> CalcCoeff(int m, List<double> x, List<double> y)//расчет коэф-тов полиномма
        {
            int n = m + 1;//размерность системы
            double[,] c = new double[n, n];//коэф-ты СЛАУ
            double[] b = new double[n];//свободные члены

            for (int i = 0; i<n; ++i)
            {
                int p = i;
                for (int j = 0; j<n; ++j)
                {
                    c[i, j] = SumXPow(p, x);
                    ++p;
                }
            }

            b[0] = y.Sum();
            for (int i = 1; i < n; ++i)
                b[i] = SumYXPow(i, x, y);

            LinearSystem system = new LinearSystem(c, b);
            return system.xVector.ToList();
        }

        private double SumXPow(int pow, List<double> x)//сумма х возведенных в степень pow
        {
            double s = 0;
            for (int i = 0; i < x.Count; i++)
                s += Math.Pow(x[i], pow);
            return s;
        }

        private double SumYXPow(int pow, List<double> x, List<double> y)//сумма произведений y и х возведенных в степень pow
        {
            double s = 0;
            for (int i = 0; i < x.Count; i++)
                s += Math.Pow(x[i], pow) * y[i];
            return s;
        }

        private double CalcY(double x, List<double> a)//расчет прогнозируемого выходного параметра 
        {
            double s = 0;
            for (int i = 0; i < a.Count; i++)
                s += a[i] * Math.Pow(x, i);
            return s;
        }

        public double SumSqResidualDisp(List<double> y, List<double> y_calc)//расчет суммы квадратов остаточной дисперсии
        {
            double s = 0;
            for (int i = 0; i < y.Count; ++i)
                s += Math.Pow(y_calc[i] - y[i], 2);
            return s;
        }

        public double ResidualDisp(List<double> y, List<double> y_calc, int m)//остаточная дисперсия уравнения регрессии
        {
            return SumSqResidualDisp(y, y_calc) / (y.Count - (m + 1));
        }

        public string GetRegressEquation(List<double> a)//уравнение регрессии
        {
            string equation = "";

            if (Math.Round(a[0], 4) != 0)
                equation = Math.Round(a[0], 4).ToString();
            else
                equation = a[0].ToString("E3");
            if (Math.Round(a[1], 4) != 0)
            {
                if (a[1] > 0)
                    equation += " + " + Math.Round(a[1], 4) + " * x";
                else
                    equation += " - " + Math.Abs(Math.Round(a[1], 4)) + " * x";
            }
            else
            {
                if (a[1] > 0)
                    equation += " + " + a[1].ToString("E3") + " * x";
                else
                    equation += " - " + Math.Abs(a[1]).ToString("E3") + " * x";
            }
            for (int i = 2; i < a.Count; ++i)
            {
                if (Math.Round(a[i], 4) != 0)
                {
                    if (a[i] > 0)
                        equation += " + " + Math.Round(a[i], 4) + " * x^" + i;
                    else
                        equation += " - " + Math.Abs(Math.Round(a[i], 4)) + " * x^" + i;
                }
                else
                {
                    if (a[i] > 0)
                        equation += " + " + a[i].ToString("E3") + " * x^" + i;
                    else
                        equation += " - " + Math.Abs(a[i]).ToString("E3") + " * x^" + i;
                }
            }
            return equation;
        }

        private double CalcTotalCoeff(List<double> y, List<List<double>> y_calc)//общий коэф-т модели
        {
            double s = 0;
            for (int i = 0; i<y.Count; ++i)
            {
                double p = 1;
                for (int j = 0; j < y_calc.Count; ++j)
                    p *= y_calc[j][i];

                s += y[i] / p;
            }
            K = s / y.Count;
            return K;
        }

        private string GetModel(List<double> y)//общее уравнение модели
        {
            string model = "D = " + Math.Round(CalcTotalCoeff(y, y_calc), 3);
            for (int i = 0; i < regressEquation.Length; ++i)
                model += "(" + regressEquation[i].Replace("x", "x"+(i+1)) + ")";
            return model;
        }

        public double CalcY(double x1, double x2)//расчет прогнозируемого выхода
        {
            return K * CalcY(x1, a[0]) * CalcY(x2, a[1]);
        }

        //расчет критериев проверки адекватности
        public void AdequateModel(List<double> y, List<List<double>> x,
            out double resDisp, out double meanModelDisp, out double Fcalc, double Ftabl, out double R, out double sigma)
        {
            List<double> y_calc = new List<double>();
            for (int i = 0; i < y.Count; ++i)
                y_calc.Add(CalcY(x[0][i], x[1][i]));
            resDisp = ResidualDisp(y, y_calc, a[0].Count * a[1].Count - 1);
            double avgY = y.Average();
            double s = 0;
            for (int i = 0; i < y.Count; ++i)
                s += Math.Pow(y[i] - avgY, 2);
            meanModelDisp = s / (y.Count - 1);
            R = 1 - ((y.Count - (a[0].Count * a[1].Count)) * resDisp / ((y.Count - 1) * meanModelDisp));
            s = 0;
            for (int i = 0; i < y.Count; ++i)
                s += Math.Pow((y[i] - y_calc[i])/y[i], 2);
            sigma = Math.Sqrt(s / (y.Count - (a[0].Count * a[1].Count)))*100;
            Fcalc = meanModelDisp / resDisp;

        }

    }
}
