using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class UnaryPlus : Operator
    {
        // Constructor (inherits from Operator class)
        public UnaryPlus() : base("p", OpPrecedence.Unary, AssocDirection.Right, 1){}
    }
}