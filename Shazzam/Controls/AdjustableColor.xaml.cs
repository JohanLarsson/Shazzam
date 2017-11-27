namespace Shazzam.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Microsoft.Windows.Controls;

    public partial class AdjustableColor : UserControl
    {
        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(Color),
            typeof(AdjustableColor),
            new FrameworkPropertyMetadata(Colors.LightYellow, OnValueChanged));

        private const double DefaultDuration = 0.5;

        private readonly Storyboard storyboard = new Storyboard();
        private readonly ColorAnimation colorValueAnimation = new ColorAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true,
            AccelerationRatio = 0.25,
            DecelerationRatio = 0.25,
        };

        private Color startColor;

        public AdjustableColor()
        {
            this.InitializeComponent();
            Storyboard.SetTarget(this.colorValueAnimation, this.ColorPicker1);
            Storyboard.SetTargetProperty(this.colorValueAnimation, new PropertyPath(ColorPicker.SelectedColorProperty));
            this.storyboard.Children.Add(this.colorValueAnimation);
            this.MainPanel.PreviewKeyDown += MainStackPanelPreviewKeyDown;

            this.NoAnimationToggleButton.Click += this.AnimationToggleButtonClick;
            this.LinearAnimationToggleButton.Click += this.AnimationToggleButtonClick;

            this.DurationTextBox.TextChanged += this.DurationTextBoxTextChanged;
            this.DurationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the Value property.  This dependency property
        /// indicates the current Value of the AdjustableColor.
        /// </summary>
        public Color Value
        {
            get => (Color)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdjustableColor)d).SetCurrentValue(ColorPicker.SelectedColorProperty, e.NewValue);
        }

        private static void MainStackPanelPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // pressing the enter key will move focus to next control
            if (e.OriginalSource is UIElement uie &&
                e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void ColorPicker1SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            this.SetCurrentValue(ValueProperty, e.NewValue);
        }

        private void DurationTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(this.DurationTextBox.Text, out double number))
            {
                var duration = TimeSpan.FromSeconds(Math.Max(0, number));
                this.colorValueAnimation.SetCurrentValue(Timeline.DurationProperty, (System.Windows.Duration)duration);
                this.UpdateAnimation();
            }
        }

        private void AnimationToggleButtonClick(object sender, RoutedEventArgs e)
        {
            this.NoAnimationToggleButton.SetCurrentValue(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.NoAnimationToggleButton));
            this.LinearAnimationToggleButton.SetCurrentValue(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, ReferenceEquals(sender, this.LinearAnimationToggleButton));
            if (this.NoAnimationToggleButton.IsChecked == true)
            {
                this.ColorPicker1.SetCurrentValue(ColorPicker.SelectedColorProperty, this.startColor);
            }
            else
            {
                this.startColor = this.ColorPicker1.SelectedColor;
            }

            this.UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            // this.sliderValueAnimation.From = this.Minimum;
            // this.sliderValueAnimation.To = this.Maximum;
            this.colorValueAnimation.SetCurrentValue(ColorAnimation.FromProperty, this.ColorPicker1.SelectedColor);
            this.colorValueAnimation.SetCurrentValue(ColorAnimation.ToProperty, this.EndColorPicker.SelectedColor);

            if (this.NoAnimationToggleButton.IsChecked.GetValueOrDefault() ||
                this.colorValueAnimation.Duration.TimeSpan == TimeSpan.Zero)
            {
                this.storyboard.Stop(this);

                this.EndColorPicker.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                this.EndColorTextBlock.SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
            else
            {
                this.storyboard.Begin(this, isControllable: true);
                //// moving the slider is distracting
                this.EndColorPicker.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                this.EndColorTextBlock.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
        }
    }
}
