using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
