namespace Shazzam.Views
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class CodeTabViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool AreGeneratedCodeTabsEnabled => Properties.Settings.Default.AreGeneratedCodeTabsEnabled;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
