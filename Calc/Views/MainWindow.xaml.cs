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
        private MainViewModel viewModel;
        
        public MainWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.DataContext = viewModel = new MainViewModel();
            InitializeComponent();
        }
        
        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ShowLogs_Click(object sender, RoutedEventArgs e)
        {
            if (LogExperession.Visibility == Visibility.Visible)
            {
                LogExperession.Visibility = Visibility.Collapsed;
                this.Width -= 240;
            } 
            else
            {
                LogExperession.Visibility = Visibility.Visible;
                this.Width += 240;
            }
        }
    }
}