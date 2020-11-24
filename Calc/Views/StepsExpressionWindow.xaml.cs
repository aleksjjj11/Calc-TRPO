using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Calc.ViewModels;
using Expression = Calc.Models.Expression;

namespace Calc.Views
{
    /// <summary>
    /// Interaction logic for StepsExpressionWindow.xaml
    /// </summary>
    public partial class StepsExpressionWindow : Window
    {
        public StepsExpressionWindow(Expression expression)
        {
            DataContext = new StepsExpressionViewModel(expression);
            InitializeComponent();
        }
    }
}
