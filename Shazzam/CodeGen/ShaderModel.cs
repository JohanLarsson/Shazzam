namespace Shazzam
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    public class ShaderModel
    {
        public ShaderModel(
            string shaderFileName,
            string generatedClassName,
            string generatedNamespace,
            string description,
            TargetFramework targetFramework,
            List<ShaderModelConstantRegister> registers)
        {
            this.ShaderFileName = shaderFileName;
            this.GeneratedClassName = generatedClassName;
            this.GeneratedNamespace = generatedNamespace;
            this.Description = description;
            this.TargetFramework = targetFramework;
            this.Registers = new ReadOnlyObservableCollection<ShaderModelConstantRegister>(new ObservableCollection<ShaderModelConstantRegister>(registers));
        }

        public string ShaderFileName { get; }

        public string GeneratedClassName { get; }

        public string GeneratedNamespace { get; }

        public string Description { get; }

        public TargetFramework TargetFramework { get; }

        public ReadOnlyObservableCollection<ShaderModelConstantRegister> Registers { get; }

        public string CSharpClass(bool defaultConstructor)
        {
            return new CodeWriter()
                   .Line($"namespace {this.GeneratedNamespace}")
                   .OpenCurly()
                   .Line("using System;")
                   .Line("using System.Windows;")
                   .Line("using System.Windows.Media;")
                   .Line("using System.Windows.Media.Effects;")
                   .Line("using System.Windows.Media.Media3D;")
                   .Line()
                   .Summary(this.Description)
                   .Line($"public class {this.GeneratedClassName} : ShaderEffect")
                   .OpenCurly()
                   .BackingFields(this)
                   .Constructor(this, defaultConstructor)
                   .Properties(this)
                   .EndCurly()
                   .EndCurly()
                   .ToString();
        }

        private class CodeWriter
        {
            private readonly StringBuilder builder = new();
            private string indentation = string.Empty;

            public override string ToString() => this.builder.ToString();

            internal CodeWriter OpenCurly()
            {
                return this
                       .Line("{")
                       .PushIndent();
            }

            internal CodeWriter EndCurly()
            {
                return this
                   .PopIndent()
                   .Line("}");
            }

            internal CodeWriter Line(string? text = null)
            {
                if (string.IsNullOrEmpty(text))
                {
                    this.builder.AppendLine();
                }
                else
                {
                    this.builder.Append(this.indentation).AppendLine(text);
                }

                return this;
            }

            internal CodeWriter Summary(string? text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return this;
                }

                if (text.Length < 60)
                {
                    this.builder.Append(this.indentation).AppendLine($"/// <summary>{text}</summary>");
                }
                else
                {
                    this.Line("/// <summary>")
                        .Line($"/// {text}")
                        .Line("/// </summary>");
                }

                return this;
            }

            internal CodeWriter BackingFields(ShaderModel model)
            {
                this.Summary("Identifies the Input dependency property.")
                    .Line("public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(")
                    .Line("    nameof(Input),")
                    .Line($"    typeof({model.GeneratedClassName}),")
                    .Line($"    0);")
                    .Line();

                foreach (var register in model.Registers)
                {
                    this.Summary($"Identifies the {register.RegisterName} dependency property.")
                        .Line($"public static readonly DependencyProperty {register.RegisterName}Property = DependencyProperty.Register(")
                        .Line($"    nameof({register.RegisterName}),")
                        .Line($"    typeof({TypeName(register.RegisterType)}),")
                        .Line($"    typeof({model.GeneratedClassName}),")
                        .Line("    new UIPropertyMetadata(")
                        .Line($"        {DefaultValue(register.DefaultValue, register.RegisterType)},")
                        .Line($"        PixelShaderConstantCallback({register.RegisterNumber})));")
                        .Line();
                }

                return this;
            }

            internal CodeWriter Constructor(ShaderModel model, bool defaultCtor)
            {
                if (defaultCtor)
                {
                    this.Line("/// <summary>")
                        .Line($"/// The uri should be something like pack://application:,,,/Gu.Wpf.Geometry;component/Effects/{model.GeneratedClassName}.ps")
                        .Line($"/// The file {model.GeneratedClassName}.ps should have BuildAction: Resource")
                        .Line("/// </summary>")
                        .Line("private static readonly PixelShader Shader = new PixelShader")
                        .Line("{")
                        .Line($"    UriSource = new Uri(\"pack://application:,,,/[assemblyname];component/[folder]/{model.GeneratedClassName}.ps\", UriKind.Absolute),")
                        .Line("};")
                        .Line()
                        .Line($"public {model.GeneratedClassName}()")
                        .OpenCurly()
                        .Line("this.PixelShader = Shader;");
                }
                else
                {
                    this.Line($"public {model.GeneratedClassName}(PixelShader shader)")
                        .OpenCurly()
                        .Line("this.PixelShader = shader;");
                }

                this.Line("this.UpdateShaderValue(InputProperty);");

                foreach (var register in model.Registers)
                {
                    this.Line($"this.UpdateShaderValue({register.RegisterName}Property);");
                }

                return this.EndCurly()
                           .Line();
            }

            internal CodeWriter Properties(ShaderModel model)
            {
                this.Summary("There has to be a property of type Brush called Input. This property contains the input image and it is usually not set directly - it is set automatically when our effect is applied to a control.")
                    .Line("public Brush Input")
                    .Line("{")
                    .Line($"    get => (Brush)this.GetValue(InputProperty);")
                    .Line($"    set => this.SetValue(InputProperty, value);")
                    .Line("}");

                foreach (var register in model.Registers)
                {
                    this.Line()
                        .Summary(register.Description)
                        .Line($"public {TypeName(register.RegisterType)} {register.RegisterName}")
                        .Line("{")
                        .Line($"    get => ({TypeName(register.RegisterType)})this.GetValue({register.RegisterName}Property);")
                        .Line($"    set => this.SetValue({register.RegisterName}Property, value);")
                        .Line("}");
                }

                return this;
            }

            private static string TypeName(Type type) => type switch
            {
                { Name: "Double" } => "double",
                { Name: "Int" } => "int",
                _ => type.Name,
            };

            private static string DefaultValue(object? value, Type type)
            {
                if (value is null)
                {
                    return $"default({TypeName(type)})";
                }

                return type switch
                {
                    { Name: "Double" } => $"{value}D",
                    _ => value.ToString(),
                };
            }

            private CodeWriter PushIndent()
            {
                this.indentation += "    ";
                return this;
            }

            private CodeWriter PopIndent()
            {
                this.indentation = this.indentation.Substring(0, this.indentation.Length - 4);
                return this;
            }
        }
    }
}
