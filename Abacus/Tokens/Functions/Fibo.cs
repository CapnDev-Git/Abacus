using Abacus.Tokens.Main_Tokens;

namespace Abacus.Tokens.Functions
{
    public class Fibo : Function
    {
        // Constructor (inherits from Function class)
        public Fibo() : base("fibo", 1){}

        public static int Fibonacci(int n)
        {
            return n < 2 ? n : Fibonacci(n - 1) + Fibonacci(n - 2);
        }
    }
}