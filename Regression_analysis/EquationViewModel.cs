using System.Collections.Generic;
using WPF_MVVM_Classes;

namespace Regression_analysis
{
    internal class EquationViewModel : ViewModelBase
    {
        public EquationViewModel(List<ResultEquation> equations1, List<ResultEquation> equations2)
        {
            Equations1 = equations1;
            Equations2 = equations2;
        }

        private List<ResultEquation> _Equations1;
        public List<ResultEquation> Equations1
        {
            get { return _Equations1; }
            set
            {
                _Equations1 = value;
                OnPropertyChanged();
            }
        }

        private List<ResultEquation> _Equations2;
        public List<ResultEquation> Equations2
        {
            get { return _Equations2; }
            set
            {
                _Equations2 = value;
                OnPropertyChanged();
            }
        }
    }
}
