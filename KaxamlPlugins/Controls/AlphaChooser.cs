namespace KaxamlPlugins.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class AlphaChooser : FrameworkElement
    {
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(this);

                if (this.Orientation == Orientation.Vertical)
                {
                    this.Alpha = 1 - (p.Y / this.ActualHeight);
                }
                else
                {
                    this.Alpha = 1 - (p.X / this.ActualWidth);
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
                    this.Alpha = 1 - (p.Y / this.ActualHeight);
                }
                else
                {
                    this.Alpha = 1 - (p.X / this.ActualWidth);
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
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(AlphaChooser), new UIPropertyMetadata(Orientation.Vertical));


        public double Alpha
        {
            get { return (double) this.GetValue(AlphaProperty); }
            set { this.SetValue(AlphaProperty, value); }
        }

        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register("Alpha", typeof(double), typeof(AlphaChooser),
                new FrameworkPropertyMetadata(1.0,
                    new PropertyChangedCallback(AlphaChanged),
                    new CoerceValueCallback(AlphaCoerce)));

        public static void AlphaChanged(object o, DependencyPropertyChangedEventArgs e)
        {
            AlphaChooser h = (AlphaChooser)o;
            h.UpdateAlphaOffset();
            h.UpdateColor();
        }

        public static object AlphaCoerce(DependencyObject d, object Brightness)
        {
            double v = (double)Brightness;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }

        public double AlphaOffset
        {
            get { return (double) this.GetValue(AlphaOffsetProperty); }
            private set { this.SetValue(AlphaOffsetProperty, value); }
        }
        public static readonly DependencyProperty AlphaOffsetProperty =
            DependencyProperty.Register("AlphaOffset", typeof(double), typeof(AlphaChooser), new UIPropertyMetadata(0.0));

        private void UpdateAlphaOffset()
        {
            double length = this.ActualHeight;
            if (this.Orientation == Orientation.Horizontal) length = this.ActualWidth;

            this.AlphaOffset = length - (length * this.Alpha);
        }

        private void UpdateColor()
        {
            this.Color = Color.FromArgb((byte)Math.Round(this.Alpha * 255), 0, 0, 0);
            this.ColorBrush = new SolidColorBrush(this.Color);
        }

        public Color Color
        {
            get { return (Color) this.GetValue(ColorProperty); }
            private set { this.SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(AlphaChooser), new UIPropertyMetadata(Colors.Red));

        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush) this.GetValue(ColorBrushProperty); }
            private set { this.SetValue(ColorBrushProperty, value); }
        }
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(AlphaChooser), new UIPropertyMetadata(Brushes.Red));

        protected override void OnRender(DrawingContext dc)
        {
            LinearGradientBrush lb = new LinearGradientBrush();

            lb.StartPoint = new Point(0, 0);

            if (this.Orientation == Orientation.Vertical)
                lb.EndPoint = new Point(0, 1);
            else
                lb.EndPoint = new Point(1, 0);

            lb.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x00, 0x00, 0x00), 0.00));
            lb.GradientStops.Add(new GradientStop(Color.FromArgb(0x00, 0x00, 0x00, 0x00), 1.00));

            dc.DrawRectangle(lb, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            this.UpdateAlphaOffset();

        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.UpdateAlphaOffset();
            return base.ArrangeOverride(finalSize);
        }

    }
}