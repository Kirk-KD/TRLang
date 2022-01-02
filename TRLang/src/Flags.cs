namespace TRLang.src
{
    class Flags
    {
        public static bool LogAll { get; private set; }
        public static bool LogLexer { get; private set; }
        public static bool LogParser { get; private set; }
        public static bool LogInterpreter { get; private set; }
        public static bool LogSemanticAnalyser { get; private set; }
        public static bool LogSymbolTables { get; private set; }

        public static bool PauseAfterExecuting { get; private set; }
        public static bool ShowInnerStacktrace { get; private set; }

        public static void InitFlags
        (
            bool all, bool lexer, bool parser, bool interpreter, bool semantic, bool symtab,
            bool pause, bool trace
        )
        {
            LogAll = all;
            LogLexer = lexer || all;
            LogParser = parser || all;
            LogInterpreter = interpreter || all;
            LogSemanticAnalyser = semantic || all;
            LogSymbolTables = symtab || all;

            PauseAfterExecuting = pause;
            ShowInnerStacktrace = trace;
        }
    }
}
