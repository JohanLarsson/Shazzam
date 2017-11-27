namespace Shazzam
{
    using System;
    using System.Windows;

    public class ShaderModelSizeRegister : ShaderModelPairRegister<Size>
    {
        public ShaderModelSizeRegister(string registerName, Type registerType, int registerNumber, string description, Size min, Size max, Size defaultValue)
            : base(
                registerName,
                registerType,
                registerNumber,
                description,
                min,
                max,
                defaultValue,
                x => new RegisterProperty<Size>("W", x, p => p.Width, (p, d) => new Size(d, p.Height)),
                x => new RegisterProperty<Size>("H", x, p => p.Height, (p, d) => new Size(p.Width, d)))
        {
        }
    }
}