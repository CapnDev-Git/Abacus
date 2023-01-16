using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class UnaryMinus : Operator
    {
        // Constructor (inherits from Operator class)
        public UnaryMinus() : base("m", OpPrecedence.Unary, AssocDirection.Right, 1){}
    }
}