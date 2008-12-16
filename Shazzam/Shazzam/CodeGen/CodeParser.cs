using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Shazzam.CodeGen {

	class CodeParser {
		private static List<String> registerLines = new List<string>();

		public static List<String> GetRegisterLines(string filePath) {
			registerLines.Clear();
			string regex = @"(?<colon>:)\s(?<firstwhitespace>register)\([Cc]\d+\)(?<endline>;)";
			RegexOptions options = RegexOptions.Multiline;

			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(fs, Encoding.UTF7);
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				line = line.Trim();
				MatchCollection matches = Regex.Matches(line, regex, options);
				foreach (Match match in matches)
				{
					registerLines.Add(line);

				}
			}
			return registerLines;

		}

	}

}