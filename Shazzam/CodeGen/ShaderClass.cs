namespace Shazzam.CodeGen
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    using Microsoft.CSharp;
    using Shazzam.Properties;

    public static class ShaderClass
    {
        public static string GetSourceText(CodeDomProvider currentProvider, ShaderModel shaderModel, bool includePixelShaderConstructor)
        {
            return GenerateCode(
                currentProvider,
                BuildPixelShaderGraph(shaderModel, includePixelShaderConstructor),
                shaderModel);
        }

        public static Assembly CompileInMemory(string code)
        {
            using var provider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v3.5" } });
            var options = new CompilerParameters
            {
                ReferencedAssemblies =
                {
                    "System.dll",
                    "System.Core.dll",
                    "WindowsBase.dll",
                    "PresentationFramework.dll",
                    "PresentationCore.dll",
                },
                IncludeDebugInformation = false,
                GenerateExecutable = false,
                GenerateInMemory = true,
            };
            var compiled = provider.CompileAssemblyFromSource(options, code);
            if (compiled.Errors.Count == 0)
            {
                return compiled.CompiledAssembly;
            }

            throw new InvalidOperationException(string.Join(Environment.NewLine, compiled.Errors.OfType<CompilerError>().Select(e => e.ErrorText)));
        }

        private static CodeCompileUnit BuildPixelShaderGraph(ShaderModel shaderModel, bool includePixelShaderConstructor)
        {
            // Create a new CodeCompileUnit to contain
            // the program graph.
            var codeGraph = new CodeCompileUnit();

            // Create the namespace.
            var codeNamespace = AssignNamespacesToGraph(codeGraph, shaderModel.GeneratedNamespace);

            // Create the appropriate constructor.
            var constructor = includePixelShaderConstructor ? CreatePixelShaderConstructor(shaderModel) : CreateDefaultConstructor(shaderModel);

            // Declare a new type.
            var shader = new CodeTypeDeclaration
            {
                Name = shaderModel.GeneratedClassName,
                BaseTypes =
                {
                    new CodeTypeReference("ShaderEffect"),
                },
                Members =
                {
                    constructor,
                    CreateSamplerDependencyProperty(shaderModel.GeneratedClassName, "Input"),
                    CreateClrProperty("Input", typeof(Brush), null),
                },
            };
            if (!string.IsNullOrEmpty(shaderModel.Description))
            {
                shader.Comments.Add(new CodeCommentStatement($"<summary>{shaderModel.Description}</summary>", docComment: true));
            }

            // Add a dependency property and a CLR property for each of the shader's register variables.
            foreach (var register in shaderModel.Registers)
            {
                shader.Members.Add(CreateShaderRegisterDependencyProperty(shaderModel, register));
                shader.Members.Add(CreateClrProperty(register.Name, register.Type, register.Description));
            }

            if (!includePixelShaderConstructor)
            {
                shader.Members.Add(CreateShaderField(shaderModel));
            }

            // Add the new type to the namespace.
            codeNamespace.Types.Add(shader);

            return codeGraph;
        }

        private static CodeMemberField CreateShaderField(ShaderModel model)
        {
            var typeReference = new CodeTypeReference("PixelShader");
            return new CodeMemberField
            {
                Type = typeReference,
                Name = "Shader",
                //// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                Attributes = MemberAttributes.Static | MemberAttributes.Private,
                InitExpression = new CodeObjectCreateExpression { CreateType = typeReference },
                Comments =
                {
                    new CodeCommentStatement("<summary>", docComment: true),
                    new CodeCommentStatement($"The uri should be something like pack://application:,,,/Gu.Wpf.Geometry;component/Effects/{model.GeneratedClassName}.ps", docComment: true),
                    new CodeCommentStatement($"The file {model.GeneratedClassName}.ps should have BuildAction: Resource", docComment: true),
                    new CodeCommentStatement("</summary>", docComment: true),
                },
            };
        }

        private static CodeMemberField CreateSamplerDependencyProperty(string className, string propertyName)
        {
            return new CodeMemberField
            {
                Type = new CodeTypeReference("DependencyProperty"),
                Name = $"{propertyName}Property",
                //// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                Attributes = MemberAttributes.Static | MemberAttributes.Public,
                InitExpression = new CodeMethodInvokeExpression
                {
                    Method = new CodeMethodReferenceExpression
                    {
                        TargetObject = new CodeTypeReferenceExpression("ShaderEffect"),
                        MethodName = "RegisterPixelShaderSamplerProperty",
                    },
                    Parameters =
                    {
                        new CodePrimitiveExpression(propertyName),
                        new CodeTypeOfExpression(className),
                        new CodePrimitiveExpression(0),
                    },
                },
            };
        }

        private static CodeMemberField CreateShaderRegisterDependencyProperty(ShaderModel shaderModel, Register register)
        {
            if (typeof(Brush).IsAssignableFrom(register.Type))
            {
                return new CodeMemberField
                {
                    Comments =
                        {
                            new CodeCommentStatement("<summary>The ShaderEffect.RegisterPixelShaderSamplerProperty() method must be used with this field as argument. Note the last parameter of this method: it is an integer and it corresponds to the S0 pixel shader register.</summary>"),
                        },
                    Type = new CodeTypeReference("DependencyProperty"),
                    Name = $"{register.Name}Property",
                    //// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                    Attributes = MemberAttributes.Public | MemberAttributes.Static,
                    InitExpression = new CodeMethodInvokeExpression
                    {
                        Method = new CodeMethodReferenceExpression
                        {
                            TargetObject = new CodeTypeReferenceExpression("ShaderEffect"),
                            MethodName = "RegisterPixelShaderSamplerProperty",
                        },
                        Parameters =
                        {
                            new CodePrimitiveExpression(register.Name),
                            new CodeTypeOfExpression(shaderModel.GeneratedClassName),
                            new CodePrimitiveExpression(register.Ordinal),
                        },
                    },
                };
            }

            return new CodeMemberField
            {
                Type = new CodeTypeReference("DependencyProperty"),
                Name = $"{register.Name}Property",
                //// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                InitExpression = new CodeMethodInvokeExpression
                {
                    Method = new CodeMethodReferenceExpression
                    {
                        TargetObject = new CodeTypeReferenceExpression("DependencyProperty"),
                        MethodName = "Register",
                    },
                    Parameters =
                    {
                        new CodePrimitiveExpression(register.Name),
                        new CodeTypeOfExpression(CreateCodeTypeReference(register.Type)),
                        new CodeTypeOfExpression(shaderModel.GeneratedClassName),
                        new CodeObjectCreateExpression
                        {
                            // Silverlight doesn't have UIPropertyMetadata.
                            CreateType =
                                new CodeTypeReference(shaderModel.TargetFramework == TargetFramework.WPF
                                    ? "UIPropertyMetadata"
                                    : "PropertyMetadata"),
                            Parameters =
                            {
                                CreateDefaultValue(register.Type, register.DefaultValue),
                                new CodeMethodInvokeExpression
                                {
                                    Method = new CodeMethodReferenceExpression(null, "PixelShaderConstantCallback"),
                                    Parameters = { new CodePrimitiveExpression(register.Ordinal) },
                                },
                            },
                        },
                    },
                },
            };
        }

        private static CodeExpression CreateDefaultValue(Type type, object defaultValue)
        {
            if (defaultValue is null)
            {
                return new CodePrimitiveExpression(null);
            }

            var codeTypeReference = CreateCodeTypeReference(type);
            if (defaultValue.GetType().IsPrimitive)
            {
                return new CodePrimitiveExpression(Convert.ChangeType(defaultValue, type));
            }

            if (defaultValue is Point || defaultValue is Vector || defaultValue is Size)
            {
                var point = (Point)RegisterValueConverter.ConvertToUsualType(defaultValue);
                return new CodeObjectCreateExpression(
                    codeTypeReference,
                    new CodePrimitiveExpression(point.X),
                    new CodePrimitiveExpression(point.Y));
            }

            if (defaultValue is Point3D || defaultValue is Vector3D)
            {
                var point3D = (Point3D)RegisterValueConverter.ConvertToUsualType(defaultValue);
                return new CodeObjectCreateExpression(
                    codeTypeReference,
                    new CodePrimitiveExpression(point3D.X),
                    new CodePrimitiveExpression(point3D.Y),
                    new CodePrimitiveExpression(point3D.Z));
            }

            if (defaultValue is Point4D point4D)
            {
                return new CodeObjectCreateExpression(
                    codeTypeReference,
                    new CodePrimitiveExpression(point4D.X),
                    new CodePrimitiveExpression(point4D.Y),
                    new CodePrimitiveExpression(point4D.Z),
                    new CodePrimitiveExpression(point4D.W));
            }

            if (defaultValue is Color color)
            {
                return new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(codeTypeReference),
                    "FromArgb",
                    new CodePrimitiveExpression(color.A),
                    new CodePrimitiveExpression(color.R),
                    new CodePrimitiveExpression(color.G),
                    new CodePrimitiveExpression(color.B));
            }

            return new CodeDefaultValueExpression(codeTypeReference);
        }

        private static CodeMemberProperty CreateClrProperty(string propertyName, Type type, string? description)
        {
            var property = new CodeMemberProperty
            {
                Name = propertyName,
                Type = CreateCodeTypeReference(type),
                //// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                HasGet = true,
                GetStatements =
                {
                    new CodeMethodReturnStatement
                    {
                        Expression = new CodeCastExpression
                        {
                            TargetType = CreateCodeTypeReference(type),
                            Expression = new CodeMethodInvokeExpression
                            {
                                Method = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "GetValue"),
                                Parameters = { new CodeVariableReferenceExpression($"{propertyName}Property") },
                            },
                        },
                    },
                },
                HasSet = true,
                SetStatements =
                {
                    new CodeMethodInvokeExpression
                    {
                        Method = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "SetValue"),
                        Parameters =
                        {
                            new CodeVariableReferenceExpression(propertyName + "Property"),
                            new CodeVariableReferenceExpression("value"),
                        },
                    },
                },
            };

            if (type == typeof(Brush))
            {
                property.Comments.Add(new CodeCommentStatement("<summary>", docComment: true));
                property.Comments.Add(new CodeCommentStatement(
                    $"There has to be a property of type Brush called \"Input\". This property contains the input image and it is usually not set directly - it is set automatically when our effect is applied to a control.",
                    docComment: true));
                property.Comments.Add(new CodeCommentStatement("</summary>", docComment: true));
            }

            if (!string.IsNullOrEmpty(description))
            {
                property.Comments.Add(new CodeCommentStatement($"<summary>{description}</summary>", docComment: true));
            }

            return property;
        }

        private static CodeTypeReference CreateCodeTypeReference(Type type)
        {
            return type.IsPrimitive ? new CodeTypeReference(type) : new CodeTypeReference(type.Name);
        }

        private static CodeConstructor CreatePixelShaderConstructor(ShaderModel shaderModel)
        {
            // Create a constructor that takes a PixelShader as its only parameter.
            var constructor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public,
                Parameters =
                {
                    new CodeParameterDeclarationExpression("PixelShader", "shader"),
                },
                Statements =
                {
                    new CodeAssignStatement
                    {
                        Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "PixelShader"),
                        Right = new CodeArgumentReferenceExpression("shader"),
                    },
                    CreateUpdateMethod("Input"),
                },
            };
            foreach (var register in shaderModel.Registers)
            {
                constructor.Statements.Add(CreateUpdateMethod(register.Name));
            }

            return constructor;
        }

        private static CodeConstructor CreateDefaultConstructor(ShaderModel shaderModel)
        {
            var constructor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public,
                Statements =
                {
                    new CodeAssignStatement
                    {
                        Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "PixelShader"),
                        Right = new CodeFieldReferenceExpression(null, "Shader"),
                    },
                    CreateUpdateMethod("Input"),
                },
            };
            foreach (var register in shaderModel.Registers)
            {
                constructor.Statements.Add(CreateUpdateMethod(register.Name));
            }

            return constructor;
        }

        private static CodeMethodInvokeExpression CreateUpdateMethod(string propertyName)
        {
            return new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "UpdateShaderValue"),
                Parameters =
                {
                    new CodeVariableReferenceExpression(propertyName + "Property"),
                },
            };
        }

        private static CodeNamespace AssignNamespacesToGraph(CodeCompileUnit codeGraph, string namespaceName)
        {
            var ns = new CodeNamespace(namespaceName);
            codeGraph.Namespaces.Add(ns);
            codeGraph.Namespaces.Add(
                new CodeNamespace
                {
                    Imports =
                    {
                        new CodeNamespaceImport("System"),
                        new CodeNamespaceImport("System.Windows"),
                        new CodeNamespaceImport("System.Windows.Media"),
                        new CodeNamespaceImport("System.Windows.Media.Effects"),
                        new CodeNamespaceImport("System.Windows.Media.Media3D"),
                    },
                });

            return ns;
        }

        private static string GenerateCode(CodeDomProvider provider, CodeCompileUnit compileUnit, ShaderModel model)
        {
            // Generate source code using the code generator.
            using var writer = new StringWriter();
            var indentString = Settings.Default.IndentUsingTabs
                                   ? "\t"
                                   : new string(' ', Settings.Default.IndentSpaces);
            var options = new CodeGeneratorOptions
            {
                IndentString = indentString,
                BlankLinesBetweenMembers = true,
                BracingStyle = "C",
            };
            provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
            var text = writer.ToString();
            //// Fix up code: make static DP fields readonly, and use triple-slash or triple-quote comments for XML doc comments.
            if (provider.FileExtension == "cs")
            {
                text = text.Replace(
                               "private static PixelShader Shader = new PixelShader()",
                               $"private static readonly PixelShader Shader = new PixelShader{Environment.NewLine}" +
                               $"        {{{Environment.NewLine}" +
                               $"            UriSource = new Uri(\"pack://application:,,,/[assemblyname];component/[folder]/{model.GeneratedClassName}.ps\", UriKind.Absolute){Environment.NewLine}" +
                               $"        }}")
                           .Replace(
                               "public static DependencyProperty",
                               "public static readonly DependencyProperty")
                           .Replace($"{writer.NewLine}    {writer.NewLine}", $"{writer.NewLine}{writer.NewLine}")
                           .Replace($"{writer.NewLine}        {writer.NewLine}", $"{writer.NewLine}{writer.NewLine}")
                           .Replace($"{writer.NewLine}{writer.NewLine}{writer.NewLine}", $"{writer.NewLine}{writer.NewLine}")
                           .Replace($"{{{writer.NewLine}{writer.NewLine}", $"{{{writer.NewLine}");
            }
            else if (provider.FileExtension == "vb")
            {
                text = text.Replace("Public Shared ", "Public Shared ReadOnly ");
                text = text.Replace("'<", "'''<");
            }

            return text;
        }
    }
}
