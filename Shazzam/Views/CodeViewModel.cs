namespace Shazzam.Views
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public sealed class CodeViewModel : INotifyPropertyChanged
    {
        public static readonly CodeViewModel Instance = new();
        private ShaderModel? shaderModel;

        private CodeViewModel()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool AreGeneratedCodeTabsEnabled => Properties.Settings.Default.AreGeneratedCodeTabsEnabled;

        public ShaderModel? ShaderModel
        {
            get => this.shaderModel;
            set
            {
                if (ReferenceEquals(value, this.shaderModel))
                {
                    return;
                }

                this.shaderModel = value;
                this.OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
