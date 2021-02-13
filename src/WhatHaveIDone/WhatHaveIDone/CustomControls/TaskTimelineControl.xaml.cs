using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WhatHaveIDone.Core.Util;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.CustomControls
{
    /// <summary>
    /// Interaction logic for TaskTimelineControl.xaml
    /// </summary>
    public partial class TaskTimelineControl : UserControl
    {
        #region SelectedTask-Property

        public static readonly DependencyProperty SelectedTaskProperty =
            DependencyProperty.Register(nameof(SelectedTask), typeof(TaskViewModel), typeof(TaskTimelineControl), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedTaskChanged)));

        private static void OnSelectedTaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public TaskViewModel SelectedTask
        {
            get { return (TaskViewModel)GetValue(SelectedTaskProperty); }
            set
            {
                SetValue(SelectedTaskProperty, value);
            }
        }

        #endregion SelectedTask-Property

        #region Tasks-Property

        public static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register(nameof(Tasks), typeof(ObservableCollection<TaskViewModel>), typeof(TaskTimelineControl));

        public ObservableCollection<TaskViewModel> Tasks
        {
            get { return (ObservableCollection<TaskViewModel>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }

        #endregion Tasks-Property

        #region TimeLineEnd-Property

        public static readonly DependencyProperty TimeLineEndProperty =
            DependencyProperty.Register(nameof(TimeLineEnd), typeof(DateTime), typeof(TaskTimelineControl), new PropertyMetadata(default(DateTime), OnTimeLineEndChanged));

        public DateTime TimeLineEnd
        {
            get { return (DateTime)GetValue(TimeLineEndProperty); }
            set { SetValue(TimeLineEndProperty, value); }
        }

        private static void OnTimeLineEndChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = (TaskTimelineControl)dependencyObject;
            control.HandleOnTimeLineEndChanged();
        }

        #endregion TimeLineEnd-Property

        #region TimeLineStart-Property

        public static readonly DependencyProperty TimeLineStartProperty =
            DependencyProperty.Register(nameof(TimeLineStart), typeof(DateTime), typeof(TaskTimelineControl), new PropertyMetadata(default(DateTime), OnTimeLineStartChanged));

        public DateTime TimeLineStart
        {
            get { return (DateTime)GetValue(TimeLineStartProperty); }
            set { SetValue(TimeLineStartProperty, value); }
        }

        private static void OnTimeLineStartChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = (TaskTimelineControl)dependencyObject;
            control.HandleOnTimeLineStartChanged();
        }

        #endregion TimeLineStart-Property

        public TaskTimelineControl()
        {
            DataContext = this;
            InitializeComponent();
            TaskClicked = new TaskClickedCommand(this);
            this.SizeChanged += TaskTimelineControl_SizeChanged;
        }

        public ICommand TaskClicked { get; }

        public const int ExtraSpacingOnBeginningAndEnd = 60;
        private double ScalingFactorForMinutes => ActualWidth / (TimeLineLength.TotalMinutes + 2 * ExtraSpacingOnBeginningAndEnd);

        private TimeSpan TimeLineLength => TimeLineEnd - TimeLineStart;

        private static PathFigure CreateLine(Point start, Point end)
        {
            return new PathFigure
            {
                StartPoint = start,
                Segments = new PathSegmentCollection
                {
                    new LineSegment
                    {
                        Point = end
                    }
                }
            };
        }

        private double CalculateHorizontalOffset(DateTime dateTime)
        {
            return (dateTime - TimeLineStart.ToLocalTime()).TotalMinutes * ScalingFactorForMinutes;
        }

        private void HandleOnTimeLineEndChanged()
        {
            UpdateRenderedElements();
        }

        private void HandleOnTimeLineStartChanged()
        {
            UpdateRenderedElements();
        }

        private void TaskTimelineControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateRenderedElements();
        }

        private void UpdateRenderedElements()
        {
            UpdateTimeAxis();
        }

        private Path _timeLine;
        private IList<TextBlock> _timeLineLabels = new List<TextBlock>();

        private void UpdateTimeAxis()
        {
            if (_timeLine != null)
            {
                _canvas.Children.Remove(_timeLine);
                foreach (var label in _timeLineLabels)
                {
                    _canvas.Children.Remove(label);
                }
            }

            const double regularHeight = 7d;
            const double textHeight = 14d;
            const double heightForQuarters = 14d;

            var yOffset = _canvas.ActualHeight;

            var horizontalLine = CreateLine(new Point(0, yOffset), new Point(ActualWidth, yOffset));

            var pathElements = new PathFigureCollection
            {
                horizontalLine,
            };

            var start = TimeLineStart == TimeLineStart.GetNextFullHour().AddHours(-1) ? TimeLineStart : TimeLineStart.GetNextFullHour();

            var startOffset = ScalingFactorForMinutes * ExtraSpacingOnBeginningAndEnd;

            for (var currentHourUtc = start; currentHourUtc <= TimeLineEnd; currentHourUtc = currentHourUtc.AddHours(1))
            {
                var currentHour = currentHourUtc.ToLocalTime();

                var x = CalculateHorizontalOffset(currentHour) + startOffset;

                var isQuarterPartOfTheDay = currentHour.Hour % 3 == 0;

                pathElements.Add(CreateLine(new Point(x, yOffset), new Point(x, yOffset - (isQuarterPartOfTheDay ? heightForQuarters : regularHeight))));

                if (isQuarterPartOfTheDay)
                {
                    TextBlock label = AddLabelForTimeline(currentHour);
                    Canvas.SetLeft(label, x - 5);
                    Canvas.SetTop(label, yOffset - 3d * textHeight);
                }
            }

            var timeLine = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Data = new PathGeometry
                {
                    Figures = pathElements
                },
            };

            _timeLine = timeLine;
            _canvas.Children.Add(timeLine);
        }

        private TextBlock AddLabelForTimeline(DateTime currentHour)
        {
            var label = new TextBlock() { Text = $"{currentHour:HH:mm}", FontSize = 10 };
            label.LayoutTransform = new RotateTransform(-45);

            _canvas.Children.Add(label);
            _timeLineLabels.Add(label);
            return label;
        }

        private class TaskClickedCommand : ICommand
        {
            private readonly TaskTimelineControl _parent;

            public TaskClickedCommand(TaskTimelineControl parent)
            {
                _parent = parent;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TaskViewModel task = (TaskViewModel)parameter;

                if (_parent.SelectedTask == task)
                {
                    _parent.SelectedTask = null;
                }
                else
                {
                    _parent.SelectedTask = task;
                }
            }
        }
    }
}