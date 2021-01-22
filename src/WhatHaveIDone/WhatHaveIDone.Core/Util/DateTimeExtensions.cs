using System;

namespace WhatHaveIDone.Core.Util
{
    public static class DateTimeExtensions
    {
        public static DateTime GetStartOfWeek(this DateTime dateTime)
        {
            return dateTime.AddDays((DayOfWeek.Monday - dateTime.DayOfWeek - 7) % 7).Date;
        }
    }
}