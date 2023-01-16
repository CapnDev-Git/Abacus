using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class Subtraction : Operator
    {
        // Constructor (inherits from Operator class)
        public Subtraction() : base("-", OpPrecedence.Addsub, AssocDirection.Left, 2){}
    }
}