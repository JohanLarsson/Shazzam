using System;
using System.Windows.Controls;

namespace Shazzam {
	/// <summary>
	///  Contains the details for each register described in a HLSL shader file
	/// </summary>
	public class ShaderModelConstantRegister {

		public ShaderModelConstantRegister(string registerName, Type registerType, int registerNumber,
			string description, object minValue, object maxValue, object defaultValue)
		{
			this.RegisterName = registerName;
			this.RegisterType = registerType;
			this.RegisterNumber = registerNumber;
			this.Description = description;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
			this.DefaultValue = defaultValue;
		}

		#region Properties
		/// <summary>
		/// The name of this register variable.
		/// </summary>
		public string RegisterName { get; private set; }

		/// <summary>
		///  The .NET type of this register variable.
		/// </summary>
		public Type RegisterType { get; private set; }

		/// <summary>
		/// The register number of this register variable.
		/// </summary>
		public int RegisterNumber { get; private set; }

		/// <summary>
		/// The description of this register variable.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// The minimum value for this register variable.
		/// </summary>
		public object MinValue { get; private set; }

		/// <summary>
		/// The maximum value for this register variable.
		/// </summary>
		public object MaxValue { get; private set; }

		/// <summary>
		/// The default value of this register variable.
		/// </summary>
		public object DefaultValue { get; private set; }

		/// <summary>
		/// The user interface control associated with this register variable.
		/// </summary>
		public Control AffiliatedControl { get; set; }
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
