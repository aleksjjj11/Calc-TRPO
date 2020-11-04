using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calc.Commands;
using Calc.Models;

namespace Calc.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private double? _resultValue;
        private string _textExpression = "";
        private ObservableCollection<string> _logExpressions;

        private string _lastExpression;

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
                        TextExpression = (-1 * Convert.ToDouble(TextExpression)).ToString();
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
                    case "=":
                    {
                        if (TextExpression == "") break;
                        Compute();
                        break;
                    }
                }
            }, x => true);

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
            }, () => Memory.Count > 0);
        }

        private ICommand _addElementInMemory;

        public ICommand AddElementInMemory
        {
            get => _addElementInMemory ?? new RelayCommand<string>(x =>
            {
                Memory.Insert(0, Calculate.Parse(x));
            }, x => TextExpression != "");
        }

        private ICommand _additionInMemory;

        public ICommand AdditionInMemory
        {
            get => _additionInMemory ?? new RelayCommand<string>(x =>
            {
                var el = Memory.First();
                Memory[0] = el + Calculate.Parse(x);
            }, x => TextExpression != "" && Memory.Count > 0);
        }

        private ICommand _subtractionInMemory;

        public ICommand SubtractionInMemory
        {
            get => _subtractionInMemory ?? new RelayCommand<string>(x =>
            {
                var el = Memory.First();
                Memory[0] = el - Calculate.Parse(x);
            }, x => TextExpression != "" && Memory.Count > 0);
        }

        private ICommand _subtractionMemoryElement;

        public ICommand SubtractionMemoryElement
        {
            get => _subtractionMemoryElement ?? new RelayCommand<TextBox>(x =>
            {
                var res = Convert.ToDouble(x.Text) - Calculate.Parse(TextExpression);
                x.Text = res.ToString();
            }, x => TextExpression != "");
        }

        private ICommand _additionMemoryElement;

        public ICommand AdditionMemoryElement
        {
            get => _subtractionMemoryElement ?? new RelayCommand<TextBox>(x =>
            {
                var res = Convert.ToDouble(x.Text) + Calculate.Parse(TextExpression);
                x.Text = res.ToString();
            }, x => TextExpression != "");
        }

        private ICommand _deleteMemoryElement;

        public ICommand DeleteMemoryElement
        {
            get => _deleteMemoryElement ?? new RelayCommand<TextBox>(x =>
            {
                Memory.RemoveAt((int)x.Tag);
            }, x => true);
        }

        public MainViewModel()
        {
            _logExpressions = new ObservableCollection<string>();
            Memory = new ObservableCollection<double>();
        }

        public string TextExpression
        {
            get => _textExpression;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _validationDictionary[nameof(TextExpression)] = "Empty";
                else
                    _validationDictionary[nameof(TextExpression)] = null;
                _textExpression = value;
                OnPropertyChanged(nameof(TextExpression));
            }
        }

        public ObservableCollection<double> Memory { get; set; }

        public ObservableCollection<string> Expressions
        {
            get => _logExpressions;
            set
            {
                _logExpressions = value;
                OnPropertyChanged(nameof(Expressions));
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
            ResultValue = Calculate.Parse(TextExpression);
            LastExpression = $"{TextExpression} = {ResultValue}";
            Expressions.Add(LastExpression);
            TextExpression = ResultValue.ToString();
            return true;
        }
    }
}