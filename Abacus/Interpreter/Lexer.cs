using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Abacus.Tokens;
using Abacus.Tokens.Main_Tokens;
using Abacus.Tokens.Operators;

namespace Abacus.Interpreter
{
    public partial class Lexer
    {
        #region CONSTRUCTOR
        private static List<Variable> _ram;
        public Lexer() { _ram = new List<Variable>(); }
        #endregion

        /**
         * <summary> Returns the list of tokens from user input. </summary>
         * <param name="input"> The string input from the LexerSplitter. </param>
         * <returns> List of tokens in order of appearance. </returns>
         */
        public List<Token> LexerInput(string input)
        {
            // Initialize the list of tokens
            var res = new List<Token>();

            // Process the input string into separate tokens
            var i = 0;
            var inputLen = input.Length;

            // Detect unary sign at the beginning of the input
            if (input[0] == '-' || input[0] == '+')
            {
                // Add the unary sign to the list & start at the next char of the input
                if (input[0] == '-') res.Add(new UnaryMinus());
                else res.Add(new UnaryPlus());
                i++;
            }

            // Go through the input & parse it into tokens
            while (i < inputLen)
            {
                // Reset the current Token value & update j
                var currToken = "";
                var j = i;

                // Switch through easy cases
                switch (input[i])
                {
                    case ' ':
                        i++;
                        continue;

                    case ',':
                    {
                        // Check if the separator is at its place
                        if (res.Count > 0)
                        {
                            var prevToken = res[^1];
                            if (prevToken is Parenthesis {Type: ParenthesisType.Right} or Number or Variable)
                            {
                                // Add the separator to the final output & go to the next token
                                res.Add(new Separator(","));
                                i++;
                                continue;
                            }

                            // Raise an error since the separator is not supposed to be there
                            Console.Error.WriteLine("Syntax Error [Lexer.LexerInput]: Misplaced separator detected.");
                            Environment.Exit(2);
                        }

                        break;
                    }
                }

                // Detect if current char is a digit => current token is a number
                while (j < inputLen && char.IsDigit(input[j]))
                {
                    // Build the string of only digits until there's no more
                    currToken += input[j];
                    j++;
                }

                // If we didn't build a digit from previous while loop, try to build a word
                if (currToken == "")
                {
                    // Potentially detect a function or a variable name
                    while (j < inputLen && (currToken == "" && Regex.IsMatch(input[j].ToString(), "[A-Za-z_]") ||
                                            currToken != "" && Regex.IsMatch(input[j].ToString(), "[A-Za-z0-9_]")))
                    {
                        // Build the string of only letters until there's no more
                        currToken += input[j];
                        j++;
                    }
                }

                // Update the i right after the loop if needed
                if (j != i) i = j - 1;

                // Check if there's at least 1 token in the final list
                if (res.Count > 0)
                {
                    // Get the last pushed token
                    var prevToken = res[^1];

                    // Detect unary sign
                    var isUnary = input[i] == '-' || input[i] == '+';
                    var isParRight = prevToken is Parenthesis &&
                                     ((Parenthesis) prevToken).Type == ParenthesisType.Right;
                    var isOp = prevToken is Operator;
                    var isSep = prevToken is Separator;

                    // Check for the unary operator to really be one
                    if (isUnary && !isParRight && (isOp || isSep))
                    {
                        // If it is then add it to the list of tokens as a regular operator
                        if (input[i] == '-') res.Add(new UnaryMinus());
                        else res.Add(new UnaryPlus());

                        // Go to the next token
                        i++;
                        continue;
                    }
                }

                // Detect & append the current token
                if (currToken == "")
                {
                    // Add the operator token to the output list
                    res.Add(GetOperatorToken(input[i]));
                }
                else
                {
                    // Check if we're dealing with a function or a number
                    if (char.IsLetter(currToken[0]) || currToken[0] == '_')
                    {
                        // Add the function/variable to the output list
                        res.Add(GetFunctionToken(currToken));
                    }
                    else
                    {
                        // Add the number token to the output list
                        res.Add(new Number(currToken));
                    }
                }

                // Go to the next char
                i++;
            }

            // Return the final list of tokens
            return res;
        }
    }
}