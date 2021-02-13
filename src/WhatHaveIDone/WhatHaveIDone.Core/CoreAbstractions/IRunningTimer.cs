using System;

namespace WhatHaveIDone.Core.CoreAbstractions
{
    public interface IRunningTimer
    {
        void Stop();
    }

    public interface IDispatcherTimer
    {
        IRunningTimer StartTimer(TimeSpan interval, Action action);
    }
}