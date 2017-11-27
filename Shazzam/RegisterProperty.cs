namespace Shazzam
{
    using System;
    using System.ComponentModel;

    public class RegisterProperty<T> : INotifyPropertyChanged, IRegisterProperty where T : struct
    {
        private readonly ShaderModelPairRegister<T> pair;
        private readonly Func<T, double> getValue;
        private readonly Func<T, double, T> create;

        public RegisterProperty(string name, ShaderModelPairRegister<T> pair, Func<T, double> getValue, Func<T, double, T> create)
        {
            this.pair = pair;
            this.getValue = getValue;
            this.create = create;
            this.Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; }

        public double Min
        {
            get => this.getValue(this.pair.Min);
            set
            {
                if (value == this.Min)
                {
                    return;
                }

                this.pair.Min = this.create(this.pair.Min, value);
                this.OnPropertyChanged();
            }
        }

        public double Max
        {
            get => this.getValue(this.pair.Max);
            set
            {
                if (value == this.Max)
                {
                    return;
                }

                this.pair.Max = this.create(this.pair.Max, value);
                this.OnPropertyChanged();
            }
        }

        public double Value
        {
            get => this.getValue(this.pair.Value);
            set
            {
                if (value == this.Value)
                {
                    return;
                }

                this.pair.Value = this.create(this.pair.Value, value);
                this.OnPropertyChanged();
            }
        }

        public void Reset()
        {
            this.OnPropertyChanged(nameof(this.Min));
            this.OnPropertyChanged(nameof(this.Max));
            this.OnPropertyChanged(nameof(this.Value));
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}