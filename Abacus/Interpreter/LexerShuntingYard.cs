using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.Tokens;
using Abacus.Tokens.Main_Tokens;
using Abacus.Tokens.Operators;

namespace Abacus.Interpreter
{
    public partial class Lexer
    {
        /**
         * <summary> Returns a list of tokens in postfix order using the Shunting-Yard algorithm (RPN). </summary>
         * <param name="tokens"> Original list of tokens from user input. </param>
         * <returns> RPN notation from infix notation. </returns>
         */
        public List<Token> ShuntingYard(List<Token> tokens)
        {
            // Create both data structures
            var operationStack = new Stack<Token>();
            var outputTokens = new List<Token>();

            // Go through the given list of tokens in order
            int i = 0, tokenLen = tokens.Count;
            while (i < tokenLen)
            {
                // Store the current token
                var currToken = tokens[i];

                // Switch through all possible token cases
                switch (currToken)
                {
                    case Number n:
                        // Add the number to the output
                        outputTokens.Add(n);
                        break;

                    case Function fun:
                        // Add the function onto the stack
                        operationStack.Push(fun);
                        break;

                    case Variable var:
                        // Add the variable to the output
                        outputTokens.Add(var);
                        break;

                    case Separator:
                    {
                        try
                        {
                            // Keep de-stacking until the top of the stack is a left parenthesis
                            while (operationStack.Any() && operationStack.Peek() is not Parenthesis || 
                                   ((Parenthesis) operationStack.Peek()).Type != ParenthesisType.Left)
                            {
                                // Store the currently popped token & add it to the output
                                outputTokens.Add(operationStack.Pop());
                            }
                        }
                        
                        // Catch any stack error
                        catch (InvalidOperationException)
                        {
                            Console.Error.WriteLine("Syntax Error [Lexer.ShuntingYard]: Incorrect function call detected.");
                            Environment.Exit(2);
                        }

                        // Check if we didn't de-stacked everything already
                        if (!operationStack.Any())
                        {
                            Console.Error.WriteLine("Syntax Error [Lexer.ShuntingYard]: Unbalanced parentheses detected for a function.");
                            Environment.Exit(2);
                        }
                        
                        break;
                    }

                    case Parenthesis par:
                    {
                        // If we're on a left parenthesis, add it to the stack
                        if (par.Type == ParenthesisType.Left) operationStack.Push(par);
                        else
                        {
                            // De-stack every element until reaching a left parenthesis (if any)
                            while (operationStack.Any() && (operationStack.Peek() is not Parenthesis ||
                                                            ((Parenthesis) operationStack.Peek()).Type !=
                                                            ParenthesisType.Left))
                            {
                                // Store the currently popped token & add it to the output
                                outputTokens.Add(operationStack.Pop());
                            }

                            // Check if we didn't de-stacked everything already
                            if (!operationStack.Any())
                            {
                                Console.Error.WriteLine("Syntax Error [Lexer.ShuntingYard]: Unbalanced parentheses for an arithmetic expression detected.");
                                Environment.Exit(2);
                            }

                            // De-stack the left parenthesis
                            operationStack.Pop();
                            
                            // Patch functions
                            if (operationStack.Any() && operationStack.Peek() is Function) outputTokens.Add(operationStack.Pop());
                        }

                        break;
                    }
                    
                    case Operator o1:
                    {
                        // Store which operator we're dealing with
                        var o2 = operationStack.Any() ? (Operator) operationStack.Peek() : null;
                        
                        // Check if there isn't already an operator on the stack
                        while (o2 != null &&
                               (o1.Associativity == AssocDirection.Left && o1.Precedence <= o2.Precedence ||
                                o1.Associativity == AssocDirection.Right && o1.Precedence < o2.Precedence))
                        {
                            // Add o2 to the final output & get new o2
                            if (!operationStack.Any()) continue;
                            outputTokens.Add(operationStack.Pop());
                            o2 = operationStack.Any() ? (Operator) operationStack.Peek() : null;
                        }

                        // Put the current operator on the stack
                        operationStack.Push(o1);
                        break;
                    }
                }

                // Go to the next token
                i++;
            }

            // Push the rest of the stack to the output
            while (operationStack.Any())
            {
                // Store the current operator on top of the stack
                var currToken = operationStack.Peek();

                // Check for any left parenthesis which would mean it's not a correct syntax
                if (currToken is Parenthesis {Type: ParenthesisType.Left})
                {
                    Console.Error.WriteLine("Syntax Error [Lexer.ShuntingYard]: Unbalanced parentheses for an arithmetic expression detected.");
                    Environment.Exit(2);
                }
                else
                {
                    // De-stack every left operator in the stack
                    outputTokens.Add(currToken);
                    operationStack.Pop();
                }
            }

            // Return the final output of tokens in RPN
            return outputTokens;
        }
    }
}