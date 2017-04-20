using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kaxaml.Plugins.Controls
{

    public class DoubleDragBox : Control
    {

        static DoubleDragBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleDragBox), new FrameworkPropertyMetadata(typeof(DoubleDragBox)));
        }


        #region Dependency Properties


        /// <summary>
        /// Indicates the number of digits that are displayed (beyond 0).
        /// </summary>
        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register("Precision", typeof(int), typeof(DoubleDragBox), new UIPropertyMetadata(2));

        public int Precision
        {
            get { return (int) this.GetValue(PrecisionProperty); }
            set { this.SetValue(PrecisionProperty, value); }
        }


        /// <summary>
        /// A string corresponding to the value of the Current property.
        /// </summary>
        public static readonly DependencyProperty CurrentTextProperty =
            DependencyProperty.Register("CurrentText", typeof(string), typeof(DoubleDragBox), new UIPropertyMetadata(""));

        public string CurrentText
        {
            get { return (string) this.GetValue(CurrentTextProperty); }
            set { this.SetValue(CurrentTextProperty, value); }
        }


        /// <summary>
        /// The current value.
        /// </summary>
        public static readonly DependencyProperty CurrentProperty =
            DependencyProperty.Register("Current", typeof(double), typeof(DoubleDragBox),
            new FrameworkPropertyMetadata(double.MinValue, new PropertyChangedCallback(CurrentChanged), new CoerceValueCallback(CurrentCoerce)));

        public double Current
        {
            get { return (double) this.GetValue(CurrentProperty); }
            set { this.SetValue(CurrentProperty, value); }
        }

        public static void CurrentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DoubleDragBox ddb = (DoubleDragBox)o;
            double d = (double)e.NewValue;

            ddb.CurrentText = Math.Round(d, ddb.Precision).ToString();
        }

        public static object CurrentCoerce(DependencyObject o, object value)
        {
            DoubleDragBox ddb = (DoubleDragBox)o;
            double v = (double)value;

            if (ddb != null)
            {
                if (v > ddb.Maximum) return ddb.Maximum;
                if (v < ddb.Minimum) return ddb.Minimum;
            }

            return v;
        }


        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(double), typeof(DoubleDragBox), new UIPropertyMetadata(0.05));

        public double Interval
        {
            get { return (double) this.GetValue(IntervalProperty); }
            set { this.SetValue(IntervalProperty, value); }
        }



        public static readonly DependencyProperty SensitivityProperty =
            DependencyProperty.Register("Sensitivity", typeof(double), typeof(DoubleDragBox), new UIPropertyMetadata(20.0));

        public double Sensitivity
        {
            get { return (double) this.GetValue(SensitivityProperty); }
            set { this.SetValue(SensitivityProperty, value); }
        }




        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(DoubleDragBox), new UIPropertyMetadata(0.0));

        public double Minimum
        {
            get { return (double) this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }


        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(DoubleDragBox), new UIPropertyMetadata(1.0));

        public double Maximum
        {
            get { return (double) this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }





        #endregion


        private Point _BeginPoint;
        private bool _IsPointValid;

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            this.CaptureMouse();
            this._BeginPoint = e.GetPosition(this);
            this._IsPointValid = true;
            base.OnPreviewMouseDown(e);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            this._IsPointValid = false;
            this.ReleaseMouseCapture();
            base.OnPreviewMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this._IsPointValid)
                {

                    if (e.GetPosition(this).Y - this._BeginPoint.Y > this.Sensitivity)
                    {
                        this.Current -= this.Interval;
                        this._BeginPoint.Y = e.GetPosition(this).Y;
                    }
                    else if (e.GetPosition(this).Y - this._BeginPoint.Y < (-1 * this.Sensitivity))
                    {
                        this.Current += this.Interval;
                        this._BeginPoint.Y = e.GetPosition(this).Y;
                    }
                }
            }

            base.OnMouseMove(e);
        }
    }
}
