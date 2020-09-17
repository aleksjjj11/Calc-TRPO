using Calc.Commands;
using Calc.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainViewModel _viewModel;
        
        public MainWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.DataContext = _viewModel = new MainViewModel();
            InitializeComponent();
        }
        
        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ShowLogs_Click(object sender, RoutedEventArgs e)
        {
            if (LogExpressions.Visibility == Visibility.Visible)
            {
                LogExpressions.Visibility = Visibility.Collapsed;
                this.Width -= 240;
            } 
            else
            {
                LogExpressions.Visibility = Visibility.Visible;
                this.Width += 240;
            }
        }
    }
}