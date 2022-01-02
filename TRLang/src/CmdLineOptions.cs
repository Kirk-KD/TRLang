using CommandLine;

namespace TRLang
{
    class CmdLineOptions
    {
        // Basic parameters
        [Option('e', "execute", Required = false, HelpText = "Execute code directly from the command.")]
        public string Code { get; private set; }

        [Value(0, MetaName = "file", HelpText = "The path to the file to run.")]
        public string Path { get; private set; }

        // Logging flags
        [Option("log-all", Required = false, HelpText = "Log everything (Ignores other logging flags).")]
        public bool LogAll { get; private set; }

        [Option("log-lexer", Required = false, HelpText = "Log lexer activities (tokenizing).")]
        public bool LogLexer { get; private set; }

        [Option("log-parser", Required = false, HelpText = "Log parser activities (generating AST).")]
        public bool LogParser { get; private set; }

        [Option("log-interpreter", Required = false, HelpText = "Log interpreter activities (visiting AST).")]
        public bool LogInterpreter { get; private set; }

        [Option("log-semantic", Required = false, HelpText = "Log semantic analyser activities (checking AST).")]
        public bool LogSemanticAnalyser { get; private set; }

        [Option("log-symtab", Required = false,
            HelpText = "Log symbol table activities (symbol insertions and lookups) and contents of symbol tables when exiting the corresponding scopes.")]
        public bool LogSymbolTables { get; private set; }

        // Other flags
        [Option('p', "pause", Required = false, HelpText = "Press any key to exit after executing.")]
        public bool PauseAfterExecuting { get; private set; }

        [Option('t', "trace", Required = false,
            HelpText = "Display the internal stacktrace when an error occurred (not including errors that occurred inside the interpreter itself).")]
        public bool ShowInnerStacktrace { get; private set; }
    }
}
