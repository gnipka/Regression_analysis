using Aspose.Cells;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_MVVM_Classes;

namespace Regression_analysis
{
    public class ResultEquation
    {
        public int Degree { get; set; }
        public string Equation { get; set; }
        public double Variance { get; set; }
    }

    internal class MainWindowViewModel : ViewModelBase
    {
        private string _HeaderTemp;
        public string HeaderTemp
        {
            get { return _HeaderTemp; }
            set
            {
                _HeaderTemp = value;
                OnPropertyChanged();
            }
        }

        private string _HeaderTime;
        public string HeaderTime
        {
            get { return _HeaderTime; }
            set
            {
                _HeaderTime = value;
                OnPropertyChanged();
            }
        }

        private string _HeaderDiametr;
        public string HeaderDiametr
        {
            get { return _HeaderDiametr; }
            set
            {
                _HeaderDiametr = value;
                OnPropertyChanged();
            }
        }

        private List<Experience> _Experiences = new List<Experience>();
        public List<Experience> Experiences
        {
            get { return _Experiences; }
            set
            {
                _Experiences = value;
                OnPropertyChanged();
            }
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

        private string _IsNormal = "Результаты проверки распределения\nна соответствие гауссовскому закону";
        public string IsNormal
        {
            get { return _IsNormal; }
            set
            {
                _IsNormal = value;
                OnPropertyChanged();
            }
        }

        private string _RegressX1;
        public string RegressX1
        {
            get { return _RegressX1; }
            set
            {
                _RegressX1 = value;
                OnPropertyChanged();
            }
        }

        private string _RegressX2;
        public string RegressX2
        {
            get { return _RegressX2; }
            set
            {
                _RegressX2 = value;
                OnPropertyChanged();
            }
        }

        private string _EmpModel;
        public string EmpModel
        {
            get { return _EmpModel; }
            set
            {
                _EmpModel = value;
                OnPropertyChanged();
            }
        }

        private double _CoefCorrTempDiam;
        public double CoefCorrTempDiam
        {
            get { return _CoefCorrTempDiam; }
            set
            {
                _CoefCorrTempDiam = value;
                OnPropertyChanged();
            }
        }

        private double _CoefCorrTimeDiam;
        public double CoefCorrTimeDiam
        {
            get { return _CoefCorrTimeDiam; }
            set
            {
                _CoefCorrTimeDiam = value;
                OnPropertyChanged();
            }
        }

        private double _CoefCorrTimeTemp;
        public double CoefCorrTimeTemp
        {
            get { return _CoefCorrTimeTemp; }
            set
            {
                _CoefCorrTimeTemp = value;
                OnPropertyChanged();
            }
        }

        private double _EtalonNormal = 0.23;
        public double EtalonNormal
        {
            get { return _EtalonNormal; }
            set
            {
                _EtalonNormal = value;
                OnPropertyChanged();
            }
        }

        private double _EtalonCorrelation = 2.035;
        public double EtalonCorrelation
        {
            get { return _EtalonCorrelation; }
            set
            {
                _EtalonCorrelation = value;
                OnPropertyChanged();
            }
        }

        private double _DiffMax;
        public double DiffMax
        {
            get { return _DiffMax; }
            set
            {
                _DiffMax = value;
                OnPropertyChanged();
            }
        }

        private double _TCriteria;
        public double TCriteria
        {
            get { return _TCriteria; }
            set
            {
                _TCriteria = value;
                OnPropertyChanged();
            }
        }


        private string _Equation;
        public string Equation
        {
            get { return _Equation; }
            set
            {
                _Equation = value;
                OnPropertyChanged();
            }
        }

        private double _FisherCrit = 1.83;
        public double FisherCrit
        {
            get { return _FisherCrit; }
            set
            {
                _FisherCrit = value;
                OnPropertyChanged();
            }
        }

        private double _FisherCalc;
        public double FisherCalc
        {
            get { return _FisherCalc; }
            set
            {
                _FisherCalc = value;
                OnPropertyChanged();
            }
        }

        private string _FisherRes;
        public string FisherRes
        {
            get { return _FisherRes; }
            set
            {
                _FisherRes = value;
                OnPropertyChanged();
            }
        }

        private double _Disp;
        public double Disp
        {
            get { return _Disp; }
            set
            {
                _Disp = value;
                OnPropertyChanged();
            }
        }

        private double _FreedomDisp;
        public double FreedomDisp
        {
            get { return _FreedomDisp; }
            set
            {
                _FreedomDisp = value;
                OnPropertyChanged();
            }
        }

        private double _MeanDisp;
        public double MeanDisp
        {
            get { return _MeanDisp; }
            set
            {
                _MeanDisp = value;
                OnPropertyChanged();
            }
        }

        private double _FreedomMeanDisp;
        public double FreedomMeanDisp
        {
            get { return _FreedomMeanDisp; }
            set
            {
                _FreedomMeanDisp = value;
                OnPropertyChanged();
            }
        }


        private Brandon _Brandon;

        private RelayCommand _Calculate;
        public RelayCommand Calculate
        {
            get
            {
                return _Calculate ??= new RelayCommand(x =>
                {
                    if (Experiences.Count != 0)
                    {
                        RegressionModel regressionModel = new RegressionModel(Experiences, EtalonNormal, EtalonCorrelation);
                        regressionModel.Sort();

                        if (regressionModel.TryNormalDistribution())
                            IsNormal = $"Рассчитанный критерий Колмагорова меньше критического значения. Гипотеза, что выборка распределена по нормальному закону подтверждена";
                        else
                            IsNormal = $"Рассчитанный критерий Колмагорова больше критического значения. Гипотеза, что выборка распределена по нормальному закону НЕ подтверждена";

                        DiffMax = Math.Round(regressionModel.DiffMax, 2);

                        Polynom polynom1;
                        Polynom polynom2;

                        double[] X1 = new double[Experiences.Count];
                        double[] X2 = new double[Experiences.Count];
                        double[] Y = new double[Experiences.Count];

                        for (int i = 0; i < Experiences.Count; i++)
                        {
                            X1[i] = Experiences[i].Temp;
                            X2[i] = Experiences[i].Time;
                            Y[i] = Experiences[i].Diametr;
                        }

                        CoefCorrTimeTemp = Math.Round(StructuralParametricSynthesis.TryCorrelation(X1, X2), 2);
                        CoefCorrTempDiam = Math.Round(StructuralParametricSynthesis.TryCorrelation(X1, Y), 2);
                        CoefCorrTimeDiam = Math.Round(StructuralParametricSynthesis.TryCorrelation(X2, Y), 2);

                        List<List<double>> x12 = new List<List<double>>();//факторы
                        List<double> y = new List<double>();//отклик

                        x12.Add(X1.ToList());
                        x12.Add(X2.ToList());

                        y = Y.ToList();

                        _Brandon = new Brandon(x12, y);
                        RegressX1 = "D = " + _Brandon.regressEquation[0].Replace('x', 'T');
                        RegressX2 = "D = " + _Brandon.regressEquation[1].Replace('x', 't');
                        EmpModel = _Brandon.model.Replace("x1", "T").Replace("x2", "t");

                        Equations1 = new List<ResultEquation>();

                        for (int i = 0; i < _Brandon.a_[0].Count; ++i)
                        {
                            Equations1.Add(new ResultEquation
                            {
                                Degree = i + 1,
                                Equation = "D = " + _Brandon.GetRegressEquation(_Brandon.a_[0][i]).Replace('x', 'T'),
                                Variance = Math.Round(_Brandon.resVarians[0][i], 3)
                            });
                        }

                        Equations2 = new List<ResultEquation>();

                        for (int i = 0; i < _Brandon.a_[1].Count; ++i)
                        {
                            Equations2.Add(new ResultEquation
                            {
                                Degree = i + 1,
                                Equation = "D = " + _Brandon.GetRegressEquation(_Brandon.a_[1][i]).Replace('x', 'T'),
                                Variance = Math.Round(_Brandon.resVarians[1][i], 3)
                            });
                        }

                        double resDisp = 0;
                        double meanModelDisp = 0;
                        double Fcalc = 0;
                        double sigma = 0;
                        _Brandon.AdequateModel(y, x12, out resDisp, out meanModelDisp, out Fcalc, _FisherCrit, out double R, out sigma);
                        Disp = Math.Round(resDisp, 2);
                        MeanDisp = Math.Round(meanModelDisp, 2);
                        FreedomDisp = (y.Count - _Brandon.a[0].Count * _Brandon.a[1].Count);
                        FreedomMeanDisp = y.Count - 1;

                        if(Disp > MeanDisp)
                            FisherCalc = Math.Round(Disp / MeanDisp, 2);
                        else
                            FisherCalc = Math.Round(MeanDisp / Disp, 2);

                        if (FisherCalc > FisherCrit)
                            FisherRes = "Рассчитанное значение больше табличного значения критерия Фишера. Модель является адекватной";
                        else
                            FisherRes = "Рассчитанное значение меньше табличного значения критерия Фишера. Модель не является адекватной";


                    }
                    else
                        MessageBox.Show("Необходимо загрузить данные");

                });
            }
        }

        private RelayCommand _Import;

        public RelayCommand Import
        {
            get
            {
                return _Import ??= new RelayCommand(x =>
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                    dlg.DefaultExt = ".xlsx";
                    dlg.Filter = "XLSX Files (*.xlsx)|*.xlsx";


                    // Display OpenFileDialog by calling ShowDialog method 
                    Nullable<bool> result = dlg.ShowDialog();


                    // Get the selected file name and display in a TextBox 
                    if (result == true)
                    {
                        // Open document 

                        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(dlg.FileName);
                        WorksheetCollection collection = wb.Worksheets;
                        Aspose.Cells.Worksheet worksheet = collection[0];

                        HeaderTemp = worksheet.Cells[0, 0].Value.ToString();
                        HeaderTime = worksheet.Cells[0, 1].Value.ToString();
                        HeaderDiametr = worksheet.Cells[0, 2].Value.ToString();

                        int rows = worksheet.Cells.MaxDataRow;
                        int cols = worksheet.Cells.MaxDataColumn;

                        var temp = new List<Experience>();

                        for (int i = 1; i < rows; i++)
                        {
                            temp.Add(new Experience { Temp = (double)worksheet.Cells[i, 0].Value, Time = (double)worksheet.Cells[i, 1].Value, Diametr = (double)worksheet.Cells[i, 2].Value });
                        }

                        Experiences = temp;
                    }

                });
            }
        }

