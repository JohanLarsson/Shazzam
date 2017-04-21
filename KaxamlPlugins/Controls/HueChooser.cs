namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class HueChooser : FrameworkElement
    {
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(this);

                if (this.Orientation == Orientation.Vertical)
                {
                    this.Hue = 1 - (p.Y / this.ActualHeight);
                }
                else
                {
                    this.Hue = 1 - (p.X / this.ActualWidth);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(this);

                if (this.Orientation == Orientation.Vertical)
                {
                    this.Hue = 1 - (p.Y / this.ActualHeight);
                }
                else
                {
                    this.Hue = 1 - (p.X / this.ActualWidth);
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


        public Orientation Orientation
        {
            get { return (Orientation) this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(HueChooser), new UIPropertyMetadata(Orientation.Vertical));


        public double Hue
        {
            get { return (double) this.GetValue(HueProperty); }
            set { this.SetValue(HueProperty, value); }
        }

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(double), typeof(HueChooser),
                new FrameworkPropertyMetadata(0.0,
                    new PropertyChangedCallback(HueChanged),
                    new CoerceValueCallback(HueCoerce)));

        public static void HueChanged(object o, DependencyPropertyChangedEventArgs e)
        {
            HueChooser h = (HueChooser)o;
            h.UpdateHueOffset();
            h.UpdateColor();
        }

        public static object HueCoerce(DependencyObject d, object Brightness)
        {
            double v = (double)Brightness;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }

        public double HueOffset
        {
            get { return (double) this.GetValue(HueOffsetProperty); }
            private set { this.SetValue(HueOffsetProperty, value); }
        }
        public static readonly DependencyProperty HueOffsetProperty =
            DependencyProperty.Register("HueOffset", typeof(double), typeof(HueChooser), new UIPropertyMetadata(0.0));

        private void UpdateHueOffset()
        {
            double length = this.ActualHeight;
            if (this.Orientation == Orientation.Horizontal) length = this.ActualWidth;

            this.HueOffset = length - (length * this.Hue);
        }

        private void UpdateColor()
        {
            this.Color = ColorPickerUtil.ColorFromHSB(this.Hue, 1, 1);
            this.ColorBrush = new SolidColorBrush(this.Color);
        }

        public Color Color
        {
            get { return (Color) this.GetValue(ColorProperty); }
            private set { this.SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(HueChooser), new UIPropertyMetadata(Colors.Red));

        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush) this.GetValue(ColorBrushProperty); }
            private set { this.SetValue(ColorBrushProperty, value); }
        }
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(HueChooser), new UIPropertyMetadata(Brushes.Red));

        protected override void OnRender(DrawingContext dc)
        {
            LinearGradientBrush lb = new LinearGradientBrush();

            lb.StartPoint = new Point(0, 0);

            if (this.Orientation == Orientation.Vertical)
                lb.EndPoint = new Point(0, 1);
            else
                lb.EndPoint = new Point(1, 0);

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
    }
}