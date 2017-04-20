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

        public AdjustableSliderPair()
        {
            this.InitializeComponent();

            Storyboard.SetTarget(this.xSliderValueAnimation, this.xSlider);
            Storyboard.SetTargetProperty(this.xSliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.xSliderValueAnimation);

            Storyboard.SetTarget(this.ySliderValueAnimation, this.ySlider);
            Storyboard.SetTargetProperty(this.ySliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.ySliderValueAnimation);
            this.mainPanel.PreviewKeyDown += this.mainStackPanel_PreviewKeyDown;

            // this.xMinTextBox.TextChanged += this.XMinTextBox_TextChanged;
            // this.xMaxTextBox.TextChanged += this.XMaxTextBox_TextChanged;
            this.xMinTextBox.LostFocus += this.xMinTextBox_LostFocus;
            this.xMaxTextBox.LostFocus += this.xMaxTextBox_LostFocus;
            this.xSlider.ValueChanged += this.XSlider_ValueChanged;

            // this.yMinTextBox.TextChanged += this.YMinTextBox_TextChanged;
            // this.yMaxTextBox.TextChanged += this.YMaxTextBox_TextChanged;
            this.yMinTextBox.LostFocus += this.yMinTextBox_LostFocus;
            this.yMaxTextBox.LostFocus += this.yMaxTextBox_LostFocus;
            this.ySlider.ValueChanged += this.YSlider_ValueChanged;

            this.noAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.linearAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.circularAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.durationTextBox.TextChanged += this.DurationTextBox_TextChanged;
            this.durationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString();
        }

        //// private void YMaxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //// {
        ////  double number;
        ////  if (Double.TryParse(this.yMaxTextBox.Text, out number))
        ////  {
        ////    this.Maximum = new Point(this.Maximum.X, number);
        ////  }
        //// }

        private void yMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.yMaxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = new Point(this.Maximum.X, number);
            }
        }

        // private void YMinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        // {
        //  double number;
        //  if (Double.TryParse(this.yMinTextBox.Text, out number))
        //  {
        //    this.Minimum = new Point(this.Minimum.X, number);
        //  }
        // }
        private void yMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.yMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point(this.Minimum.X, number);
            }
        }

        private void xMaxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.xMaxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = new Point(number, this.Maximum.Y);
            }
        }

        private void xMinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.xMinTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = new Point(number, this.Minimum.Y);
            }
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

        private void XSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = new Point(e.NewValue, this.Value.Y);
        }

        private void YSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = new Point(this.Value.X, e.NewValue);
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
                this.UpdateAnimation();
            }
        }

        private void UpdateAnimation()
        {
            Point minimum = this.Minimum;
            Point maximum = this.Maximum;
            this.xSliderValueAnimation.From = minimum.X;
            this.xSliderValueAnimation.To = maximum.X;

            this.ySliderValueAnimation.From = minimum.Y;
            this.ySliderValueAnimation.To = maximum.Y;

            if (this.noAnimationToggleButton.IsChecked.GetValueOrDefault() ||
                this.xSliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
            {
                this.storyboard.Stop(this);
                this.xSlider.Visibility = Visibility.Visible;
                this.xSliderText.Visibility = Visibility.Collapsed;
                this.ySlider.Visibility = Visibility.Visible;
                this.ySliderText.Visibility = Visibility.Collapsed;
            }
            else
            {
                double duration = this.xSliderValueAnimation.Duration.TimeSpan.TotalSeconds;
                double yBeginTime = this.circularAnimationToggleButton.IsChecked.GetValueOrDefault() ? duration / 2 : 0;
                this.ySliderValueAnimation.BeginTime = TimeSpan.FromSeconds(yBeginTime);
                this.storyboard.Begin(this, isControllable: true);
                this.xSlider.Visibility = Visibility.Collapsed;
                this.xSliderText.Visibility = Visibility.Visible;
                this.ySlider.Visibility = Visibility.Collapsed;
                this.ySliderText.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(Point),
            typeof(AdjustableSliderPair),
            new FrameworkPropertyMetadata(new Point(0, 0), OnValueChanged));

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current value of the AdjustableSliderPair.
        /// </summary>
        public Point Value
        {
            get { return (Point)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderPair)d).OnValueChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            Point point = (Point)e.NewValue;
            this.xSlider.Value = point.X;
            this.ySlider.Value = point.Y;
        }

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(Point),
            typeof(AdjustableSliderPair),
                new FrameworkPropertyMetadata(new Point(0, 0), OnMinimumChanged));

        /// <summary>
        /// Gets or sets the Minimum property.  This dependency property
        /// indicates Minimum allowed value for the control.
        /// </summary>
        public Point Minimum
        {
            get { return (Point)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Minimum property.
        /// </summary>
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderPair)d).OnMinimumChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Minimum property.
        /// </summary>
        protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
        {
            Point point = (Point)e.NewValue;
            this.xMinTextBox.Text = point.X.ToString();
            this.yMinTextBox.Text = point.Y.ToString();
            this.UpdateAnimation();
        }

        /// <summary>
        /// Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(Point),
            typeof(AdjustableSliderPair),
            new FrameworkPropertyMetadata(new Point(100, 100), OnMaximumChanged));

        /// <summary>
        /// Gets or sets the Maximum property.  This dependency property
        /// indicates Maximum allowed value for control.
        /// </summary>
        public Point Maximum
        {
            get { return (Point)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Maximum property.
        /// </summary>
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSliderPair)d).OnMaximumChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
        {
            Point point = (Point)e.NewValue;
            this.xMaxTextBox.Text = point.X.ToString();
            this.yMaxTextBox.Text = point.Y.ToString();
            this.UpdateAnimation();
        }
    }
}
