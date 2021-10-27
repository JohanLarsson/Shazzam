namespace Shazzam
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///  Contains the details for each register described in a HLSL shader file
    /// </summary>
    public class ShaderModelConstantRegister : INotifyPropertyChanged
    {
        private object value;

        public ShaderModelConstantRegister(
            string registerName,
            Type registerType,
            int registerNumber,
            string description,
            object minValue,
            object maxValue,
            object defaultValue)
        {
            this.RegisterName = registerName;
            this.RegisterType = registerType;
            this.RegisterNumber = registerNumber;
            this.Description = description;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.DefaultValue = defaultValue;
            this.value = defaultValue;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// The name of this register variable.
        /// </summary>
        public string RegisterName { get; }

        /// <summary>
        ///  The .NET type of this register variable.
        /// </summary>
        public Type RegisterType { get; }

        /// <summary>
        /// The register number of this register variable.
        /// </summary>
        public int RegisterNumber { get; }

        /// <summary>
        /// The description of this register variable.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The minimum value for this register variable.
        /// </summary>
        public object MinValue { get; }

        /// <summary>
        /// The maximum value for this register variable.
        /// </summary>
        public object MaxValue { get; }

        /// <summary>
        /// The default value of this register variable.
        /// </summary>
        public object DefaultValue { get; }

        public object Value
        {
            get => this.value;
            set
            {
                if (ReferenceEquals(value, this.value))
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
