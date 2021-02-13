using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

using WhatHaveIDone.Core.Models;
using WhatHaveIDone.Core.ViewModels;
using WhatHaveIDone.CustomControls;

namespace WhatHaveIDone.UITest.TestCases
{
    public class TaskTimeLineControlTestFixture : ITestCaseCollection
    {
        private static readonly DateTime _startUtc = new DateTime(2021, 1, 1, 23, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime _endUtc = new DateTime(2021, 1, 2, 23, 0, 0, DateTimeKind.Utc);

        private static readonly TaskCategory _greenCategory = new TaskCategory { Color = System.Drawing.Color.Green, Name = "Green" };
        private static readonly TaskCategory _redCategory = new TaskCategory { Color = System.Drawing.Color.Red, Name = "Red" };

        public IEnumerable<ITestCase> GetAllTestCases()
        {
            yield return new TestCase
            {
                Name = "Empty Timeline",
                StartUtc = _startUtc,
                EndUtc = _endUtc,
                Tasks = new ObservableCollection<TaskViewModel>()
            };

            yield return new TestCase
            {
                Name = "unfinished Task",
                StartUtc = _startUtc,
                EndUtc = _endUtc,
                Tasks = new ObservableCollection<TaskViewModel>()
                {
                    new TaskViewModel
                    {
                        Begin = _startUtc.AddHours(1),
                        Category = _greenCategory
                    }
                }
            };

            yield return new TestCase
            {
                Name = "finished Task",
                StartUtc = _startUtc,
                EndUtc = _endUtc,
                Tasks = new ObservableCollection<TaskViewModel>()
                {
                    new TaskViewModel
                    {
                        Begin = _startUtc.AddHours(1),
                        End = _startUtc.AddHours(3),
                        Category = _greenCategory
                    }
                }
            };

            yield return new TestCase
            {
                Name = "two finished Task",
                StartUtc = _startUtc,
                EndUtc = _endUtc,
                Tasks = new ObservableCollection<TaskViewModel>()
                {
                    new TaskViewModel
                    {
                        Begin = _startUtc.AddHours(1),
                        End = _startUtc.AddHours(3),
                        Category = _greenCategory
                    },

                    new TaskViewModel
                    {
                        Begin = _startUtc.AddHours(5),
                        End = _startUtc.AddHours(8),
                        Category = _redCategory
                    }
                }
            };
        }

        private class TestCase : ITestCase
        {
            public ObservableCollection<TaskViewModel> Tasks { get; set; }

            public DateTime StartUtc { get; set; }
            public DateTime EndUtc { get; set; }
            public string Name { get; set; }

            public UserControl CreateControl()
            {
                return new TaskTimelineControl
                {
                    TimeLineStart = StartUtc,
                    TimeLineEnd = EndUtc,
                    Tasks = Tasks
                };
            }
        }
    }
}