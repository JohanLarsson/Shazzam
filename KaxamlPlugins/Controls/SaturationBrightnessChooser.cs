namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public class SaturationBrightnessChooser : FrameworkElement
    {
        public Thickness OffsetPadding
        {
            get { return (Thickness) this.GetValue(OffsetPaddingProperty); }
            set { this.SetValue(OffsetPaddingProperty, value); }
        }
        public static readonly DependencyProperty OffsetPaddingProperty =
            DependencyProperty.Register("OffsetPadding", typeof(Thickness), typeof(SaturationBrightnessChooser), new UIPropertyMetadata(new Thickness(0.0)));

        public double Hue
        {
            private get { return (double) this.GetValue(HueProperty); }
            set { this.SetValue(HueProperty, value); }
        }
        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(double), typeof(SaturationBrightnessChooser), new
                FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(HueChanged)));


        public static void HueChanged(object o, DependencyPropertyChangedEventArgs e)
        {
            SaturationBrightnessChooser h = (SaturationBrightnessChooser)o;
            h.UpdateColor();
        }

        public double SaturationOffset
        {
            get { return (double) this.GetValue(SaturationOffsetProperty); }
            set { this.SetValue(SaturationOffsetProperty, value); }
        }
        public static readonly DependencyProperty SaturationOffsetProperty =
            DependencyProperty.Register("SaturationOffset", typeof(double), typeof(SaturationBrightnessChooser), new UIPropertyMetadata(0.0));

        public double Saturation
        {
            get { return (double) this.GetValue(SaturationProperty); }
            set { this.SetValue(SaturationProperty, value); }
        }

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(double), typeof(SaturationBrightnessChooser),
                new FrameworkPropertyMetadata(0.0,
                    new PropertyChangedCallback(SaturationChanged),
                    new CoerceValueCallback(SaturationCoerce)));

        public static void SaturationChanged(object o, DependencyPropertyChangedEventArgs e)
        {
            SaturationBrightnessChooser h = (SaturationBrightnessChooser)o;
            h.UpdateSaturationOffset();
        }

        public static object SaturationCoerce(DependencyObject d, object Brightness)
        {
            double v = (double)Brightness;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }




        public Color Color
        {
            get { return (Color) this.GetValue(ColorProperty); }
            private set { this.SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(SaturationBrightnessChooser), new UIPropertyMetadata(Colors.Red));

        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush) this.GetValue(ColorBrushProperty); }
            private set { this.SetValue(ColorBrushProperty, value); }
        }
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(SaturationBrightnessChooser), new UIPropertyMetadata(Brushes.Red));


        public double BrightnessOffset
        {
            get { return (double) this.GetValue(BrightnessOffsetProperty); }
            set { this.SetValue(BrightnessOffsetProperty, value); }
        }
        public static readonly DependencyProperty BrightnessOffsetProperty =
            DependencyProperty.Register("BrightnessOffset", typeof(double), typeof(SaturationBrightnessChooser), new UIPropertyMetadata(0.0));

        public double Brightness
        {
            get { return (double) this.GetValue(BrightnessProperty); }
            set { this.SetValue(BrightnessProperty, value); }
        }

        public static readonly DependencyProperty BrightnessProperty =
            DependencyProperty.Register("Brightness", typeof(double), typeof(SaturationBrightnessChooser),
                new FrameworkPropertyMetadata(0.0,
                    new PropertyChangedCallback(BrightnessChanged),
                    new CoerceValueCallback(BrightnessCoerce)));

        public static void BrightnessChanged(object o, DependencyPropertyChangedEventArgs e)
        {
            SaturationBrightnessChooser h = (SaturationBrightnessChooser)o;
            h.UpdateBrightnessOffset();
        }

        public static object BrightnessCoerce(DependencyObject d, object Brightness)
        {
            double v = (double)Brightness;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }

        private void UpdateSaturationOffset()
        {
            this.SaturationOffset = this.OffsetPadding.Left + ((this.ActualWidth - (this.OffsetPadding.Right + this.OffsetPadding.Left)) * this.Saturation);
        }

        private void UpdateBrightnessOffset()
        {
            this.BrightnessOffset = this.OffsetPadding.Top + ((this.ActualHeight - (this.OffsetPadding.Bottom + this.OffsetPadding.Top)) - ((this.ActualHeight - (this.OffsetPadding.Bottom + this.OffsetPadding.Top)) * this.Brightness));
        }

        protected override void OnRender(DrawingContext dc)
        {
            LinearGradientBrush h = new LinearGradientBrush();
            h.StartPoint = new Point(0, 0);
            h.EndPoint = new Point(1, 0);
            h.GradientStops.Add(new GradientStop(Colors.White, 0.00));
            h.GradientStops.Add(new GradientStop(ColorPickerUtil.ColorFromHSB(this.Hue, 1, 1), 1.0));
            dc.DrawRectangle(h, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            LinearGradientBrush v = new LinearGradientBrush();
            v.StartPoint = new Point(0, 0);
            v.EndPoint = new Point(0, 1);
            v.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0, 0, 0), 1.00));
            v.GradientStops.Add(new GradientStop(Color.FromArgb(0x80, 0, 0, 0), 0.50));
            v.GradientStops.Add(new GradientStop(Color.FromArgb(0x00, 0, 0, 0), 0.00));
            dc.DrawRectangle(v, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            this.UpdateSaturationOffset();
            this.UpdateBrightnessOffset();
        }

        public void UpdateColor()
        {
            this.Color = ColorPickerUtil.ColorFromHSB(this.Hue, this.Saturation, this.Brightness);
            this.ColorBrush = new SolidColorBrush(this.Color);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(this);
                this.Saturation = (p.X / (this.ActualWidth - this.OffsetPadding.Right));
                this.Brightness = (((this.ActualHeight - this.OffsetPadding.Bottom) - p.Y) / (this.ActualHeight - this.OffsetPadding.Bottom));
                this.UpdateColor();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            this.Saturation = (p.X / (this.ActualWidth - this.OffsetPadding.Right));
            this.Brightness = (((this.ActualHeight - this.OffsetPadding.Bottom) - p.Y) / (this.ActualHeight - this.OffsetPadding.Bottom));
            this.UpdateColor();

            Mouse.Capture(this);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            base.OnMouseUp(e);
        }
    }
}