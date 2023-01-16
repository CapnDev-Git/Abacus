using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.Interpreter;
using Abacus.Tokens;

namespace Abacus
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            // Get the user input & check for any errors
            var input = Console.ReadLine();

            // Break out if the given input is empty
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine(0);
                return 0;
            }

            // Launch the Abacus depending on the mode given by user
            #region ABACUS LAUNCH
            // Get invalid args & print them with the error message
            var unknownArgs = "";
            var nbWrongArgs = 0;
            foreach (var arg in args)
            {
                if (arg == "--rpn") continue;
                unknownArgs += $"'{arg}' ; ";
                nbWrongArgs++;
            }
            
            // Check for non-existing parameters
            if (unknownArgs != "")
            {
                // Better format the error string
                unknownArgs = unknownArgs.Remove(unknownArgs.Length-3);
                
                // Send an error message
                Console.Error.WriteLine(nbWrongArgs == 0
                    ? $"Unknown provided argument [Program.Main]: {unknownArgs}"
                    : $"Unknown provided arguments [Program.Main]: {unknownArgs}");
                Environment.Exit(1);
            }
            else
            {
                // Initialize lexer & output
                var lexer = new Lexer();
                var outputs = new List<int>();
                var expressions = lexer.SplitInput(input);

                // Detect an empty expression (only ';')
                if (expressions.Count == 0)
                {
                    Console.WriteLine(0);
                    return 0;
                }

                // Check for input mode
                if (args.Contains("--rpn"))
                {
                    #region RPN MODE
                    // Add every evaluated expression to the final outputs
                    foreach (var res in from expr in expressions select Lexer.LexerInputRpn(expr) into lexedTokens select Ast.ConvertToAst(lexedTokens) into tree select Ast.SolverAst(tree))
                    {
                        lexer.UpdateRam(res);
                        outputs.Add(res);
                    }

                    // Only print the last result
                    Console.WriteLine(outputs[^1]);
                    #endregion
                }
                else
                {
                    #region ABACUS LAUNCH
                    // Add every evaluated expression to the final outputs
                    foreach (var res in from expr in expressions select lexer.ParseLexer(lexer.LexerInput(expr)) into lexedTokens select lexer.ShuntingYard(lexedTokens) into rpn select Ast.ConvertToAst(rpn) into tree select Ast.SolverAst(tree))
                    {
                        // Update the current RAM state & add the result to the output
                        lexer.UpdateRam(res);
                        outputs.Add(res);
                    }

                    // Only print the last evaluation in the results list
                    Console.WriteLine(outputs[^1]);
                    #endregion
                }
            }
            #endregion
            
            // Returns an error code of 0, everything went fine!
            return 0;
        }
    }
}