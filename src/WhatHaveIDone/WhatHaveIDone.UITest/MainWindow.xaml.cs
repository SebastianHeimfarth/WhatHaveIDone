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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhatHaveIDone.UITest.TestCases;

namespace WhatHaveIDone.UITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            TestCases = TestCaseDiscovery.DiscoverAllTestCases(typeof(MainWindow).Assembly).ToList();
            InitializeComponent();
            DataContext = this;
        }

        public IReadOnlyList<ITestCase> TestCases
        {
            get; 
        }

        private void ExecuteTestCase(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var testCase = (ITestCase)button.Tag;

            var control = testCase.CreateControl();

            var window = new Window
            {
                Content = control,
                Width = 800,
                Height = 600,
                Title = testCase.Name
            };

            window.Show();

        }
    }
}
