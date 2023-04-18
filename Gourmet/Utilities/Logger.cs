using System.Drawing;
using Console = Colorful.Console;


namespace Gourmet.Utilities
{
    internal class Logger
    {
        public static void Log(string Prefix, string Msg, Color color)
        {
            Console.Write(" {", color);
            Console.Write(Prefix, Color.White);
            Console.Write("} ", color);
            Console.Write(Msg + "\n", color);
        }
    }
}
