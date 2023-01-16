using System;

namespace Abacus.Tokens.Main_Tokens
{
    public class Number : Token
    {
        // Getters
        public int Value { get; }

        // Constructor
        public Number(string input) : base(input)
        {
            try
            {
                this.Value = int.Parse(input);
            }
            
            catch (FormatException)
            {
                // Raise a syntax error
                Console.Error.WriteLine("Runtime Exception [AST.SolverAST]: Unbound variable.");
                Environment.Exit(3);
            }
        }

        #region DEBUG PRINTS
        public override string ToString() { return this.Value.ToString(); }
        #endregion
    }
}