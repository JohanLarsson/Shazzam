namespace Shazzam.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class RangeControl : UserControl
    {
        public static readonly DependencyProperty MinProperty = DependencyProperty.Register(
            nameof(Min),
            typeof(double),
            typeof(RangeControl),
            new FrameworkPropertyMetadata(
                default(double),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(
            nameof(Max),
            typeof(double),
            typeof(RangeControl),
            new FrameworkPropertyMetadata(
                default(double),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(RangeControl),
            new FrameworkPropertyMetadata(
                default(double),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public RangeControl()
        {
            this.InitializeComponent();
        }

        public double Min
        {
            get => (double)this.GetValue(MinProperty);
            set => this.SetValue(MinProperty, value);
        }

        public double Max
        {
            get => (double)this.GetValue(MaxProperty);
            set => this.SetValue(MaxProperty, value);
        }

        public double Value
        {
            get => (double)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }
    }
}
