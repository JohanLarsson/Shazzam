namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Media;

    public class ColorChangedEventArgs : RoutedEventArgs
    {
        public ColorChangedEventArgs(RoutedEvent routedEvent, Color color)
        {
            this.RoutedEvent = routedEvent;
            this.Color = color;
        }

        private Color _Color;
        public Color Color
        {
            get { return this._Color; }
            set { this._Color = value; }
        }
    }
}