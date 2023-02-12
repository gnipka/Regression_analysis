using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regression_analysis
{
    internal class RegressionModel
    {
        
        private double _EtalonNormal;
        private double _EtalonCorrelation;
        private int _SizeDistribution;

        public List<Experience> Experiences;
        public double[] X1;
        public double[] X2;
        public double[] Y;
        public double DiffMax;
        public double CoefCorrelation;
        public double CoefCorrelationAuto;
        public double TCriteria;

        public RegressionModel(List<Experience> experiences, double etalonNormal, double etalonCorrelation)
        {
            Experiences = experiences;
            _EtalonNormal = etalonNormal;
            _EtalonCorrelation = etalonCorrelation;
            _SizeDistribution = experiences.Count();
            SplitIntoArray();
        }

        public void SplitIntoArray()
        {
            X1 = new double[_SizeDistribution];
            X2 = new double[_SizeDistribution];
            Y = new double[_SizeDistribution];

            for (int i = 0; i < _SizeDistribution; i++)
            {
                X1[i] = Experiences[i].Temp;
                X2[i] = Experiences[i].Time;
                Y[i] = Experiences[i].Diametr;
            }
        }

        public void Sort()
        {
            double temp;
            for (int i = 0; i < Experiences.Count - 1; i++)
            {
                for (int j = i + 1; j < Experiences.Count; j++)
                {
                    if (Experiences[i].Diametr > Experiences[j].Diametr)
                    {
                        temp = Experiences[i].Diametr;
                        Experiences[i].Diametr = Experiences[j].Diametr;
                        Experiences[j].Diametr = temp;

                        temp = Experiences[i].Temp;
                        Experiences[i].Temp = Experiences[j].Temp;
                        Experiences[j].Temp = temp;

                        temp = Experiences[i].Time;
                        Experiences[i].Time = Experiences[j].Time;
                        Experiences[j].Time = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Проверка на нормальное распределение
        /// </summary>
        public bool TryNormalDistribution()
        {
            double[] arrayY = new double[_SizeDistribution];

            for (int i = 0; i < Y.Length; i++)
            {
                arrayY[i] = Y[i];
            }

            Array.Sort(arrayY);

            DescriptiveStatistics statistics = new DescriptiveStatistics(arrayY);

            double mean = statistics.Mean; // среднее
            double deviation = statistics.StandardDeviation; // стандартное отклонение

            double[] arrayYNormal = new double[arrayY.Length];
            double[] arrayYEmpirical = new double[arrayY.Length];
            double[] arrayDiff = new double[arrayY.Length];

            // Расчет эмпирического распределения
            for (int i = 0; i < _SizeDistribution; i++)
            {
                arrayYEmpirical[i] = ((double)i/(double)arrayY.Length);
            }

            // Расчет теоретического распределения
            for (int i = 0; i < _SizeDistribution; i++)
            {
                arrayYNormal[i] = Normal.CDF(mean, deviation, arrayY[i]);
            }

            // Разность эмпирического и теоретического распределения
            for (int i = 0; i < _SizeDistribution; i++)
            {
                arrayDiff[i] = Math.Abs(arrayYEmpirical[i] - arrayYNormal[i]);
            }

            DiffMax = arrayDiff.Max();

            if (DiffMax < _EtalonNormal)
                return true;
            else
                return false;
        }

    }
}
