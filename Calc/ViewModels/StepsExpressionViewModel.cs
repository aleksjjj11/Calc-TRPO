using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Calc.Commands;
using Expression = Calc.Models.Expression;

namespace Calc.ViewModels
{
    public class StepsExpressionViewModel : BaseViewModel
    {
        public Expression MExpression { get; }

        public StepsExpressionViewModel(Expression expression)
        {
            MExpression = expression;
        }

        private ICommand _closeWindowCommand;

        public ICommand CloseWindowCommand => _closeWindowCommand ?? new RelayCommand<Window>(x =>
        {
            x.Close();
        }, x => true);
    }
}
