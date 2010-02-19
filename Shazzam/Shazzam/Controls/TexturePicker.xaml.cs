using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Shazzam.Controls
{

	public partial class TexturePicker : UserControl
	{
		static Dictionary<int, ImageBrush> _images = new Dictionary<int, ImageBrush>();
		ShaderModelConstantRegister _register = null;
		public TexturePicker(ShaderModelConstantRegister register)

		{ 
			InitializeComponent();
			_register = register;
			//this.Loaded += new RoutedEventHandler(TexturePicker_Loaded);
			ImageBrush result;
			_images.TryGetValue(_register.RegisterNumber, out result);
			Value = result;
			if (Value == null)
			{
				Value = new ImageBrush(image1.Source);
			}
		}

		void TexturePicker_Loaded(object sender, RoutedEventArgs e)
		{
			//ImageBrush result;
			//_images.TryGetValue(_register.RegisterNumber, out result);
			//Value = result;
			//if (Value == null)
			//{
			//  Value = new ImageBrush(image1.Source);
			//}
		}

		//public TexturePicker()
		//{
		//  InitializeComponent();

		//  ImageBrush brush = new ImageBrush(image1.Source);

		//  Value = brush;
		//}

		#region Value

		/// <summary>
		/// Value Dependency Property
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
				DependencyProperty.Register("Value", typeof(ImageBrush), typeof(TexturePicker),
						new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));

		/// <summary>
		/// Gets or sets the Value property.  This dependency property
		/// indicates the current value of the AdjustableSliderPair.
		/// </summary>
		public ImageBrush Value
		{
			get { return (ImageBrush)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Handles changes to the Value property.
		/// </summary>
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TexturePicker)d).OnValueChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the Value property.
		/// </summary>
		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
		{
			ImageBrush brush = (ImageBrush)e.NewValue;
			_images[_register.RegisterNumber] = brush;
			image1.Source = image2.Source= brush.ImageSource;
		}

		#endregion

		private void btnOpenImage_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.DefaultExt = ".jpg"; // Default file extension
			dialog.Filter = "JPEG Images (.jpg); PNG files(.png)|*.jpg;*.png"; // Filter files by extension
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			if (dialog.ShowDialog() == true)
			{
				string filename = dialog.FileName;
				ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(filename)));
				Value = brush;
			}

		}

		private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
		//	this.Height = 180;
		}

		private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
		//	this.Height = 70;
		}
	}
}
