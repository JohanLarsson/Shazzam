namespace Shazzam.CodeGen
{
    using System;

    [Serializable]
    public class CompilerException : Exception
    {
        public CompilerException()
            : base()
        {
        }

        public CompilerException(string message)
            : base(message)
        {
        }

        public CompilerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
