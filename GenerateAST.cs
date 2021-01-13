using System;
using System.Collections.Generic;
using System.IO;

namespace ASTGenerator
{
    public static class GenerateAST
    {
        private static string fileText { get; set; } = "";
        public static void GenerateASTFromPath(string path)
        {
            defineAst(path, "Expr", new List<string>(){
                "Binary   : Expr left, Token op, Expr right",
                "Grouping : Expr expression",
                "Literal  : object value",
                "Unary    : Token op, Expr right"
            });
        }

        private static void defineAst(string outputDir, string baseName, List<string> types)
        {
            string path = $"{outputDir}/{baseName}.cs";
            fileText = "using LoxSharp;\n" +
                $"namespace {baseName} {{\n" +
                $"\tabstract class {baseName} {{";

            // AST classes
            foreach (string type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                defineType(baseName, className, fields);
            }

            fileText = fileText +
                "\n\t}\n}" + Environment.NewLine;

            File.WriteAllText(outputDir, fileText);
        }

        // Don't wanna use refs so I'll make this return string
        private static void defineType(string baseName, string className, string fieldList)
        {
            fileText += $"\n\t\tclass {className} : {baseName} {{" + Environment.NewLine
                     + $"\t\t\tpublic {className}({fieldList}) {{\n";
            string[] fields = fieldList.Split(", ");
            foreach (string field in fields)
            {
                string name = field.Split(" ")[1];
                fileText += $"\t\t\t\tthis.{name} = {name};\n";
            }
            fileText += "\n\t\t\t}\n";
            foreach (string field in fields)
            {
                fileText += $"\t\t\t{field};\n";
            }
            fileText += "\t\t}";
        }
    }
}