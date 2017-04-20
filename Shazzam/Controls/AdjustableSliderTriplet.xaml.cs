namespace Shazzam.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// Interaction logic for AdjustableSliderTriplet.xaml.
    /// </summary>
    public partial class AdjustableSliderTriplet : UserControl
    {
        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(Point3D),
            typeof(AdjustableSliderTriplet),
            new FrameworkPropertyMetadata(new Point3D(0, 0, 0), OnValueChanged));

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(Point3D),
            typeof(AdjustableSliderTriplet),
            new FrameworkPropertyMetadata(new Point3D(0, 0, 0), OnMinimumChanged));

        /// <summary>
        /// Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(Point3D),
            typeof(AdjustableSliderTriplet),
            new FrameworkPropertyMetadata(new Point3D(100, 100, 100), OnMaximumChanged));

        private const double DefaultDuration = 0.5;

        private readonly Storyboard storyboard = new Storyboard();
        private readonly DoubleAnimation xSliderValueAnimation = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true,
            AccelerationRatio = 0.25,
            DecelerationRatio = 0.25
        };

        private readonly DoubleAnimation ySliderValueAnimation = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true,
            AccelerationRatio = 0.25,
            DecelerationRatio = 0.25
        };

        private readonly DoubleAnimation zSliderValueAnimation = new DoubleAnimation
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
            Storyboard.SetTargetProperty(this.xSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.xSliderValueAnimation);

            Storyboard.SetTarget(this.ySliderValueAnimation, this.ySlider);
            Storyboard.SetTargetProperty(this.ySliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.ySliderValueAnimation);

            Storyboard.SetTarget(this.zSliderValueAnimation, this.zSlider);
            Storyboard.SetTargetProperty(this.zSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.zSliderValueAnimation);

            this.mainPanel.PreviewKeyDown += this.MainStackPanel_PreviewKeyDown;

            this.xMinTextBox.LostFocus += this.XMinTextBox_LostFocus;
            this.xMaxTextBox.LostFocus += this.XMaxTextBox_LostFocus;
            this.xSlider.ValueChanged += this.XSlider_ValueChanged;

            this.yMinTextBox.LostFocus += this.YMinTextBox_LostFocus;
            this.yMaxTextBox.LostFocus += this.YMaxTextBox_LostFocus;
            this.ySlider.ValueChanged += this.YSlider_ValueChanged;

            this.zMinTextBox.LostFocus += this.ZMinTextBox_LostFocus;
            this.zMaxTextBox.LostFocus += this.ZMaxTextBox_LostFocus;
            this.zSlider.ValueChanged += this.ZSlider_ValueChanged;

            this.noAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.linearAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.circularAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.durationTextBox.TextChanged += this.DurationTextBox_TextChanged;
            this.durationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current value of the AdjustableSliderTriplet.
        /// </summary>
        public Point3D Value
        {
            get { return (Point3D)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Minimum property.  This dependency property
        /// indicates Minimum allowed value for the control.
        /// </summary>
        public Point3D Minimum
        {
            get { return (Point3D)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Maximum property.  This dependency property
        /// indicates Maximum allowed value for control.
        /// </summary>
        public Point3D Maximum
        {
            get { return (Point3D)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
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

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
        {
            Point3D point = (Point3D)e.NewValue;
            this.xMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.X.ToString(CultureInfo.InvariantCulture));
            this.yMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.Y.ToString(CultureInfo.InvariantCulture));
            this.zMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.Z.ToString(CultureInfo.InvariantCulture));
            this.UpdateAnimation();
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Minimum property.
        /// </summary>
        protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
        {
            Point3D point = (Point3D)e.NewValue;
            this.xMinTextBox.SetCurrentValue(TextBox.TextProperty, point.X.ToString(CultureInfo.InvariantCulture));
            this.yMinTextBox.SetCurrentValue(TextBox.TextProperty, point.Y.ToString(CultureInfo.InvariantCulture));
            this.zMinTextBox.SetCurrentValue(TextBox.TextProperty, point.Z.ToString(CultureInfo.InvariantCulture));
            this.UpdateAnimation();
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderTriplet)d).OnValueChanged(e);
        }

        /// <summary>
        /// Handles changes to the Minimum property.
        /// </summary>
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderTriplet)d).OnMinimumChanged(e);
        }

        /// <summary>
        /// Handles changes to the Maximum property.
        /// </summary>
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderTriplet)d).OnMaximumChanged(e);
        }

        private void MainStackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // pressing the enter key will move focus to next control
            var uie = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void XMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.xMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point3D(number, this.Minimum.Y, this.Minimum.Z);
            }
        }

        private void XMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.xMaxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = new Point3D(number, this.Maximum.Y, this.Maximum.Z);
            }
        }

        private void XSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = new Point3D(e.NewValue, this.Value.Y, this.Value.Z);
        }

        private void YMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.yMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point3D(this.Minimum.X, number, this.Minimum.Z);
            }
        }

        private void YMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.yMaxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = new Point3D(this.Maximum.X, number, this.Maximum.Z);
            }
        }

        private void YSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = new Point3D(this.Value.X, e.NewValue, this.Value.Z);
        }

        private void ZMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.zMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point3D(this.Minimum.X, this.Minimum.Y, number);
            }
        }

        private void ZMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.zMaxTextBox.Text, NumberStyles.Any, null, out number))
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
            this.noAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, sender == this.noAnimationToggleButton);
            this.linearAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, sender == this.linearAnimationToggleButton);
            this.circularAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, sender == this.circularAnimationToggleButton);
            this.UpdateAnimation();
        }

        private void DurationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double number;
            if (double.TryParse(this.durationTextBox.Text, out number))
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
                this.xSlider.Visibility = Visibility.Visible;
                this.xSliderText.Visibility = Visibility.Collapsed;
                this.ySlider.Visibility = Visibility.Visible;
                this.ySliderText.Visibility = Visibility.Collapsed;
                this.zSlider.Visibility = Visibility.Visible;
                this.zSliderText.Visibility = Visibility.Collapsed;
            }
            else
            {
                double duration = this.xSliderValueAnimation.Duration.TimeSpan.TotalSeconds;
                double yBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? duration / 3 : 0;
                double zBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? 2 * duration / 3 : 0;
                this.ySliderValueAnimation.BeginTime = TimeSpan.FromSeconds(yBeginTime);
                this.zSliderValueAnimation.BeginTime = TimeSpan.FromSeconds(zBeginTime);
                this.storyboard.Begin(this, isControllable: true);
                this.xSlider.Visibility = Visibility.Collapsed;
                this.xSliderText.Visibility = Visibility.Visible;
                this.ySlider.Visibility = Visibility.Collapsed;
                this.ySliderText.Visibility = Visibility.Visible;
                this.zSlider.Visibility = Visibility.Collapsed;
                this.zSliderText.Visibility = Visibility.Visible;
            }
        }
    }
}
