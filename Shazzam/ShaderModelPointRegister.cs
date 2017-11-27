namespace Shazzam
{
    using System;
    using System.Windows;

    public class ShaderModelPointRegister : ShaderModelPairRegister<Point>
    {
        public ShaderModelPointRegister(string registerName, Type registerType, int registerNumber, string description, Point min, Point max, Point defaultValue)
            : base(
                registerName,
                registerType,
                registerNumber,
                description,
                min,
                max,
                defaultValue,
                x => new RegisterProperty<Point>("X", x, p => p.X, (p, d) => new Point(d, p.Y)),
                x => new RegisterProperty<Point>("Y", x, p => p.Y, (p, d) => new Point(p.X, d)))
        {
        }
    }
}