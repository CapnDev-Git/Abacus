using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class Equal : Operator
    {
        // Constructor (inherits from Operator class)
        public Equal() : base("=", OpPrecedence.Equal, AssocDirection.Right, 2){}
    }
}