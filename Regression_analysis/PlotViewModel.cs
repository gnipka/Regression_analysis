using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_Classes;

namespace Regression_analysis
{
    internal class PlotViewModel : ViewModelBase
    {

        private List<Experience> _ExperiencePlot;
        public List<Experience> ExperiencePlot
        {
            get
            {
                return _ExperiencePlot;
            }
            set
            {
                _ExperiencePlot = value;
                OnPropertyChanged();
            }
        }

        private int _Diff;
        public int Diff
        {
            get { return _Diff; }
            set { _Diff = value; OnPropertyChanged(); }
        }

        private int[] _Rows;
        public int[] Rows
        {
            get { return _Rows; }
            set { _Rows = value; OnPropertyChanged(); }
        }

        private int[] _Columns;
        public int[] Columns
        {
            get { return _Columns; }
            set { _Columns = value; OnPropertyChanged(); }
        }

        private double[,] _Values;
        public double[,] Values
        {
            get { return _Values; }
            set { _Values = value; OnPropertyChanged(); }
        }

        public PlotViewModel(Brandon brandon, List<Experience> Experiences)
        {

            double[] X1 = new double[Experiences.Count];
            double[] X2 = new double[Experiences.Count];
            double[] Y = new double[Experiences.Count];

            for(int i = 0; i < Experiences.Count; i++)
            {
                X1[i] = Experiences[i].Temp;
                X2[i] = Experiences[i].Time;
                Y[i] =  Experiences[i].Diametr;
            }

            int min2 = (int)X2.Min();
            int max2 = (int)X2.Max();

            int count2 = (int)(max2 - min2)/100;

            int min1 = (int)X1.Min();
            int max1 = (int)X1.Max();

            int count1 = (int)(max1 - min1)/100;

            var temp = new List<Experience>();

            int[] rows = new int[max1 - min1];
            int[] columns = new int[max2 - min2];
            double[,] values = new double[rows.Length, columns.Length];

            int countRow = 0;
            int countConumn = 0;

            for (int i = min1; i < max1; i++)
            {
                for (int j = min2; j < max2; j++)
                {
                    values[countRow, countConumn] = Math.Round(brandon.CalcY(j, i), 2);

                    temp.Add(new Experience
                    {
                        Temp = j,
                        Time = i,
                        Diametr = values[countRow, countConumn]
                    });

                    if (countRow == 0)
                        columns[countConumn] = j;

                    countConumn++;
                }
                countConumn = 0;
                rows[countRow] = i;
                countRow++;
            }
            _Values = new double[rows.Length, columns.Length];
            _Columns = new int[columns.Length];
            _Rows = new int[rows.Length];

            for (int i = 0; i < values.GetUpperBound(0) + 1; i++)
            {
                for(int j = 0; j < (int)(values.Length / (values.GetUpperBound(0) + 1)); j++)
                {
                    _Values[i,j] = values[i,j];
                }
            }

            for (int i = 0; i < rows.Length; i++)
            {
                _Rows[i] = rows[i];
            }

            for (int i = 0; i < columns.Length; i++)
            {
                _Columns[i] = columns[i];
            }

            ExperiencePlot = temp;
            Diff = (int)Math.Sqrt(ExperiencePlot.Count);
        }
    }
}
