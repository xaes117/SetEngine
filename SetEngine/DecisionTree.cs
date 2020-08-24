using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetEngine
{
	public static class DecisionTree
    {
        public static void createDiscreteSets(Dictionary<int, string> inputSet, string filePath, BlobPusher pusher)
        {
			
            List<Dictionary<int, string>> dictionaryList = inputSet.GroupBy(pair => pair.Value)
                                                                     .Select(d => d.ToDictionary(pair => pair.Key, pair => pair.Value))
                                                                     .ToList();

            foreach (Dictionary<int, string> dict in dictionaryList)
            {
                pusher.push(dict, filePath + "/" + dict.First().Value);
            }

        }

        public static void createPercentiles(Dictionary<int, string> inputSet, string filePath, BlobPusher pusher)
        {

            decimal numberOfGroups = 100;
            int i = 0;

            if (inputSet.Count < 100)
            {
                numberOfGroups = 10;
            }

            int groupSize = Convert.ToInt32(Math.Ceiling((decimal)inputSet.Count / numberOfGroups));

            List<Dictionary<int, string>> result = inputSet.OrderBy(pair => double.Parse(pair.Value))
                                 .GroupBy(x => i++ / groupSize)
                                 .Select(d => d.ToDictionary(pair => pair.Key, pair => pair.Value))
                                 .ToList();

            int counter = 1;
            Console.WriteLine(filePath);
            foreach (Dictionary<int, string> dictionary in result)
            {
                pusher.push(dictionary, filePath + "/" + counter++);
            }
            Console.WriteLine("COUNTER VALUE" + counter);

        }

        public static void parse(Dictionary<int, string> inputSet, string filePath, BlobPusher pusher)
        {

            HashSet<string> values = new HashSet<string>();
            Boolean allNumeric = true;
            double number;
            int numericThreshold = 20;
            double stringThreshold = 0.20;

            foreach (KeyValuePair<int, string> pair in inputSet)
            {
                if (!values.Contains(pair.Value))
                {
                    values.Add(pair.Value);

                }

                if (!double.TryParse(pair.Value, out number))
                {
                    allNumeric = false;
                }

            }

            Console.WriteLine("Unique Values:" + values.Count);

            // If Set only has 2 unique elements
            if (values.Count <= 2)
            {

                createDiscreteSets(inputSet, filePath, pusher);

                // If set only has numeric values
            }
            else if (allNumeric)
            {

                // Numeric values
                Console.WriteLine("Numbers");

                // Check if unique sets can be created 
                if (values.Count < numericThreshold)
                {

                    // Categorise sets
                    createDiscreteSets(inputSet, filePath, pusher);

                }
                else
                {
                    // Create sets based on percentile
                    createPercentiles(inputSet, filePath, pusher);
                }

            }
            else
            {

                // String values
                Console.WriteLine("Strings");

                Console.WriteLine("Ratio = " + ((double)values.Count / (double)inputSet.Count));

                if ((double)values.Count / (double)inputSet.Count < stringThreshold)
                {

                    // Create unique sets
                    createDiscreteSets(inputSet, filePath, pusher);

                }
                else
                {

                    // Do nothing
                    // Console.WriteLine("Do nothing");

                }
            }
        }
    }
}
