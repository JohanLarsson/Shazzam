namespace Shazzam
{
    using System;
    using System.Windows.Media;

    public class ShaderModelColorRegister : ShaderModelConstantRegister
    {
        private Color value;

        public ShaderModelColorRegister(string registerName, Type registerType, int registerNumber, string description, Color defaultValue)
            : base(registerName, registerType, registerNumber, description, defaultValue)
        {
            this.DefaultValue = defaultValue;
            this.value = defaultValue;
        }

        /// <summary>
        /// The default value of this register variable.
        /// </summary>
        public new Color DefaultValue { get; }

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