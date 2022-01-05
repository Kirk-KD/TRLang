using System;
using TRLang.src.CallStack;
using TRLang.src.Error;

namespace Tests
{
    class TestResult
    {
        public bool Success { get; private set; } = false;
        public TestResultStatus ResultStatus { get; private set; } = TestResultStatus.Success;

        public Error LangError { get; private set; }
        public Exception UnexpectedException { get; private set; }
        public ActivationRecord ActivationRecord { get; private set; }
        public string ArName { get; private set; }
        public string VariableName { get; private set; }
        public object ExpectedVariableValue { get; private set; }
        public object ActualVariableValue { get; private set; }

        public TestResult() // No errors, test case passed
        {
            this.Success = true;
        }

        public TestResult(Error error) // An TRLang error occurred
        {
            this.ResultStatus = TestResultStatus.LangError;
            this.LangError = error;
        }

        public TestResult(Exception exception) // An unexpected error occurred
        {
            this.ResultStatus = TestResultStatus.Exception;
            this.UnexpectedException = exception;
        }

        public TestResult(string arName) // An AR was not created
        {
            this.ResultStatus = TestResultStatus.ArNotFound;
            this.ArName = arName;
        }

        public TestResult(ActivationRecord ar, string name) // A variable was not defined
        {
            this.ResultStatus = TestResultStatus.VariableNotDefined;
            this.ActivationRecord = ar;
            this.VariableName = name;
        }

        public TestResult(ActivationRecord ar, string name, object expectedValue, object actualValue) // A variable has unexpected value
        {
            this.ResultStatus = TestResultStatus.IncorrectVariableValue;
            this.ActivationRecord = ar;
            this.VariableName = name;
            this.ExpectedVariableValue = expectedValue;
            this.ActualVariableValue = actualValue;
        }

        public override string ToString()
        {
            if (this.Success) return "PASS";
            else
            {
                return $"FAIL ({this.ResultStatus}): " + (this.ResultStatus switch
                {
                    TestResultStatus.LangError => this.LangError,
                    TestResultStatus.Exception => this.UnexpectedException.ToString(),
                    TestResultStatus.ArNotFound => this.ArName,
                    TestResultStatus.VariableNotDefined => $"Variable \"{this.VariableName}\" expected in AR \"{this.ActivationRecord}\"",
                    TestResultStatus.IncorrectVariableValue =>
                        $"Expecting variable \"{this.VariableName}\" in AR \"{this.ActivationRecord}\" to have value " +
                        $"{this.ExpectedVariableValue} ({this.ExpectedVariableValue.GetType()}), got " +
                        $"{this.ActualVariableValue} ({this.ActualVariableValue.GetType()})",

                    _ => throw new Exception("Invalid Test Result Status.")
                });
            }
        }
    }
}
