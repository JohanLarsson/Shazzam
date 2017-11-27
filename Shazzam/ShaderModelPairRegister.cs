namespace Shazzam
{
    using System;

    public abstract class ShaderModelPairRegister<T> : ShaderModelConstantRegister<T>
        where T : struct
    {
        protected ShaderModelPairRegister(string registerName, Type registerType, int registerNumber, string description, T min, T max, T defaultValue, Func<ShaderModelPairRegister<T>, RegisterProperty<T>> property1, Func<ShaderModelPairRegister<T>, RegisterProperty<T>> property2)
            : base(registerName, registerType, registerNumber, description, min, max, defaultValue)
        {
            this.Property1 = property1(this);
            this.Property2 = property2(this);
        }

        public RegisterProperty<T> Property1 { get; }

        public RegisterProperty<T> Property2 { get; }

        protected override void Reset()
        {
            base.Reset();
            this.Property1.Reset();
            this.Property2.Reset();
        }
    }
}