        private PlotWindow? _PlotWindow = null;
        private PlotViewModel _PlotViewModel;

        private RelayCommand _Plot;

        public RelayCommand Plot
        {
            get
            {
                return _Plot ??= new RelayCommand(x =>
                {


                    _PlotViewModel = new PlotViewModel(_Brandon, Experiences);
                    _PlotWindow = new PlotWindow();
                    _PlotWindow.DataContext = _PlotViewModel;
                    _PlotWindow.Show();


                    //((Window)x).Hide();
                });
            }
        }

        private Equation? _EquationWindow = null;
        private EquationViewModel _EquationViewModel;

        private RelayCommand _ShowEquation;

        public RelayCommand ShowEquation
        {
            get
            {
                return _ShowEquation ??= new RelayCommand(x =>
                {
                    _EquationViewModel = new EquationViewModel(_Equations1, Equations2);
                    _EquationWindow = new Equation();
                    _EquationWindow.DataContext = _EquationViewModel;
                    _EquationWindow.Show();
                });
            }
        }

        private RelayCommand _Export;

        public RelayCommand Export
        {
            get
            {
                return _Export ??= new RelayCommand(x =>
                {

                    var directory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Отчеты"));

                    if (!directory.Exists)
                    {
                        directory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Отчеты"));
                        directory.Create();
                    }

                    string path = Path.Combine(directory.FullName, $"Отчет №1. {DateTime.Today.ToShortDateString()}.xlsx");
                    var file = new FileInfo(Path.Combine(directory.FullName, path));

                    int num = 1;
                    while (file.Exists)
                    {
                        path = Path.Combine(directory.FullName, $"Отчет №{num}. {DateTime.Today.ToShortDateString()}.xlsx");
                        path.Replace(',', ' ');
                        file = new FileInfo(Path.Combine(path));
                        num++;
                    };

                    ExportExcel(file);

                    var result = MessageBox.Show($"Открыть файл в формате Excel? Он также будет сохранен по пути {directory.FullName}", "Экспорт в Excel", MessageBoxButton.YesNo);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Save(path);
                            break;
                        case MessageBoxResult.No:
                            SaveAndClose(path);
                            break;
                    }
                });
            }
        }


