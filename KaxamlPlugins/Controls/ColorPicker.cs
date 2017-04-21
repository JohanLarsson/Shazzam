namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ColorPicker : Control
    {
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        /// <summary>
        /// ColorBrush Property
        /// </summary>

        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush) this.GetValue(ColorBrushProperty); }
            set { this.SetValue(ColorBrushProperty, value); }
        }
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(ColorPicker), new UIPropertyMetadata(Brushes.Black));



        /// <summary>
        /// Color Property
        /// </summary>

        bool HSBSetInternally = false;
        bool RGBSetInternally = false;

        public static void OnColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)o;


            if (e.NewValue is Color)
            {
                Color color = (Color)e.NewValue;

                if (!c.HSBSetInternally)
                {
                    // update HSB value based on new value of color

                    double H = 0;
                    double S = 0;
                    double B = 0;
                    ColorPickerUtil.HSBFromColor(color, ref H, ref S, ref B);

                    c.HSBSetInternally = true;

                    c.Alpha = (double)(color.A / 255);
                    c.Hue = H;
                    c.Saturation = S;
                    c.Brightness = B;

                    c.HSBSetInternally = false;
                }

                if (!c.RGBSetInternally)
                {
                    // update RGB value based on new value of color

                    c.RGBSetInternally = true;

                    c.A = color.A;
                    c.R = color.R;
                    c.G = color.G;
                    c.B = color.B;

                    c.RGBSetInternally = false;
                }

                c.RaiseColorChangedEvent((Color) e.NewValue);

            }
        }

        public Color Color
        {
            get { return (Color) this.GetValue(ColorProperty); }
            set
            {
                this.SetValue(ColorProperty, value);

            }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker), new UIPropertyMetadata(Colors.Black, OnColorChanged));

        /// <summary>
        /// Hue Property
        /// </summary>

        public double Hue
        {
            get { return (double) this.GetValue(HueProperty); }
            set { this.SetValue(HueProperty, value); }
        }

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(double), typeof(ColorPicker),
            new FrameworkPropertyMetadata(0.0,
            new PropertyChangedCallback(UpdateColorHSB),
            new CoerceValueCallback(HueCoerce)));

        public static object HueCoerce(DependencyObject d, object Hue)
        {
            double v = (double)Hue;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }

        /// <summary>
        /// Brightness Property
        /// </summary>

        public double Brightness
        {
            get { return (double) this.GetValue(BrightnessProperty); }
            set { this.SetValue(BrightnessProperty, value); }
        }

        public static readonly DependencyProperty BrightnessProperty =
            DependencyProperty.Register("Brightness", typeof(double), typeof(ColorPicker),
            new FrameworkPropertyMetadata(0.0,
            new PropertyChangedCallback(UpdateColorHSB),
            new CoerceValueCallback(BrightnessCoerce)));

        public static object BrightnessCoerce(DependencyObject d, object Brightness)
        {
            double v = (double)Brightness;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }

        /// <summary>
        /// Saturation Property
        /// </summary>

        public double Saturation
        {
            get { return (double) this.GetValue(SaturationProperty); }
            set { this.SetValue(SaturationProperty, value); }
        }

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(double), typeof(ColorPicker),
            new FrameworkPropertyMetadata(0.0,
            new PropertyChangedCallback(UpdateColorHSB),
            new CoerceValueCallback(SaturationCoerce)));

        public static object SaturationCoerce(DependencyObject d, object Saturation)
        {
            double v = (double)Saturation;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }

        /// <summary>
        /// Alpha Property
        /// </summary>

        public double Alpha
        {
            get { return (double) this.GetValue(AlphaProperty); }
            set { this.SetValue(AlphaProperty, value); }
        }

        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register("Alpha", typeof(double), typeof(ColorPicker),
            new FrameworkPropertyMetadata(1.0,
            new PropertyChangedCallback(UpdateColorHSB),
            new CoerceValueCallback(AlphaCoerce)));

        public static object AlphaCoerce(DependencyObject d, object Alpha)
        {
            double v = (double)Alpha;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }


        /// <summary>
        /// Shared property changed callback to update the Color property
        /// </summary>

        public static void UpdateColorHSB(object o, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)o;
            Color n = ColorPickerUtil.ColorFromAHSB(c.Alpha, c.Hue, c.Saturation, c.Brightness);

            c.HSBSetInternally = true;

            c.Color = n;
            c.ColorBrush = new SolidColorBrush(n);

            c.HSBSetInternally = false;
        }

        /// <summary>
        /// R Property
        /// </summary>

        public int R
        {
            get { return (int) this.GetValue(RProperty); }
            set { this.SetValue(RProperty, value); }
        }

        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register("R", typeof(int), typeof(ColorPicker),
            new FrameworkPropertyMetadata(default(int),
            new PropertyChangedCallback(UpdateColorRGB),
            new CoerceValueCallback(RGBCoerce)));


        /// <summary>
        /// G Property
        /// </summary>

        public int G
        {
            get { return (int) this.GetValue(GProperty); }
            set { this.SetValue(GProperty, value); }
        }

        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register("G", typeof(int), typeof(ColorPicker),
            new FrameworkPropertyMetadata(default(int),
            new PropertyChangedCallback(UpdateColorRGB),
            new CoerceValueCallback(RGBCoerce)));

        /// <summary>
        /// B Property
        /// </summary>

        public int B
        {
            get { return (int) this.GetValue(BProperty); }
            set { this.SetValue(BProperty, value); }
        }

        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register("B", typeof(int), typeof(ColorPicker),
            new FrameworkPropertyMetadata(default(int),
            new PropertyChangedCallback(UpdateColorRGB),
            new CoerceValueCallback(RGBCoerce)));


        /// <summary>
        /// A Property
        /// </summary>

        public int A
        {
            get { return (int) this.GetValue(AProperty); }
            set { this.SetValue(AProperty, value); }
        }

        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register("A", typeof(int), typeof(ColorPicker),
            new FrameworkPropertyMetadata(255,
            new PropertyChangedCallback(UpdateColorRGB),
            new CoerceValueCallback(RGBCoerce)));



        public static object RGBCoerce(DependencyObject d, object value)
        {
            int v = (int)value;
            if (v < 0) return 0;
            if (v > 255) return 255;
            return v;
        }

        /// <summary>
        /// Shared property changed callback to update the Color property
        /// </summary>

        public static void UpdateColorRGB(object o, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)o;
            Color n = Color.FromArgb((byte)c.A, (byte)c.R, (byte)c.G, (byte)c.B);

            c.RGBSetInternally = true;
            
            c.Color = n;
            c.ColorBrush = new SolidColorBrush(n);

            c.RGBSetInternally = false;
        }

        #region ColorChanged Event

        public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);

        public static readonly RoutedEvent ColorChangedEvent =
            EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble, typeof(ColorChangedEventHandler), typeof(ColorPicker));

        public event ColorChangedEventHandler ColorChanged
        {
            add { this.AddHandler(ColorChangedEvent, value); }
            remove { this.RemoveHandler(ColorChangedEvent, value); }
        }

        void RaiseColorChangedEvent(Color color)
        {
            ColorChangedEventArgs newEventArgs = new ColorChangedEventArgs(ColorChangedEvent, color);
            this.RaiseEvent(newEventArgs);
        }

        #endregion

    }
}
