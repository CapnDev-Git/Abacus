using System;

namespace Abacus.Tokens.Main_Tokens
{
    public class Variable : Token
    {
        // Getters
        public string Name { get; }
        public int? Value { get; set; }

        // Constructor
        public Variable(string input, int? value) : base(input)
        {
            this.Name = input;
            this.Value = value;
        }

        #region DEBUG PRINTS
        public override string ToString() { return $"[{this.Name}={this.Value}]"; }
        #endregion
    }
}