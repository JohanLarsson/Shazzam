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
            Storyboard.SetTargetProperty(this.xSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.xSliderValueAnimation);

            Storyboard.SetTarget(this.ySliderValueAnimation, this.ySlider);
            Storyboard.SetTargetProperty(this.ySliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.ySliderValueAnimation);

            Storyboard.SetTarget(this.zSliderValueAnimation, this.zSlider);
            Storyboard.SetTargetProperty(this.zSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.zSliderValueAnimation);

            Storyboard.SetTarget(this.wSliderValueAnimation, this.wSlider);
            Storyboard.SetTargetProperty(this.wSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.wSliderValueAnimation);

            this.mainPanel.PreviewKeyDown += this.mainStackPanel_PreviewKeyDown;

            this.xMinTextBox.LostFocus += this.xMinTextBox_LostFocus;
            this.xMaxTextBox.LostFocus += this.xMaxTextBox_LostFocus;
            this.xSlider.ValueChanged += this.XSlider_ValueChanged;

            this.yMinTextBox.LostFocus += this.yMinTextBox_LostFocus;
            this.yMaxTextBox.LostFocus += this.yMaxTextBox_LostFocus;
            this.ySlider.ValueChanged += this.YSlider_ValueChanged;

            this.zMinTextBox.LostFocus += this.zMinTextBox_LostFocus;
            this.zMaxTextBox.LostFocus += this.zMaxTextBox_LostFocus;
            this.zSlider.ValueChanged += this.ZSlider_ValueChanged;

            // this.wMinTextBox.TextChanged += this.WMinTextBox_TextChanged;
            // this.wMaxTextBox.TextChanged += this.WMaxTextBox_TextChanged;
            this.wMinTextBox.LostFocus += this.wMinTextBox_LostFocus;
            this.wMaxTextBox.LostFocus += this.wMaxTextBox_LostFocus;
            this.wSlider.ValueChanged += this.WSlider_ValueChanged;

            this.noAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.linearAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.circularAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.durationTextBox.TextChanged += this.DurationTextBox_TextChanged;
            this.durationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString();
        }

        private void mainStackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // pressing the enter key will move focus to next control
            var uie = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void xMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.xMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point4D(number, this.Minimum.Y, this.Minimum.Z, this.Minimum.W);
            }
        }

        private void xMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.xMaxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = new Point4D(number, this.Maximum.Y, this.Maximum.Z, this.Maximum.W);
            }
        }

        private void XSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = new Point4D(e.NewValue, this.Value.Y, this.Value.Z, this.Value.W);
        }

        private void yMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.yMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point4D(this.Minimum.X, number, this.Minimum.Z, this.Minimum.W);
            }
        }

        private void yMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.yMaxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = new Point4D(this.Maximum.X, number, this.Maximum.Z, this.Maximum.W);
            }
        }

        private void YSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = new Point4D(this.Value.X, e.NewValue, this.Value.Z, this.Value.W);
        }

        private void zMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.zMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point4D(this.Minimum.X, this.Minimum.Y, number, this.Minimum.W);
            }
        }

        private void zMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.zMaxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = new Point4D(this.Maximum.X, this.Maximum.Y, number, this.Maximum.W);
            }
        }

        private void ZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = new Point4D(this.Value.X, this.Value.Y, e.NewValue, this.Value.W);
        }

        private void wMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.wMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point4D(this.Minimum.X, this.Minimum.Y, this.Minimum.Z, number);
            }
        }

        private void wMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.wMaxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = new Point4D(this.Maximum.X, this.Maximum.Y, this.Maximum.Z, number);
            }
        }

        private void WSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = new Point4D(this.Value.X, this.Value.Y, this.Value.Z, e.NewValue);
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
            if (double.TryParse(this.durationTextBox.Text, out number))
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
                this.xSlider.Visibility = Visibility.Visible;
                this.xSliderText.Visibility = Visibility.Collapsed;
                this.ySlider.Visibility = Visibility.Visible;
                this.ySliderText.Visibility = Visibility.Collapsed;
                this.zSlider.Visibility = Visibility.Visible;
                this.zSliderText.Visibility = Visibility.Collapsed;
                this.wSlider.Visibility = Visibility.Visible;
                this.wSliderText.Visibility = Visibility.Collapsed;
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
                this.xSlider.Visibility = Visibility.Collapsed;
                this.xSliderText.Visibility = Visibility.Visible;
                this.ySlider.Visibility = Visibility.Collapsed;
                this.ySliderText.Visibility = Visibility.Visible;
                this.zSlider.Visibility = Visibility.Collapsed;
                this.zSliderText.Visibility = Visibility.Visible;
                this.wSlider.Visibility = Visibility.Collapsed;
                this.wSliderText.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(Point4D),
            typeof(AdjustableSliderQuadruplet),
            new FrameworkPropertyMetadata(new Point4D(0, 0, 0, 0), OnValueChanged));

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

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(Point4D),
            typeof(AdjustableSliderQuadruplet),
            new FrameworkPropertyMetadata(new Point4D(0, 0, 0, 0), OnMinimumChanged));

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

        /// <summary>
        /// Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(Point4D),
            typeof(AdjustableSliderQuadruplet),
            new FrameworkPropertyMetadata(new Point4D(100, 100, 100, 100), OnMaximumChanged));

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
    }
}
