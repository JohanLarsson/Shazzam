using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Shazzam {
	/// <summary>
	///  Contains the details for each register described in a HLSL shader file
	/// </summary>
	public class ShaderModelConstantRegister {

		public static ShaderModelConstantRegister Parse(string rawStringFromHLSL) {
			try
			{
				rawStringFromHLSL = rawStringFromHLSL.Trim();
				ShaderModelConstantRegister temp = new ShaderModelConstantRegister();
				rawStringFromHLSL = rawStringFromHLSL.Replace(":", " : ");
				temp.ConstantRegister = ExtractRegisterNumber(rawStringFromHLSL);
				temp.VariableType = ExtractVariableType(rawStringFromHLSL);
				temp.VariableName = ExtractVariableName(rawStringFromHLSL);
				return temp;
			}
			catch (Exception)
			{

				throw new Exception("Cannot parse the string");
			}
		}



		private Int32 _constantRegister;
		private String _variableName;
		#region Parsing
		/// <summary>
		/// Determines the Register Number
		/// Given a string from in the following format:
		/// float twistAmount : register(C1);
		/// ExtractRegisterNumber will determine the Register number (1)
		/// </summary>
		///
		/// <param name="rawData">A string contain raw data to parse. Should be in the following format: 'float twistAmount : register(C1);'</param>
		/// <returns>The number of the register</returns>
		private static int ExtractRegisterNumber(string rawData) {
			string pattern = @"(?<paren>\([Cc]\d+\))";

			if (!Regex.IsMatch(rawData, pattern))
			{
				return -1;
			}
			var m = Regex.Match(rawData, pattern);
			string result = m.ToString();
			result = result.ToUpper();
			result = result.Replace("(", "").Replace(")", "").Replace("C", "");
			return Int32.Parse(result);
		}

		/// <summary>
		/// Determines the Register PropertyType
		/// Given a string from in the following format:
		/// float twistAmount : register(C1);
		/// ExtractRegisterNumber will determine the Register  (1)
		/// </summary>
		/// <param name="rawData">A string contain raw data to parse. Should be in the following format: 'float twistAmount : register(C1);'</param>
		/// <returns>The Type of the variable (Double, Point, Color)</returns>
		private static Type ExtractVariableType(string rawData) {
			if (rawData.StartsWith("float "))
			{
				return typeof(double);
			}
			else if (rawData.StartsWith("float2 "))
			{
				return typeof(System.Windows.Point);
			}
			else if (rawData.StartsWith("float4 "))
			{
				return typeof(System.Windows.Media.Color);
			}
			else
			{
				return typeof(object);

			}

		}

		/// <summary>
		/// Determines the variable name
		/// Given a string from in the following format:
		/// float twistAmount : register(C1);
		/// ExtractRegisterNumber will determine the Register  (1)
		/// </summary>
		/// <param name="rawData">A string contain raw data to parse. Should be in the following format: 'float twistAmount : register(C1);'</param>
		/// <returns>The name of the variable </returns>
		public static string ExtractVariableName(string rawData) {
			
			string pattern = @"(?<=:)\\s\w+";
			string[] parts = Regex.Split(rawData, pattern);
			parts = rawData.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
			return parts[1];
		}
		#endregion

		#region Properties
		/// <summary>
		/// The Register Number for this Register variable
		/// </summary>
		public Int32 ConstantRegister {
			get {
				return _constantRegister;
			}
			set {
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(String.Format("Negative values not allowed for {0}.{1}", this.GetType().Name, "ConstantRegister"));
				}
				else
				{
					_constantRegister = value;
				}
			}
		}
		/// <summary>
		///  The data type of this Register variable
		/// </summary>
		public Type VariableType { get; set; }

		/// <summary>
		/// the desired Variable name for this Register variable
		/// </summary>
		public String VariableName {
			get {
				return _variableName;
			}
			set {
				_variableName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value);
			}
		}
		public Control AffliatedControl { get; set; }
		#endregion

	}
}

/*
 The DependencyProperties that are bound to floating point shader constant registers can be any of the following types:

Double
Single ('float' in C#)
Color
Size
Point
Vector
Point3D
Vector3D
Point4D

 * They each will go into their shader register filling up whatever number of components of that register are appropriate.  For instance, Double and Single go into one component, Color into 4, Size, Point and Vector into 2, etc.  Unfilled components are set to '1'.

Some minutiae
Register Limit: There is a limit of 32 floating point registers that can be used in PS 2.0.  In the unlikely event that you have more values than that that you want to pack in, you might consider tricks like packing, for instance, two Points into a single Point4D, etc.

 */
