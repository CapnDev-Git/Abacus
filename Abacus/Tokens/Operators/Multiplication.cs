using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class Multiplication : Operator
    {
        // Constructor (inherits from Operator class)
        public Multiplication() : base("*", OpPrecedence.Timesdiv, AssocDirection.Left, 2){}
    }
}