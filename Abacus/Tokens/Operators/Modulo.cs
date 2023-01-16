using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class Modulo : Operator
    {
        // Constructor (inherits from Operator class)
        public Modulo() : base("%", OpPrecedence.Timesdiv, AssocDirection.Left, 2){}
    }
}