using MvvmCross.Platforms.Wpf.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Views
{
    public interface ICleanupOnClose
    {
        void OnBeforeClosed();
    }

    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    public partial class TaskDayEditorView : MvxWpfView, ICleanupOnClose
    {
        public TaskDayEditorView()
        {
            InitializeComponent();

            this.Loaded += TaskListView_Loaded;
            
            this.Unloaded += TaskDayEditorView_Unloaded;
        }

        private async void TaskDayEditorView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is TaskDayEditorViewModel viewModel)
            {
                await viewModel.OnBeforeClosing();
            }
        }

        private static MvxWindow GetParentWindow(DependencyObject dependencyObject)
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent is MvxWindow window)
            {
                return window;
            }

            if (parent == null)
            {
                return null;
            }

            return GetParentWindow(parent);
        }

        private void DockPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GetParentWindow(this).DragMove();
        }

        private void MinimizeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnBeforeClosed();
            GetParentWindow(this)?.Hide();
        }

        private async void TaskListView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is TaskDayEditorViewModel viewModel)
            {
                await viewModel.Load();
            }
        }

        public void OnBeforeClosed()
        {
            if(DataContext is TaskDayEditorViewModel viewModel)
            {
                viewModel.OnBeforeClosing().Wait();
            }
        }
    }
}