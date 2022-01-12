using System;
using System.Collections.Generic;
using System.Text.Json;
using TRLang.src;
using TRLang.src.CallStack;
using TRLang.src.Error;
using TRLang.src.Parser.AbstractSyntaxTree;
using Lexer = TRLang.src.Lexer.Lexer;
using Parser = TRLang.src.Parser.Parser;

namespace Tests
{
    class TestCase
    {
        private readonly string _text;
        private readonly Dictionary<string, Dictionary<string, object[]>> _arExpectations;

        public TestCase(string text, Dictionary<string, Dictionary<string, object[]>> arExpectations)
        {
            this._text = text;
            this._arExpectations = arExpectations;
        }

        public TestResult RunTest()
        {
            Lexer lexer = new Lexer(this._text);
            Parser parser = new Parser(lexer);

            try
            {
                // Try generating tokens, parsing, doing semantic analyse, and interpreting
                AstNode parserResult = parser.Parse();

                SemanticAnalyser semanticAnalyser = new SemanticAnalyser(parserResult);
                semanticAnalyser.Analyse();

                Interpreter interpreter = new Interpreter(parserResult);
                interpreter.Interpret();

                // Check if all the expected variables exist and have correct values in each corresponding AR
                foreach (KeyValuePair<string, Dictionary<string, object[]>> nameArPair in this._arExpectations)
                {
                    string name = nameArPair.Key;
                    ActivationRecord record = null;

                    foreach (ActivationRecord ar in interpreter.ArHistory)
                        if (ar.Name == name) record = ar;

                    // Check if AR exists
                    if (record == null) return new TestResult(name);

                    // Check if each variable exists and has correct value
                    foreach (KeyValuePair<string, object[]> keyValuePair in nameArPair.Value)
                    {
                        string key = keyValuePair.Key;
                        if (!record.ContainsKey(key)) return new TestResult(record, key);

                        JsonElement rawExpected = (JsonElement)keyValuePair.Value[1];

                        string type = ((JsonElement)keyValuePair.Value[0]).GetString();
                        object expected;

                        switch (type)
                        {
                            case "int": expected = rawExpected.GetInt32(); break;
                            case "dbl": expected = rawExpected.GetDouble(); break;
                            case "str": expected = rawExpected.GetString(); break;

                            default: throw new Exception($"Invalid type specification \"{type}\"");
                        }

                        object actual = record.Get(key);
                        if (!actual.Equals(expected)) return new TestResult(record, key, expected, actual);
                    }
                }

                return new TestResult();
            }
            catch (Error err)
            {
                return new TestResult(err);
            }
            catch (Exception err)
            {
                return new TestResult(err);
            }
        }
    }
}
