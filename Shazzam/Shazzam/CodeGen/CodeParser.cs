using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using Shazzam.Properties;

namespace Shazzam.CodeGen {

	static class CodeParser {
		// Regular expression that matches a comment from double-slash to end-of-line (but not a triple-slash comment):
		private static readonly Regex CommentRegex = new Regex(@"(?<!/)//$|(?<!/)//[^/].*?$", RegexOptions.Compiled | RegexOptions.Multiline);

		// Patterns that match special triple-slash comments in the header:
		private const string ClassPattern = @"<class>(?<class>.*)</class>";
		private const string NamespacePattern = @"<namespace>(?<namespace>.*)</namespace>";
		private const string DescriptionPattern = @"<description>(?<description>.*)</description>";
		private const string TargetPattern = @"<target>(?<target>.*)</target>";
		private const string HeaderCommentPattern = @"^///\s*(" + ClassPattern + @"|" +
			NamespacePattern + @"|" + DescriptionPattern + @"|" + TargetPattern + @")\s*?$\s*";
		private static readonly Regex HeaderCommentsRegex = new Regex(@"(" + HeaderCommentPattern + @")+",
			RegexOptions.Compiled | RegexOptions.Multiline);

		// Patterns that match special triple-slash comments before each register declaration:
		private const string SummaryPattern = @"<summary>(?<summary>.*)</summary>";
		private const string TypePattern = @"<type>(?<type>.*)</type>";
		private const string MinValuePattern = @"<minValue>(?<minValue>.*)</minValue>";
		private const string MaxValuePattern = @"<maxValue>(?<maxValue>.*)</maxValue>";
		private const string DefaultValuePattern = @"<defaultValue>(?<defaultValue>.*)</defaultValue>";
		private const string SpecialCommentPattern = @"^///\s*(" + SummaryPattern + @"|" + TypePattern + @"|" +
		    MinValuePattern + @"|" + MaxValuePattern + @"|" + DefaultValuePattern + @")\s*?$\s*";
		private const string SpecialCommentsPattern = @"(" + SpecialCommentPattern + @")*";

		// Patterns used in a constant register declaration in HLSL:
		private const string RegisterTypePattern = @"(?<registerType>\w+?)";
		private const string RegisterNamePattern = @"(?<registerName>\w+)";
		private const string RequiredWhitespacePattern = @"\s+";
		private const string OptionalWhitespacePattern = @"\s*";
		private const string RegisterConstantNumberPattern = @"[CcSs](?<registerNumber>\d+)";
		private const string InitializerValuePattern = @"(?<initializerValue>[^;]+)";
		private const string OptionalInitializerPattern = @"(?<initializer>=" + OptionalWhitespacePattern + InitializerValuePattern + OptionalWhitespacePattern + @")?";

		// Regular expression that matches an entire constant register declaration, including the preceding special comments:
		private static readonly Regex RegisterConstantDeclarationRegex = new Regex(SpecialCommentsPattern +
			RegisterTypePattern + RequiredWhitespacePattern + RegisterNamePattern + OptionalWhitespacePattern +
			@":" + OptionalWhitespacePattern + @"register" + OptionalWhitespacePattern +
			@"\(" + OptionalWhitespacePattern + RegisterConstantNumberPattern + OptionalWhitespacePattern + @"\)" + OptionalWhitespacePattern +
			OptionalInitializerPattern + @";", RegexOptions.Compiled | RegexOptions.Multiline);

