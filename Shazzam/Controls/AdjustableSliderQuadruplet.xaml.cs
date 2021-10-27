﻿namespace Shazzam.Controls
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
    /// Interaction logic for AdjustableSliderQuadruplet.xaml.
    /// </summary>
    public partial class AdjustableSliderQuadruplet : UserControl
    {
        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(Point4D),
            typeof(AdjustableSliderQuadruplet),
            new FrameworkPropertyMetadata(new Point4D(0, 0, 0, 0), OnValueChanged));

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum),
            typeof(Point4D),
            typeof(AdjustableSliderQuadruplet),
            new FrameworkPropertyMetadata(new Point4D(0, 0, 0, 0), OnMinimumChanged));

        /// <summary>
        /// Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(Point4D),
            typeof(AdjustableSliderQuadruplet),
            new FrameworkPropertyMetadata(new Point4D(100, 100, 100, 100), OnMaximumChanged));

        private const double DefaultDuration = 0.5;

        private readonly Storyboard storyboard = new();
        private readonly DoubleAnimation xSliderValueAnimation = new()
        {
            Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true,
            AccelerationRatio = 0.25,
            DecelerationRatio = 0.25
        };

        private readonly DoubleAnimation ySliderValueAnimation = new()
        {
            Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true,
            AccelerationRatio = 0.25,
            DecelerationRatio = 0.25
        };

        private readonly DoubleAnimation zSliderValueAnimation = new()
        {
            Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true,
            AccelerationRatio = 0.25,
            DecelerationRatio = 0.25
        };

        private readonly DoubleAnimation wSliderValueAnimation = new()
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

            Storyboard.SetTarget(this.xSliderValueAnimation, this.XSlider);
            Storyboard.SetTargetProperty(this.xSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.xSliderValueAnimation);

            Storyboard.SetTarget(this.ySliderValueAnimation, this.YSlider);
            Storyboard.SetTargetProperty(this.ySliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.ySliderValueAnimation);

            Storyboard.SetTarget(this.zSliderValueAnimation, this.ZSlider);
            Storyboard.SetTargetProperty(this.zSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.zSliderValueAnimation);

            Storyboard.SetTarget(this.wSliderValueAnimation, this.WSlider);
            Storyboard.SetTargetProperty(this.wSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.wSliderValueAnimation);

            this.MainPanel.PreviewKeyDown += this.MainStackPanelPreviewKeyDown;

            this.XMinTextBox.LostFocus += this.XMinTextBoxLostFocus;
            this.XMaxTextBox.LostFocus += this.XMaxTextBoxLostFocus;
            this.XSlider.ValueChanged += this.XSliderValueChanged;

            this.YMinTextBox.LostFocus += this.YMinTextBoxLostFocus;
            this.YMaxTextBox.LostFocus += this.YMaxTextBoxLostFocus;
            this.YSlider.ValueChanged += this.YSliderValueChanged;

            this.ZMinTextBox.LostFocus += this.ZMinTextBoxLostFocus;
            this.ZMaxTextBox.LostFocus += this.ZMaxTextBoxLostFocus;
            this.ZSlider.ValueChanged += this.ZSliderValueChanged;

            // this.wMinTextBox.TextChanged += this.WMinTextBox_TextChanged;
            // this.wMaxTextBox.TextChanged += this.WMaxTextBox_TextChanged;
            this.WMinTextBox.LostFocus += this.WMinTextBoxLostFocus;
            this.WMaxTextBox.LostFocus += this.WMaxTextBoxLostFocus;
            this.WSlider.ValueChanged += this.WSliderValueChanged;

            this.NoAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.LinearAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.CircularAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.DurationTextBox.TextChanged += this.DurationTextBoxTextChanged;
            this.DurationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current value of the AdjustableSliderQuadruplet.
        /// </summary>
        public Point4D Value
        {
            get => (Point4D)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the Minimum property.  This dependency property
        /// indicates Minimum allowed value for the control.
        /// </summary>
        public Point4D Minimum
        {
            get => (Point4D)this.GetValue(MinimumProperty);
            set => this.SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Gets or sets the Maximum property.  This dependency property
        /// indicates Maximum allowed value for control.
        /// </summary>
        public Point4D Maximum
        {
            get => (Point4D)this.GetValue(MaximumProperty);
            set => this.SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var point = (Point4D)e.NewValue;
            this.XSlider.SetCurrentValue(RangeBase.ValueProperty, point.X);
            this.YSlider.SetCurrentValue(RangeBase.ValueProperty, point.Y);
            this.ZSlider.SetCurrentValue(RangeBase.ValueProperty, point.Z);
            this.WSlider.SetCurrentValue(RangeBase.ValueProperty, point.W);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Minimum property.
        /// </summary>
        protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
        {
            var point = (Point4D)e.NewValue;
            this.XMinTextBox.SetCurrentValue(TextBox.TextProperty, point.X.ToString(CultureInfo.InvariantCulture));
            this.YMinTextBox.SetCurrentValue(TextBox.TextProperty, point.Y.ToString(CultureInfo.InvariantCulture));
            this.ZMinTextBox.SetCurrentValue(TextBox.TextProperty, point.Z.ToString(CultureInfo.InvariantCulture));
            this.WMinTextBox.SetCurrentValue(TextBox.TextProperty, point.W.ToString(CultureInfo.InvariantCulture));
            this.UpdateAnimation();
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
        {
            var point = (Point4D)e.NewValue;
            this.XMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.X.ToString(CultureInfo.InvariantCulture));
            this.YMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.Y.ToString(CultureInfo.InvariantCulture));
            this.ZMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.Z.ToString(CultureInfo.InvariantCulture));
            this.WMaxTextBox.SetCurrentValue(TextBox.TextProperty, point.W.ToString(CultureInfo.InvariantCulture));
            this.UpdateAnimation();
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderQuadruplet)d).OnValueChanged(e);
        }

        /// <summary>
        /// Handles changes to the Minimum property.
        /// </summary>
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderQuadruplet)d).OnMinimumChanged(e);
        }

        /// <summary>
        /// Handles changes to the Maximum property.
        /// </summary>
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderQuadruplet)d).OnMaximumChanged(e);
        }

        private void MainStackPanelPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // pressing the enter key will move focus to next control
            if (e.OriginalSource is UIElement uie &&
                e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void XMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.XMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point4D(number, this.Minimum.Y, this.Minimum.Z, this.Minimum.W));
            }
        }

        private void XMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.XMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point4D(number, this.Maximum.Y, this.Maximum.Z, this.Maximum.W));
            }
        }

        private void XSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point4D(e.NewValue, this.Value.Y, this.Value.Z, this.Value.W));
        }

        private void YMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.YMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point4D(this.Minimum.X, number, this.Minimum.Z, this.Minimum.W));
            }
        }

        private void YMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.YMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point4D(this.Maximum.X, number, this.Maximum.Z, this.Maximum.W));
            }
        }

        private void YSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point4D(this.Value.X, e.NewValue, this.Value.Z, this.Value.W));
        }

        private void ZMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.ZMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point4D(this.Minimum.X, this.Minimum.Y, number, this.Minimum.W));
            }
        }

        private void ZMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.ZMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point4D(this.Maximum.X, this.Maximum.Y, number, this.Maximum.W));
            }
        }

        private void ZSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point4D(this.Value.X, this.Value.Y, e.NewValue, this.Value.W));
        }

        private void WMinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.WMinTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, new Point4D(this.Minimum.X, this.Minimum.Y, this.Minimum.Z, number));
            }
        }

        private void WMaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.WMaxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, new Point4D(this.Maximum.X, this.Maximum.Y, this.Maximum.Z, number));
            }
        }

        private void WSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, new Point4D(this.Value.X, this.Value.Y, this.Value.Z, e.NewValue));
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
                this.zSliderValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);
                this.wSliderValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);
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

            this.wSliderValueAnimation.SetCurrentValue(DoubleAnimation.FromProperty, minimum.W);
            this.wSliderValueAnimation.SetCurrentValue(DoubleAnimation.ToProperty, maximum.W);

            if (this.NoAnimationToggleButton.IsChecked.GetValueOrDefault() ||
                this.xSliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
            {
                this.storyboard.Stop(this);
                this.XSlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.XSliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.YSlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.YSliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.ZSlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.ZSliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.WSlider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.WSliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                var duration = this.xSliderValueAnimation.Duration.TimeSpan.TotalSeconds;
                var yBeginTime = this.CircularAnimationToggleButton.IsChecked.GetValueOrDefault() ? duration / 4 : 0;
                var zBeginTime = this.CircularAnimationToggleButton.IsChecked.GetValueOrDefault() ? 2 * duration / 4 : 0;
                var wBeginTime = this.CircularAnimationToggleButton.IsChecked.GetValueOrDefault() ? 3 * duration / 4 : 0;
                this.ySliderValueAnimation.SetCurrentValue(Timeline.BeginTimeProperty, TimeSpan.FromSeconds(yBeginTime));
                this.zSliderValueAnimation.SetCurrentValue(Timeline.BeginTimeProperty, TimeSpan.FromSeconds(zBeginTime));
                this.wSliderValueAnimation.SetCurrentValue(Timeline.BeginTimeProperty, TimeSpan.FromSeconds(wBeginTime));
                this.storyboard.Begin(this, isControllable: true);
                this.XSlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.XSliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.YSlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.YSliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.ZSlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.ZSliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.WSlider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.WSliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
        }
    }
}
