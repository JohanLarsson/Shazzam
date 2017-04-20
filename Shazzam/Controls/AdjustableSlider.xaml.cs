namespace Shazzam.Controls
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Interaction logic for AdjustableSlider.xaml.
    /// </summary>
    public partial class AdjustableSlider : UserControl
    {
        private const double DefaultDuration = 0.5;

        private Storyboard storyboard = new Storyboard();
        private DoubleAnimation sliderValueAnimation = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true,
            AccelerationRatio = 0.25,
            DecelerationRatio = 0.25
        };

        public AdjustableSlider()
        {
            this.InitializeComponent();

            Storyboard.SetTarget(this.sliderValueAnimation, this.slider);
            Storyboard.SetTargetProperty(this.sliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.sliderValueAnimation);
            this.mainPanel.PreviewKeyDown += this.mainStackPanel_PreviewKeyDown;
            //// this.minTextBox.TextChanged += this.MinTextBox_TextChanged;
            this.minTextBox.LostFocus += this.MinTextBox_LostFocus;
            this.maxTextBox.LostFocus += this.maxTextBox_LostFocus;
            //// this.maxTextBox.TextChanged += this.MaxTextBox_TextChanged;
            this.slider.ValueChanged += this.Slider_ValueChanged;

            this.noAnimationToggleButton.Click += this.AnimationToggleButton_Click;
            this.linearAnimationToggleButton.Click += this.AnimationToggleButton_Click;

            this.durationTextBox.TextChanged += this.DurationTextBox_TextChanged;
            this.durationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString();
        }

        void mainStackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // pressing the enter key will move focus to next control
            var uie = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void MinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(this.minTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Minimum = number;
            }
        }

        void maxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;
            var separator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;

            if (double.TryParse(this.maxTextBox.Text, NumberStyles.Any, null, out number))
            {
                this.Maximum = number;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = e.NewValue;
        }

        private void AnimationToggleButton_Click(object sender, RoutedEventArgs e)
        {
            this.noAnimationToggleButton.IsChecked = sender == this.noAnimationToggleButton;
            this.linearAnimationToggleButton.IsChecked = sender == this.linearAnimationToggleButton;
            this.UpdateAnimation();
        }

        private void DurationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double number;
            if (double.TryParse(this.durationTextBox.Text, out number))
            {
                TimeSpan duration = TimeSpan.FromSeconds(Math.Max(0, number));
                this.sliderValueAnimation.Duration = duration;

                this.UpdateAnimation();
            }
        }

        private void UpdateAnimation()
        {
            this.sliderValueAnimation.From = this.Minimum;
            this.sliderValueAnimation.To = this.Maximum;

            if (this.noAnimationToggleButton.IsChecked.GetValueOrDefault() ||
                this.sliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
            {
                this.storyboard.Stop(this);

                this.slider.Visibility = Visibility.Visible;
                this.sliderText.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.storyboard.Begin(this, true);
                // moving the slider is distracting
                this.slider.Visibility = Visibility.Collapsed;
                this.sliderText.Visibility = Visibility.Visible;

                // get binding for reuse

                // if (this.sliderValueAnimation.Duration< new TimeSpan(0, 0, 1))
                // {
                // slider.Visibility = Visibility.Hidden;
                // sliderText.Visibility = Visibility.Visible;
                // //slider.IsEnabled = false;

                // }
                // else
                // {
                // slider.Visibility = Visibility.Visible;
                // sliderText.Visibility = Visibility.Hidden;
                // //slider.IsEnabled = true;
                // }
            }
        }

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(AdjustableSlider),
            new FrameworkPropertyMetadata(0.0, OnValueChanged));

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current value of the AdjustableSlider.
        /// </summary>
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSlider)d).OnValueChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            this.slider.Value = (double)e.NewValue;
        }

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(double),
            typeof(AdjustableSlider),
                new FrameworkPropertyMetadata(0.0, OnMinimumChanged));

        /// <summary>
        /// Gets or sets the Minimum property.  This dependency property
        /// indicates Minimum allowed value for the control.
        /// </summary>
        public double Minimum
        {
            get { return (double)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Minimum property.
        /// </summary>
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSlider)d).OnMinimumChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Minimum property.
        /// </summary>
        protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
        {
            this.minTextBox.Text = e.NewValue.ToString();
            this.UpdateAnimation();
        }

        /// <summary>
        /// Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(double),
            typeof(AdjustableSlider),
            new FrameworkPropertyMetadata(100.0, OnMaximumChanged));

        /// <summary>
        /// Gets or sets the Maximum property.  This dependency property
        /// indicates Maximum allowed value for control.
        /// </summary>
        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Maximum property.
        /// </summary>
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSlider)d).OnMaximumChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
        {
            this.maxTextBox.Text = e.NewValue.ToString();
            this.UpdateAnimation();
        }
    }
}