		/// <summary>
		/// Returns a shader model constructed from the information found in the
		/// given pixel shader file.
		/// </summary>
		public static ShaderModel ParseShader(string shaderFileName, string shaderText)
		{
			// Remove all double-slash comments (but not triple-slash comments).
			// This helps us avoid parsing register declarations that are commented out.
			shaderText = CommentRegex.Replace(shaderText, String.Empty);

			// Find all header comments.
			Match headerMatch = HeaderCommentsRegex.Match(shaderText);

			// Determine the class name, namespace, description, and target platform.
			string defaultClassName = Path.GetFileNameWithoutExtension(shaderFileName);
			defaultClassName = Char.ToUpperInvariant(defaultClassName[0]) + defaultClassName.Substring(1) + "Effect";
			string className = GetValidId(headerMatch.Groups["class"].Value, defaultClassName, false);
			string namespaceName = GetValidId(headerMatch.Groups["namespace"].Value, Settings.Default.GeneratedNamespace, true);
			string description = headerMatch.Groups["description"].Success ? headerMatch.Groups["description"].Value : null;
			string targetFrameworkName = headerMatch.Groups["target"].Success ? headerMatch.Groups["target"].Value : Settings.Default.TargetFramework;
			TargetFramework targetFramework = targetFrameworkName == "Silverlight" ? TargetFramework.Silverlight : TargetFramework.WPF;

			// Find all register declarations.
			MatchCollection matches = RegisterConstantDeclarationRegex.Matches(shaderText);

			// Create a list of shader model constant registers.
			List<ShaderModelConstantRegister> registers = new List<ShaderModelConstantRegister>();
			foreach (Match match in matches)
			{
				ShaderModelConstantRegister register = CreateRegister(targetFramework, match);
				if (register != null)
				{
					registers.Add(register);
				}
			}

			// Return a new shader model.
			return new ShaderModel
			{
				ShaderFileName = shaderFileName,
				GeneratedClassName = className,
				GeneratedNamespace = namespaceName,
				Description = description,
				TargetFramework = targetFramework,
				Registers = registers
			};
		}

		/// <summary>
		/// Returns a ShaderModelConstantRegister object with the information contained in
		/// the given regular expression match.
		/// </summary>
		private static ShaderModelConstantRegister CreateRegister(TargetFramework targetFramework, Match match)
		{
			ShaderModelConstantRegister register = null;

			// Figure out the .NET type that corresponds to the register type.
			string registerTypeInHLSL = match.Groups["registerType"].Value;
			Type registerType = GetRegisterType(targetFramework, registerTypeInHLSL);
			if (registerType != null)
			{
				// See if the user prefers to specify a different type in a comment.
				if (match.Groups["type"].Success)
				{
					OverrideTypeIfAllowed(targetFramework, match.Groups["type"].Value, ref registerType);
				}

				// Capitalize the first letter of the variable name.  Leave the rest alone.
				string registerName = match.Groups["registerName"].Value;
				registerName = Char.ToUpperInvariant(registerName[0]) + registerName.Substring(1);

				// Get the register number and the optional summary comment.
				int registerNumber = Int32.Parse(match.Groups["registerNumber"].Value);
                if (typeof(Brush).IsAssignableFrom(registerType) && (registerNumber == 0)) return null; // ignore the implicit input sampler
				string summary = match.Groups["summary"].Value;

				// Get the standard min, max, and default value for the register type.
				object minValue;
				object maxValue;
				object defaultValue;
				GetStandardValues(registerType, out minValue, out maxValue, out defaultValue);

				// Allow the user to override the defaults with values from their comments.
				ConvertValue(match.Groups["minValue"].Value, registerType, ref minValue);
				ConvertValue(match.Groups["maxValue"].Value, registerType, ref maxValue);
				ConvertValue(match.Groups["defaultValue"].Value, registerType, ref defaultValue);

				// And if the user specified an initializer for the register value in HLSL,
				// that value overrides any other default value.
				if (match.Groups["initializer"].Success)
				{
					ParseInitializerValue(match.Groups["initializerValue"].Value, registerType, ref defaultValue);
				}

				// Create a structure to hold the register information.
				register = new ShaderModelConstantRegister(registerName, registerType, registerNumber,
					summary, minValue, maxValue, defaultValue);
			}
			return register;
		}

