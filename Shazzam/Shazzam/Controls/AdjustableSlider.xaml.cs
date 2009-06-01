using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Shazzam.Controls {
  /// <summary>
  /// Interaction logic for AdjustableSlider.xaml
  /// </summary>
  public partial class AdjustableSlider : UserControl {
    public AdjustableSlider()
    {
      InitializeComponent();
      internalSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(internalSlider_ValueChanged);
      maxTextBox.TextChanged += new TextChangedEventHandler(maxTextBox_TextChanged);
      minTextBox.TextChanged += new TextChangedEventHandler(minTextBox_TextChanged);
      animationTextBox.TextChanged += new TextChangedEventHandler(animationTextBox_TextChanged);
      animationTextBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(animationTextBox_PreviewTextInput);
      NameScope.SetNameScope(sliderStackPanel, new NameScope());
      sliderStackPanel.RegisterName(internalSlider.Name, internalSlider);
    }

    void animationTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      if (!(char.IsNumber(e.Text[0]) || e.Text == "."))
      {
        e.Handled = true;
      }

    }

    DoubleAnimation sliderAnim = new DoubleAnimation();
    Storyboard story = new Storyboard();
    void animationTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      int candidate;

      if (int.TryParse(animationTextBox.Text, out candidate))
      {
        // negative animations should not be allowed
        if (candidate < 0)
        {
          return;
        }
        // do not show the slider when animation is active
        // reason:  the UI is too distracting on short animations
        if (candidate == 0)
        {
          sliderStackPanel.Visibility = Visibility.Visible;
        }
        else
        {
          sliderStackPanel.Visibility = Visibility.Collapsed;
        }
        ChangeAnimationTimespan(new TimeSpan(0, 0, 0, 0, candidate));
      }
    }

    private void ChangeAnimationTimespan(TimeSpan candidate)
    {

      sliderAnim.RepeatBehavior = RepeatBehavior.Forever;
      sliderAnim.AccelerationRatio = .25;
      sliderAnim.DecelerationRatio = .25;
      sliderAnim.From = internalSlider.Minimum;
      sliderAnim.To = internalSlider.Maximum;
      sliderAnim.Duration = candidate; 
      sliderAnim.AutoReverse = true;

      if (!story.Children.Contains(sliderAnim))
      {
        story.Children.Add(sliderAnim);

        Storyboard.SetTargetName(sliderAnim, internalSlider.Name);
        Storyboard.SetTargetProperty(sliderAnim, new PropertyPath(Slider.ValueProperty));

      }

      if (candidate == TimeSpan.Zero)
      {
        sliderAnim.BeginTime = null;
        story.Stop(sliderStackPanel);

      }
      else
      {
        sliderAnim.BeginTime = new TimeSpan(0, 0, 0, 0, 1);
        story.Begin(sliderStackPanel, true); // be sure and set the IsControllable param to true
      }


    }

    void minTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      double candidate;
      if (double.TryParse(minTextBox.Text, out candidate))
      {
        this.Minimum = candidate;
        try
        {
          ChangeAnimationTimespan(sliderAnim.Duration.TimeSpan);
        }
        catch
        { }
      }
    }

    void maxTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      double candidate;
      if (double.TryParse(maxTextBox.Text, out candidate))
      {
        this.Maximum = candidate;
        try
        {
          ChangeAnimationTimespan(sliderAnim.Duration.TimeSpan);
        }
        catch
        {
          //ignore any errors
        }

      }
    }

    void internalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      this.Value = e.NewValue;
      RaiseValueChangedEvent();
    }
    #region Value

    /// <summary>
    /// Value Dependency Property
    /// </summary>
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(Double), typeof(AdjustableSlider),
            new FrameworkPropertyMetadata((Double)1,
                new PropertyChangedCallback(OnValueChanged)));

    /// <summary>
    /// Gets or sets the Value property.  This dependency property
    /// indicates the current value of the AdjustableSlider.
    /// </summary>
    public Double Value
    {
      get { return (Double)GetValue(ValueProperty); }
      set { SetValue(ValueProperty, value); }
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
      internalSlider.Value = (Double)e.NewValue;
    }

    #endregion
    #region Minimum

    /// <summary>
    /// Minimum Dependency Property
    /// </summary>
    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register("Minimum", typeof(Double), typeof(AdjustableSlider),
            new FrameworkPropertyMetadata((Double)0,
                new PropertyChangedCallback(OnMinimumChanged)));

    /// <summary>
    /// Gets or sets the Minimum property.  This dependency property
    /// indicates Minimum allowed value for the control.
    /// </summary>
    public Double Minimum
    {
      get { return (Double)GetValue(MinimumProperty); }
      set { SetValue(MinimumProperty, value); }
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
    }

    #endregion

    #region Maximum

    /// <summary>
    /// Maximum Dependency Property
    /// </summary>
    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(Double), typeof(AdjustableSlider),
            new FrameworkPropertyMetadata((Double)100,
                new PropertyChangedCallback(OnMaximumChanged)));

    /// <summary>
    /// Gets or sets the Maximum property.  This dependency property
    /// indicates Maximum allowed value for control.
    /// </summary>
    public Double Maximum
    {
      get { return (Double)GetValue(MaximumProperty); }
      set { SetValue(MaximumProperty, value); }
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
    }

    #endregion

    #region AnimationValue

    /// <summary>
    /// AnimationValue Dependency Property
    /// </summary>
    public static readonly DependencyProperty AnimationValueProperty =
        DependencyProperty.Register("AnimationValue", typeof(Double), typeof(AdjustableSlider),
            new FrameworkPropertyMetadata((Double)0.0,
                new PropertyChangedCallback(OnAnimationValueChanged)));

    /// <summary>
    /// Gets or sets the AnimationValue property.  This dependency property
    /// indicates AnimationValue allowed value for control.
    /// </summary>
    public Double AnimationValue
    {
      get { return (Double)GetValue(AnimationValueProperty); }
      set { SetValue(AnimationValueProperty, value); }
    }

    /// <summary>
    /// Handles changes to the AnimationValue property.
    /// </summary>
    private static void OnAnimationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((AdjustableSlider)d).OnAnimationValueChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the AnimationValue property.
    /// </summary>
    protected virtual void OnAnimationValueChanged(DependencyPropertyChangedEventArgs e)
    {
      this.animationTextBox.Text = e.NewValue.ToString();
    }

    #endregion

    #region IsForwardAnimation

    /// <summary>
    /// IsForwardAnimation Dependency Property
    /// </summary>
    public static readonly DependencyProperty IsForwardAnimationProperty =
        DependencyProperty.Register("IsForwardAnimation", typeof(bool), typeof(AdjustableSlider),
            new FrameworkPropertyMetadata((bool)false,
                new PropertyChangedCallback(OnIsForwardAnimationChanged)));

    /// <summary>
    /// Gets or sets the IsForwardAnimation property.  This dependency property
    /// indicates whether animation is forward or backward.
    /// </summary>
    public bool IsForwardAnimation
    {
      get { return (bool)GetValue(IsForwardAnimationProperty); }
      set { SetValue(IsForwardAnimationProperty, value); }
    }

    /// <summary>
    /// Handles changes to the IsForwardAnimation property.
    /// </summary>
    private static void OnIsForwardAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((AdjustableSlider)d).OnIsForwardAnimationChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the IsForwardAnimation property.
    /// </summary>
    protected virtual void OnIsForwardAnimationChanged(DependencyPropertyChangedEventArgs e)
    {
    }

    #endregion

    #region ValueChanged

    /// <summary>
    /// ValueChanged Routed Event
    /// </summary>
    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
        RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AdjustableSlider));

    /// <summary>
    /// Occurs when Value changes
    /// </summary>
    public event RoutedEventHandler ValueChanged
    {
      add { AddHandler(ValueChangedEvent, value); }
      remove { RemoveHandler(ValueChangedEvent, value); }
    }

    /// <summary>
    /// A helper method to raise the ValueChanged event.
    /// </summary>
    protected RoutedEventArgs RaiseValueChangedEvent()
    {
      return RaiseValueChangedEvent(this);
    }

    /// <summary>
    /// A static helper method to raise the ValueChanged event on a target element.
    /// </summary>
    /// <param name="target">UIElement or ContentElement on which to raise the event</param>
    internal static RoutedEventArgs RaiseValueChangedEvent(DependencyObject target)
    {
      if (target == null) return null;

      RoutedEventArgs args = new RoutedEventArgs();
      args.RoutedEvent = ValueChangedEvent;
      RoutedEventHelper.RaiseEvent(target, args);
      return args;
    }

    #endregion

  }
}
