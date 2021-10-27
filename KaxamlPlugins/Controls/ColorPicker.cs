// ReSharper disable ConvertClosureToMethodGroup
namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ColorPicker : Control
    {
        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ColorChanged),
            RoutingStrategy.Bubble,
            typeof(ColorChangedEventHandler),
            typeof(ColorPicker));

        public static readonly DependencyProperty ColorBrushProperty = DependencyProperty.Register(
            nameof(ColorBrush),
            typeof(SolidColorBrush),
            typeof(ColorPicker),
            new UIPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color),
            typeof(Color),
            typeof(ColorPicker),
            new UIPropertyMetadata(Colors.Black, OnColorChanged));

        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            nameof(Hue),
            typeof(double),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(
                0.0,
                (o, _) => OnHsbaChanged(o),
                (_, hue) => (double)hue! switch
                {
                    < 0 => 0.0,
                    > 1 => 1.0,
                    _ => hue,
                }));

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            nameof(Saturation),
            typeof(double),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(0.0, (o, _) => OnHsbaChanged(o), CoerceSaturation));

        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register(
            nameof(Brightness),
            typeof(double),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(0.0, (o, _) => OnHsbaChanged(o), CoerceBrightness));

        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            nameof(Alpha),
            typeof(double),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(1.0, (o, _) => OnHsbaChanged(o), CoerceAlpha));

        public static readonly DependencyProperty RProperty = DependencyProperty.Register(
            nameof(R),
            typeof(int),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(default(int), (o, _) => OnRgbaChanged(o), (_, o) => CoerceRgba(o)));

        public static readonly DependencyProperty GProperty = DependencyProperty.Register(
            nameof(G),
            typeof(int),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(default(int), (o, _) => OnRgbaChanged(o), (_, o) => CoerceRgba(o)));

        public static readonly DependencyProperty BProperty = DependencyProperty.Register(
            nameof(B),
            typeof(int),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(default(int), (o, _) => OnRgbaChanged(o), (_, o) => CoerceRgba(o)));

        public static readonly DependencyProperty AProperty = DependencyProperty.Register(
            nameof(A),
            typeof(int),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(255, (o, _) => OnRgbaChanged(o), (_, o) => CoerceRgba(o)));

        private bool hsbSetInternally;
        private bool rgbSetInternally;

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);

        public event ColorChangedEventHandler ColorChanged
        {
            add => this.AddHandler(ColorChangedEvent, value);
            remove => this.RemoveHandler(ColorChangedEvent, value);
        }

        public SolidColorBrush ColorBrush
        {
            get => (SolidColorBrush)this.GetValue(ColorBrushProperty);
            set => this.SetValue(ColorBrushProperty, value);
        }

        public Color Color
        {
            get => (Color)this.GetValue(ColorProperty);
            set => this.SetValue(ColorProperty, value);
        }

        public double Hue
        {
            get => (double)this.GetValue(HueProperty);
            set => this.SetValue(HueProperty, value);
        }

        public double Saturation
        {
            get => (double)this.GetValue(SaturationProperty);
            set => this.SetValue(SaturationProperty, value);
        }

        public double Brightness
        {
            get => (double)this.GetValue(BrightnessProperty);
            set => this.SetValue(BrightnessProperty, value);
        }

        public double Alpha
        {
            get => (double)this.GetValue(AlphaProperty);
            set => this.SetValue(AlphaProperty, value);
        }

        public int R
        {
            get => (int)this.GetValue(RProperty);
            set => this.SetValue(RProperty, value);
        }

        public int G
        {
            get => (int)this.GetValue(GProperty);
            set => this.SetValue(GProperty, value);
        }

        public int B
        {
            get => (int)this.GetValue(BProperty);
            set => this.SetValue(BProperty, value);
        }

        public int A
        {
            get => (int)this.GetValue(AProperty);
            set => this.SetValue(AProperty, value);
        }

        private static void OnColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var c = (ColorPicker)o;
            if (e.NewValue is Color color)
            {
                if (!c.hsbSetInternally)
                {
                    // update HSB value based on new value of color
                    ColorPickerUtil.HsbFromColor(color, out var h, out var s, out var b);
                    c.hsbSetInternally = true;
                    c.SetCurrentValue(AlphaProperty, color.A / 255.0);
                    c.SetCurrentValue(HueProperty, h);
                    c.SetCurrentValue(SaturationProperty, s);
                    c.SetCurrentValue(BrightnessProperty, b);
                    c.hsbSetInternally = false;
                }

                if (!c.rgbSetInternally)
                {
                    // update RGB value based on new value of color
                    c.rgbSetInternally = true;
                    c.SetCurrentValue(AProperty, (int)color.A);
                    c.SetCurrentValue(RProperty, (int)color.R);
                    c.SetCurrentValue(GProperty, (int)color.G);
                    c.SetCurrentValue(BProperty, (int)color.B);
                    c.rgbSetInternally = false;
                }

                c.RaiseColorChangedEvent((Color)e.NewValue);
            }
        }

        private static object CoerceBrightness(DependencyObject d, object? brightness)
        {
            return (double)brightness! switch
            {
                < 0 => 0.0,
                > 1 => 1.0,
                _ => brightness,
            };
        }

        private static object CoerceSaturation(DependencyObject d, object? saturation)
        {
            return (double)saturation! switch
            {
                < 0 => 0.0,
                > 1 => 1.0,
                _ => saturation,
            };
        }

        private static object CoerceAlpha(DependencyObject d, object? alpha)
        {
            var v = (double)alpha;
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

        private static void OnHsbaChanged(object sender)
        {
            var c = (ColorPicker)sender;
            var n = ColorPickerUtil.ColorFromAhsb(c.Alpha, c.Hue, c.Saturation, c.Brightness);

            c.hsbSetInternally = true;

            c.SetCurrentValue(ColorProperty, n);
            c.SetCurrentValue(ColorBrushProperty, new SolidColorBrush(n));

            c.hsbSetInternally = false;
        }

        private static object CoerceRgba(object value)
        {
            var v = (int)value;
            if (v < 0)
            {
                return 0;
            }

            if (v > 255)
            {
                return 255;
            }

            return v;
        }

        private static void OnRgbaChanged(object sender)
        {
            var c = (ColorPicker)sender;
            var n = Color.FromArgb((byte)c.A, (byte)c.R, (byte)c.G, (byte)c.B);

            c.rgbSetInternally = true;

            c.SetCurrentValue(ColorProperty, n);
            c.SetCurrentValue(ColorBrushProperty, new SolidColorBrush(n));

            c.rgbSetInternally = false;
        }

        private void RaiseColorChangedEvent(Color color)
        {
            var newEventArgs = new ColorChangedEventArgs(ColorChangedEvent, color);
            this.RaiseEvent(newEventArgs);
        }
    }
}
