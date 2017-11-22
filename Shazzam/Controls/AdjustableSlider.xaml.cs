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
    /// Interaction logic for AdjustableSlider.xaml.
    /// </summary>
    public partial class AdjustableSlider : UserControl
    {
        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(AdjustableSlider),
            new FrameworkPropertyMetadata(0.0, OnValueChanged));

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum),
            typeof(double),
            typeof(AdjustableSlider),
            new FrameworkPropertyMetadata(0.0, OnMinimumChanged));

        /// <summary>
        /// Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(double),
            typeof(AdjustableSlider),
            new FrameworkPropertyMetadata(100.0, OnMaximumChanged));

        private const double DefaultDuration = 0.5;

        private readonly Storyboard storyboard = new Storyboard();
        private readonly DoubleAnimation sliderValueAnimation = new DoubleAnimation
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
            this.mainPanel.PreviewKeyDown += this.MainStackPanel_PreviewKeyDown;
            //// this.minTextBox.TextChanged += this.MinTextBox_TextChanged;
            this.minTextBox.LostFocus += this.MinTextBoxLostFocus;
            this.maxTextBox.LostFocus += this.MaxTextBoxLostFocus;
            //// this.maxTextBox.TextChanged += this.MaxTextBox_TextChanged;
            this.slider.ValueChanged += this.SliderValueChanged;

            this.noAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.linearAnimationToggleButton.Click += this.AnimationToggleButtonClick;

            this.durationTextBox.TextChanged += this.DurationTextBoxTextChanged;
            this.durationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current value of the AdjustableSlider.
        /// </summary>
        public double Value
        {
            // ReSharper disable once UnusedMember.Global
            get => (double)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the Minimum property.  This dependency property
        /// indicates Minimum allowed value for the control.
        /// </summary>
        public double Minimum
        {
            get => (double)this.GetValue(MinimumProperty);
            set => this.SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Gets or sets the Maximum property.  This dependency property
        /// indicates Maximum allowed value for control.
        /// </summary>
        public double Maximum
        {
            get => (double)this.GetValue(MaximumProperty);
            set => this.SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            this.slider.SetCurrentValue(RangeBase.ValueProperty, (double)e.NewValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Minimum property.
        /// </summary>
        protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
        {
            this.minTextBox.SetCurrentValue(TextBox.TextProperty, e.NewValue.ToString());
            this.UpdateAnimation();
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
        {
            this.maxTextBox.SetCurrentValue(TextBox.TextProperty, e.NewValue.ToString());
            this.UpdateAnimation();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSlider)d).OnValueChanged(e);
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSlider)d).OnMinimumChanged(e);
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableSlider)d).OnMaximumChanged(e);
        }

        private void MainStackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // pressing the enter key will move focus to next control
            if (e.Key == Key.Enter &&
                e.OriginalSource is UIElement uie)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void MinTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.minTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MinimumProperty, number);
            }
        }

        private void MaxTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(this.maxTextBox.Text, NumberStyles.Any, null, out double number))
            {
                this.SetCurrentValue(MaximumProperty, number);
            }
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, e.NewValue);
        }

        private void AnimationToggleButtonClick(object sender, RoutedEventArgs e)
        {
            this.noAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.noAnimationToggleButton));
            this.linearAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.linearAnimationToggleButton));
            this.UpdateAnimation();
        }

        private void DurationTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(this.durationTextBox.Text, out double number))
            {
                var duration = TimeSpan.FromSeconds(Math.Max(0, number));
                this.sliderValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);

                this.UpdateAnimation();
            }
        }

        private void UpdateAnimation()
        {
            this.sliderValueAnimation.SetCurrentValue(DoubleAnimation.FromProperty, this.Minimum);
            this.sliderValueAnimation.SetCurrentValue(DoubleAnimation.ToProperty, this.Maximum);

            if (this.noAnimationToggleButton.IsChecked.GetValueOrDefault() ||
                this.sliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
            {
                this.storyboard.Stop(this);

                this.slider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.sliderText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                this.storyboard.Begin(this, isControllable: true);

                // moving the slider is distracting
                this.slider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.sliderText.SetCurrentValue(VisibilityProperty, Visibility.Visible);

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
    }
}
