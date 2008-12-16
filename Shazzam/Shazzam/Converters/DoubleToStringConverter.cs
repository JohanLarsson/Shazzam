using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Shazzam.Converters {
	public class DoubleToStringConverter : IValueConverter {


		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			double number = (double)value;
			return number.ToString("F2");
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			String number = value.ToString();
			double cleanedNumber;
			if (Double.TryParse(number, NumberStyles.Any, null, out cleanedNumber))
			{
				return cleanedNumber;
			}
			return number;
		}
	}
}

