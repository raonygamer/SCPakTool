using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public static class Log {
        public static ConsoleColor Color {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public static void Write(object? obj = null, ConsoleColor color = ConsoleColor.White) {
            var oldColor = Console.ForegroundColor;
            Color = color;
            Console.WriteLine(obj);
            Color = oldColor;
        }

        public static void Trace(object? obj = null) => Write($"[SCPakTool {DateTime.Now:HH:mm:ss}] {obj}", ConsoleColor.White);
        public static void Warn(object? obj = null) => Write($"[SCPakTool {DateTime.Now:HH:mm:ss}] {obj}", ConsoleColor.Yellow);
        public static void Error(object? obj = null) => Write($"[SCPakTool {DateTime.Now:HH:mm:ss}] {obj}", ConsoleColor.Red);
    }
}
