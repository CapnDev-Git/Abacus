using System;

namespace Abacus.Tokens.Main_Tokens
{
    public class Separator : Token
    {
        // Getters
        private char Symbol { get; }

        // Constructor
        public Separator(string input) : base(input)
        {
            this.Symbol = input[0];
        }

        #region DEBUG PRINTS
        public override string ToString() { return this.Symbol.ToString(); }
        #endregion
    }
}