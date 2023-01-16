using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Functions
{
    public class GCD : Function
    {
        // Constructor (inherits from Function class)
        public GCD() : base("gcd", 2){}

        public static int GetGcd(int n1, int n2)
        {
            while (true)
            {
                if (n1 == 0) return n2;
                var n3 = n1;
                n1 = n2 % n1;
                n2 = n3;
            }
        }
    }
}