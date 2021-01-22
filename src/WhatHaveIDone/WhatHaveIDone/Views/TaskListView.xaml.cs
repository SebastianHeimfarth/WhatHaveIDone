using MvvmCross.Platforms.Wpf.Views;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Views
{
    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    public partial class TaskListView : MvxWpfView
    {
        public TaskListView()
        {
            InitializeComponent();

            this.Loaded += TaskListView_Loaded;
        }

        private async void TaskListView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if(DataContext is TaskDayEditorViewModel viewModel)
            {
                await viewModel.Load();
            }
        }
    }
}