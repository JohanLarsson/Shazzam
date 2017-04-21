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

            this.xMinTextBox.LostFocus += this.XMinTextBoxLostFocus;
            this.xMaxTextBox.LostFocus += this.XMaxTextBoxLostFocus;
            this.xSlider.ValueChanged += this.XSliderValueChanged;

            this.yMinTextBox.LostFocus += this.YMinTextBoxLostFocus;
            this.yMaxTextBox.LostFocus += this.YMaxTextBoxLostFocus;
            this.ySlider.ValueChanged += this.YSliderValueChanged;

            this.zMinTextBox.LostFocus += this.ZMinTextBoxLostFocus;
            this.zMaxTextBox.LostFocus += this.ZMaxTextBoxLostFocus;
            this.zSlider.ValueChanged += this.ZSliderValueChanged;

            this.noAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.linearAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.circularAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.durationTextBox.TextChanged += this.DurationTextBoxTextChanged;
            this.durationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current value of the AdjustableSliderTriplet.
        /// </summary>
        public Point3D Value
        {
            get => (Point3D)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the Minimum property.  This dependency property
        /// indicates Minimum allowed value for the control.
        /// </summary>
        public Point3D Minimum
        {
            get => (Point3D)this.GetValue(MinimumProperty);
            set => this.SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Gets or sets the Maximum property.  This dependency property
        /// indicates Maximum allowed value for control.
        /// </summary>
        public Point3D Maximum
        {
            get => (Point3D)this.GetValue(MaximumProperty);
            set => this.SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var point = (Point3D)e.NewValue;
            this.xSlider.SetCurrentValue(RangeBase.ValueProperty, point.X);
            this.ySlider.SetCurrentValue(RangeBase.ValueProperty, point.Y);
            this.zSlider.SetCurrentValue(RangeBase.ValueProperty, point.Z);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
        {
            var point = (Point3D)e.NewValue;
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
            var point = (Point3D)e.NewValue;
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

        private void XMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.xMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point3D(number, this.Minimum.Y, this.Minimum.Z));
            }
        }

        private void XMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.xMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point3D(number, this.Maximum.Y, this.Maximum.Z));
            }
        }

        private void XSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point3D(e.NewValue, this.Value.Y, this.Value.Z));
        }

        private void YMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.yMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point3D(this.Minimum.X, number, this.Minimum.Z));
            }
        }

        private void YMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.yMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point3D(this.Maximum.X, number, this.Maximum.Z));
            }
        }

        private void YSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point3D(this.Value.X, e.NewValue, this.Value.Z));
        }

        private void ZMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.zMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point3D(this.Minimum.X, this.Minimum.Y, number));
            }
        }

        private void ZMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.zMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point3D(this.Maximum.X, this.Maximum.Y, number));
            }
        }

        private void ZSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point3D(this.Value.X, this.Value.Y, e.NewValue));
        }

        private void AnimationToggleButtonClick(object sender, RoutedEventArgs e)
        {
            this.noAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.noAnimationToggleButton));
            this.linearAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.linearAnimationToggleButton));
            this.circularAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.circularAnimationToggleButton));
            this.UpdateAnimation();
        }

        private void DurationTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(this.durationTextBox.Text, out double number))
            {
                var duration = TimeSpan.FromSeconds(Math.Max(0, number));
                this.xSliderValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);
                this.ySliderValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);
                this.zSliderValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);
                this.UpdateAnimation();
            }
        }

        private void UpdateAnimation()
        {
            var minimum = this.Minimum;
            var maximum = this.Maximum;
            this.xSliderValueAnimation.SetCurrentValue(DoubleAnimation.FromProperty, minimum.X);
            this.xSliderValueAnimation.SetCurrentValue(DoubleAnimation.ToProperty, maximum.X);

            this.ySliderValueAnimation.SetCurrentValue(DoubleAnimation.FromProperty, minimum.Y);
            this.ySliderValueAnimation.SetCurrentValue(DoubleAnimation.ToProperty, maximum.Y);

            this.zSliderValueAnimation.SetCurrentValue(DoubleAnimation.FromProperty, minimum.Z);
            this.zSliderValueAnimation.SetCurrentValue(DoubleAnimation.ToProperty, maximum.Z);

            if (this.noAnimationToggleButton.IsChecked.GetValueOrDefault() ||
                this.xSliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
            {
                this.storyboard.Stop(this);
                this.xSlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.xSliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.ySlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.ySliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.zSlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.zSliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                var duration = this.xSliderValueAnimation.Duration.TimeSpan.TotalSeconds;
                var yBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? duration / 3 : 0;
                var zBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? 2 * duration / 3 : 0;
                this.ySliderValueAnimation.SetCurrentValue(Timeline.BeginTimeProperty, TimeSpan.FromSeconds(yBeginTime));
                this.zSliderValueAnimation.SetCurrentValue(Timeline.BeginTimeProperty, TimeSpan.FromSeconds(zBeginTime));
                this.storyboard.Begin(this, isControllable: true);
                this.xSlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.xSliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.ySlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.ySliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.zSlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.zSliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
        }
    }
}
