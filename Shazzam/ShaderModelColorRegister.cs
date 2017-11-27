namespace Shazzam
{
    using System;
    using System.Windows.Input;
    using System.Windows.Media;
    using Shazzam.Commands;

    public class ShaderModelColorRegister : ShaderModelConstantRegister
    {
        private Color value;

        public ShaderModelColorRegister(string registerName, Type registerType, int registerNumber, string description, Color defaultValue)
            : base(registerName, registerType, registerNumber, description, defaultValue)
        {
            this.DefaultValue = defaultValue;
            this.value = defaultValue;
            this.ResetCommand = new RelayCommand(() => this.Value = this.DefaultValue);
        }

        /// <summary>
        /// The default value of this register variable.
        /// </summary>
        public new Color DefaultValue { get; }

        public ICommand ResetCommand { get; }

        public Color Value
        {
            get => this.value;
            set
            {
                if (value == this.value)
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }
    }
}