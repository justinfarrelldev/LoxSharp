using System;
using System.IO;

namespace LoxSharp
{
    class Program
    {
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
            string[] file = System.IO.File.ReadAllLines(@path);
            run(file);
        }

        private static void runPrompt()
        {
            string userInput = "";

            while (userInput.ToLower() != "quit" && userInput.ToLower() != "exit")
            {
                Console.Write("> ");
                userInput = Console.ReadLine();
            }
        }

        private static void run(string[] source)
        {
            // For now, we're printing the tokens 
            foreach (string s in source)
            {
                Console.WriteLine(s);
            }
        }
    }
}
