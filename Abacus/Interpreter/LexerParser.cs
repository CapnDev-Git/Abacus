using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Abacus.Tokens;
using Abacus.Tokens.Main_Tokens;
using Abacus.Tokens.Operators;

namespace Abacus.Interpreter
{
    public partial class Lexer
    {
        /**
         * <summary> Checks the lexed expression. </summary>
         * <param name="lexer"> The given lexed expression. </param>
         * <returns> Exact same list as input if no error has been found. </returns>
         */
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: Abacus.Tokens.Operators.Multiplication")]
        public List<Token> ParseLexer(List<Token> lexer)
        {
            // Compute the length
            var lexerLen = lexer.Count;

            // Patch case where first token is an operator
            if (lexer[0] is Operator && lexer[0] is not UnaryPlus && lexer[0] is not UnaryMinus && lexer[0] is not Parenthesis)
            {
                // Raise a syntax error
                Console.Error.WriteLine("Syntax Error [Lexer.ParseLexer]: First token can't be an operator.");
                Environment.Exit(2);
            }
            
            // Patch case where given input is in RPN but expected normal mode
            if (lexer[^1] is Operator && lexer[^1] is not Parenthesis || lexer[^1] is Function)
            {
                // Raise a syntax error
                Console.Error.WriteLine("Syntax Error [Lexer.ParseLexer]: Invalid input mode.");
                Environment.Exit(2);
            }
            
            // Go through all the tokens from the lexed expression
            for (var i = 0; i < lexerLen; i++)
            {
                // Switch through all possible tokens
                switch (lexer[i])
                {
                    case Function:
                    {
                        // Patch case where no parenthesis after a function name
                        if (i + 1 < lexerLen && (lexer[i+1] is not Parenthesis || lexer[i+1] is Parenthesis && ((Parenthesis)lexer[i+1]).Type != ParenthesisType.Left))
                        {
                            // Raise a syntax error
                            Console.Error.WriteLine("Syntax Error [Lexer.ParseLexer]: Function called without parentheses.");
                            Environment.Exit(2);
                        }
                        
                        break;
                    }

                    case Number:
                    {
                        // Check for the previous token to either be another number or a right parenthesis
                        if (i - 1 >= 0 && (lexer[i - 1] is Number || lexer[i - 1] is Parenthesis &&
                            ((Parenthesis) lexer[i - 1]).Type == ParenthesisType.Right))
                        {
                            // Raise a syntax error
                            Console.Error.WriteLine("Syntax Error [Lexer.ParseLexer]: Impossible consecutive tokens.");
                            Environment.Exit(2);
                        }
                        
                        break;
                    }

                    case Equal:
                    {
                        // Check for an equality after anything but a variable
                        if (i - 1 >= 0 && lexer[i - 1] is not Variable)
                        {
                            // Raise a syntax error
                            Console.Error.WriteLine("Syntax Error [Lexer.ParseLexer]: Invalid assignation.");
                            Environment.Exit(2);
                        }
                        
                        break;
                    }

                    case Variable var:
                    {
                        // Detect assignation
                        if (i + 1 < lexerLen && lexer[i + 1] is Equal && var.Value == null)
                        {
                            _ram.Add(var);
                            break;
                        }

                        // Detect usage or re-assignation
                        foreach (var variable in _ram.Where(variable => variable.Name == var.Name).Where(variable => i + 1 < lexerLen && lexer[i + 1] is not Equal || lexer[^1] == var))
                        {
                            // Replace the variable token by its value
                            lexer[i] = new Number(variable.Value.ToString());
                                    
                            // Patch single case where variable is in reverse implicit multiplication
                            if (i - 1 >= 0 && lexer[i - 1] is Parenthesis &&
                                ((Parenthesis) lexer[i - 1]).Type == ParenthesisType.Right)
                            {
                                lexer.Insert(i, new Multiplication());
                                lexerLen = lexer.Count;
                            }
                                    
                            break;
                        }
                        
                        // Detect implicit multiplication
                        if (i - 1 >= 0 && lexer[i - 1] is Number)
                        {
                            // Add a multiplication operator
                            lexer.Insert(i, new Multiplication());
                            lexerLen = lexer.Count;
                        }

                        break;
                    }

                    case Parenthesis par:
                    {
                        // Patch empty parentheses
                        if (i - 1 >= 0 && lexer[i - 1] is Parenthesis && ((Parenthesis) lexer[i - 1]).Type == ParenthesisType.Left && par.Type == ParenthesisType.Right)
                        {
                            Console.Error.WriteLine("Syntax Error [Lexer.ParseLexer]: Empty parentheses detected.");
                            Environment.Exit(2);
                        }
                        
                        // Detect implicit multiplication
                        if (i - 1 >= 0 && lexer[i - 1] is Number && par.Type == ParenthesisType.Left)
                        {
                            // Add a multiplication operator
                            lexer.Insert(i, new Multiplication());
                            lexerLen = lexer.Count;
                        }

                        // Patch unbound implicit multiplication
                        if (i - 1 >= 0 && lexer[i - 1] is Variable && ((Variable) lexer[i - 1]).Value == null &&
                            par.Type == ParenthesisType.Left)
                        {
                            Console.Error.WriteLine("Runtime Exception [Lexer.LexerParser]: Unbound variable.");
                            Environment.Exit(3);
                        }
                        
                        // Implicit multiplication for parenthesis expressions
                        if (i + 1 < lexerLen && par.Type == ParenthesisType.Right && lexer[i + 1] is Parenthesis && ((Parenthesis) lexer[i + 1]).Type == ParenthesisType.Left)
                        {
                            // Add a multiplication operator
                            lexer.Insert(i+1, new Multiplication());
                            lexerLen = lexer.Count;
                        }

                        break;
                    }
                }
            }

            return lexer;
        }
    }
}