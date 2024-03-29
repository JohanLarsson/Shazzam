﻿namespace Shazzam.Controls
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
        /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(AdjustableSlider),
            new FrameworkPropertyMetadata(
                0.0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (d, e) => ((AdjustableSlider)d).OnValueChanged(e)));

        /// <summary>Identifies the <see cref="Minimum"/> dependency property.</summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum),
            typeof(double),
            typeof(AdjustableSlider),
            new FrameworkPropertyMetadata(
                0.0,
                (d, e) => ((AdjustableSlider)d).OnMinimumChanged(e)));

        /// <summary>Identifies the <see cref="Maximum"/> dependency property.</summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(double),
            typeof(AdjustableSlider),
            new FrameworkPropertyMetadata(
                100.0,
                (d, e) => ((AdjustableSlider)d).OnMaximumChanged(e)));

        private const double DefaultDuration = 2.0;

        private readonly Storyboard storyboard = new();
        private readonly DoubleAnimation sliderValueAnimation = new()
        {
            Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true,
            AccelerationRatio = 0.25,
            DecelerationRatio = 0.25,
        };

        public AdjustableSlider()
        {
            this.InitializeComponent();

            Storyboard.SetTarget(this.sliderValueAnimation, this.Slider);
            Storyboard.SetTargetProperty(this.sliderValueAnimation, new PropertyPath(RangeBase.ValueProperty));
            this.storyboard.Children.Add(this.sliderValueAnimation);
            this.MainPanel.PreviewKeyDown += this.MainStackPanel_PreviewKeyDown;
            this.Slider.ValueChanged += this.SliderValueChanged;

            this.NoAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.LinearAnimationToggleButton.Click += this.AnimationToggleButtonClick;

            this.DurationTextBox.TextChanged += this.DurationTextBoxTextChanged;
            this.DurationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString(CultureInfo.InvariantCulture);
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
            this.Slider.SetCurrentValue(RangeBase.ValueProperty, (double)e.NewValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Minimum property.
        /// </summary>
        protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e)
        {
            this.UpdateAnimation();
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e)
        {
            this.UpdateAnimation();
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

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SetCurrentValue(ValueProperty, e.NewValue);
        }

        private void AnimationToggleButtonClick(object sender, RoutedEventArgs e)
        {
            this.NoAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.NoAnimationToggleButton));
            this.LinearAnimationToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.LinearAnimationToggleButton));
            this.UpdateAnimation();
        }

        private void DurationTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(this.DurationTextBox.Text, out double number))
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

            if (this.NoAnimationToggleButton.IsChecked.GetValueOrDefault() ||
                this.sliderValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
            {
                this.storyboard.Stop(this);

                this.Slider.SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
            else
            {
                this.storyboard.Begin(this, isControllable: true);

                // moving the slider is distracting
                this.Slider.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
        }
    }
}
