﻿namespace ShazzamUnitTests.CodeGen
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CSharp;

    using NUnit.Framework;

    using Shazzam;
    using Shazzam.CodeGen;

    public class CreatePixelShaderClassTests
    {
        [Test]
        public void GetSourceTextPixelShaderCtor()
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
                            defaultValue: 0)
                    });

            var actual = CreatePixelShaderClass.GetSourceText(new CSharpCodeProvider(), shaderModel, includePixelShaderConstructor: true);
            Console.Write(actual);
            var expected = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace Shaders
{
    
    
    /// <summary>This is Foo</summary>
    public class Foo : ShaderEffect
    {
        
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(""Input"", typeof(Foo), 0);
        
        public static readonly DependencyProperty BarProperty = DependencyProperty.Register(""Bar"", typeof(double), typeof(Foo), new UIPropertyMetadata(0D, PixelShaderConstantCallback(1)));
        
        public Foo(PixelShader shader)
        {
            this.PixelShader = shader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(BarProperty);
        }
        
        /// <summary>There has to be a property of type Brush called ""Input"". This property contains the input image and it is usually not set directly - it is set automatically when our effect is applied to a control.</summary>
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        
        /// <summary>This is Bar</summary>
        public double Bar
        {
            get
            {
                return ((double)(this.GetValue(BarProperty)));
            }
            set
            {
                this.SetValue(BarProperty, value);
            }
        }
    }
}

";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSourceTextDefaultCtor()
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
                                       defaultValue: 0)
                               });

            var actual = CreatePixelShaderClass.GetSourceText(new CSharpCodeProvider(), shaderModel, includePixelShaderConstructor: false);
            Console.Write(actual);
            var expected = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace Shaders
{
    
    
    /// <summary>This is Foo</summary>
    public class Foo : ShaderEffect
    {
        
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(""Input"", typeof(Foo), 0);
        
        public static readonly DependencyProperty BarProperty = DependencyProperty.Register(""Bar"", typeof(double), typeof(Foo), new UIPropertyMetadata(0D, PixelShaderConstantCallback(1)));
        
        private static readonly PixelShader Shader = new PixelShader { UriSource = new Uri(""pack://application:,,,/[assemblyname];component/Effects/Foo.ps"", UriKind.Absolute) };
        
        public Foo()
        {
            this.PixelShader = Shader;
            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(BarProperty);
        }
        
        /// <summary>There has to be a property of type Brush called ""Input"". This property contains the input image and it is usually not set directly - it is set automatically when our effect is applied to a control.</summary>
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        
        /// <summary>This is Bar</summary>
        public double Bar
        {
            get
            {
                return ((double)(this.GetValue(BarProperty)));
            }
            set
            {
                this.SetValue(BarProperty, value);
            }
        }
    }
}

";
            Assert.AreEqual(expected, actual);
        }
    }
}
