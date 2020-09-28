using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calc.Commands;

namespace Calc.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private double? _leftValue;
        private double? _rightValue;
        private double? _resultValue;
        private char? _operation;
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
                x => !(TextExpression == "" && Operation == '/') && TextExpression != "0");

        private ICommand _action;

        public ICommand Action =>
            _action ?? new RelayCommand<string>(x =>
            {
                switch (x)
                {
                    case "C":
                    {
                        TextExpression = "";
                        LeftValue = RightValue = _operation = null;
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
                        if ((TextExpression == "" || TextExpression == "0") && Operation != null)
                        {
                            Operation = Convert.ToChar(x);
                            LastExpression = $"{LeftValue} {Operation}";
                            break;
                        }

                        if (TextExpression == "" && Operation is null) break;
                        if (LeftValue != null && Operation != null)
                        {
                            var res = Compute();
                            if (!res) break;
                        }

                        try
                        {
                            LeftValue = Convert.ToDouble(TextExpression);
                            Operation = Convert.ToChar(x);
                            TextExpression = "";
                            LastExpression = $"{LeftValue} {Operation}";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}");
                        }

                        break;
                    }
                    case "=":
                    {
                        if (LeftValue is null || Operation is null) break;
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
            _showLogs ?? new RelayCommand<ScrollViewer>(x => 
            { 
                x.Visibility = x.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; 
            }, x => true);

        public MainViewModel()
        {
            _logExpressions = new ObservableCollection<string>();
        }

        public string TextExpression
        {
            get => _textExpression;
            set
            {
                _textExpression = value;
                OnPropertyChanged(nameof(TextExpression));
            }
        }

        public ObservableCollection<string> Expressions
        {
            get => _logExpressions;
            set
            {
                _logExpressions = value;
                OnPropertyChanged(nameof(Expressions));
            }
        }

        public double? LeftValue
        {
            get => _leftValue;
            set
            {
                _leftValue = value;
                OnPropertyChanged(nameof(LeftValue));
                OnPropertyChanged(nameof(LastExpression));
            }
        }

        public double? RightValue
        {
            get => _rightValue;
            set
            {
                _rightValue = value;
                OnPropertyChanged(nameof(RightValue));
                OnPropertyChanged(nameof(LastExpression));
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

        public char? Operation
        {
            get => _operation;
            set
            {
                _operation = value;
                OnPropertyChanged(nameof(Operation));
                OnPropertyChanged(nameof(LastExpression));
            }
        }

        private bool Compute()
        {
            if (TextExpression == "") return false;
            try
            {
                RightValue = Convert.ToDouble(TextExpression);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }

            switch (Operation)
            {
                case '+':
                {
                    ResultValue = LeftValue + RightValue;
                    break;
                }
                case '-':
                {
                    ResultValue = LeftValue - RightValue;
                    break;
                }
                case '*':
                {
                    ResultValue = LeftValue * RightValue;
                    break;
                }
                case '/':
                {
                    if (RightValue == 0)
                    {
                        MessageBox.Show("Не надо делить на ноль друг)");
                        return false;
                    }

                    ResultValue = LeftValue / RightValue;
                    break;
                }
            }

            LastExpression = $"{LeftValue} {Operation} {RightValue} = {ResultValue}";
            Expressions.Add(LastExpression);
            TextExpression = ResultValue.ToString();
            RightValue = ResultValue = LeftValue = Operation = null;
            return true;
        }
    }
}