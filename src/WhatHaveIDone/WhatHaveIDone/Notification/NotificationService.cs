using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using WhatHaveIDone.Views;

namespace WhatHaveIDone.Notification
{
    public class NotificationService : IDisposable
    {
        private static readonly TimeSpan NotificationInterval = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan NotificationOpenTimeSpan = TimeSpan.FromSeconds(10);

        private Timer _timer;

        public NotificationService(NotificationPopupView notificationPopupView)
        {
            _timer = new Timer { AutoReset = true };
            _timer.Elapsed += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    MoveToBottomRightScreenPosition(notificationPopupView);

                    notificationPopupView.IsOpen = true;

                    await Task.Delay(NotificationOpenTimeSpan);
                    notificationPopupView.IsOpen = false;
                });
            };

            _timer.Interval = NotificationInterval.TotalMilliseconds;
            _timer.Start();
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _timer = null;
        }

        private static void MoveToBottomRightScreenPosition(NotificationPopupView notificationPopupView)
        {
            var screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            var taskBarHeight = screenHeight - System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

            notificationPopupView.HorizontalOffset = screenWidth - notificationPopupView.Width;
            notificationPopupView.VerticalOffset = screenHeight - notificationPopupView.Height - taskBarHeight;
        }
    }
}