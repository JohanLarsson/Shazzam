using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shazzam.CodeGen {
	[Serializable()]
	public class CompilerException : System.Exception {
		public CompilerException() : base(){ }
		public CompilerException(string message) :base(message) { }
		public CompilerException(string message, System.Exception inner) :base(message,inner){ }


	}
}
