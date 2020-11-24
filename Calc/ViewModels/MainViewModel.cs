using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calc.Commands;
using Calc.Interfaces;
using Calc.Models;

namespace Calc.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private double? _resultValue;
        private string _textExpression = "";
        public IHistory History { get;  }
        public IMemory Memory { get;  }
        public ICalculate Calculate { get; }
        public DateTime TimeLastSave { get; set; }
        public DateTime TimeNextSave { get; set; }
        public string TimeToSave => (TimeNextSave - DateTime.Now).ToString(@"hh\:mm\:ss");
        public Timer SaveTimer { get; }
        public Timer UpdateTimer { get; }

        private string _lastExpression;

        public MainViewModel()
        {
            History = new HistoryJson("myHistory.txt");
            Memory = new Memory("myMemory.txt");
            Calculate = new Calculate();
            SaveTimer = new Timer
            {
                Interval = 30000,
                AutoReset = true,
                Enabled = true,
            };
            TimeLastSave = DateTime.Now;
            TimeNextSave = TimeLastSave.AddSeconds(30);
            SaveTimer.Elapsed += (sender, args) =>
            {
                History.Save();
                Memory.Save();
                TimeLastSave = DateTime.Now;
                TimeNextSave = TimeLastSave.AddSeconds(30);
            };

            UpdateTimer = new Timer
            {
                Interval = 1000,
                AutoReset = true,
                Enabled = true
            };
            UpdateTimer.Elapsed += (sender, args) => OnPropertyChanged(nameof(TimeToSave));
        }

        public string LastExpression
        {
            get => _lastExpression;
            set
            {
                _lastExpression = value;
                OnPropertyChanged(nameof(LastExpression));
            }
        }

        private ICommand _addNumber;

        public ICommand AddNumber =>
            _addNumber ??
            new RelayCommand<string>(x => { TextExpression = TextExpression == "0" ? x : TextExpression + x; },
                x => true);

        private ICommand _addZero;

        public ICommand AddZero =>
            _addZero ?? new RelayCommand<string>(x => { TextExpression += x; },
                x => true);

        private ICommand _action;

        public ICommand Action =>
            _action ?? new RelayCommand<string>(x =>
            {
                switch (x)
                {
                    case "C":
                    {
                        TextExpression = "";
                        break;
                    }
                    case "CE":
                    {
                        TextExpression = "";
                        break;
                    }
                    case "+/-":
                    {
                        TextExpression = TextExpression[0] == '-' ? TextExpression.Remove(0, 1) : "-" + TextExpression;//(-1 * Convert.ToDouble(TextExpression)).ToString();
                        break;
                    }
                    case ".":
                    {
                        TextExpression = TextExpression == "" ? TextExpression += "0." : TextExpression += ".";
                        break;
                    }
                }
            }, x => x == "." && !TextExpression.Contains(".") || x != ".");

        private ICommand _arithmeticAction;

        public ICommand ArithmeticAction =>
            _arithmeticAction ?? new RelayCommand<string>(x =>
            {
                switch (x)
                {
                    case "*":
                    case "/":
                    case "+":
                    case "-":
                    {
                        if (TextExpression == "") break;
                        if (TextExpression[TextExpression.Length - 1] == '*' ||
                            TextExpression[TextExpression.Length - 1] == '+' ||
                            TextExpression[TextExpression.Length - 1] == '-' ||
                            TextExpression[TextExpression.Length - 1] == '/')
                            TextExpression = TextExpression.Remove(TextExpression.Length - 1, 1);
                        TextExpression += x;
                        break;
                    }
                }
            }, x => true);

        private ICommand _computeCommand;

        public ICommand ComputeCommand => _computeCommand ?? new RelayCommand(() =>
        {
            Compute();
        }, () => Calculate.Parse(TextExpression).HasError == false && TextExpression.Any());

        private ICommand _closeApp;

        public ICommand CloseApp =>
            _closeApp ?? new RelayCommand<Window>(x => { x.Close(); }, x => true);

        private ICommand _showLogs;

        public ICommand ShowLogsCommand =>
            _showLogs ?? new RelayCommand<TabControl>(x => 
            { 
                x.Visibility = x.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; 
            }, x => true);

        private ICommand _resetMemoryCommand;

        public ICommand ResetMemoryCommand
        {
            get => _resetMemoryCommand ?? new RelayCommand(() =>
            {
                Memory.Clear();
            }, () => Memory.TryClear());
        }

        private ICommand _addElementInMemory;

        public ICommand AddElementInMemory
        {
            get => _addElementInMemory ?? new RelayCommand<string>(x =>
            {
                Memory.Add(Calculate.Parse(x).Result);
            }, x => TextExpression != "" && Calculate.Parse(x).HasError == false);
        }

        private ICommand _additionInMemory;

        public ICommand AdditionInMemory
        {
            get => _additionInMemory ?? new RelayCommand<string>(x =>
            {
                Memory.Increase(Calculate.Parse(x).Result, 0);
            }, x => TextExpression != "" && Memory.TryIncrease(0) && Calculate.Parse(x).HasError == false);
        }

        private ICommand _subtractionInMemory;

        public ICommand SubtractionInMemory
        {
            get => _subtractionInMemory ?? new RelayCommand<string>(x =>
            {
                Memory.Decrease(Calculate.Parse(x).Result, 0);
            }, x => TextExpression != "" && Memory.TryDecrease(0) && Calculate.Parse(x).HasError == false);
        }

        private ICommand _subtractionMemoryElement;

        public ICommand SubtractionMemoryElement
        {
            get => _subtractionMemoryElement ?? new RelayCommand<TextBox>(x =>
            {
                Memory.Decrease(Calculate.Parse(TextExpression).Result, (int)x.Tag);
            }, x => TextExpression != "" && Memory.TryDecrease((int)x.Tag) && Calculate.Parse(TextExpression).HasError == false);
        }

        private ICommand _additionMemoryElement;

        public ICommand AdditionMemoryElement
        {
            get => _additionMemoryElement ?? new RelayCommand<TextBox>(x =>
            {
                Memory.Increase(Calculate.Parse(TextExpression).Result, (int)x.Tag);
            }, x => TextExpression != "" && Memory.TryIncrease((int)x.Tag) && Calculate.Parse(TextExpression).HasError == false);
        }

        private ICommand _deleteMemoryElement;

        public ICommand DeleteMemoryElement
        {
            get => _deleteMemoryElement ?? new RelayCommand<TextBox>(x =>
            {
                Memory.Delete((int)x.Tag);
            }, x => Memory.TryDelete((int)x.Tag));
        }

        public string TextExpression
        {
            get => _textExpression;
            set
            {
                _textExpression = value;
                if (string.IsNullOrWhiteSpace(value))
                    _validationDictionary[nameof(TextExpression)] = "Empty";
                else if (Calculate.Parse(value).HasError)
                    _validationDictionary[nameof(TextExpression)] = "Error in computing";
                else
                    _validationDictionary[nameof(TextExpression)] = null;
                OnPropertyChanged(nameof(TextExpression));
            }
        }

        public double? ResultValue
        {
            get => _resultValue;
            set
            {
                _resultValue = value;
                OnPropertyChanged(nameof(ResultValue));
                OnPropertyChanged(nameof(LastExpression));
            }
        }

        private bool Compute()
        {
            if (TextExpression == "") return false;
            var resultExpression = Calculate.Parse(TextExpression);
            ResultValue = resultExpression.Result;
            LastExpression = $"{TextExpression} = {ResultValue}";
            resultExpression.Action = LastExpression;
            History.Add(resultExpression);
            TextExpression = ResultValue.ToString();
            return true;
        }
    }
}