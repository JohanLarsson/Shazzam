namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ColorTextBox : TextBox
    {
        public static readonly DependencyProperty ColorBrushProperty = DependencyProperty.Register(
            nameof(ColorBrush),
            typeof(SolidColorBrush),
            typeof(ColorTextBox),
            new UIPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color),
            typeof(Color),
            typeof(ColorTextBox),
            new UIPropertyMetadata(Colors.Black));

        private bool colorSetInternally;

        public SolidColorBrush ColorBrush
        {
            get => (SolidColorBrush)this.GetValue(ColorBrushProperty);
            set => this.SetValue(ColorBrushProperty, value);
        }

        public Color Color
        {
            get => (Color)this.GetValue(ColorProperty);
            set
            {
                this.SetCurrentValue(ColorProperty, value);

                if (!this.colorSetInternally)
                {
                    this.SetCurrentValue(TextProperty, value.ToString());
                }
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            this.colorSetInternally = true;
            this.SetCurrentValue(ColorProperty, ColorPickerUtil.ColorFromString(this.Text));
            this.SetCurrentValue(ColorBrushProperty, new SolidColorBrush(this.Color));
            this.colorSetInternally = false;
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0)
            {
                var c = e.Text[0];

                var isValid = false;

                if (c >= 'a' && c <= 'f')
                {
                    isValid = true;
                }

                if (c >= 'A' && c <= 'F')
                {
                    isValid = true;
                }

                if (c >= '0' && c <= '9' && Keyboard.Modifiers != ModifierKeys.Shift)
                {
                    isValid = true;
                }

                if (!isValid)
                {
                    e.Handled = true;
                }

                if (this.Text.Length >= 8)
                {
                    e.Handled = true;
                }
            }

            base.OnPreviewTextInput(e);
        }
    }
}
