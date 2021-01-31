using System;
using System.Collections.Generic;

namespace WhatHaveIDone.Core.Util
{
    public static class DateTimeExtensions
    {
        public static DateTime GetStartOfWeek(this DateTime dateTime)
        {
            return dateTime.AddDays((DayOfWeek.Monday - dateTime.DayOfWeek - 7) % 7).Date;
        }

        public static DateTime GetNextFullHour(this DateTime dateTime)
        {
            var nextHour = (int) Math.Ceiling(dateTime.TimeOfDay.TotalHours);

            if(nextHour == dateTime.TimeOfDay.TotalHours)
            {
                return dateTime.AddHours(1);
            }

            return dateTime.Date.AddHours(nextHour);
        }

        public static IEnumerable<DateTime> IterateFullHoursUntil(this DateTime fromUtc, DateTime toUtc)
        {
            var currentTime = fromUtc.GetNextFullHour();

            if(currentTime == fromUtc.AddHours(1))
            {            
                yield return fromUtc;
            }

            while(currentTime <= toUtc)
            {
                
                yield return currentTime;

                currentTime = currentTime.AddHours(1);
            }
        }
    }
}