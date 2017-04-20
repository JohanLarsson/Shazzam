using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Shazzam.Controls
{
	/// <summary>
	/// Interaction logic for AdjustableSliderQuadruplet.xaml.
	/// </summary>
	public partial class AdjustableSliderQuadruplet : UserControl
	{
		private const double DefaultDuration = 0.5;

		private Storyboard storyboard = new Storyboard();
		private DoubleAnimation xSliderValueAnimation = new DoubleAnimation
		{
			Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
			RepeatBehavior = RepeatBehavior.Forever,
			AutoReverse = true,
			AccelerationRatio = 0.25,
			DecelerationRatio = 0.25
		};
		private DoubleAnimation ySliderValueAnimation = new DoubleAnimation
		{
			Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
			RepeatBehavior = RepeatBehavior.Forever,
			AutoReverse = true,
			AccelerationRatio = 0.25,
			DecelerationRatio = 0.25
		};
		private DoubleAnimation zSliderValueAnimation = new DoubleAnimation
		{
			Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
			RepeatBehavior = RepeatBehavior.Forever,
			AutoReverse = true,
			AccelerationRatio = 0.25,
			DecelerationRatio = 0.25
		};
		private DoubleAnimation wSliderValueAnimation = new DoubleAnimation
		{
			Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
			RepeatBehavior = RepeatBehavior.Forever,
			AutoReverse = true,
			AccelerationRatio = 0.25,
			DecelerationRatio = 0.25
		};

		public AdjustableSliderQuadruplet()
		{
			this.InitializeComponent();

			Storyboard.SetTarget(this.xSliderValueAnimation, this.xSlider);
			Storyboard.SetTargetProperty(this.xSliderValueAnimation, new PropertyPath(Slider.ValueProperty));
			this.storyboard.Children.Add(this.xSliderValueAnimation);

			Storyboard.SetTarget(this.ySliderValueAnimation, this.ySlider);
			Storyboard.SetTargetProperty(this.ySliderValueAnimation, new PropertyPath(Slider.ValueProperty));
			this.storyboard.Children.Add(this.ySliderValueAnimation);

			Storyboard.SetTarget(this.zSliderValueAnimation, this.zSlider);
			Storyboard.SetTargetProperty(this.zSliderValueAnimation, new PropertyPath(Slider.ValueProperty));
			this.storyboard.Children.Add(this.zSliderValueAnimation);

			Storyboard.SetTarget(this.wSliderValueAnimation, this.wSlider);
			Storyboard.SetTargetProperty(this.wSliderValueAnimation, new PropertyPath(Slider.ValueProperty));
			this.storyboard.Children.Add(this.wSliderValueAnimation);

			this.mainPanel.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(mainStackPanel_PreviewKeyDown);

			this.xMinTextBox.LostFocus += new RoutedEventHandler(xMinTextBox_LostFocus);
			this.xMaxTextBox.LostFocus += new RoutedEventHandler(xMaxTextBox_LostFocus);
			this.xSlider.ValueChanged += this.XSlider_ValueChanged;

			this.yMinTextBox.LostFocus += new RoutedEventHandler(yMinTextBox_LostFocus);
			this.yMaxTextBox.LostFocus += new RoutedEventHandler(yMaxTextBox_LostFocus);
			this.ySlider.ValueChanged += this.YSlider_ValueChanged;

			this.zMinTextBox.LostFocus += new RoutedEventHandler(zMinTextBox_LostFocus);
			this.zMaxTextBox.LostFocus += new RoutedEventHandler(zMaxTextBox_LostFocus);
			this.zSlider.ValueChanged += this.ZSlider_ValueChanged;

			//	this.wMinTextBox.TextChanged += this.WMinTextBox_TextChanged;
			//	this.wMaxTextBox.TextChanged += this.WMaxTextBox_TextChanged;
			this.wMinTextBox.LostFocus += new RoutedEventHandler(wMinTextBox_LostFocus);
			this.wMaxTextBox.LostFocus += new RoutedEventHandler(wMaxTextBox_LostFocus);
			this.wSlider.ValueChanged += this.WSlider_ValueChanged;

			this.noAnimationToggleButton.Click += this.AnimationToggleButton_Click;
			this.linearAnimationToggleButton.Click += this.AnimationToggleButton_Click;
			this.circularAnimationToggleButton.Click += this.AnimationToggleButton_Click;
			this.durationTextBox.TextChanged += this.DurationTextBox_TextChanged;
			durationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString();

		}

		void mainStackPanel_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			// pressing the enter key will move focus to next control
			var uie = e.OriginalSource as UIElement;
			if (e.Key == Key.Enter)
			{
				e.Handled = true;
				uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}
		}
		#region XControls

		void xMinTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.xMinTextBox.Text, NumberStyles.Any, null, out number))
			{
				this.Minimum = new Point4D(number, this.Minimum.Y, this.Minimum.Z, this.Minimum.W);
			}
		}

		void xMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.xMaxTextBox.Text, NumberStyles.Any, null, out number))
			{
				this.Maximum = new Point4D(number, this.Maximum.Y, this.Maximum.Z, this.Maximum.W);
			}
		}

		private void XSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			this.Value = new Point4D(e.NewValue, this.Value.Y, this.Value.Z, this.Value.W);
		}
		#endregion

		#region YControls

		void yMinTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.yMinTextBox.Text, NumberStyles.Any, null, out number))
			{
				this.Minimum = new Point4D(this.Minimum.X, number, this.Minimum.Z, this.Minimum.W);
			}
		}

		void yMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.yMaxTextBox.Text, NumberStyles.Any, null, out number))
			{
				this.Maximum = new Point4D(this.Maximum.X, number, this.Maximum.Z, this.Maximum.W);
			}
		}

		private void YSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			this.Value = new Point4D(this.Value.X, e.NewValue, this.Value.Z, this.Value.W);
		}
		#endregion

		#region ZControls

		void zMinTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.zMinTextBox.Text, NumberStyles.Any, null, out number))
			{
				this.Minimum = new Point4D(this.Minimum.X, this.Minimum.Y, number, this.Minimum.W);
			}
		}

		void zMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.zMaxTextBox.Text, NumberStyles.Any, null, out number))
			{
				this.Maximum = new Point4D(this.Maximum.X, this.Maximum.Y, number, this.Maximum.W);
			}
		}

		private void ZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			this.Value = new Point4D(this.Value.X, this.Value.Y, e.NewValue, this.Value.W);
		}
		#endregion

		#region WControls

		void wMinTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.wMinTextBox.Text, NumberStyles.Any, null, out number))
			{
				this.Minimum = new Point4D(this.Minimum.X, this.Minimum.Y, this.Minimum.Z, number);
			}
		}
		void wMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.wMaxTextBox.Text, NumberStyles.Any, null, out number))
			{
				this.Maximum = new Point4D(this.Maximum.X, this.Maximum.Y, this.Maximum.Z, number);
			}
		}

		private void WSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			this.Value = new Point4D(this.Value.X, this.Value.Y, this.Value.Z, e.NewValue);
		}
		#endregion

		private void AnimationToggleButton_Click(object sender, RoutedEventArgs e)
		{
			this.noAnimationToggleButton.IsChecked = sender == this.noAnimationToggleButton;
			this.linearAnimationToggleButton.IsChecked = sender == this.linearAnimationToggleButton;
			this.circularAnimationToggleButton.IsChecked = sender == this.circularAnimationToggleButton;
			this.UpdateAnimation();
		}

		private void DurationTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.durationTextBox.Text, out number))
			{
				TimeSpan duration = TimeSpan.FromSeconds(Math.Max(0, number));
				this.xSliderValueAnimation.Duration = duration;
				this.ySliderValueAnimation.Duration = duration;
				this.zSliderValueAnimation.Duration = duration;
				this.wSliderValueAnimation.Duration = duration;
				this.UpdateAnimation();
			}
		}

		private void UpdateAnimation()
		{
			Point4D minimum = this.Minimum;
			Point4D maximum = this.Maximum;
			this.xSliderValueAnimation.From = minimum.X;
			this.xSliderValueAnimation.To = maximum.X;

			this.ySliderValueAnimation.From = minimum.Y;
			this.ySliderValueAnimation.To = maximum.Y;

			this.zSliderValueAnimation.From = minimum.Z;
			this.zSliderValueAnimation.To = maximum.Z;

			this.wSliderValueAnimation.From = minimum.W;
			this.wSliderValueAnimation.To = maximum.W;

			if (this.noAnimationToggleButton.IsChecked.GetValueOrDefault() ||
				this.xSliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
			{
				this.storyboard.Stop(this);
				xSlider.Visibility = Visibility.Visible;
				xSliderText.Visibility = Visibility.Collapsed;
				ySlider.Visibility = Visibility.Visible;
				ySliderText.Visibility = Visibility.Collapsed;
				zSlider.Visibility = Visibility.Visible;
				zSliderText.Visibility = Visibility.Collapsed;
				wSlider.Visibility = Visibility.Visible;
				wSliderText.Visibility = Visibility.Collapsed;

			}
			else
			{
				double duration = this.xSliderValueAnimation.Duration.TimeSpan.TotalSeconds;
				double yBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? duration / 4 : 0;
				double zBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? 2 * duration / 4 : 0;
				double wBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? 3 * duration / 4 : 0;
				this.ySliderValueAnimation.BeginTime = TimeSpan.FromSeconds(yBeginTime);
				this.zSliderValueAnimation.BeginTime = TimeSpan.FromSeconds(zBeginTime);
				this.wSliderValueAnimation.BeginTime = TimeSpan.FromSeconds(wBeginTime);
				this.storyboard.Begin(this, true);
				xSlider.Visibility = Visibility.Collapsed;
				xSliderText.Visibility = Visibility.Visible;
				ySlider.Visibility = Visibility.Collapsed;
				ySliderText.Visibility = Visibility.Visible;
				zSlider.Visibility = Visibility.Collapsed;
				zSliderText.Visibility = Visibility.Visible;
				wSlider.Visibility = Visibility.Collapsed;
				wSliderText.Visibility = Visibility.Visible;
			}
		}

		#region Value

		/// <summary>
		/// Value Dependency Property
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(Point4D), typeof(AdjustableSliderQuadruplet),
				new FrameworkPropertyMetadata(new Point4D(0, 0, 0, 0), new PropertyChangedCallback(OnValueChanged)));

		/// <summary>
		/// Gets or sets the Value property.  This dependency property
		/// indicates the current value of the AdjustableSliderQuadruplet.
		/// </summary>
		public Point4D Value
		{
			get { return (Point4D)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Handles changes to the Value property.
		/// </summary>
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AdjustableSliderQuadruplet)d).OnValueChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the Value property.
		/// </summary>
		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
		{
			Point4D point = (Point4D)e.NewValue;
			this.xSlider.Value = point.X;
			this.ySlider.Value = point.Y;
			this.zSlider.Value = point.Z;
			this.wSlider.Value = point.W;
		}

		#endregion

		#region Minimum

		/// <summary>
		/// Minimum Dependency Property
		/// </summary>
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(Point4D), typeof(AdjustableSliderQuadruplet),
				new FrameworkPropertyMetadata(new Point4D(0, 0, 0, 0), new PropertyChangedCallback(OnMinimumChanged)));

		/// <summary>
		/// Gets or sets the Minimum property.  This dependency property
		/// indicates Minimum allowed value for the control.
		/// </summary>
		public Point4D Minimum
		{
			get { return (Point4D)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		/// <summary>
		/// Handles changes to the Minimum property.
		/// </summary>
		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AdjustableSliderQuadruplet)d).OnMinimumChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the Minimum property.
		/// </summary>
		protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
		{
			Point4D point = (Point4D)e.NewValue;
			this.xMinTextBox.Text = point.X.ToString();
			this.yMinTextBox.Text = point.Y.ToString();
			this.zMinTextBox.Text = point.Z.ToString();
			this.wMinTextBox.Text = point.W.ToString();
			this.UpdateAnimation();
		}

		#endregion

		#region Maximum

		/// <summary>
		/// Maximum Dependency Property
		/// </summary>
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(Point4D), typeof(AdjustableSliderQuadruplet),
				new FrameworkPropertyMetadata(new Point4D(100, 100, 100, 100), new PropertyChangedCallback(OnMaximumChanged)));

		/// <summary>
		/// Gets or sets the Maximum property.  This dependency property
		/// indicates Maximum allowed value for control.
		/// </summary>
		public Point4D Maximum
		{
			get { return (Point4D)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		/// <summary>
		/// Handles changes to the Maximum property.
		/// </summary>
		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AdjustableSliderQuadruplet)d).OnMaximumChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the Maximum property.
		/// </summary>
		protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
		{
			Point4D point = (Point4D)e.NewValue;
			this.xMaxTextBox.Text = point.X.ToString();
			this.yMaxTextBox.Text = point.Y.ToString();
			this.zMaxTextBox.Text = point.Z.ToString();
			this.wMaxTextBox.Text = point.W.ToString();
			this.UpdateAnimation();
		}

		#endregion
	}
}
