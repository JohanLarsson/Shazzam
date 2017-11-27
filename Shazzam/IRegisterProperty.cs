namespace Shazzam
{
    using System.ComponentModel;

    public interface IRegisterProperty : INotifyPropertyChanged
    {
        string Name { get; }

        double Min { get; set; }

        double Max { get; set; }

        double Value { get; set; }
    }
}