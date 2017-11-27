namespace Shazzam
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    ///  Contains the details for each register described in a HLSL shader file
    /// </summary>
    public abstract class ShaderModelConstantRegister : INotifyPropertyChanged
    {
        private double animationDuration = 2.0;

        protected ShaderModelConstantRegister(
            string registerName,
            Type registerType,
            int registerNumber,
            string description,
            object defaultValue)
        {
            this.RegisterName = registerName;
            this.RegisterType = registerType;
            this.RegisterNumber = registerNumber;
            this.Description = description;
            this.DefaultValue = defaultValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
        /// The default value of this register variable.
        /// </summary>
        public object DefaultValue { get; }

        public double AnimationDuration
        {
            get => this.animationDuration;
            set
            {
                if (value == this.animationDuration)
                {
                    return;
                }

                this.animationDuration = value;
                this.OnPropertyChanged();
            }
        }

        public static ShaderModelConstantRegister Create(string registerName, Type registerType, int registerNumber, string summary, object minValue, object maxValue, object defaultValue)
        {
            switch (defaultValue)
            {
                case double _:
                    return new ShaderModelDoubleRegister(registerName, registerType, registerNumber, summary, (double)minValue, (double)maxValue, (double)defaultValue);
                case float _:
                    return new ShaderModelFloatRegister(registerName, registerType, registerNumber, summary, (float)minValue, (float)maxValue, (float)defaultValue);
                case Color color:
                    return new ShaderModelColorRegister(registerName, registerType, registerNumber, summary, color);
                case Point _:
                    return new ShaderModelPointRegister(registerName, registerType, registerNumber, summary, (Point)minValue, (Point)maxValue, (Point)defaultValue);
                case Vector _:
                    return new ShaderModelVectorRegister(registerName, registerType, registerNumber, summary, (Vector)minValue, (Vector)maxValue, (Vector)defaultValue);
                case Size _:
                    return new ShaderModelSizeRegister(registerName, registerType, registerNumber, summary, (Size)minValue, (Size)maxValue, (Size)defaultValue);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(registerType),
                        registerType,
                        "Could not create a register viewmodel for the type");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
