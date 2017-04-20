namespace Kaxaml
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class Plugin
    {
        public UserControl Root { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Key Key { get; set; }

        public ModifierKeys ModifierKeys { get; set; }

        public ImageSource Icon { get; set; }
    }
}
