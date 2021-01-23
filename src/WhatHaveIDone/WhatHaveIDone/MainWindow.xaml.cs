using MvvmCross.Platforms.Wpf.Views;

namespace WhatHaveIDone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void MinimizeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Hide();
        }
    }
}