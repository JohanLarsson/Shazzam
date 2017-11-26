namespace Shazzam.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Effects;
    using System.Windows.Media.Media3D;
    using System.Xml;
    using ICSharpCode.TextEditor.Document;
    using Microsoft.CSharp;
    using Shazzam.CodeGen;
    using Shazzam.Controls;
    using Shazzam.Converters;

    public partial class CodeTabView : UserControl, IDisposable
    {
        public static readonly RoutedEvent ShaderEffectChangedEvent = EventManager.RegisterRoutedEvent(
            "ShaderEffectChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>),
            typeof(CodeTabView));

        private readonly ICSharpCode.TextEditor.TextEditorControl shaderTextEditor;
        private readonly ICSharpCode.TextEditor.TextEditorControl csTextEditor;

        private readonly Storyboard blurStoryBoard = new Storyboard();
        private readonly DoubleAnimation blurAnimation = new DoubleAnimation();
        private readonly DoubleAnimation opacityAnimation = new DoubleAnimation();
        private readonly BlurEffect blur = new BlurEffect { Radius = 0, RenderingBias = RenderingBias.Performance };
        private readonly DefaultHighlightingStrategy hlslHs;
        private readonly ShaderCompiler compiler;

        private ShaderModel shaderModel;
        private ShaderEffect currentShaderEffect;
        private int dirtyCounter;
        private int storedDocHash;

        public CodeTabView()
        {
            this.InitializeComponent();

            this.shaderTextEditor = this.CreateTextEditor();
            this.shaderTextEditor.Encoding = System.Text.Encoding.ASCII;
            //// _shaderTextEditor.TextChanged += new EventHandler(_shaderTextEditor_TextChanged);
            //// _shaderTextEditor.Document.TextContentChanged += new EventHandler(Document_TextContentChanged);
            this.shaderTextEditor.Document.DocumentChanged += this.DocumentDocumentChanged;
            using (var stream = typeof(CodeTabView).Assembly.GetManifestResourceStream("Shazzam.Resources.HLSLSyntax.xshd"))
            {
                if (stream != null)
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        var sm = new SyntaxMode("HLSL.xshd", "HLSL", ".fx");
                        this.hlslHs = HighlightingDefinitionParser.Parse(sm, reader);
                        this.hlslHs.ResolveReferences(); // don't forget this!
                        reader.Close();
                    }
                }
            }

            this.shaderTextEditor.Document.HighlightingStrategy = this.hlslHs;
            this.formsHost.Child = this.shaderTextEditor;

            this.csTextEditor = this.CreateTextEditor();
            this.formsHostCs.Child = this.csTextEditor;

            this.compiler = new ShaderCompiler();
            this.compiler.Reset();
            this.outputTextBox.DataContext = this.compiler;
            this.Loaded += this.CodeTabViewLoaded;
        }

        public event RoutedPropertyChangedEventHandler<object> ShaderEffectChanged
        {
            add => this.AddHandler(ShaderEffectChangedEvent, value);
            remove => this.RemoveHandler(ShaderEffectChangedEvent, value);
        }

        public string CodeText
        {
            get => this.shaderTextEditor.Text;
            set => this.shaderTextEditor.Text = value;
        }

        public string OutputText
        {
            get => this.outputTextBox.Text;
            set => this.outputTextBox.SetValue(TextBlock.TextProperty, value);
        }

        internal ShaderEffect CurrentShaderEffect
        {
            get => this.currentShaderEffect;

            private set
            {
                if (!Equals(this.currentShaderEffect, value))
                {
                    var oldShaderEffect = this.currentShaderEffect;
                    this.currentShaderEffect = value;
                    this.BindShaderEffectToControls();
                    this.OnShaderEffectChanged(oldShaderEffect, this.currentShaderEffect);
                }
            }
        }

        private string MergedCode
        {
            get
            {
                string ReadHlsli(string name)
                {
                    var file = Directory.EnumerateFiles(
                                              Directory.GetCurrentDirectory(),
                                              "*.hlsli",
                                              SearchOption.AllDirectories)
                                          .Single(x => x.EndsWith(name));
                    return File.ReadAllText(file);
                }

                return Regex.Replace(
                    this.CodeText,
                    @"#include <(?<hlsli>\w+.hlsli)>",
                    x => ReadHlsli(x.Groups["hlsli"].Value));
            }
        }

        public void SetupBlurAnimation()
        {
            var duration = new Duration(TimeSpan.FromSeconds(1.2));
            this.opacityAnimation.SetCurrentValue(Timeline.DurationProperty, duration);
            this.blurAnimation.SetCurrentValue(Timeline.DurationProperty, duration);
            this.blurAnimation.SetCurrentValue(DoubleAnimation.FromProperty, (double?)0);
            this.blurAnimation.SetCurrentValue(DoubleAnimation.ToProperty, (double?)6);

            this.opacityAnimation.SetCurrentValue(DoubleAnimation.FromProperty, .3);
            this.opacityAnimation.SetCurrentValue(DoubleAnimation.ToProperty, (double?)1);
            this.blurAnimation.SetCurrentValue(Timeline.FillBehaviorProperty, FillBehavior.Stop);
            this.opacityAnimation.SetCurrentValue(Timeline.FillBehaviorProperty, FillBehavior.Stop);

            Storyboard.SetTarget(this.blurAnimation, ShazzamSwitchboard.MainWindow);
            Storyboard.SetTargetProperty(this.blurAnimation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));
            //// this.blurStoryBoard.Children.Add(this.blurAnimation);

            Storyboard.SetTarget(this.opacityAnimation, ShazzamSwitchboard.MainWindow);
            Storyboard.SetTargetProperty(this.opacityAnimation, new PropertyPath(OpacityProperty));
            this.blurStoryBoard.Children.Add(this.opacityAnimation);
        }

        public void CompileShader()
        {
            try
            {
                ShazzamSwitchboard.MainWindow.SetCurrentValue(EffectProperty, this.blur);
                this.versionNotSupported.SetCurrentValue(VisibilityProperty, Visibility.Hidden);

                this.blurStoryBoard.Begin(this, isControllable: true);

                if (Properties.Settings.Default.TargetFramework == "WPF_PS3")
                {
                    this.versionNotSupported.SetCurrentValue(
                        VisibilityProperty,
                        RenderCapability.IsPixelShaderVersionSupported(3, 0)
                            ? Visibility.Hidden
                            : Visibility.Visible);

                    this.compiler.Compile(this.MergedCode, ShaderProfile.PixelShader3);
                    //// _compiler.Compile(this.CodeText);
                }
                else
                {
                    this.compiler.Compile(this.MergedCode, ShaderProfile.PixelShader2);
                    //// _compiler.Compile(this.CodeText);
                }

                this.compileStatusText.SetCurrentValue(TextBlock.TextProperty, $"Last Compiled at: {DateTime.Now.ToLongTimeString()}");
            }
            catch (CompilerException ex)
            {
                MessageBox.Show(ShazzamSwitchboard.MainWindow, ex.Message, "Could not compile", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RenderShader()
        {
            this.CompileShader();
            if (this.compiler.IsCompiled == false)
            {
                return;
            }

            try
            {
                this.InputControlsTab.SetCurrentValue(IsEnabledProperty, false);
                var path = Properties.Settings.Default.FolderPath_Output;
                if (!File.Exists(path + Constants.FileNames.TempShaderPs))
                {
                    return;
                }

                var ps = new PixelShader { UriSource = new Uri(path + Constants.FileNames.TempShaderPs) };

                this.shaderModel = CodeGen.CodeParser.ParseShader(this.shaderTextEditor.FileName, this.CodeText);
                var code = ShaderClass.GetSourceText(new CSharpCodeProvider(), this.shaderModel, includePixelShaderConstructor: true);
                var autoAssembly = ShaderClass.CompileInMemory(code);

                if (autoAssembly == null)
                {
                    MessageBox.Show(ShazzamSwitchboard.MainWindow, "Cannot compile the generated C# code.", "Compile error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var t = autoAssembly.GetType($"{this.shaderModel.GeneratedNamespace}.{this.shaderModel.GeneratedClassName}");
                this.FillEditControls();
                var outputFolder = $"{Properties.Settings.Default.FolderPath_Output}{this.shaderModel.GeneratedClassName}";

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                this.csTextEditor.SaveFile($"{outputFolder}\\{this.shaderModel.GeneratedClassName}.cs");
                File.Copy(this.shaderTextEditor.FileName, $"{outputFolder}\\{Path.GetFileName(this.shaderTextEditor.FileName)}", overwrite: true);
                this.CreateFileCopies(outputFolder + @"\", this.shaderModel.GeneratedClassName);
                this.CurrentShaderEffect = (ShaderEffect)Activator.CreateInstance(t, ps);
                this.InputControlsTab.SetCurrentValue(IsEnabledProperty, true);
            }
            catch (Exception)
            {
                MessageBox.Show(ShazzamSwitchboard.MainWindow, "Cannot create a WPF shader from the code snippet.", "Compile error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveFile()
        {
            this.ResetDirty();
            this.storedDocHash = this.shaderTextEditor.Document.TextContent.GetHashCode();
            this.shaderTextEditor.SaveFile(this.shaderTextEditor.FileName);
        }

        public void SaveFile(string fileName)
        {
            this.ResetDirty();
            //// _dirtyCounter = 2;
            this.storedDocHash = this.shaderTextEditor.Document.TextContent.GetHashCode();
            this.shaderTextEditor.SaveFile(fileName);
            this.codeTabHeaderText.SetCurrentValue(TextBlock.TextProperty, Path.GetFileName(fileName));
        }

        public void OpenFile(string fileName)
        {
            this.SaveFileFirst();
            this.ResetDirty();
            this.dirtyCounter = 0;
            this.codeTab.Focus();
            this.codeTabHeaderText.SetCurrentValue(TextBlock.TextProperty, Path.GetFileName(fileName));
            this.shaderTextEditor.LoadFile(fileName);
            this.shaderTextEditor.Document.HighlightingStrategy = this.hlslHs;
            Properties.Settings.Default.FilePath_LastFx = fileName;
            Properties.Settings.Default.Save();
            this.RenderShader();
        }

        public void SaveFileFirst()
        {
            if (this.dirtyStatusText.Visibility == Visibility.Visible)
            {
                var message = "The fx file has unsaved changes.  Would you like to save your work?";
                if (MessageBox.Show(message, "Save file", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.SaveFile();
                }
            }
        }

        public void Dispose()
        {
            this.shaderTextEditor?.Dispose();
            this.csTextEditor?.Dispose();
        }

        protected virtual void OnShaderEffectChanged(object oldItem, object newItem)
        {
            var args = new RoutedPropertyChangedEventArgs<object>(oldItem, newItem) { RoutedEvent = ShaderEffectChangedEvent };
            this.RaiseEvent(args);
        }

        private void OutputTextBoxMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var match = Regex.Match(this.OutputText, @"\((?<lineNumber>\d+),(?<charNumber>\d+)\)");
                if (match.Success)
                {
                    var lineNumber = int.Parse(match.Groups["lineNumber"].Value);
                    var charNumber = int.Parse(match.Groups["charNumber"].Value);
                    this.shaderTextEditor.ActiveTextAreaControl.Caret.Line = lineNumber - 1;
                    this.shaderTextEditor.ActiveTextAreaControl.Caret.Column = charNumber - 1;
                    this.shaderTextEditor.Focus();
                }

                e.Handled = true;
            }
        }

        private void ResetDirty()
        {
            this.storedDocHash = 0;
            this.dirtyStatusText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
        }

        private void SetupInputBindings()
        {
            var changeToCodeTab = new RoutedUICommand("Change To Code Tab", "ChangeToCodeTab", typeof(CodeTabView));
            var changeToEditTab = new RoutedUICommand("Change To Edit Tab", "ChangeToEditTab", typeof(CodeTabView));

            var cb = new CommandBinding(changeToCodeTab, (s, e) => this.codeTabControl.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, this.codeTab));
            var kb = new KeyBinding(changeToCodeTab, Key.F9, ModifierKeys.Control);

            ShazzamSwitchboard.MainWindow.CommandBindings.Add(cb);
            //// this.CommandBindings.Add(cb);
            ShazzamSwitchboard.MainWindow.InputBindings.Add(kb);
            //// this.InputBindings.Add(kb);

            var cb2 = new CommandBinding(changeToEditTab, (s, e) => this.codeTabControl.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, this.InputControlsTab));
            kb = new KeyBinding(changeToEditTab, Key.F10, ModifierKeys.Control);

            ShazzamSwitchboard.MainWindow.CommandBindings.Add(cb2);
            ShazzamSwitchboard.MainWindow.InputBindings.Add(kb);
            //// this.CommandBindings.Add(cb2);
            //// this.InputBindings.Add(kb);
        }

        private void DocumentDocumentChanged(object sender, DocumentEventArgs e)
        {
            // this smells bad, but the TextEditor doesn't have a isDirty flag
            // the DocumentChanged event fires twice when a document is loaded.
            this.dirtyCounter += 1;
            if (this.dirtyCounter == 2)
            {
                this.storedDocHash = this.shaderTextEditor.Document.TextContent.GetHashCode();
            }

            if (this.shaderTextEditor.Document.TextContent.GetHashCode() == this.storedDocHash)
            {
                ShazzamSwitchboard.CodeTabView.dirtyStatusText.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                ShazzamSwitchboard.CodeTabView.dirtyStatusText.SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
        }

        private void CodeTabViewLoaded(object sender, RoutedEventArgs e)
        {
            this.SetupBlurAnimation();
            this.SetupInputBindings();
        }

        private void CreateFileCopies(string path, string newFileName)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.FilePath_LastFx))
            {
                return;
            }

            ////var currentFileName = Path.GetFileNameWithoutExtension(Properties.Settings.Default.FilePath_LastFx);
            if (File.Exists(Properties.Settings.Default.FolderPath_Output + Constants.FileNames.TempShaderPs))
            {
                File.Copy(Properties.Settings.Default.FolderPath_Output + Constants.FileNames.TempShaderPs, path + newFileName + ".ps", overwrite: true);
            }
        }

        private ICSharpCode.TextEditor.TextEditorControl CreateTextEditor()
        {
            var currentEditor = new ICSharpCode.TextEditor.TextEditorControl
            {
                ShowLineNumbers = true,
                ShowInvalidLines = false,
                ShowEOLMarkers = false,
                ShowSpaces = false,
                ShowTabs = false,
                ShowVRuler = false,
                ShowMatchingBracket = true,
                AutoScroll = true
            };

            currentEditor.Document.TextEditorProperties.IndentationSize = 4;
            currentEditor.EnableFolding = true;
            currentEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            currentEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            currentEditor.Font = new System.Drawing.Font("Consolas", 10);
            return currentEditor;
        }

        private void GenerateBlankInputControls()
        {
            var textBlock = new TextBlock
            {
                Foreground = Brushes.White,
                Margin = new Thickness(5),
                Text = "The current effect has no input parameters."
            };
            this.inputControlPanel.Children.Add(textBlock);
        }

        private void GenerateShaderInputControl(ShaderModelConstantRegister register)
        {
            var toolTipText = string.IsNullOrEmpty(register.Description) ? null : register.Description;

            var textBlock = new TextBlock
            {
                Foreground = Brushes.White,
                Margin = new Thickness(5),
                Inlines =
                                        {
                                            new Run { Foreground = (Brush)Application.Current.FindResource("HighlightBrush"), Text = register.RegisterName },
                                            new Run { Text = $" : {register.RegisterType.Name}" },
                                        },
                ToolTip = toolTipText
            };
            this.inputControlPanel.Children.Add(textBlock);

            Control control = null;
            if (register.RegisterType == typeof(Brush))
            {
                control = new TexturePicker(register);
            }
            else if (register.RegisterType == typeof(double) || register.RegisterType == typeof(float))
            {
                var minValue = Convert.ToDouble(register.MinValue);
                var maxValue = Convert.ToDouble(register.MaxValue);
                //// double defaultValue = Double.Parse(register.DefaultValue.ToString(), NumberStyles.Any, null);
                var defaultValue = Convert.ToDouble(register.DefaultValue);
                control = new AdjustableSlider
                {
                    Minimum = Math.Min(minValue, defaultValue),
                    Maximum = Math.Max(maxValue, defaultValue),
                    Value = defaultValue
                };
            }
            else if (register.RegisterType == typeof(Point) || register.RegisterType == typeof(Vector) || register.RegisterType == typeof(Size))
            {
                var minValue = (Point)RegisterValueConverter.ConvertToUsualType(register.MinValue);
                var maxValue = (Point)RegisterValueConverter.ConvertToUsualType(register.MaxValue);
                var defaultValue = (Point)RegisterValueConverter.ConvertToUsualType(register.DefaultValue);
                control = new AdjustableSliderPair
                {
                    Minimum = new Point(Math.Min(minValue.X, defaultValue.X), Math.Min(minValue.Y, defaultValue.Y)),
                    Maximum = new Point(Math.Max(maxValue.X, defaultValue.X), Math.Max(maxValue.Y, defaultValue.Y)),
                    Value = defaultValue
                };
            }
            else if (register.RegisterType == typeof(Point3D) || register.RegisterType == typeof(Vector3D))
            {
                var minValue = (Point3D)RegisterValueConverter.ConvertToUsualType(register.MinValue);
                var maxValue = (Point3D)RegisterValueConverter.ConvertToUsualType(register.MaxValue);
                var defaultValue = (Point3D)RegisterValueConverter.ConvertToUsualType(register.DefaultValue);
                control = new AdjustableSliderTriplet
                {
                    Minimum = new Point3D(Math.Min(minValue.X, defaultValue.X), Math.Min(minValue.Y, defaultValue.Y), Math.Min(minValue.Z, defaultValue.Z)),
                    Maximum = new Point3D(Math.Max(maxValue.X, defaultValue.X), Math.Max(maxValue.Y, defaultValue.Y), Math.Max(maxValue.Z, defaultValue.Z)),
                    Value = defaultValue
                };
            }
            else if (register.RegisterType == typeof(Color))
            {
                var defaultValue = (Color)register.DefaultValue;
                //// control = new Telerik.Windows.Controls.RadColorEditor
                //// {

                //// HorizontalAlignment = HorizontalAlignment.Left,
                ////  SelectedColor = defaultValue
                //// };

                control = new AdjustableColor
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Value = defaultValue
                };
                //// ((control) as AdjustableColor).;
                //// control = new TextBox
                //// {
                ////  Background = Brushes.LightYellow,
                ////  Width = 150,
                ////  HorizontalAlignment = HorizontalAlignment.Left,
                ////  Text = defaultValue.ToString()
                //// };
            }
            else if (register.RegisterType == typeof(Point4D))
            {
                var minValue = (Point4D)register.MinValue;
                var maxValue = (Point4D)register.MaxValue;
                var defaultValue = (Point4D)register.DefaultValue;
                control = new AdjustableSliderQuadruplet
                {
                    Minimum = new Point4D(Math.Min(minValue.X, defaultValue.X), Math.Min(minValue.Y, defaultValue.Y), Math.Min(minValue.Z, defaultValue.Z), Math.Min(minValue.W, defaultValue.W)),
                    Maximum = new Point4D(Math.Max(maxValue.X, defaultValue.X), Math.Max(maxValue.Y, defaultValue.Y), Math.Max(maxValue.Z, defaultValue.Z), Math.Max(maxValue.W, defaultValue.W)),
                    Value = defaultValue
                };
            }

            if (control != null)
            {
                control.SetCurrentValue(MarginProperty, new Thickness(15, 2, 25, 5));
                control.SetCurrentValue(ToolTipProperty, toolTipText);
                this.inputControlPanel.Children.Add(control);
                register.AffiliatedControl = control;
            }
        }

        private void BindShaderEffectToControls()
        {
            var shaderEffect = this.CurrentShaderEffect;
            if (shaderEffect != null)
            {
                foreach (var register in this.shaderModel.Registers)
                {
                    if (register.AffiliatedControl != null)
                    {
                        var fieldInfo = shaderEffect.GetType().GetField($"{register.RegisterName}Property", BindingFlags.Public | BindingFlags.Static);
                        if (fieldInfo != null)
                        {
                            if (fieldInfo.GetValue(null) is DependencyProperty dependencyProperty)
                            {
                                var controlPropertyName = "Value";

                                var binding = new Binding(controlPropertyName)
                                {
                                    Source = register.AffiliatedControl,
                                    Converter = new RegisterValueConverter()
                                };
                                BindingOperations.SetBinding(shaderEffect, dependencyProperty, binding);
                            }
                        }
                    }
                }
            }
        }

        private void FillEditControls()
        {
            this.inputControlPanel.Children.Clear();
            if (this.shaderModel.Registers.Count == 0)
            {
                this.GenerateBlankInputControls();
            }

            this.shaderModel.Registers.ForEach(this.GenerateShaderInputControl);
            this.csTextEditor.Text = ShaderClass.GetSourceText(CodeDomProvider.CreateProvider("CSharp"), this.shaderModel, includePixelShaderConstructor: false);
            this.csTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(".cs");
        }
    }
}
