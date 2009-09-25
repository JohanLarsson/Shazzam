using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace Shazzam.Converters {
	public class RegisterValueConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			// Convert double to float, Point to Vector, Point to Size, and Point3D to Vector3D.
			// Leave anythign else unchanged.
			if (targetType == typeof(float))
			{
				return (float)(double)value;
			}
			else if (targetType == typeof(Vector))
			{
				return (Vector)(Point)value;
			}
			else if (targetType == typeof(Size))
			{
				Point p = (Point)value;
				return new Size(Math.Max(0, p.X), Math.Max(0, p.Y));
			}
			else if (targetType == typeof(Vector3D))
			{
				return (Vector3D)(Point3D)value;
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return ConvertToUsualType(value);
		}

		public static object ConvertToUsualType(object value)
		{
			// Convert float to double, Vector to Point, Size to Point, and Vector3D to Point3D.
			// Leave anything else unchanged.
			if (value.GetType() == typeof(float))
			{
				return (double)(float)value;
			}
			else if (value.GetType() == typeof(Vector))
			{
				return (Point)(Vector)value;
			}
			else if (value.GetType() == typeof(Size))
			{
				return (Point)(Size)value;
			}
			else if (value.GetType() == typeof(Vector3D))
			{
				return (Point3D)(Vector3D)value;
			}
			return value;
		}
	}
}

