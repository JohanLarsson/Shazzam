using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Shazzam.Controls
{
	/// <summary>
	/// Interaction logic for AdjustableSliderTriplet.xaml.
	/// </summary>
	public partial class AdjustableSliderTriplet : UserControl
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

		public AdjustableSliderTriplet()
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

			this.xMinTextBox.TextChanged += this.XMinTextBox_TextChanged;
			this.xMaxTextBox.TextChanged += this.XMaxTextBox_TextChanged;
			this.xSlider.ValueChanged += this.XSlider_ValueChanged;

			this.yMinTextBox.TextChanged += this.YMinTextBox_TextChanged;
			this.yMaxTextBox.TextChanged += this.YMaxTextBox_TextChanged;
			this.ySlider.ValueChanged += this.YSlider_ValueChanged;

			this.zMinTextBox.TextChanged += this.ZMinTextBox_TextChanged;
			this.zMaxTextBox.TextChanged += this.ZMaxTextBox_TextChanged;
			this.zSlider.ValueChanged += this.ZSlider_ValueChanged;

			this.noAnimationToggleButton.Click += this.AnimationToggleButton_Click;
			this.linearAnimationToggleButton.Click += this.AnimationToggleButton_Click;
			this.circularAnimationToggleButton.Click += this.AnimationToggleButton_Click;
			this.durationTextBox.TextChanged += this.DurationTextBox_TextChanged;
		}

		private void XMinTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.xMinTextBox.Text, out number))
			{
				this.Minimum = new Point3D(number, this.Minimum.Y, this.Minimum.Z);
			}
		}

		private void XMaxTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.xMaxTextBox.Text, out number))
			{
				this.Maximum = new Point3D(number, this.Maximum.Y, this.Maximum.Z);
			}
		}

		private void XSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			this.Value = new Point3D(e.NewValue, this.Value.Y, this.Value.Z);
		}

		private void YMinTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.yMinTextBox.Text, out number))
			{
				this.Minimum = new Point3D(this.Minimum.X, number, this.Minimum.Z);
			}
		}

		private void YMaxTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.yMaxTextBox.Text, out number))
			{
				this.Maximum = new Point3D(this.Maximum.X, number, this.Maximum.Z);
			}
		}

		private void YSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			this.Value = new Point3D(this.Value.X, e.NewValue, this.Value.Z);
		}

		private void ZMinTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.zMinTextBox.Text, out number))
			{
				this.Minimum = new Point3D(this.Minimum.X, this.Minimum.Y, number);
			}
		}

		private void ZMaxTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			double number;
			if (Double.TryParse(this.zMaxTextBox.Text, out number))
			{
				this.Maximum = new Point3D(this.Maximum.X, this.Maximum.Y, number);
			}
		}

		private void ZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			this.Value = new Point3D(this.Value.X, this.Value.Y, e.NewValue);
		}

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
				this.UpdateAnimation();
			}
		}

		private void UpdateAnimation()
		{
			Point3D minimum = this.Minimum;
			Point3D maximum = this.Maximum;
			this.xSliderValueAnimation.From = minimum.X;
			this.xSliderValueAnimation.To = maximum.X;

			this.ySliderValueAnimation.From = minimum.Y;
			this.ySliderValueAnimation.To = maximum.Y;

			this.zSliderValueAnimation.From = minimum.Z;
			this.zSliderValueAnimation.To = maximum.Z;

			if (this.noAnimationToggleButton.IsChecked.GetValueOrDefault() ||
				this.xSliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
			{
				this.storyboard.Stop(this);
			}
			else
			{
				double duration = this.xSliderValueAnimation.Duration.TimeSpan.TotalSeconds;
				double yBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? duration / 3 : 0;
				double zBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? 2 * duration / 3 : 0;
				this.ySliderValueAnimation.BeginTime = TimeSpan.FromSeconds(yBeginTime);
				this.zSliderValueAnimation.BeginTime = TimeSpan.FromSeconds(zBeginTime);
				this.storyboard.Begin(this, true);
			}
		}

		#region Value

		/// <summary>
		/// Value Dependency Property
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(Point3D), typeof(AdjustableSliderTriplet),
				new FrameworkPropertyMetadata(new Point3D(0, 0, 0), new PropertyChangedCallback(OnValueChanged)));

		/// <summary>
		/// Gets or sets the Value property.  This dependency property
		/// indicates the current value of the AdjustableSliderTriplet.
		/// </summary>
		public Point3D Value
		{
			get { return (Point3D)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Handles changes to the Value property.
		/// </summary>
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AdjustableSliderTriplet)d).OnValueChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the Value property.
		/// </summary>
		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
		{
			Point3D point = (Point3D)e.NewValue;
			this.xSlider.Value = point.X;
			this.ySlider.Value = point.Y;
			this.zSlider.Value = point.Z;
		}

		#endregion

		#region Minimum

		/// <summary>
		/// Minimum Dependency Property
		/// </summary>
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(Point3D), typeof(AdjustableSliderTriplet),
				new FrameworkPropertyMetadata(new Point3D(0, 0, 0), new PropertyChangedCallback(OnMinimumChanged)));

		/// <summary>
		/// Gets or sets the Minimum property.  This dependency property
		/// indicates Minimum allowed value for the control.
		/// </summary>
		public Point3D Minimum
		{
			get { return (Point3D)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		/// <summary>
		/// Handles changes to the Minimum property.
		/// </summary>
		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AdjustableSliderTriplet)d).OnMinimumChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the Minimum property.
		/// </summary>
		protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
		{
			Point3D point = (Point3D)e.NewValue;
			this.xMinTextBox.Text = point.X.ToString();
			this.yMinTextBox.Text = point.Y.ToString();
			this.zMinTextBox.Text = point.Z.ToString();
			this.UpdateAnimation();
		}

		#endregion

		#region Maximum

		/// <summary>
		/// Maximum Dependency Property
		/// </summary>
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(Point3D), typeof(AdjustableSliderTriplet),
				new FrameworkPropertyMetadata(new Point3D(100, 100, 100), new PropertyChangedCallback(OnMaximumChanged)));

		/// <summary>
		/// Gets or sets the Maximum property.  This dependency property
		/// indicates Maximum allowed value for control.
		/// </summary>
		public Point3D Maximum
		{
			get { return (Point3D)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		/// <summary>
		/// Handles changes to the Maximum property.
		/// </summary>
		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AdjustableSliderTriplet)d).OnMaximumChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the Maximum property.
		/// </summary>
		protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
		{
			Point3D point = (Point3D)e.NewValue;
			this.xMaxTextBox.Text = point.X.ToString();
			this.yMaxTextBox.Text = point.Y.ToString();
			this.zMaxTextBox.Text = point.Z.ToString();
			this.UpdateAnimation();
		}

		#endregion
	}
}
