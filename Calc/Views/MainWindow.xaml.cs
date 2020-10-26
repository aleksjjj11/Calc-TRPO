using System.Windows;
using System.Windows.Input;
using Calc.ViewModels;

namespace Calc
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            WindowStyle = WindowStyle.None;
            DataContext = _viewModel = new MainViewModel();
            InitializeComponent();
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}