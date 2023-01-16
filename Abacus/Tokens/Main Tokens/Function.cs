using System;

namespace Abacus.Tokens.Main_Tokens
{
    public class Function : Token
    {
        // Getters
        public int Arity { get; }
        private string Name { get; }

        // Constructor
        protected Function(string input, int arity) : base(input)
        {
            this.Name = input;
            this.Arity = arity;
        }

        #region DEBUG PRINTS
        public override string ToString() { return this.Name; }
        #endregion
    }
}