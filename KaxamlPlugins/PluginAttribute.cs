#pragma warning disable WPF1011 // Implement INotifyPropertyChanged.
namespace KaxamlPlugins
{
    using System;
    using System.Windows.Input;

    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Key Key { get; set; }

        public ModifierKeys ModifierKeys { get; set; }

        public string Icon { get; set; }
    }
}
