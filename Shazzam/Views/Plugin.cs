namespace Shazzam
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class Plugin : INotifyPropertyChanged
    {
        private UserControl root;
        private string name;
        private string description;
        private Key key;
        private ModifierKeys modifierKeys;
        private ImageSource icon;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserControl Root
        {
            get => this.root;

            set
            {
                if (ReferenceEquals(value, this.root))
                {
                    return;
                }

                this.root = value;
                this.OnPropertyChanged();
            }
        }

        public string Name
        {
            get => this.name;

            set
            {
                if (value == this.name)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public string Description
        {
            get => this.description;

            set
            {
                if (value == this.description)
                {
                    return;
                }

                this.description = value;
                this.OnPropertyChanged();
            }
        }

        public Key Key
        {
            get => this.key;

            set
            {
                if (value == this.key)
                {
                    return;
                }

                this.key = value;
                this.OnPropertyChanged();
            }
        }

        public ModifierKeys ModifierKeys
        {
            get => this.modifierKeys;

            set
            {
                if (value == this.modifierKeys)
                {
                    return;
                }

                this.modifierKeys = value;
                this.OnPropertyChanged();
            }
        }

        public ImageSource Icon
        {
            get => this.icon;

            set
            {
                if (ReferenceEquals(value, this.icon))
                {
                    return;
                }

                this.icon = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
