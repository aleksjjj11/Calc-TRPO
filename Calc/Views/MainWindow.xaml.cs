using System.Windows;
using System.Windows.Input;
using Calc.Models;
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
            DataContext = _viewModel = new MainViewModel(new Calculate());
            InitializeComponent();
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}