        public Microsoft.Office.Interop.Excel.Application? _Excel;
        public Microsoft.Office.Interop.Excel.Workbook? _Workbook;
        public Microsoft.Office.Interop.Excel.Worksheet? _Worksheet;

        public void SaveAndClose(string fileName)
        {
            _Excel.Visible = false;
            _Workbook.Activate();
            _Workbook.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
    Missing.Value, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlUserResolution, true,
    Missing.Value, Missing.Value, Missing.Value);
            _Workbook.Close();
            _Excel.Quit();
        }

        public void Save(string fileName)
        {
            _Excel.Visible = true;
            _Workbook.Activate();
            _Workbook.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
    Missing.Value, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlUserResolution, true,
    Missing.Value, Missing.Value, Missing.Value);
        }

        public void ExportExcel(FileInfo file)
        {
            _Excel = new Microsoft.Office.Interop.Excel.Application();
            _Workbook = _Excel.Workbooks.Add();
            //_Workbook = _Excel.Workbooks.Open(file.FullName);
            _Worksheet = (Microsoft.Office.Interop.Excel.Worksheet)_Workbook.ActiveSheet;

            _Worksheet.Range["A1"].Offset[0, 0].Value = "Результаты статистического анализа данных";
            _Worksheet.Range["A1"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["A1"].Font.Bold = true;
            _Worksheet.Range["A2"].Offset[0, 0].Value = "Табличное значение критерия Колмогорова";
            _Worksheet.Range["A2"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["B2"].Offset[0, 0].Value = EtalonNormal;
            _Worksheet.Range["B2"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["A3"].Offset[0, 0].Value = "Рассчитанный критерий Колмогорова";
            _Worksheet.Range["A3"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["B3"].Offset[0, 0].Value = DiffMax;
            _Worksheet.Range["B3"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["A4"].Offset[0, 0].Value = "Коэффициент парной корреляции";
            _Worksheet.Range["A4"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["B4"].Offset[0, 0].Value = CoefCorrTimeTemp;
            _Worksheet.Range["B4"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;


            _Worksheet.Range["D1"].Offset[0, 0].Value = "Результаты регрессионного анализа данных";
            _Worksheet.Range["D1"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["D1"].Font.Bold = true;
            _Worksheet.Range["D2"].Offset[0, 0].Value = "Коэффициент парной корреляции между температурой спекания и диаметром зерна";
            _Worksheet.Range["D2"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["E2"].Offset[0, 0].Value = CoefCorrTempDiam;
            _Worksheet.Range["E2"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["D3"].Offset[0, 0].Value = "Коэффициент парной корреляции между временем выдержки и диаметром зерна";
            _Worksheet.Range["D3"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["E3"].Offset[0, 0].Value = CoefCorrTimeDiam;
            _Worksheet.Range["E3"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["D4"].Offset[0, 0].Value = "Уравнение регрессии для температуры спекания:";
            _Worksheet.Range["D4"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["E4"].Offset[0, 0].Value = RegressX1;
            _Worksheet.Range["E4"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["D5"].Offset[0, 0].Value = "Уравнение регрессии для времени выдержки:";
            _Worksheet.Range["D5"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["E5"].Offset[0, 0].Value = RegressX2;
            _Worksheet.Range["E5"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["D6"].Offset[0, 0].Value = "Эмпирическая модель:";
            _Worksheet.Range["D6"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["E6"].Offset[0, 0].Value = EmpModel;
            _Worksheet.Range["E6"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Range["D8"].Offset[0, 0].Value = "Этапы построения эмпирической модели";
            _Worksheet.Range["D8"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["D8"].Font.Bold = true;

            _Worksheet.Range["D9"].Offset[0, 0].Value = "Этапы построения уравнения регрессии для температуры спекания и диаметра зерна:";
            _Worksheet.Range["D9"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Range["D10"].Offset[0, 0].Value = "Степень полинома";
            _Worksheet.Range["D10"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Range["E10"].Offset[0, 0].Value = "Уравнение регрессии";
            _Worksheet.Range["E10"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Range["F10"].Offset[0, 0].Value = "Остаточная дисперсия";
            _Worksheet.Range["F10"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            for (int i = 0; i < Equations1.Count; i++)
            {
                _Worksheet.Range["D11"].Offset[i, 0].Value = Equations1[i].Degree;
                _Worksheet.Range["D11"].Offset[i, 1].Value = Equations1[i].Equation;
                _Worksheet.Range["D11"].Offset[i, 2].Value = Equations1[i].Variance;

                //прорисовка границ
                _Worksheet.Range["D11"].Offset[i, 0].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
                _Worksheet.Range["D11"].Offset[i, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
                _Worksheet.Range["D11"].Offset[i, 2].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            }

            _Worksheet.Range["D11"].Offset[Equations1.Count + 1, 0].Value = "Этапы построения уравнения регрессии для температуры спекания и диаметра зерна:";
            _Worksheet.Range["D11"].Offset[Equations1.Count + 1, 0].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Range["D11"].Offset[Equations1.Count + 2, 0].Value = "Степень полинома";
            _Worksheet.Range["D11"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Range["E11"].Offset[Equations1.Count + 2, 0].Value = "Уравнение регрессии";
            _Worksheet.Range["E11"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Range["F11"].Offset[Equations1.Count + 2, 0].Value = "Остаточная дисперсия";
            _Worksheet.Range["F11"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            int j = 0;
            for (int i = Equations1.Count + 3; i < Equations1.Count + 3 + Equations2.Count; i++)
            {
                _Worksheet.Range["D11"].Offset[i, 0].Value = Equations2[j].Degree;
                _Worksheet.Range["D11"].Offset[i, 1].Value = Equations2[j].Equation;
                _Worksheet.Range["D11"].Offset[i, 2].Value = Equations2[j].Variance;

                //прорисовка границ
                _Worksheet.Range["D11"].Offset[i, 0].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
                _Worksheet.Range["D11"].Offset[i, 1].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
                _Worksheet.Range["D11"].Offset[i, 2].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
                j++;
            }

            _Worksheet.Range["H1"].Offset[0, 0].Value = "Результаты проверки модели на адекватность";
            _Worksheet.Range["H1"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["H1"].Font.Bold = true;
            _Worksheet.Range["H2"].Offset[0, 0].Value = "Остаточная дисперсия";
            _Worksheet.Range["H2"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["I2"].Offset[0, 0].Value = Disp;
            _Worksheet.Range["I2"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["J2"].Offset[0, 0].Value = "Дисперсия среднего";
            _Worksheet.Range["J2"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["K2"].Offset[0, 0].Value = MeanDisp;
            _Worksheet.Range["K2"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["H3"].Offset[0, 0].Value = "Число степеней свободы";
            _Worksheet.Range["H3"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["I3"].Offset[0, 0].Value = FreedomDisp;
            _Worksheet.Range["I3"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["J3"].Offset[0, 0].Value = "Число степеней свободы";
            _Worksheet.Range["J3"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["K3"].Offset[0, 0].Value = FreedomMeanDisp;
            _Worksheet.Range["K3"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["H4"].Offset[0, 0].Value = "Рассчитанный критерий Фишера";
            _Worksheet.Range["H4"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["I4"].Offset[0, 0].Value = FisherCalc;
            _Worksheet.Range["I4"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["J4"].Offset[0, 0].Value = "Табличный критерий Фишера";
            _Worksheet.Range["J4"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;
            _Worksheet.Range["K4"].Offset[0, 0].Value = FisherCrit;
            _Worksheet.Range["K4"].Borders.LineStyle = XlAboveBelow.xlBelowAverage;

            _Worksheet.Columns.AutoFit();
        }

    }
}
