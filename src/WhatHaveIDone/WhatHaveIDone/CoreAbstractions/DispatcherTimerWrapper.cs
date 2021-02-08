using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using WhatHaveIDone.Core.CoreAbstractions;

namespace WhatHaveIDone.CoreAbstractions
{
    public class DispatcherTimerWrapper : IDispatcherTimer
    {
        public IRunningTimer StartTimer(TimeSpan interval, Action action)
        {
            var timer = new DispatcherTimer(DispatcherPriority.Normal);
            
            timer.Interval = interval;

            var runningTimer = new RunningTimer(timer, action);
            return runningTimer;
        }


        private class RunningTimer : IRunningTimer
        {
            private readonly DispatcherTimer _timer;
            private readonly Action _action;

            public RunningTimer(DispatcherTimer timer, Action action)
            {
                _timer = timer;
                _action = action;
                _timer.Tick += Timer_Tick;
                timer.Start();
            }

            public void Stop()
            {
                _timer.Stop();
                _timer.Tick -= Timer_Tick;
            }

            private void Timer_Tick(object sender, EventArgs e)
            {
                _action?.Invoke();
            }
        }
        
    }
}
