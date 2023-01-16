using System;
using System.Linq;
using Abacus.Tokens.Functions;
using Abacus.Tokens.Main_Tokens;
using Abacus.Tokens.Operators;

namespace Abacus.Interpreter
{
    public partial class Ast
    {
        /**
         * <summary> Returns the result of the given computation. </summary>
         * <param name="ast"> The given Abstract Syntax Tree. </param>
         * <returns> Final result from user's input. </returns>
         */
        public static int SolverAst(Ast ast)
        {
            try
            {
                // Check for potential left child
                if (ast.LeftChild == null) return ((Number) ast.Value).Value;
                // Check for potential right child
                if (ast.RightChild != null)
                {
                    // Check double point
                    return ast.Value is Operator ? EvalExpression((Operator) ast.Value, SolverAst(ast.RightChild), SolverAst(ast.LeftChild)) : EvalFunction((Function) ast.Value, SolverAst(ast.LeftChild), SolverAst(ast.RightChild));
                }

                return ast.Value switch
                {
                    // Check for left single node
                    Operator @operator => EvalExpression(@operator, 0, SolverAst(ast.LeftChild)),
                    Function function => EvalFunction(function, SolverAst(ast.LeftChild), 0),
                    Variable => SolverAst(ast.LeftChild),
                    _ => ((Number) ast.Value).Value
                };

                // If it's a number return its value
            }
            
            catch (InvalidCastException)
            {
                // Patch non-defined variable given
                Console.Error.WriteLine("Runtime Exception [AST.SolverAST]: Unbound variable.");
                Environment.Exit(3);
                return 0;
            }
        }
        
        /**
         * <summary> Evaluates the given binary expression. </summary>
         * <param name="op"> The operator to evaluate. </param>
         * <param name="n1"> The first operand. </param>
         * <param name="n2"> The second operand. </param>
         * <returns> Integer from the computation. </returns>
         */
        private static int EvalExpression(Operator op, int n1, int n2)
        {
            // Switch through every possible operators
            switch (op)
            {
                case Addition or UnaryPlus:
                    return n1 + n2;
                case Subtraction or UnaryMinus:
                    return n1 - n2;
                case Multiplication:
                    return n1 * n2;
                case Division:
                    try { return n1 / n2; }
                    
                    // Catch division per zero
                    catch (DivideByZeroException)
                    {
                        Console.Error.WriteLine("Runtime Exception [AST-Solve.SolverAST.EvalExpression]: Division by zero detected.");
                        Environment.Exit(3);
                        return 0;
                    }
                case Modulo:
                    return n1 % n2;
                case Exponent:
                    return (int) Math.Pow(n1, n2);
                default:
                    Console.Error.WriteLine("Syntax Error [AST-Solve.SolverAST.EvalExpression]: Unknown operator token (shouldn't be reached).");
                    Environment.Exit(3);
                    return 0;
            }
        }

        /**
         * <summary> Evaluates the given function. </summary>
         * <param name="fun"> The function to evaluate. </param>
         * <param name="p1"> The first parameter of the function. </param>
         * <param name="p2"> The second parameter of the function (might be empty). </param>
         * <returns> Integer from the computation. </returns>
         */
        private static int EvalFunction(Function fun, int p1, int p2)
        {
            // Switch through every possible functions
            switch (fun)
            {
                case Sqrt:
                    // Patch square-root of a negative number
                    if (p1 >= 0) return (int) Math.Sqrt(p1);
                    Console.Error.WriteLine("Runtime Exception [AST-Solve.EvalFunction]: Imaginary number detected.");
                    Environment.Exit(3);
                    return (int) Math.Sqrt(p1);
                case Max:
                    return Math.Max(p1, p2);
                case Min:
                    return Math.Min(p1, p2);
                case Facto:
                    return Enumerable.Range(1, p1).Aggregate(1, (p, item) => p * item);
                case IsPrime:
                    return Enumerable.Range(1, p1).Where(x => p1 % x == 0).SequenceEqual(new[] {1, p1}) ? 1 : 0;
                case Fibo fibo:
                    return Fibo.Fibonacci(p1);
                case GCD gcd:
                    return GCD.GetGcd(p1, p2);
                default:
                    Console.Error.WriteLine("Syntax Error [AST-Solve.EvalFunction]: Unknown function token (shouldn't be reached).");
                    Environment.Exit(3);
                    return 0;
            }
        }
    }
}