using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Abacus.Tokens;
using Abacus.Tokens.Main_Tokens;

namespace Abacus.Interpreter
{
    public partial class Lexer
    {
        /**
         * <summary> Returns the list of tokens from user input. </summary>
         * <param name="input"> The string input directly from Stdin input. </param>
         * <returns> List of tokens in order of appearance. </returns>
         */
        public static List<Token> LexerInputRpn(string input)
        {
            // Initialize the list of tokens
            var res = new List<Token>();

            // Process the input string into separate tokens
            var i = 0;
            var inputLen = input.Length;
            
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