namespace Errors
{
    public class RuntimeError : System.Exception
    {
        public LoxSharp.Token token;

        public RuntimeError(LoxSharp.Token token, string message)
        {
            this.token = token;
        }
    }
}