using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class Exponent : Operator
    {
        // Constructor (inherits from Operator class)
        public Exponent() : base("^", OpPrecedence.Power, AssocDirection.Right, 2){}
    }
}