		/// <summary>
		/// Returns the CLR type used to represent the given HLSL register type.
		/// </summary>
		private static Type GetRegisterType(TargetFramework targetFramework, string registerTypeInHLSL)
		{
			switch (registerTypeInHLSL.ToLower())
			{
				case "float":
				case "float1":
					return typeof(double);
				case "float2":
					return typeof(Point);
				case "float3":
					// Silverlight doesn't have any types that correspond to float3 registers.
					return targetFramework == TargetFramework.WPF ? typeof(Point3D) : null;
				case "float4":
					return typeof(Color);

                case "sampler1d":
                    return typeof(Brush);

                case "sampler2d":
                    return typeof(Brush);
            }
			return null;
		}

		/// <summary>
		/// Replaces the register type with the desired type, if they are compatible.
		/// </summary>
		private static void OverrideTypeIfAllowed(TargetFramework targetFramework, string desiredTypeName, ref Type registerType)
		{
			switch (desiredTypeName)
			{
				case "float":
				case "Single":
					if (registerType == typeof(double))
						registerType = typeof(float);
					break;
				case "Size":
					if (registerType == typeof(Point))
						registerType = typeof(Size);
					break;
				case "Vector":
					if (registerType == typeof(Point))
						registerType = typeof(Vector);
					break;
				case "Vector3D":
					// Silverlight doesn't have Vector3D.
					if (registerType == typeof(Point3D) && targetFramework == TargetFramework.WPF)
						registerType = typeof(Vector3D);
					break;
				case "Point4D":
					// Silverlight doesn't have Point4D.
					if (registerType == typeof(Color) && targetFramework == TargetFramework.WPF)
						registerType = typeof(Point4D);
					break;
			}
		}

		/// <summary>
		/// Sets the out parameters to the standard min, max, and default values for the given type.
		/// </summary>
		private static void GetStandardValues(Type registerType, out object minValue, out object maxValue, out object defaultValue)
		{
			if (registerType == typeof(double))
			{
				minValue = 0.0;
				maxValue = 1.0;
				defaultValue = 0.0;
			}
			else if (registerType == typeof(float))
			{
				minValue = 0f;
				maxValue = 1f;
				defaultValue = 0f;
			}
			else if (registerType == typeof(Point))
			{
				minValue = new Point(0, 0);
				maxValue = new Point(1, 1);
				defaultValue = new Point(0, 0);
			}
			else if (registerType == typeof(Size))
			{
				minValue = new Size(0, 0);
				maxValue = new Size(1, 1);
				defaultValue = new Size(0, 0);
			}
			else if (registerType == typeof(Vector))
			{
				minValue = new Vector(0, 0);
				maxValue = new Vector(1, 1);
				defaultValue = new Vector(0, 0);
			}
			else if (registerType == typeof(Point3D))
			{
				minValue = new Point3D(0, 0, 0);
				maxValue = new Point3D(1, 1, 1);
				defaultValue = new Point3D(0, 0, 0);
			}
			else if (registerType == typeof(Vector3D))
			{
				minValue = new Vector3D(0, 0, 0);
				maxValue = new Vector3D(1, 1, 1);
				defaultValue = new Vector3D(0, 0, 0);
			}
			else if (registerType == typeof(Point4D))
			{
				minValue = new Point4D(0, 0, 0, 0);
				maxValue = new Point4D(1, 1, 1, 1);
				defaultValue = new Point4D(0, 0, 0, 0);
			}
			else if (registerType == typeof(Color))
			{
				minValue = Color.FromArgb(0, 0, 0, 0);
				maxValue = Color.FromArgb(255, 255, 255, 255);
				defaultValue = Colors.Black;
			}
			else
			{
				minValue = maxValue = defaultValue = null;
			}
		}

