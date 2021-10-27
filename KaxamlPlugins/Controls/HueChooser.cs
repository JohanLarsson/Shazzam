namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class HueChooser : FrameworkElement
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(HueChooser),
            new UIPropertyMetadata(Orientation.Vertical));

        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            nameof(Hue),
            typeof(double),
            typeof(HueChooser),
            new FrameworkPropertyMetadata(0.0, OnHueChanged, CoerceHue));

        public static readonly DependencyProperty HueOffsetProperty = DependencyProperty.Register(
            nameof(HueOffset),
            typeof(double),
            typeof(HueChooser),
            new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color),
            typeof(Color),
            typeof(HueChooser),
            new UIPropertyMetadata(Colors.Red));

        public static readonly DependencyProperty ColorBrushProperty = DependencyProperty.Register(
            nameof(ColorBrush),
            typeof(SolidColorBrush),
            typeof(HueChooser),
            new UIPropertyMetadata(Brushes.Red));

        public Orientation Orientation
        {
            get => (Orientation)this.GetValue(OrientationProperty);
            set => this.SetValue(OrientationProperty, value);
        }

        public double Hue
        {
            get => (double)this.GetValue(HueProperty);
            set => this.SetValue(HueProperty, value);
        }

        public double HueOffset
        {
            get => (double)this.GetValue(HueOffsetProperty);
            set => this.SetValue(HueOffsetProperty, value);
        }

        public Color Color
        {
            get => (Color)this.GetValue(ColorProperty);
            set => this.SetValue(ColorProperty, value);
        }

        public SolidColorBrush ColorBrush
        {
            get => (SolidColorBrush)this.GetValue(ColorBrushProperty);
            set => this.SetValue(ColorBrushProperty, value);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var p = e.GetPosition(this);

                if (this.Orientation == Orientation.Vertical)
                {
                    this.SetCurrentValue(HueProperty, 1 - (p.Y / this.ActualHeight));
                }
                else
                {
                    this.SetCurrentValue(HueProperty, 1 - (p.X / this.ActualWidth));
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var p = e.GetPosition(this);

                if (this.Orientation == Orientation.Vertical)
                {
                    this.SetCurrentValue(HueProperty, 1 - (p.Y / this.ActualHeight));
                }
                else
                {
                    this.SetCurrentValue(HueProperty, 1 - (p.X / this.ActualWidth));
                }
            }

            Mouse.Capture(this);
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            base.OnMouseUp(e);
        }

        protected override void OnRender(DrawingContext dc)
        {
            var lb = new LinearGradientBrush
                         {
                             StartPoint = new Point(0, 0),
                             EndPoint = this.Orientation == Orientation.Vertical
                                            ? new Point(0, 1)
                                            : new Point(1, 0),
                         };

            lb.GradientStops.Add(new GradientStop(Color.FromRgb(0xFF, 0x00, 0x00), 1.00));
            lb.GradientStops.Add(new GradientStop(Color.FromRgb(0xFF, 0xFF, 0x00), 0.85));
            lb.GradientStops.Add(new GradientStop(Color.FromRgb(0x00, 0xFF, 0x00), 0.76));
            lb.GradientStops.Add(new GradientStop(Color.FromRgb(0x00, 0xFF, 0xFF), 0.50));
            lb.GradientStops.Add(new GradientStop(Color.FromRgb(0x00, 0x00, 0xFF), 0.33));
            lb.GradientStops.Add(new GradientStop(Color.FromRgb(0xFF, 0x00, 0xFF), 0.16));
            lb.GradientStops.Add(new GradientStop(Color.FromRgb(0xFF, 0x00, 0x00), 0.00));

            dc.DrawRectangle(lb, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            this.UpdateHueOffset();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.UpdateHueOffset();
            return base.ArrangeOverride(finalSize);
        }

        private static void OnHueChanged(object o, DependencyPropertyChangedEventArgs e)
        {
            var h = (HueChooser)o;
            h.UpdateHueOffset();
            h.UpdateColor();
        }

        private static object CoerceHue(DependencyObject d, object brightness)
        {
            var v = (double)brightness;
            if (v < 0)
            {
                return 0.0;
            }

            if (v > 1)
            {
                return 1.0;
            }

            return v;
        }

        private void UpdateHueOffset()
        {
            var length = this.ActualHeight;
            if (this.Orientation == Orientation.Horizontal)
            {
                length = this.ActualWidth;
            }

            this.SetCurrentValue(HueOffsetProperty, length - (length * this.Hue));
        }

        private void UpdateColor()
        {
            this.SetCurrentValue(ColorProperty, ColorPickerUtil.ColorFromHsb(this.Hue, 1, 1));
            this.SetCurrentValue(ColorBrushProperty, new SolidColorBrush(this.Color));
        }
    }
}
