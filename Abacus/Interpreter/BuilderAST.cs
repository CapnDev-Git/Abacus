using System;
using System.Collections.Generic;
using Abacus.Tokens;
using Abacus.Tokens.Main_Tokens;
using Abacus.Tokens.Operators;

namespace Abacus.Interpreter
{
    public partial class Ast
    {
        #region ATTRIB, GETTERS & CONSTRUCTOR
        // Attributes

        // Getters
        private Ast LeftChild { get; }
        private Ast RightChild { get; }
        private Token Value { get; }

        // Constructor
        private Ast(Token token, Ast leftChild, Ast rightChild)
        {
            this.Value = token;
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
        }
        #endregion
        
        /**
         * <summary> Returns the Abstract Syntax Tree from a RPN token notation. </summary>
         * <param name="rpn"> The given list of Tokens in RPN notation order. </param>
         * <returns> AST of given RPN. </returns>
         */
        public static Ast ConvertToAst(List<Token> rpn)
        {
            // Store the length & reverse the token list
            var tokensLen = rpn.Count;
            rpn.Reverse();
            
            // Convert to AST (patch empty given expression)
            if (tokensLen == 0) return new Ast(new Number("0"), null, null);
            var (item1, item2) = __ConvertToAST(rpn, 0, tokensLen);
            if (item1 == tokensLen-1) return item2;
            
            // Catch any missing operator error
            Console.Error.WriteLine("Syntax Error [AST.ConvertToAST]: AST couldn't be built properly (invalid function call / not enough operator arguments?).");
            Environment.Exit(2);
            return null;
        }

        /**
         * <summary> Auxiliary function of ConvertToAST. </summary>
         * <param name="rpn"> The given list of Tokens in RPN notation order. </param>
         * <param name="i"> The index of the current Token. </param>
         * <param name="len"> The length of the given list of Tokens. </param>
         * <returns> AST of given RPN. </returns>
         */
        private static Tuple<int, Ast> __ConvertToAST(List<Token> rpn, int i, int len)
        {
            try
            {
                switch (rpn[i])
                {
                    case Operator op:
                    {
                        switch (op)
                        {
                            case UnaryPlus or UnaryMinus:
                            {
                                // Check for potential left child
                                if (i + 1 < len)
                                {
                                    var (item1, item2) = __ConvertToAST(rpn, i + 1, len);
                                    return new Tuple<int, Ast>(item1, new Ast(rpn[i], item2, null));
                                }

                                break;
                            }

                            case Equal:
                            {
                                // Remove the equal, replace it by the var name & keep building
                                var (item1, item2) = __ConvertToAST(rpn, i+1, len);
                                return new Tuple<int, Ast>(item1+1, new Ast(rpn[item1+1], item2, null));
                            }

                            default:
                            {
                                // Check for potential left child
                                if (i + 1 < len)
                                {
                                    // Check for potential right child
                                    if (i + 2 < len)
                                    {
                                        // Double point
                                        var (item1, item2) = __ConvertToAST(rpn, i + 1, len);
                                        var (i1, ast) = __ConvertToAST(rpn, item1 + 1, len);
                                        return new Tuple<int, Ast>(i1, new Ast(rpn[i], item2, ast));
                                    }
                            
                                    // Patch only 1 operand given
                                    Console.Error.WriteLine("Syntax Error [AST.__ConvertToAST]: Missing operand (left child).");
                                    Environment.Exit(2);
                                }

                                break;
                            }
                        }
                        
                        // Check right single point
                        if (i + 2 >= len) return new Tuple<int, Ast>(i + 1, new Ast(rpn[i], null, null));
                        
                        // Patch only 1 operand given  
                        Console.Error.WriteLine("Syntax Error [AST.__ConvertToAST]: Missing operand (right child).");
                        Environment.Exit(2);
                        return null;

                        // Current node is a leaf 
                    }

                    case Function:
                    {
                        // Store the current function number of necessary arguments 
                        var currFuncArity = ((Function) rpn[i]).Arity;

                        // Switch through all possible arity cases of the current function
                        switch (currFuncArity)
                        {
                            case 1:
                                var (item1, item2) = __ConvertToAST(rpn, i + 1, len);
                                return new Tuple<int, Ast>(item1, new Ast(rpn[i], item2, null));
                            case 2:
                                var (i1, ast) = __ConvertToAST(rpn, i + 1, len);
                                var (item3, ast1) = __ConvertToAST(rpn, i1 + 1, len);
                                return new Tuple<int, Ast>(item3, new Ast(rpn[i], ast, ast1));
                            default:
                                Console.Error.WriteLine("Syntax Error [AST.__ConvertToAST: Wrong arity value for given function.");
                                Environment.Exit(2);
                                return null;
                        }
                    }

                    default:
                    {
                        // Current node is a number, just append it to the tree
                        return new Tuple<int, Ast>(i, new Ast(rpn[i], null, null));
                    }
                }
            }
            
            // Patch weird index cases
            catch (ArgumentOutOfRangeException)
            {
                Console.Error.WriteLine("Syntax Error [AST.__ConvertToAST]: Index went OOB when building the AST.");
                Environment.Exit(2);
                return null;
            }
        }

        #region DEBUG PRINTS
        public void PrettyPrint() { this.__PrettyPrint(""); }

        void __PrettyPrint(string spacing)
        {
            if (spacing != "") Console.WriteLine(spacing + "> " + this.Value); else Console.WriteLine(this.Value);
            if (this.LeftChild != null) this.LeftChild.__PrettyPrint(spacing + "--");
            if (this.RightChild != null) this.RightChild.__PrettyPrint(spacing + "--");
        }
        #endregion
    }
}