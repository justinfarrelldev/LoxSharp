using System;
using System.IO;
using System.Collections.Generic;
using ASTGenerator;

namespace LoxSharp
{
    class Program
    {
        private static Interpreter interpreter = new Interpreter();
        private static bool hadError { get; set; } = false;
        private static bool hadRuntimeError { get; set; } = false;
        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                Console.WriteLine("Usage: jlox [script] [DEBUG REMOVE ME AST PATH]");
                Environment.Exit(64);
            }
            else if (args.Length == 2)
            {
                ASTGenerator.GenerateAST.GenerateASTFromPath(args[1]);
                runFile(args[0]);
            }
            else if (args.Length == 1)
            {
                runFile(args[0]);
                AstPrinter.Test();
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
            if (hadRuntimeError) Environment.Exit(70);
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
            Parser parser = new Parser(tokens);
            ExprNamespace.Expr expression = parser.parse();

            // Stop if there was an error.
            if (hadError) return;
            interpreter.interpret(expression);
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

        public static void error(Token token, string message)
        {
            if (token.type == TokenType.EOF)
            {
                report(token.line, " at end", message);
            }
            else
            {
                report(token.line, $" at '{token.lexeme}'", message);
            }
        }

        public static void runtimeError(Errors.RuntimeError error)
        {
            Console.WriteLine(error.Message + $"\n[line {error.token.line}]");
            hadRuntimeError = true;
        }
    }
}
