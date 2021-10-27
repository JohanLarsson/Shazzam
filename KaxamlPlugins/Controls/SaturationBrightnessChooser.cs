namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public class SaturationBrightnessChooser : FrameworkElement
    {
        public static readonly DependencyProperty OffsetPaddingProperty = DependencyProperty.Register(
            nameof(OffsetPadding),
            typeof(Thickness),
            typeof(SaturationBrightnessChooser),
            new UIPropertyMetadata(new Thickness(0.0)));

        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            nameof(Hue),
            typeof(double),
            typeof(SaturationBrightnessChooser),
            new FrameworkPropertyMetadata(
                0.0,
                FrameworkPropertyMetadataOptions.AffectsRender,
                (o, _) => ((SaturationBrightnessChooser)o).UpdateColor()));

        public static readonly DependencyProperty SaturationOffsetProperty = DependencyProperty.Register(
            nameof(SaturationOffset),
            typeof(double),
            typeof(SaturationBrightnessChooser),
            new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            nameof(Saturation),
            typeof(double),
            typeof(SaturationBrightnessChooser),
            new FrameworkPropertyMetadata(
                0.0,
                (o, _) => ((SaturationBrightnessChooser)o).UpdateSaturationOffset(),
                (_, baseValue) => Coerce.ClampDouble(baseValue, 0, 1)));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color),
            typeof(Color),
            typeof(SaturationBrightnessChooser),
            new UIPropertyMetadata(Colors.Red));

        public static readonly DependencyProperty ColorBrushProperty = DependencyProperty.Register(
            nameof(ColorBrush),
            typeof(SolidColorBrush),
            typeof(SaturationBrightnessChooser),
            new UIPropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty BrightnessOffsetProperty = DependencyProperty.Register(
            nameof(BrightnessOffset),
            typeof(double),
            typeof(SaturationBrightnessChooser),
            new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register(
            nameof(Brightness),
            typeof(double),
            typeof(SaturationBrightnessChooser),
            new FrameworkPropertyMetadata(
                0.0,
                (o, _) => ((SaturationBrightnessChooser)o).UpdateBrightnessOffset(),
                (_, baseValue) => Coerce.ClampDouble(baseValue, 0, 1)));

        public Thickness OffsetPadding
        {
            get => (Thickness)this.GetValue(OffsetPaddingProperty);
            set => this.SetValue(OffsetPaddingProperty, value);
        }

        public double Hue
        {
            private get => (double)this.GetValue(HueProperty);
            set => this.SetValue(HueProperty, value);
        }

        public double SaturationOffset
        {
            get => (double)this.GetValue(SaturationOffsetProperty);
            set => this.SetValue(SaturationOffsetProperty, value);
        }

        public double Saturation
        {
            get => (double)this.GetValue(SaturationProperty);
            set => this.SetValue(SaturationProperty, value);
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

        public double BrightnessOffset
        {
            get => (double)this.GetValue(BrightnessOffsetProperty);
            set => this.SetValue(BrightnessOffsetProperty, value);
        }

        public double Brightness
        {
            get => (double)this.GetValue(BrightnessProperty);
            set => this.SetValue(BrightnessProperty, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            var h = new LinearGradientBrush { StartPoint = new Point(0, 0), EndPoint = new Point(1, 0) };
            h.GradientStops.Add(new GradientStop(Colors.White, 0.00));
            h.GradientStops.Add(new GradientStop(ColorPickerUtil.ColorFromHsb(this.Hue, 1, 1), 1.0));
            dc.DrawRectangle(h, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            var v = new LinearGradientBrush { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
            v.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0, 0, 0), 1.00));
            v.GradientStops.Add(new GradientStop(Color.FromArgb(0x80, 0, 0, 0), 0.50));
            v.GradientStops.Add(new GradientStop(Color.FromArgb(0x00, 0, 0, 0), 0.00));
            dc.DrawRectangle(v, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            this.UpdateSaturationOffset();
            this.UpdateBrightnessOffset();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var p = e.GetPosition(this);
                this.SetCurrentValue(SaturationProperty, p.X / (this.ActualWidth - this.OffsetPadding.Right));
                this.SetCurrentValue(
                    BrightnessProperty,
                    (this.ActualHeight - this.OffsetPadding.Bottom - p.Y) / (this.ActualHeight - this.OffsetPadding.Bottom));
                this.UpdateColor();
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            this.SetCurrentValue(SaturationProperty, p.X / (this.ActualWidth - this.OffsetPadding.Right));
            this.SetCurrentValue(
                BrightnessProperty,
                (this.ActualHeight - this.OffsetPadding.Bottom - p.Y) / (this.ActualHeight - this.OffsetPadding.Bottom));
            this.UpdateColor();

            Mouse.Capture(this);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            base.OnMouseUp(e);
        }

        private void UpdateColor()
        {
            this.SetCurrentValue(ColorProperty, ColorPickerUtil.ColorFromHsb(this.Hue, this.Saturation, this.Brightness));
            this.SetCurrentValue(ColorBrushProperty, new SolidColorBrush(this.Color));
        }

        private void UpdateSaturationOffset()
        {
            this.SetCurrentValue(
                SaturationOffsetProperty,
                this.OffsetPadding.Left + ((this.ActualWidth - (this.OffsetPadding.Right + this.OffsetPadding.Left)) * this.Saturation));
        }

        private void UpdateBrightnessOffset()
        {
            this.SetCurrentValue(
                BrightnessOffsetProperty,
                this.OffsetPadding.Top + (this.ActualHeight - (this.OffsetPadding.Bottom + this.OffsetPadding.Top) - ((this.ActualHeight - (this.OffsetPadding.Bottom + this.OffsetPadding.Top)) * this.Brightness)));
        }
    }
}
