using System;
using System.Collections.Generic;
using System.IO;

namespace ASTGenerator
{
    public class GenerateAST
    {
        public static void GenerateASTFromPath(string path)
        {
            defineAst(path, "Expr", new List<string>(){
                "Binary   : Expr left, Token operator, Expr right",
                "Grouping : Expr expression",
                "Literal  : Object value",
                "Unary    : Token operator, Expr right"
            });
        }

        private static void defineAst(string outputDir, string baseName, List<string> types)
        {
            string path = $"{outputDir}/{baseName}.cs";
            string fileText =
                $"namespace {baseName}Namespace" + Environment.NewLine +
                "{" + Environment.NewLine +
                $"\tclass {baseName}" +
                "\t{" + Environment.NewLine +
                "\t}" + Environment.NewLine +
                "}" + Environment.NewLine;

            File.WriteAllText(outputDir, fileText);
        }
    }
}