using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WhatHaveIDone.Behaviours
{
    public class ButtonHoldBehaviour
    {
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.RegisterAttached(nameof(IsActive), typeof(bool),
           typeof(Button), new PropertyMetadata(false, (o, e) =>
           {
               if (o is Button button)
               {
                   var behaviour = GetOrCreateBehaviour(button);
                   behaviour.IsActive = (bool)e.NewValue;
               }
           }
        ));

        private static readonly IDictionary<Button, ButtonHoldBehaviour> _buttonBehaviours = new Dictionary<Button, ButtonHoldBehaviour>();
        private const int DefaultToFastIntervalThreshold = 5;
        private const int FastIntervalToSuperfastThreshold = 10;

        private static readonly TimeSpan DefaultInterval = TimeSpan.FromMilliseconds(150);
        private static readonly TimeSpan FastInterval = TimeSpan.FromMilliseconds(75);
        private static readonly TimeSpan SuperFastInterval = TimeSpan.FromMilliseconds(35);

        private Button _button;

        private DispatcherTimer _timer;

        private int _timerExecutionCount;

        public ButtonHoldBehaviour(Button button)
        {
            _button = button;

            _button.PreviewMouseLeftButtonDown += PreviewLeftButtonDown;
            _button.PreviewMouseLeftButtonUp += PreviewLeftButtonUp;
            _button.MouseEnter += MouseEnter;
            _button.MouseLeave += MouseLeave;
        }

        private bool IsActive { get; set; }

        public static bool GetIsActive(System.Windows.UIElement element)
        {
            return (bool)element.GetValue(IsActiveProperty);
        }

        public static void SetIsActive(System.Windows.UIElement element, bool value)
        {
            element.SetValue(IsActiveProperty, value);
        }
        private static ButtonHoldBehaviour GetOrCreateBehaviour(Button button)
        {
            if (!_buttonBehaviours.TryGetValue(button, out var behaviour))
            {
                _buttonBehaviours[button] = behaviour = new ButtonHoldBehaviour(button);
            }

            return behaviour;
        }

        private void ButtonHoldExecuteCommand(object sender, EventArgs e)
        {
            if (IsActive)
            {
                _timerExecutionCount++;
                ExecuteButtonCommand();

                if (_timerExecutionCount >= DefaultToFastIntervalThreshold)
                {
                    _timer.Interval = FastInterval;
                }
                if (_timerExecutionCount >= FastIntervalToSuperfastThreshold)
                {
                    _timer.Interval = SuperFastInterval;
                }
            }
        }

        private void ExecuteButtonCommand()
        {
            if (_button.Command.CanExecute(null))
            {
                _button.Command.Execute(null);
            }
        }

        private void MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_timer != null)
            {
                _timer.IsEnabled = true;
            }
        }

        private void MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_timer != null)
            {
                _timer.IsEnabled = false;
            }
        }

        private void PreviewLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                if (_timer != null)
                {
                    RemoveTimer();
                }

                _timerExecutionCount = 0;

                _timer = new DispatcherTimer(DispatcherPriority.Normal)
                {
                    Interval = DefaultInterval
                };
                _timer.Tick += ButtonHoldExecuteCommand;

                _timer.Start();

                e.Handled = true;
            }
        }

        private void PreviewLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RemoveTimer();
            if (_timerExecutionCount == 0)
            {
                ExecuteButtonCommand();
            }
        }
        private void RemoveTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= ButtonHoldExecuteCommand;
                _timer = null;
            }
        }
    }
}