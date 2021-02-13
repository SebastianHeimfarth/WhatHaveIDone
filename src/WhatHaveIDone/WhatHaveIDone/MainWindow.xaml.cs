using MvvmCross.Platforms.Wpf.Views;
using WhatHaveIDone.Views;

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
            if(Content is ICleanupOnClose cleanup)
            {
                cleanup.OnBeforeClosed();
            }

            e.Cancel = true;
            Hide();
        }

    }
}