using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;

namespace WhatHaveIDone.UITest.TestCases
{
    public interface ITestCase
    {
        string Name { get; }
        UserControl CreateControl();
    }

    public interface ITestCaseCollection
    {
        IEnumerable<ITestCase> GetAllTestCases();
    }

    public static class TestCaseDiscovery
    {
        public static IEnumerable<ITestCase> DiscoverAllTestCases(Assembly assembly)
        {
            var testCaseCollections = assembly.GetTypes().
                Where(x => typeof(ITestCaseCollection).IsAssignableFrom(x) && x.IsClass);

            return testCaseCollections.
                Select(x => Activator.CreateInstance(x)).
                Cast<ITestCaseCollection>(). 
                SelectMany(x => x.GetAllTestCases());
        }
    }
}
