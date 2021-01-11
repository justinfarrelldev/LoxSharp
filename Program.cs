using System;
using System.IO;
using System.Collections.Generic;

namespace LoxSharp
{
    class Program
    {
        private static bool hadError { get; set; } = false;
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: jlox [script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                runFile(args[0]);
            }
            else
            {
                runPrompt();
            }
        }

        private static void runFile(string path)
        {
            string file = System.IO.File.ReadAllText(@path);
            run(file);

            if (hadError) Environment.Exit(65);
        }

        private static void runPrompt()
        {
            string userInput = "";

            while (userInput.ToLower() != "quit" && userInput.ToLower() != "exit")
            {
                Console.Write("> ");
                userInput = Console.ReadLine();
                if (userInput == null) break;
                run(userInput);
                hadError = false;
            }
        }

        private static void run(string source)
        {
            Scanner scanner = new LoxSharp.Scanner(source);
            List<Token> tokens = scanner.scanTokens();
            // For now, we're printing the tokens 
            foreach (Token t in tokens)
            {
                Console.WriteLine(t.toString());
            }
        }

        public static void error(int line, string message)
        {
            report(line, "", message);
        }

        private static void report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error{where}: {message}");
            hadError = true;
        }
    }
}
