namespace Shazzam.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Interaction logic for AdjustableSliderPair.xaml.
    /// </summary>
    public partial class AdjustableSliderPair : UserControl
    {
        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(Point),
            typeof(AdjustableSliderPair),
            new FrameworkPropertyMetadata(new Point(0, 0), OnValueChanged));

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum),
            typeof(Point),
            typeof(AdjustableSliderPair),
            new FrameworkPropertyMetadata(new Point(0, 0), OnMinimumChanged));

        /// <summary>
        /// Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(Point),
            typeof(AdjustableSliderPair),
            new FrameworkPropertyMetadata(new Point(100, 100), OnMaximumChanged));

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

        public AdjustableSliderPair()
        {
            this.InitializeComponent();

            Storyboard.SetTarget(this.xSliderValueAnimation, this.XSlider);
            Storyboard.SetTargetProperty(this.xSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.xSliderValueAnimation);

            Storyboard.SetTarget(this.ySliderValueAnimation, this.YSlider);
            Storyboard.SetTargetProperty(this.ySliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.ySliderValueAnimation);
            this.MainPanel.PreviewKeyDown += this.MainStackPanelPreviewKeyDown;

            // this.xMinTextBox.TextChanged += this.XMinTextBox_TextChanged;
            // this.xMaxTextBox.TextChanged += this.XMaxTextBox_TextChanged;
            this.XMinTextBox.LostFocus += this.XMinTextBoxLostFocus;
            this.XMaxTextBox.LostFocus += this.XMaxTextBoxLostFocus;
            this.XSlider.ValueChanged += this.XSliderValueChanged;

            // this.yMinTextBox.TextChanged += this.YMinTextBox_TextChanged;
            // this.yMaxTextBox.TextChanged += this.YMaxTextBox_TextChanged;
            this.YMinTextBox.LostFocus += this.YMinTextBoxLostFocus;
            this.YMaxTextBox.LostFocus += this.YMaxTextBoxLostFocus;
            this.YSlider.ValueChanged += this.YSliderValueChanged;

            this.NoAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.LinearAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.CircularAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.DurationTextBox.TextChanged += this.DurationTextBoxTextChanged;
            this.DurationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current value of the AdjustableSliderPair.
        /// </summary>
        public Point Value
        {
            get => (Point)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the Minimum property.  This dependency property
        /// indicates Minimum allowed value for the control.
        /// </summary>
        public Point Minimum
        {
            get => (Point)this.GetValue(MinimumProperty);
            set => this.SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Gets or sets the Maximum property.  This dependency property
        /// indicates Maximum allowed value for control.
        /// </summary>
        public Point Maximum
        {
            get => (Point)this.GetValue(MaximumProperty);
            set => this.SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var point = (Point)e.NewValue;
            this.XSlider.SetCurrentValue(RangeBase.ValueProperty, point.X);
            this.YSlider.SetCurrentValue(RangeBase.ValueProperty, point.Y);
        }

        protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
        {
            var point = (Point)e.NewValue;
            this.XMinTextBox.SetCurrentValue(TextBox.TextProperty, point.X.ToString(CultureInfo.InvariantCulture));
            this.YMinTextBox.SetCurrentValue(TextBox.TextProperty, point.Y.ToString(CultureInfo.InvariantCulture));
            this.UpdateAnimation();
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
        {
            var point = (Point)e.NewValue;
            this.XMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.X.ToString(CultureInfo.InvariantCulture));
            this.YMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.Y.ToString(CultureInfo.InvariantCulture));
            this.UpdateAnimation();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderPair)d).OnValueChanged(e);
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderPair)d).OnMinimumChanged(e);
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderPair)d).OnMaximumChanged(e);
        }

        private void YMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.YMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point(this.Maximum.X, number));
            }
        }

        private void YMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.YMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point(this.Minimum.X, number));
            }
        }

        private void XMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.XMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point(number, this.Maximum.Y));
            }
        }

        private void XMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.XMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point(number, this.Minimum.Y));
            }
        }

        private void MainStackPanelPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // pressing the enter key will move focus to next control
            if (e.Key == Key.Enter &&
                e.OriginalSource is UIElement uie)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void XSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point(e.NewValue, this.Value.Y));
        }

        private void YSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point(this.Value.X, e.NewValue));
        }

        private void AnimationToggleButtonClick(object sender, RoutedEventArgs e)
        {
            this.NoAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.NoAnimationToggleButton));
            this.LinearAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.LinearAnimationToggleButton));
            this.CircularAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.CircularAnimationToggleButton));
            this.UpdateAnimation();
        }

        private void DurationTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(this.DurationTextBox.Text, out double number))
            {
                var duration = TimeSpan.FromSeconds(Math.Max(0, number));
                this.xSliderValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);
                this.ySliderValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);
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

            if (this.NoAnimationToggleButton.IsChecked.GetValueOrDefault() ||
                this.xSliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
            {
                this.storyboard.Stop(this);
                this.XSlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.XSliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.YSlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.YSliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                var duration = this.xSliderValueAnimation.Duration.TimeSpan.TotalSeconds;
                var yBeginTime = this.CircularAnimationToggleButton.IsChecked.GetValueOrDefault() ? duration / 2 : 0;
                this.ySliderValueAnimation.SetCurrentValue(Timeline.BeginTimeProperty, TimeSpan.FromSeconds(yBeginTime));
                this.storyboard.Begin(this, isControllable: true);
                this.XSlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.XSliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.YSlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.YSliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
        }
    }
}
