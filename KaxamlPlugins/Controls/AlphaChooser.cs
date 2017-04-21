namespace KaxamlPlugins.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class AlphaChooser : FrameworkElement
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation",
            typeof(Orientation),
            typeof(AlphaChooser),
            new UIPropertyMetadata(Orientation.Vertical));

        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            "Alpha",
            typeof(double),
            typeof(AlphaChooser),
            new FrameworkPropertyMetadata(1.0, OnAlphaChanged, CoerceAlpha));

        public static readonly DependencyProperty AlphaOffsetProperty = DependencyProperty.Register(
            "AlphaOffset",
            typeof(double),
            typeof(AlphaChooser),
            new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color",
            typeof(Color),
            typeof(AlphaChooser),
            new UIPropertyMetadata(Colors.Red));

        public static readonly DependencyProperty ColorBrushProperty = DependencyProperty.Register(
            "ColorBrush",
            typeof(SolidColorBrush),
            typeof(AlphaChooser),
            new UIPropertyMetadata(Brushes.Red));

        public Orientation Orientation
        {
            get => (Orientation)this.GetValue(OrientationProperty);
            set => this.SetValue(OrientationProperty, value);
        }

        public double Alpha
        {
            get => (double)this.GetValue(AlphaProperty);
            set => this.SetValue(AlphaProperty, value);
        }

        public double AlphaOffset
        {
            get => (double)this.GetValue(AlphaOffsetProperty);
            set => this.SetValue(AlphaOffsetProperty, value);
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
                    this.SetCurrentValue(AlphaProperty, 1 - (p.Y / this.ActualHeight));
                }
                else
                {
                    this.SetCurrentValue(AlphaProperty, 1 - (p.X / this.ActualWidth));
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
                    this.SetCurrentValue(AlphaProperty, 1 - (p.Y / this.ActualHeight));
                }
                else
                {
                    this.SetCurrentValue(AlphaProperty, 1 - (p.X / this.ActualWidth));
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
                                            : new Point(1, 0)
                         };

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

        private static void OnAlphaChanged(object o, DependencyPropertyChangedEventArgs e)
        {
            var h = (AlphaChooser)o;
            h.UpdateAlphaOffset();
            h.UpdateColor();
        }

        private static object CoerceAlpha(DependencyObject d, object brightness)
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

        private void UpdateAlphaOffset()
        {
            var length = this.ActualHeight;
            if (this.Orientation == Orientation.Horizontal)
            {
                length = this.ActualWidth;
            }

            this.SetCurrentValue(AlphaOffsetProperty, length - (length * this.Alpha));
        }

        private void UpdateColor()
        {
            this.SetCurrentValue(ColorProperty, Color.FromArgb((byte)Math.Round(this.Alpha * 255), 0, 0, 0));
            this.SetCurrentValue(ColorBrushProperty, new SolidColorBrush(this.Color));
        }
    }
}