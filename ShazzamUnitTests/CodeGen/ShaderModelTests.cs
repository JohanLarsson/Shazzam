namespace ShazzamUnitTests.CodeGen
{
    using System.Collections.Generic;

    using Microsoft.CSharp;

    using NUnit.Framework;

    using Shazzam;
    using Shazzam.CodeGen;

    public class ShaderModelTests
    {
        [Test]
        public void CSharpClassWithCtor()
        {
            var shaderModel = new ShaderModel(
                shaderFileName: "MyEffect.cs",
                generatedClassName: "MyEffect",
                generatedNamespace: "Shaders",
                description: "This is MyEffect",
                targetFramework: TargetFramework.WPF,
                registers: new List<ShaderModelConstantRegister>
                {
                    new ShaderModelConstantRegister(
                        registerName: "Value",
                        registerType: typeof(double),
                        registerNumber: 1,
                        description: "This is the value",
                        minValue: null,
                        maxValue: null,
                        defaultValue: 0),
                });

            var actual = shaderModel.CSharpClass(defaultConstructor: false);
            var expected = @"namespace Shaders
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using System.Windows.Media.Media3D;

    /// <summary>This is MyEffect</summary>
    public class MyEffect : ShaderEffect
    {
        /// <summary>Identifies the Input dependency property.</summary>
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(
            nameof(Input),
            typeof(MyEffect),
            0);

        /// <summary>Identifies the Value dependency property.</summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(MyEffect),
            new UIPropertyMetadata(
                0D,
                PixelShaderConstantCallback(1)));

        public MyEffect(PixelShader shader)
        {
            this.PixelShader = shader;
            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(ValueProperty);
        }

        /// <summary>
        /// There has to be a property of type Brush called Input. This property contains the input image and it is usually not set directly - it is set automatically when our effect is applied to a control.
        /// </summary>
        public Brush Input
        {
            get => (Brush)this.GetValue(InputProperty);
            set => this.SetValue(InputProperty, value);
        }

        /// <summary>This is the value</summary>
        public double Value
        {
            get => (double)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }
    }
}
";
            Assert.AreEqual(expected.Replace("\r", string.Empty), actual.Replace("\r", string.Empty));
        }

        [Test]
        public void CSharpClassDefaultCtor()
        {
            var shaderModel = new ShaderModel(
                shaderFileName: "MyEffect.cs",
                generatedClassName: "MyEffect",
                generatedNamespace: "Shaders",
                description: "This is MyEffect",
                targetFramework: TargetFramework.WPF,
                registers: new List<ShaderModelConstantRegister>
                {
                    new ShaderModelConstantRegister(
                        registerName: "Value",
                        registerType: typeof(double),
                        registerNumber: 1,
                        description: "This is the value",
                        minValue: null,
                        maxValue: null,
                        defaultValue: 0),
                });

            var actual = shaderModel.CSharpClass(defaultConstructor: true);
            var expected = @"namespace Shaders
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using System.Windows.Media.Media3D;

    /// <summary>This is MyEffect</summary>
    public class MyEffect : ShaderEffect
    {
        /// <summary>Identifies the Input dependency property.</summary>
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(
            nameof(Input),
            typeof(MyEffect),
            0);

        /// <summary>Identifies the Value dependency property.</summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(MyEffect),
            new UIPropertyMetadata(
                0D,
                PixelShaderConstantCallback(1)));

        /// <summary>
        /// The uri should be something like pack://application:,,,/Gu.Wpf.Geometry;component/Effects/MyEffect.ps
        /// The file MyEffect.ps should have BuildAction: Resource
        /// </summary>
        private static readonly PixelShader Shader = new PixelShader
        {
            UriSource = new Uri(""pack://application:,,,/[assemblyname];component/[folder]/MyEffect.ps"", UriKind.Absolute),
        };

        public MyEffect()
        {
            this.PixelShader = Shader;
            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(ValueProperty);
        }

        /// <summary>
        /// There has to be a property of type Brush called Input. This property contains the input image and it is usually not set directly - it is set automatically when our effect is applied to a control.
        /// </summary>
        public Brush Input
        {
            get => (Brush)this.GetValue(InputProperty);
            set => this.SetValue(InputProperty, value);
        }

        /// <summary>This is the value</summary>
        public double Value
        {
            get => (double)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }
    }
}
";
            Assert.AreEqual(expected.Replace("\r", string.Empty), actual.Replace("\r", string.Empty));
        }

        [Test]
        public void CompileInMemory()
        {
            var shaderModel = new ShaderModel(
                shaderFileName: "Foo.cs",
                generatedClassName: "Foo",
                generatedNamespace: "Shaders",
                description: "This is Foo",
                targetFramework: TargetFramework.WPF,
                registers: new List<ShaderModelConstantRegister>
                {
                    new ShaderModelConstantRegister(
                        registerName: "Bar",
                        registerType: typeof(double),
                        registerNumber: 1,
                        description: "This is Bar",
                        minValue: null,
                        maxValue: null,
                        defaultValue: 0),
                });

            var code = ShaderClass.GetSourceText(new CSharpCodeProvider(), shaderModel, includePixelShaderConstructor: false);
            Assert.NotNull(ShaderClass.CompileInMemory(code));
        }
    }
}
