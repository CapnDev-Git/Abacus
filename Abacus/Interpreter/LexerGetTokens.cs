using System;
using System.Text.RegularExpressions;
using Abacus.Tokens;
using Abacus.Tokens.Functions;
using Abacus.Tokens.Main_Tokens;
using Abacus.Tokens.Operators;

namespace Abacus.Interpreter
{
    public partial class Lexer
    {
        /**
         * <summary> Returns the token corresponding to the given operator character. Also used in the LexerRPN </summary>
         * <param name="symbol"> Character corresponding to the symbol of the operator. </param>
         * <returns> Operator token of the right type. </returns>
         */
        private static Token GetOperatorToken(char symbol)
        {
            // Switch though all the possible operator symbols
            switch (symbol)
            {
                case '+':
                    return new Addition();
                case '-':
                    return new Subtraction();
                case '*':
                    return new Multiplication();
                case '/':
                    return new Division();
                case '%':
                    return new Modulo();
                case '^':
                    return new Exponent();
                case '(':
                    return new Parenthesis("(");
                case ')':
                    return new Parenthesis(")");
                case '=':
                    return new Equal();
                default:
                    // Patch weird symbols (not number nor operator)
                    Console.Error.WriteLine("Syntax Error [Lexer.GetOperatorToken]: Unexpected token detected.");
                    Environment.Exit(2);
                    return null;
            }
        }

        /**
         * <summary> Returns the token corresponding to the given current token string. Also used in the LexerRPN </summary>
         * <param name="currToken"> String corresponding to the name of the function. </param>
         * <returns> Function token of the right type. </returns>
         */
        private static Token GetFunctionToken(string currToken)
        {
            switch (currToken)
            {
                case "sqrt":
                    return new Sqrt();
                case "max":
                    return new Max();
                case "min":
                    return new Min();
                case "facto":
                    return new Facto();
                case "isprime":
                    return new IsPrime();
                case "fibo":
                    return new Fibo();
                case "gcd":
                    return new GCD();
                default:
                    // Given string of characters can only be a variable name
                    if (Regex.IsMatch(currToken, "[A-Za-z_][A-Za-z0-9_]*"))
                    {
                        // Create the new variable & add it to the RAM
                        var newVar = new Variable(currToken, null);
                        
                        // Return the created variable
                        return newVar;
                    }
                    
                    // If the given variable name doesn't correspond to the RegEx, then it's an 
                    Console.Error.WriteLine("Syntax Error [Lexer.GetFunctionToken]: Unexpected function or invalid variable name detected.");
                    Environment.Exit(2);
                    return null;
            }
        }
    }
}