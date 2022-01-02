using CommandLine;
using System;
using System.Collections.Generic;
using TRLang.src;
using TRLang.src.Interpreter;
using TRLang.src.Lexer;
using TRLang.src.Parser.AbstractSyntaxTree;
using Error = TRLang.src.Error.Error;
using Parser = TRLang.src.Parser.Parser;

namespace TRLang
{
    class TRLang
    {
        static void Main(string[] args)
        {
            ParserResult<CmdLineOptions> result = CommandLine.Parser.Default.ParseArguments<CmdLineOptions>(args);
            result.WithParsed(RunOptions);
        }

        static void RunOptions(CmdLineOptions options)
        {
            // Set flags
            Flags.InitFlags
            (
                options.LogAll, options.LogLexer, options.LogParser, options.LogInterpreter, options.LogSemanticAnalyser, options.LogSymbolTables,
                options.PauseAfterExecuting, options.ShowInnerStacktrace
            );

            // Get code to execute
            string text = "";

            string cmdLineCode = options.Code;
            string filePath = options.Path;

            if (filePath != null)
            {
                filePath = System.IO.Path.GetFullPath(filePath);

                try
                {
                    text = System.IO.File.ReadAllText(filePath);
                }
                catch (System.IO.FileNotFoundException)
                {
                    Console.WriteLine($"Cannot find file \"{filePath}\".");
                    return;
                }
                catch (System.IO.IOException err)
                {
                    Console.WriteLine($"Cannot read file \"{filePath}\" (Message: {err.Message})");
                    return;
                }
            }
            else if (cmdLineCode != null) text = cmdLineCode;

            if (text.Length != 0)
            {
                try
                {
                    Execute(text);
                }
                catch (Error e)
                {
                    Console.WriteLine($"[!] {e.Message}");
                    if (Flags.ShowInnerStacktrace) Console.WriteLine($"[!] Actual Trace:\n[!] {e.StackTrace.Replace("\n", "\n[!] ")}");
                }

                if (Flags.PauseAfterExecuting) Console.ReadKey();
            }
        }

        private static void Execute(string text)
        {
            Lexer lexer = new Lexer(text);
            Parser parser = new Parser(lexer);

            AstNode parserResult = parser.Parse();

            SemanticAnalyser semanticAnalyser = new SemanticAnalyser(parserResult);
            semanticAnalyser.Analyse();

            Interpreter interpreter = new Interpreter(parserResult);
            interpreter.Interpret();

            Console.WriteLine("GlobalMemory Contents:");
            foreach (KeyValuePair<string, object> entry in interpreter.GlobalMemory)
            {
                Console.WriteLine($"  {entry.Key} = {entry.Value}");
            }
        }
    }
}
