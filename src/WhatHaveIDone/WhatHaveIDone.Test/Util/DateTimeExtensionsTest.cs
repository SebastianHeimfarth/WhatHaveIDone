using NUnit.Framework;
using Shouldly;
using System;
using System.Globalization;
using System.Linq;
using WhatHaveIDone.Core.Util;

namespace WhatHaveIDone.Test.Util
{
    public class DateTimeExtensionsTest
    {
        private static readonly DateTime _someMonday = new DateTime(2021, 1, 18, 0, 0, 0, DateTimeKind.Utc);

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void GetStartOfWeek(int offsetFromMonday)
        {
            //arrange
            var dateTime = _someMonday.AddDays(offsetFromMonday);

            //act
            var result = dateTime.GetStartOfWeek();

            //assert
            result.ShouldBe(_someMonday);
        }

        [Test]
        
        public void GetNextFullHour_DateContainsMinuteFractions_ShouldGetNextFullHour([Range(0, 25)] int hourOffset)
        {
            //arrange
            var dateWithMinuteFraction = new DateTime(2021, 1, 18, 1, 30, 0, DateTimeKind.Utc).AddHours(hourOffset);

            //act
            var result = dateWithMinuteFraction.GetNextFullHour();

            //assert
            (result - dateWithMinuteFraction).TotalMinutes.ShouldBe(30d);
        }

        [Test]

        public void GetNextFullHour_DateIsOnFullHour_ShouldGetNextFullHour()
        {
            //arrange
            var dateWithMinuteFraction = new DateTime(2021, 1, 18, 1, 0, 0, DateTimeKind.Utc);

            //act
            var result = dateWithMinuteFraction.GetNextFullHour();

            //assert
            (result - dateWithMinuteFraction).TotalMinutes.ShouldBe(60d);
        }


        [Test]
        
        public void IterateFullHoursUntil_WinterToSummerTime_ShouldNotIncludeInvalidTime()
        {
            NewMethod();
            Console.WriteLine(TimeZoneInfo.Local.ToString());
            //arrange
            var start = new DateTime(2021, 3, 27, 23, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(2021, 3, 28, 22, 0, 0, DateTimeKind.Utc);
            var invalidTime = new DateTime(2021, 3, 28, 2, 0, 0, DateTimeKind.Local);
            TimeZoneInfo.Local.IsInvalidTime(invalidTime).ShouldBe(true);

            //act
            var result = start.IterateFullHoursUntil(end).ToList();

            //assert
            var containsInvalidTime = result.Any(x => TimeZoneInfo.Local.IsInvalidTime(x));
            containsInvalidTime.ShouldBe(false);
        }

        private static void NewMethod()
        {
            TimeZoneInfo.Local.BaseUtcOffset.ShouldBe(TimeSpan.FromHours(1), "This test should be run in W. Europe Time Zone");
        }

        [Test]

        public void IterateFullHoursUntil_DayLightSavings_ShouldInclude()
        {
            //arrange
            var start = new DateTime(2021, 3, 27, 23, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(2021, 3, 28, 22, 0, 0, DateTimeKind.Utc);
            var invalidTime = new DateTime(2021, 3, 28, 2, 0, 0, DateTimeKind.Local);
            TimeZoneInfo.Local.IsInvalidTime(invalidTime).ShouldBe(true);

            //act
            var result = start.IterateFullHoursUntil(end).ToList();

            //assert
            var containsInvalidTime = result.Any(x => TimeZoneInfo.Local.IsInvalidTime(x));
            containsInvalidTime.ShouldBe(false);
        }
    }
}