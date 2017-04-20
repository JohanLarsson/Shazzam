using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Shazzam.Controls
{
    using Microsoft.Windows.Controls;

    public partial class AdjustableColor : UserControl
  {
    private const double DefaultDuration = 0.5;

    private Storyboard storyboard = new Storyboard();
    private ColorAnimation colorValueAnimation = new ColorAnimation
    {
      Duration = new Duration(TimeSpan.FromSeconds(DefaultDuration)),
      RepeatBehavior = RepeatBehavior.Forever,
      AutoReverse = true,
      AccelerationRatio = 0.25,
      DecelerationRatio = 0.25,

    };
    private Color _startColor;
    public AdjustableColor() {
      InitializeComponent();

      Storyboard.SetTarget(this.colorValueAnimation, this.colorPicker1);
      Storyboard.SetTargetProperty(this.colorValueAnimation, new PropertyPath(ColorPicker.SelectedColorProperty));
      this.storyboard.Children.Add(this.colorValueAnimation);
      this.mainPanel.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(mainStackPanel_PreviewKeyDown);

      this.noAnimationToggleButton.Click += this.AnimationToggleButton_Click;
      this.linearAnimationToggleButton.Click += this.AnimationToggleButton_Click;

      this.durationTextBox.TextChanged += this.DurationTextBox_TextChanged;
      durationTextBox.Text = Properties.Settings.Default.AnimationLengthDefault.ToString();

    }

    void mainStackPanel_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
      // pressing the enter key will move focus to next control
      var uie = e.OriginalSource as UIElement;
      if (e.Key == Key.Enter) {
        e.Handled = true;
        uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
      }
    }
    private void DurationTextBox_TextChanged(object sender, TextChangedEventArgs e) {
      double number;
      if (Double.TryParse(this.durationTextBox.Text, out number)) {
        TimeSpan duration = TimeSpan.FromSeconds(Math.Max(0, number));
        this.colorValueAnimation.Duration = duration;

        this.UpdateAnimation();
      }
    }
    private void AnimationToggleButton_Click(object sender, RoutedEventArgs e) {
      this.noAnimationToggleButton.IsChecked = (sender == this.noAnimationToggleButton);
      this.linearAnimationToggleButton.IsChecked = (sender == this.linearAnimationToggleButton);
      if (noAnimationToggleButton.IsChecked == true) {
        this.colorPicker1.SelectedColor = _startColor;
      }
      else {
        _startColor = this.colorPicker1.SelectedColor;
      }
      this.UpdateAnimation();
    }
    private void UpdateAnimation() {
      //this.sliderValueAnimation.From = this.Minimum;
      //this.sliderValueAnimation.To = this.Maximum;

      colorValueAnimation.From = colorPicker1.SelectedColor;
      colorValueAnimation.To = endColorPicker.SelectedColor;

      if (this.noAnimationToggleButton.IsChecked.GetValueOrDefault() ||
        this.colorValueAnimation.Duration.TimeSpan == TimeSpan.Zero) {
        this.storyboard.Stop(this);

        endColorPicker.Visibility = Visibility.Visible;
        endColorTextBlock.Visibility = Visibility.Visible;
      }
      else {
        this.storyboard.Begin(this, true);
        // moving the slider is distracting
        endColorPicker.Visibility = Visibility.Collapsed;
        endColorTextBlock.Visibility = Visibility.Collapsed;

      }
    }

    #region Value

    /// <summary>
    /// Value Dependency Property
    /// </summary>
    public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register("Value", typeof(Color), typeof(AdjustableColor),
        new FrameworkPropertyMetadata(Colors.LightYellow, new PropertyChangedCallback(OnValueChanged)));

    /// <summary>
    /// Gets or sets the Value property.  This dependency property
    /// indicates the current Value of the AdjustableColor.
    /// </summary>
    public Color Value {
      get { return (Color)GetValue(ValueProperty); }
      set { SetValue(ValueProperty, value); }
    }

    /// <summary>
    /// Handles changes to the Value property.
    /// </summary>
    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      ((AdjustableColor)d).OnValueChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the Value property.
    /// </summary>
    protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e) {
      // this.colorEditor1.SelectedColor = (Color)e.NewValue;
      this.colorPicker1.SelectedColor = (Color)e.NewValue;
      //  this.slider.Value = (Color)e.NewValue;
    }

    #endregion



    private void colorPicker1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e) {
      this.Value = e.NewValue;
    }
  }
}
