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

        public Color Color { get; }
    }
}