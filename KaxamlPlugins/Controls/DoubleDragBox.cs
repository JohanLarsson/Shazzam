namespace KaxamlPlugins.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class DoubleDragBox : Control
    {
        public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register(
            nameof(Precision),
            typeof(int),
            typeof(DoubleDragBox),
            new UIPropertyMetadata(2));

        public static readonly DependencyProperty CurrentTextProperty = DependencyProperty.Register(
            nameof(CurrentText),
            typeof(string),
            typeof(DoubleDragBox),
            new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register(
            nameof(Current),
            typeof(double),
            typeof(DoubleDragBox),
            new FrameworkPropertyMetadata(
                double.MinValue,
                OnCurrentChanged,
                (d, baseValue) => Coerce.ClampDouble(baseValue, ((DoubleDragBox)d).Minimum, ((DoubleDragBox)d).Maximum)));

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            nameof(Interval),
            typeof(double),
            typeof(DoubleDragBox),
            new UIPropertyMetadata(0.05));

        public static readonly DependencyProperty SensitivityProperty = DependencyProperty.Register(
            nameof(Sensitivity),
            typeof(double),
            typeof(DoubleDragBox),
            new UIPropertyMetadata(20.0));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            nameof(Minimum),
            typeof(double),
            typeof(DoubleDragBox),
            new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum),
            typeof(double),
            typeof(DoubleDragBox),
            new UIPropertyMetadata(1.0));

        private Point beginPoint;
        private bool isPointValid;

        static DoubleDragBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleDragBox), new FrameworkPropertyMetadata(typeof(DoubleDragBox)));
        }

        public int Precision
        {
            get => (int)this.GetValue(PrecisionProperty);
            set => this.SetValue(PrecisionProperty, value);
        }

        public string CurrentText
        {
            get => (string)this.GetValue(CurrentTextProperty);
            set => this.SetValue(CurrentTextProperty, value);
        }

        public double Current
        {
            get => (double)this.GetValue(CurrentProperty);
            set => this.SetValue(CurrentProperty, value);
        }

        public double Interval
        {
            get => (double)this.GetValue(IntervalProperty);
            set => this.SetValue(IntervalProperty, value);
        }

        public double Sensitivity
        {
            get => (double)this.GetValue(SensitivityProperty);
            set => this.SetValue(SensitivityProperty, value);
        }

        public double Minimum
        {
            get => (double)this.GetValue(MinimumProperty);
            set => this.SetValue(MinimumProperty, value);
        }

        public double Maximum
        {
            get => (double)this.GetValue(MaximumProperty);
            set => this.SetValue(MaximumProperty, value);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            this.CaptureMouse();
            this.beginPoint = e.GetPosition(this);
            this.isPointValid = true;
            base.OnPreviewMouseDown(e);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            this.isPointValid = false;
            this.ReleaseMouseCapture();
            base.OnPreviewMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.isPointValid)
                {
                    if (e.GetPosition(this).Y - this.beginPoint.Y > this.Sensitivity)
                    {
                        this.Current -= this.Interval;
                        this.beginPoint.Y = e.GetPosition(this).Y;
                    }
                    else if (e.GetPosition(this).Y - this.beginPoint.Y < (-1 * this.Sensitivity))
                    {
                        this.Current += this.Interval;
                        this.beginPoint.Y = e.GetPosition(this).Y;
                    }
                }
            }

            base.OnMouseMove(e);
        }

        private static void OnCurrentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ddb = (DoubleDragBox)o;
            var d = (double)e.NewValue;
            ddb.SetCurrentValue(CurrentTextProperty, Math.Round(d, ddb.Precision).ToString(CultureInfo.InvariantCulture));
        }
    }
}
