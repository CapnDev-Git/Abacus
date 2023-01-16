using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class Addition : Operator
    {
        // Constructor (inherits from Operator class)
        public Addition() : base("+", OpPrecedence.Addsub, AssocDirection.Left, 2){}
    }
}