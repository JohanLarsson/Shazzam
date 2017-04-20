using System;
using System.Globalization;
using System.Windows.Data;

namespace Shazzam.Converters {
	public class EqualityConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value != null && value.ToString() == parameter.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return (bool)value ? parameter : Binding.DoNothing;
		}
	}
}

