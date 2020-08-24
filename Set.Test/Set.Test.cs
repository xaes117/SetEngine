using System;
using SE = SetEngine;
using DBConnector;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Set.Test
{
    [TestClass]
    public class SetTest
    {

        List<Dictionary<int, string>> predictedAnswer = SE.SetEngine.GenerateSets(
            new DBConnection("127.0.0.1", "testdb", "root", "password"), true);

        [TestMethod]
        public void SetOne()
        {

            // Arrange
            List<KeyValuePair<int, string>> testList = new List<KeyValuePair<int, string>>();
            testList.Add(new KeyValuePair<int, string>(1, "2"));
            testList.Add(new KeyValuePair<int, string>(2, "5"));

            Dictionary<int, string> setOne = predictedAnswer[0];
            CompareSets(testList, setOne);

        }

        [TestMethod]
        public void SetTwo()
        {
            List<KeyValuePair<int, string>> testList = new List<KeyValuePair<int, string>>();
            testList.Add(new KeyValuePair<int, string>(1, "hello"));
            testList.Add(new KeyValuePair<int, string>(2, "world"));

            Dictionary<int, string> setTwo = predictedAnswer[1];
            CompareSets(testList, setTwo);
        }

        private static void CompareSets(List<KeyValuePair<int, string>> testSet, Dictionary<int, string> predictedSet)
        {
            int iterator = 0;
            foreach (KeyValuePair<int, string> pair in predictedSet)
            {
                Assert.AreEqual(testSet[iterator].Key, pair.Key);
                Assert.AreEqual(testSet[iterator].Value, pair.Value);
                iterator++;
            }
        }
    } 
}
