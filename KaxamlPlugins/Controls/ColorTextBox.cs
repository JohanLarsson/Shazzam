namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ColorTextBox : TextBox
    {

        #region Properties

        /// <summary>
        /// ColorBrush Property
        /// </summary>

        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush) this.GetValue(ColorBrushProperty); }
            set { this.SetValue(ColorBrushProperty, value); }
        }
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(ColorTextBox), new UIPropertyMetadata(Brushes.Black));

        /// <summary>
        /// Color Property
        /// </summary>
        bool ColorSetInternally = false;

        public Color Color
        {
            get { return (Color) this.GetValue(ColorProperty); }
            set
            {
                this.SetValue(ColorProperty, value);

                if (!this.ColorSetInternally)
                {
                    this.SetValue(TextProperty, value.ToString());
                }
            }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorTextBox), new UIPropertyMetadata(Colors.Black));

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Updates the Color property any time the text changes
        /// </summary>

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            this.ColorSetInternally = true;
            this.Color = ColorPickerUtil.ColorFromString(this.Text);
            this.ColorBrush = new SolidColorBrush(this.Color);
            this.ColorSetInternally = false;
        }

        /// <summary>
        /// Restricts input to chacters that are valid for defining a color
        /// </summary>

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0)
            {
                char c = e.Text[0];

                bool IsValid = false;

                if (c >= 'a' && c <= 'f') IsValid = true;
                if (c >= 'A' && c <= 'F') IsValid = true;
                if (c >= '0' && c <= '9' && Keyboard.Modifiers != ModifierKeys.Shift) IsValid = true;

                if (!IsValid)
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

        #endregion

    }
}