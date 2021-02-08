using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace WhatHaveIDone.Persistence
{
    public class DateTimeToUtcDateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeToUtcDateTimeConverter(): base(x=> DateTime.SpecifyKind(x, DateTimeKind.Utc), x => DateTime.SpecifyKind(x, DateTimeKind.Utc))
        {

        }
    }
}