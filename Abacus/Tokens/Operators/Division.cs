using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class Division : Operator
    {
        // Constructor (inherits from Operator class)
        public Division() : base("/", OpPrecedence.Timesdiv, AssocDirection.Left, 2){}
    }
}