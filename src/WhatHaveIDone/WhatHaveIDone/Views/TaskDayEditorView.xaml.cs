using MvvmCross.Platforms.Wpf.Views;
using System.Windows;
using System.Windows.Media;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Views
{
    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    public partial class TaskDayEditorView : MvxWpfView
    {
        public TaskDayEditorView()
        {
            InitializeComponent();

            this.Loaded += TaskListView_Loaded;
        }

        private async void TaskListView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is TaskDayEditorViewModel viewModel)
            {
                await viewModel.Load();
            }
        }

        private void MinimizeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetParentWindow(this)?.Hide();
        }

        private static MvxWindow GetParentWindow(DependencyObject dependencyObject)
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if(parent is MvxWindow window)
            {
                return window;
            }

            if(parent == null)
            {
                return null;
            }

            return GetParentWindow(parent);
        }

        private void DockPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GetParentWindow(this).DragMove();
        }
    }
}