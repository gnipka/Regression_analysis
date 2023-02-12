using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regression_analysis
{
    internal class Polynom
    {
        public int InitialDegree;

        public Polynom(int initialDegree)
        {
            InitialDegree = initialDegree;
        }

        public List<SummandPolynom> Equation;

        public void AddPolynom()
        {
            if (Equation == null)
            {
                Equation = new List<SummandPolynom>();
                Equation.Add(new SummandPolynom(InitialDegree));
            }
            else
            {
                Equation.Add(new SummandPolynom(Equation.Last().Degree + 1));
            }
        }

        public override string ToString()
        {
            string result = String.Empty;
            for(int i = 0; i < Equation.Count; i++)
            {
                if (Math.Round(Equation[i].CoefA, 2) == 0)
                    result = result;
                else if (i == 0)
                {
                    if (Equation[i].Degree == 0)
                        result += $" {-Math.Round(Equation[i].CoefA, 2)}";
                    else if (Equation[i].Degree == 1)
                        result += $" {-Math.Round(Equation[i].CoefA, 2)}x";
                    else
                        result += $" {-Math.Round(Equation[i].CoefA, 2)}x^{Equation[i].Degree}";
                }
                else if (Equation[i].CoefA < 0)
                {
                    if (Equation[i].Degree == 0)
                        result += $" - {-Math.Round(Equation[i].CoefA, 2)}";
                    else if (Equation[i].Degree == 1)
                        result += $" - {-Math.Round(Equation[i].CoefA, 2)}x";
                    else
                        result += $" - {-Math.Round(Equation[i].CoefA, 2)}x^{Equation[i].Degree}";
                }
                else
                {
                    if (Equation[i].Degree == 0)
                        result += $" + {-Math.Round(Equation[i].CoefA, 2)}";
                    else if (Equation[i].Degree == 1)
                        result += $" + {-Math.Round(Equation[i].CoefA, 2)}x";
                    else
                        result += $" + {-Math.Round(Equation[i].CoefA, 2)}x^{Equation[i].Degree}";
                }
            }

            return result;
        }
    }

    internal class SummandPolynom
    {
        public SummandPolynom(int degree)
        {
            Degree = degree;
        }

        public double CoefA;
        public int Degree;
    }
}
