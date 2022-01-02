namespace TRLang.src
{
    class Utilities
    {
        public static string SmartCharToString(char c)
        {
            return c switch
            {
                '\t' => "\\t",
                '\n' => "\\n",
                '\r' => "\\r",
                '\0' => "\\0",
                _ => c.ToString()
            };
        }
    }
}
