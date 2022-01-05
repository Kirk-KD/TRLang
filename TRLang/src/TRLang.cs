using CommandLine;
using System;
using TRLang.src;
using TRLang.src.Lexer;
using TRLang.src.Parser.AbstractSyntaxTree;
using Error = TRLang.src.Error.Error;
using Parser = TRLang.src.Parser.Parser;

namespace TRLang
{
    internal class TRLang
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
                options.LogAll, options.LogLexer, options.LogParser, options.LogInterpreter, options.LogSemanticAnalyser, options.LogSymbolTables, options.LogCallStack,
                options.PauseAfterExecuting, options.ShowInnerStacktrace
            );

            // Get code to execute
            string text = String.Empty;

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

        private static void Execute(string text)
        {
            Lexer lexer = new Lexer(text);
            Parser parser = new Parser(lexer);

            AstNode parserResult = parser.Parse();

            SemanticAnalyser semanticAnalyser = new SemanticAnalyser(parserResult);
            semanticAnalyser.Analyse();

            Interpreter interpreter = new Interpreter(parserResult);
            interpreter.Interpret();
        }
    }
}
