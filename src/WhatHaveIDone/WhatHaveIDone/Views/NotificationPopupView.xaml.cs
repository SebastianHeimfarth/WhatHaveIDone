using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Views
{
    /// <summary>
    /// Interaction logic for NotificationPopup.xaml
    /// </summary>
    public partial class NotificationPopupView : Popup
    {
        public NotificationPopupView()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += OpenMainWindow;
        }

        private void OpenMainWindow(object sender, MouseButtonEventArgs e)
        {
            var app = (App)Application.Current;
            app.Dispatcher.Invoke(() => 
            {
                app.ShowApp();
            });
            
        }

        protected override void OnOpened(EventArgs e)
        {
            var viewModel = (NotificationViewModel)DataContext;
            Dispatcher.Invoke(async () => await  viewModel.Initialize());
        }
    }
}
