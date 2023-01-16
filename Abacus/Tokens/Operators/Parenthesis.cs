using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Operators
{
    public class Parenthesis : Operator
    {
        // Attribute
        private ParenthesisType type;
        
        // Getter
        public ParenthesisType Type => type;
        
        // Constructor (inherits from Operator class)
        public Parenthesis(string input) : base(input, OpPrecedence.Parenthesis, AssocDirection.None, 0)
        {
            // Assign the correct parenthesis type
            if (input == "(") this.type = ParenthesisType.Left;
            else this.type = ParenthesisType.Right;
        }
    }
}