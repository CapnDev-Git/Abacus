using System;

namespace Abacus.Tokens.Main_Tokens
{
    public enum OpPrecedence
    {
        Parenthesis = 1,
#pragma warning disable CA1069
        Equal = 1,
#pragma warning restore CA1069
        Addsub = 2,
        Timesdiv = 3,
        Power = 4,
        Unary = 5
    }

    public enum AssocDirection
    {
        None = 0,
        Right = 1,
        Left = -1
    }
    
    public enum ParenthesisType
    {
        Right = 1,
        Left = -1
    }
    
    public class Operator : Token
    {
        // Getters
        public OpPrecedence Precedence { get; }
        public AssocDirection Associativity { get; }
        private char Symbol { get; }

        // Constructor
        protected Operator(string input, OpPrecedence precedence, AssocDirection associativity, int arity) : base(input)
        {
            this.Symbol = input[0];
            this.Precedence = precedence;
            this.Associativity = associativity;
        }

        #region DEBUG PRINTS
        public override string ToString() { return this.Symbol.ToString(); }
        #endregion
    }
}