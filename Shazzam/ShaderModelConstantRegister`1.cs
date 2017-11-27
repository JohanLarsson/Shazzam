namespace Shazzam
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Shazzam.Commands;

    public abstract class ShaderModelConstantRegister<T> : ShaderModelConstantRegister
        where T : struct
    {
        private T min;
        private T max;
        private T value;

        protected ShaderModelConstantRegister(string registerName, Type registerType, int registerNumber, string description, T min, T max, T defaultValue)
            : base(registerName, registerType, registerNumber, description, defaultValue)
        {
            this.min = min;
            this.max = max;
            this.DefaultValue = defaultValue;
            this.value = defaultValue;
            this.ResetCommand = new RelayCommand(() =>
            {
                this.Min = min;
                this.Max = max;
                this.Value = this.DefaultValue;
            });
        }

        /// <summary>
        /// The default value of this register variable.
        /// </summary>
        public new T DefaultValue { get; }

        public ICommand ResetCommand { get; }

        /// <summary>
        /// The minimum value for this register variable.
        /// </summary>
        public T Min
        {
            get => this.min;
            set
            {
                if (EqualityComparer<T>.Default.Equals(value, this.min))
                {
                    return;
                }

                this.min = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The maximum value for this register variable.
        /// </summary>
        public T Max
        {
            get => this.max;
            set
            {
                if (EqualityComparer<T>.Default.Equals(value, this.max))
                {
                    return;
                }

                this.max = value;
                this.OnPropertyChanged();
            }
        }

        public T Value
        {
            get => this.value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(value, this.value))
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }
    }
}