		/// <summary>
		/// Converts the given string value into a double, Point, Point3D, or Color.
		/// </summary>
		private static void ConvertValue(string valueText, Type type, ref object value)
		{
			try
			{
				if (type == typeof(double))
				{
					value = Double.Parse(valueText);
				}
				else if (type == typeof(float))
				{
					value = Single.Parse(valueText);
				}
				else if (type == typeof(Point))
				{
					value = Point.Parse(valueText);
				}
				else if (type == typeof(Size))
				{
					value = Size.Parse(valueText);
				}
				else if (type == typeof(Vector))
				{
					value = Vector.Parse(valueText);
				}
				else if (type == typeof(Point3D))
				{
					value = Point3D.Parse(valueText);
				}
				else if (type == typeof(Vector3D))
				{
					value = Vector3D.Parse(valueText);
				}
				else if (type == typeof(Point4D))
				{
					value = Point4D.Parse(valueText);
				}
				else if (type == typeof(Color))
				{
					value = ColorConverter.ConvertFromString(valueText);
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Parses the string representation of an HLSL float, float2, float3, or float4 value,
		/// setting the final parameter to the corresponding double, Point, Point3D, or Color if possible.
		/// </summary>
		private static void ParseInitializerValue(string initializerValueText, Type registerType, ref object initializerValue)
		{
			double[] numbers = ParseNumbers(initializerValueText);
			if (registerType == typeof(double) && numbers.Length >= 1)
			{
				initializerValue = numbers[0];
			}
			else if (registerType == typeof(float) && numbers.Length >= 1)
			{
				initializerValue = (float)numbers[0];
			}
			else if (registerType == typeof(Point) && numbers.Length >= 2)
			{
				initializerValue = new Point(numbers[0], numbers[1]);
			}
			else if (registerType == typeof(Size) && numbers.Length >= 2)
			{
				initializerValue = new Size(Math.Max(0, numbers[0]), Math.Max(0, numbers[1]));
			}
			else if (registerType == typeof(Vector) && numbers.Length >= 2)
			{
				initializerValue = new Vector(numbers[0], numbers[1]);
			}
			else if (registerType == typeof(Point3D) && numbers.Length >= 3)
			{
				initializerValue = new Point3D(numbers[0], numbers[1], numbers[2]);
			}
			else if (registerType == typeof(Vector3D) && numbers.Length >= 3)
			{
				initializerValue = new Vector3D(numbers[0], numbers[1], numbers[2]);
			}
			else if (registerType == typeof(Point4D) && numbers.Length >= 4)
			{
				initializerValue = new Point4D(numbers[0], numbers[1], numbers[2], numbers[3]);
			}
			else if (registerType == typeof(Color) && numbers.Length >= 4)
			{
				initializerValue = Color.FromArgb(ConvertToByte(numbers[3]), ConvertToByte(numbers[0]), ConvertToByte(numbers[1]), ConvertToByte(numbers[2]));
			}
		}
		
		/// <summary>
		/// Parses the string representation of an HLSL float, float2, float3, or float4 value,
		/// returning an array of doubles (possibly empty).
		/// </summary>
		private static double[] ParseNumbers(string text)
		{
			// Get rid of any leading "float(", "float2(", "float3(", or "float4" and trailing ")".
			text = Regex.Replace(text, @"^\s*float[1234]?\s*\((.*)\)\s*$", @"$1");

			// Split at commas.
			string[] textValues = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			// Parse the numbers.
			List<double> numbers = new List<double>();
			foreach (string textValue in textValues)
			{
				string trimmedValue = textValue.Trim();
				double number;
				if (Double.TryParse(trimmedValue, out number))
				{
					numbers.Add(number);
				}
				else
				{
					break;
				}
			}
			return numbers.ToArray();
		}

		/// <summary>
		/// Converts a double-precision floating point number between 0 and 1 to a byte.
		/// </summary>
		private static byte ConvertToByte(double number)
		{
			return (byte)Math.Max(0, Math.Min(Math.Round(255 * number), 255));
		}

		/// <summary>
		/// Returns a valid C# or Visual Basic identifier based on the given string.
		/// </summary>
		private static string GetValidId(string firstChoice, string secondChoice, bool isDotAllowed)
		{
			if (String.IsNullOrEmpty(firstChoice))
			{
				firstChoice = secondChoice;
			}

			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in firstChoice)
			{
				if (c == '_' || Char.IsLetter(c) || (stringBuilder.Length > 0 && (Char.IsDigit(c) || (c == '.' && isDotAllowed))))
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append('_');
				}
			}
			return stringBuilder.ToString();
		}
	}
}