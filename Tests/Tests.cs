using System;
using System.IO;
using System.Text.Json;

using ExpectedActivationRecords = System.Collections.Generic.Dictionary
<
    string, System.Collections.Generic.Dictionary
    <
        string, System.Collections.Generic.Dictionary<string, object[]>
    >
>;

namespace Tests
{
    internal class Tests
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Preparing inputs.");

            string testScriptsPath = @"..\..\..\test_trlang_scripts\";
            string jsonPath = @"..\..\..\test_expectations.json";

            ExpectedActivationRecords expectedActivationRecords =
                JsonSerializer.Deserialize<ExpectedActivationRecords>(File.ReadAllText(jsonPath));

            Console.WriteLine("Starting tests.");

            int passes = 0;
            foreach (string name in expectedActivationRecords.Keys)
            {
                string path = $"{testScriptsPath}\\{name}.tr";
                string text = File.ReadAllText(path);

                TestCase testCase = new TestCase(text, expectedActivationRecords[name]);
                TestResult testResult = testCase.RunTest();

                Console.WriteLine($"Test case \"{name}\": {testResult}");

                if (testResult.Success) passes++;
            }

            Console.WriteLine($"Tests done, passed {passes}/{expectedActivationRecords.Count} test case(s).");

            Console.ReadKey();
        }
    }
}
