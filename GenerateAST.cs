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

            defineVisitor(baseName, types);

            // AST classes
            foreach (string type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                defineType(baseName, className, fields);
            }

            fileText += "\n\tpublic abstract R accept<R>(Visitor<R> visitor);";

            fileText = fileText +
                "\n\t}\n}" + Environment.NewLine;

            File.WriteAllText(outputDir, fileText);
        }

        private static void defineVisitor(string baseName, List<string> types)
        {
            fileText += "\n\t\tpublic interface Visitor<R> {\n";
            foreach (string type in types)
            {
                string typeName = type.Split(":")[0].Trim();
                fileText += $"\t\t\tR visit{typeName + baseName}({typeName} {baseName.ToLower()});\n";
            }
            fileText += "\t\t}";
        }

        // Don't wanna use refs so I'll make this return string
        private static void defineType(string baseName, string className, string fieldList)
        {


            fileText += $"\n\t\tpublic class {className} : {baseName} {{" + Environment.NewLine
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

            fileText += "\t\t\tpublic override R accept<R>(Visitor<R> visitor) {\n";
            fileText += $"\t\t\t\treturn visitor.visit{className}{baseName}(this);\n";
            fileText += "\t\t\t}\n";
            fileText += "\t\t}\n";
        }
    }
}