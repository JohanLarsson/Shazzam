using System;
using System.CodeDom.Compiler;
using System.IO;
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
using System.Windows.Threading;
using System.Xml;
using ICSharpCode.TextEditor.Document;
using Microsoft.CSharp;
using Shazzam.CodeGen;
using Shazzam.Controls;
using Shazzam.Converters;

namespace Shazzam.Views
{

	public partial class CodeTabView : UserControl
	{
		private ICSharpCode.TextEditor.TextEditorControl _shaderTextEditor;
		private ICSharpCode.TextEditor.TextEditorControl _csTextEditor;
		private ICSharpCode.TextEditor.TextEditorControl _vbTextEditor;
		private ShaderModel _shaderModel;
		private DefaultHighlightingStrategy _hlslHS;
		private ShaderCompiler _compiler;
		private ShaderEffect _currentShaderEffect;
		private DispatcherTimer timer;

		public CodeTabView()
		{
			InitializeComponent();

			_shaderTextEditor = CreateTextEditor();
			_shaderTextEditor.Encoding = System.Text.Encoding.ASCII;
			using (var stream = typeof(CodeTabView).Assembly.GetManifestResourceStream("Shazzam.Resources.HLSLSyntax.xshd"))
			{
				using (var reader = new XmlTextReader(stream)){
				var sm = new SyntaxMode("HLSL.xshd", "HLSL", ".fx");
				_hlslHS = HighlightingDefinitionParser.Parse(sm, reader);
				_hlslHS.ResolveReferences(); // don't forget this!
				reader.Close();
			}
			}
			_shaderTextEditor.Document.HighlightingStrategy = _hlslHS;
			this.formsHost.Child = _shaderTextEditor;

			_csTextEditor = CreateTextEditor();
			this.formsHostCs.Child = _csTextEditor;

			_vbTextEditor = CreateTextEditor();
			this.formsHostVb.Child = _vbTextEditor;

			_compiler = new ShaderCompiler();
			_compiler.Reset();
			outputTextBox.DataContext = _compiler;
			this.Loaded += CodeTabView_Loaded;
		}

		void CodeTabView_Loaded(object sender, RoutedEventArgs e)
		{
			SetupBlurAnimation();
			//messagePopup.PlacementTarget = ShazzamSwitchboard.MainWindow;
			
		}

		private void ClosePopup(object o, EventArgs e)
		{
		//	this.messagePopup.IsOpen = false;
		//	timer.Stop();
			//blurStoryBoard.Stop;

			ShazzamSwitchboard.MainWindow.Effect = null;
			ShazzamSwitchboard.MainWindow.Opacity = 1;
		}
		private Storyboard blurStoryBoard = new Storyboard();
		private DoubleAnimation blurAnimation = new DoubleAnimation();
		private DoubleAnimation opacityAnimation = new DoubleAnimation();
		private BlurEffect blur = new BlurEffect { Radius = 0, RenderingBias = RenderingBias.Performance };
		public void SetupBlurAnimation()
		{
			blurAnimation.Duration = opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(1.2));
			blurAnimation.From = 0;
			blurAnimation.To = 6;

			opacityAnimation.From = .3;
			opacityAnimation.To = 1;
			opacityAnimation.FillBehavior = blurAnimation.FillBehavior = FillBehavior.Stop;

			Storyboard.SetTarget(this.blurAnimation, ShazzamSwitchboard.MainWindow);
			Storyboard.SetTargetProperty(this.blurAnimation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));
			//this.blurStoryBoard.Children.Add(this.blurAnimation);

			Storyboard.SetTarget(this.opacityAnimation, ShazzamSwitchboard.MainWindow);
			Storyboard.SetTargetProperty(this.opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
			this.blurStoryBoard.Children.Add(this.opacityAnimation);

		}
		public void CompileShader()
		{
			try
			{

				ShazzamSwitchboard.MainWindow.Effect = blur;


				blurStoryBoard.Begin(this, true);

				_compiler.Compile(this.CodeText);
				
				//	this.messagePopup.IsOpen = true;
					compileStatusText.Text = String.Format("Last Compiled at: {0}", DateTime.Now.ToLongTimeString());
				
			//	timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 2000), DispatcherPriority.Background, ClosePopup, this.Dispatcher);
			//	timer.Start();

			}
			catch (CompilerException ex)
			{
				MessageBox.Show(ShazzamSwitchboard.MainWindow, ex.Message, "Could not compile", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private ICSharpCode.TextEditor.TextEditorControl CreateTextEditor()
		{

			var currentEditor = new ICSharpCode.TextEditor.TextEditorControl();
			currentEditor.ShowLineNumbers = true;
			currentEditor.ShowInvalidLines = false; // don't show error squiggle on empty lines
			currentEditor.ShowEOLMarkers = false;
			currentEditor.ShowSpaces = false;
			currentEditor.ShowTabs = false;
			currentEditor.ShowVRuler = false;
			currentEditor.ShowMatchingBracket = true;
			currentEditor.AutoScroll = true;

			currentEditor.Document.TextEditorProperties.IndentationSize = 2;
			currentEditor.EnableFolding = true;
			currentEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

			currentEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			currentEditor.Font = new System.Drawing.Font("Consolas", 10);
			return currentEditor;

		}

		private void GenerateBlankInputControls()
		{
			TextBlock textBlock = new TextBlock
			{
				Foreground = Brushes.White,
				Margin = new Thickness(5),
				Text = "The current effect has no input parameters."
			};
			inputControlPanel.Children.Add(textBlock);
		}

		private void GenerateShaderInputControl(ShaderModelConstantRegister register)
		{
			TextBlock textBlock = new TextBlock
			{
				Foreground = Brushes.White,
				Margin = new Thickness(5),
				Inlines =
				{
					new Run { Foreground = (Brush)Application.Current.FindResource("HighlightBrush"), Text = register.RegisterName },
					new Run { Text = String.Format(" : {0}", register.RegisterType.Name) },
				},
				ToolTip = String.IsNullOrEmpty(register.Description) ? null : register.Description
			};
			inputControlPanel.Children.Add(textBlock);

			Control control = null;
			if (register.RegisterType == typeof(double) || register.RegisterType == typeof(float))
			{
				double minValue = Convert.ToDouble(register.MinValue);
				double maxValue = Convert.ToDouble(register.MaxValue);
				double defaultValue = Convert.ToDouble(register.DefaultValue);
				control = new AdjustableSlider
				{
					Minimum = Math.Min(minValue, defaultValue),
					Maximum = Math.Max(maxValue, defaultValue),
					Value = defaultValue
				};
			}
			else if (register.RegisterType == typeof(Point) || register.RegisterType == typeof(Vector) || register.RegisterType == typeof(Size))
			{
				Point minValue = (Point)RegisterValueConverter.ConvertToUsualType(register.MinValue);
				Point maxValue = (Point)RegisterValueConverter.ConvertToUsualType(register.MaxValue);
				Point defaultValue = (Point)RegisterValueConverter.ConvertToUsualType(register.DefaultValue);
				control = new AdjustableSliderPair
				{
					Minimum = new Point(Math.Min(minValue.X, defaultValue.X), Math.Min(minValue.Y, defaultValue.Y)),
					Maximum = new Point(Math.Max(maxValue.X, defaultValue.X), Math.Max(maxValue.Y, defaultValue.Y)),
					Value = defaultValue
				};
			}
			else if (register.RegisterType == typeof(Point3D) || register.RegisterType == typeof(Vector3D))
			{
				Point3D minValue = (Point3D)RegisterValueConverter.ConvertToUsualType(register.MinValue);
				Point3D maxValue = (Point3D)RegisterValueConverter.ConvertToUsualType(register.MaxValue);
				Point3D defaultValue = (Point3D)RegisterValueConverter.ConvertToUsualType(register.DefaultValue);
				control = new AdjustableSliderTriplet
				{
					Minimum = new Point3D(Math.Min(minValue.X, defaultValue.X), Math.Min(minValue.Y, defaultValue.Y), Math.Min(minValue.Z, defaultValue.Z)),
					Maximum = new Point3D(Math.Max(maxValue.X, defaultValue.X), Math.Max(maxValue.Y, defaultValue.Y), Math.Max(maxValue.Z, defaultValue.Z)),
					Value = defaultValue
				};
			}
			else if (register.RegisterType == typeof(Color))
			{
				Color defaultValue = (Color)register.DefaultValue;
				control = new TextBox
				{
					Background = Brushes.LightYellow,
					Width = 150,
					HorizontalAlignment = HorizontalAlignment.Left,
					Text = defaultValue.ToString()
				};
			}
			else if (register.RegisterType == typeof(Point4D))
			{
				Point4D minValue = (Point4D)register.MinValue;
				Point4D maxValue = (Point4D)register.MaxValue;
				Point4D defaultValue = (Point4D)register.DefaultValue;
				control = new AdjustableSliderQuadruplet
				{
					Minimum = new Point4D(Math.Min(minValue.X, defaultValue.X), Math.Min(minValue.Y, defaultValue.Y), Math.Min(minValue.Z, defaultValue.Z), Math.Min(minValue.W, defaultValue.W)),
					Maximum = new Point4D(Math.Max(maxValue.X, defaultValue.X), Math.Max(maxValue.Y, defaultValue.Y), Math.Max(maxValue.Z, defaultValue.Z), Math.Max(maxValue.W, defaultValue.W)),
					Value = defaultValue
				};
			}

			if (control != null)
			{
				control.Margin = new Thickness(15, 2, 25, 5);
				this.inputControlPanel.Children.Add(control);
				register.AffiliatedControl = control;
			}
		}

		private void BindShaderEffectToControls()
		{
			ShaderEffect shaderEffect = this.CurrentShaderEffect;
			if (shaderEffect != null)
			{
				foreach (ShaderModelConstantRegister register in this._shaderModel.Registers)
				{
					if (register.AffiliatedControl != null)
					{
						FieldInfo fieldInfo = shaderEffect.GetType().GetField(String.Format("{0}Property", register.RegisterName), BindingFlags.Public | BindingFlags.Static);
						if (fieldInfo != null)
						{
							DependencyProperty dependencyProperty = fieldInfo.GetValue(null) as DependencyProperty;
							if (dependencyProperty != null)
							{
								string controlPropertyName = "Value";
								if (register.RegisterType == typeof(Color))
								{
									controlPropertyName = "Text";
								}
								Binding binding = new Binding(controlPropertyName)
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
		string csFolder, vbFolder;
		private void FillEditControls()
		{
			inputControlPanel.Children.Clear();
			if (_shaderModel.Registers.Count == 0)
			{
				GenerateBlankInputControls();
			}
			_shaderModel.Registers.ForEach(GenerateShaderInputControl);

			_csTextEditor.Text = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("CSharp"), _shaderModel, false);
			_csTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(".cs");

			
			_vbTextEditor.Text = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("VisualBasic"), _shaderModel, false);
			_vbTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(".vb");

			
		}

		public void RenderShader()
		{
			CompileShader();
			if (_compiler.IsCompiled == false)
			{
				return;
			}

			try
			{
				this.InputControlsTab.IsEnabled = false;
				string path = Properties.Settings.Default.FolderOutput;
				if (!File.Exists(path + Constants.FileNames.TempShaderPs))
				{
					return;
				}
				var ps = new PixelShader { UriSource = new Uri(path + Constants.FileNames.TempShaderPs) };

				this._shaderModel = Shazzam.CodeGen.CodeParser.ParseShader(this._shaderTextEditor.FileName, this.CodeText);
				string codeString = CreatePixelShaderClass.GetSourceText(new CSharpCodeProvider(), this._shaderModel, true);
				Assembly autoAssembly = CreatePixelShaderClass.CompileInMemory(codeString);

				if (autoAssembly == null)
				{
					MessageBox.Show(ShazzamSwitchboard.MainWindow, "Cannot compile the generated C# code.", "Compile error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				Type t = autoAssembly.GetType(String.Format("{0}.{1}", _shaderModel.GeneratedNamespace, _shaderModel.GeneratedClassName));
				this.FillEditControls();
				csFolder = String.Format("{0}CS", Properties.Settings.Default.FolderOutput);
				if (!Directory.Exists(csFolder))
				{
					Directory.CreateDirectory(csFolder);
				}
				_csTextEditor.SaveFile(String.Format("{0}\\{1}.cs", csFolder, _shaderModel.GeneratedClassName));
				vbFolder = String.Format("{0}VB", Properties.Settings.Default.FolderOutput);
				if (!Directory.Exists(vbFolder))
				{
					Directory.CreateDirectory(vbFolder);
				}
				_vbTextEditor.SaveFile(String.Format("{0}\\{1}.vb", vbFolder, _shaderModel.GeneratedClassName));
			

				this.CurrentShaderEffect = (ShaderEffect)Activator.CreateInstance(t, new object[] { ps });
				this.InputControlsTab.IsEnabled = true;
			}
			catch (Exception)
			{
				MessageBox.Show(ShazzamSwitchboard.MainWindow, "Cannot create a WPF shader from the code snippet.", "Compile error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		#region Properties

		public String CodeText
		{
			get
			{

				return _shaderTextEditor.Text;
			}
			set
			{
				_shaderTextEditor.Text = value;
			}
		}
		public String OutputText
		{
			get
			{
				return outputTextBox.Text;
			}
			set
			{
				outputTextBox.Text = value;
			}
		}
		internal ShaderEffect CurrentShaderEffect
		{
			get
			{
				return _currentShaderEffect;
			}
			private set
			{
				if (this._currentShaderEffect != value)
				{
					ShaderEffect oldShaderEffect = this._currentShaderEffect;
					this._currentShaderEffect = value;
					this.BindShaderEffectToControls();
					this.OnShaderEffectChanged(oldShaderEffect, this._currentShaderEffect);
				}
			}
		}

		#endregion
		#region Events

		public event RoutedPropertyChangedEventHandler<object> ShaderEffectChanged
		{
			add
			{
				AddHandler(ShaderEffectChangedEvent, value);
			}

			remove
			{
				RemoveHandler(ShaderEffectChangedEvent, value);
			}
		}

		public static readonly RoutedEvent ShaderEffectChangedEvent = EventManager.RegisterRoutedEvent(
						"ShaderEffectChanged",
						RoutingStrategy.Bubble,
						typeof(RoutedPropertyChangedEventHandler<object>),
						typeof(CodeTabView)
				);

		protected virtual void OnShaderEffectChanged(object oldItem, object newItem)
		{
			RoutedPropertyChangedEventArgs<object> args =
					new RoutedPropertyChangedEventArgs<object>(newItem, newItem);
			args.RoutedEvent = CodeTabView.ShaderEffectChangedEvent;
			RaiseEvent(args);
		}

		private void OutputTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				Match match = Regex.Match(this.OutputText, @"\((?<lineNumber>\d+),(?<charNumber>\d+)\)");
				if (match.Success)
				{
					int lineNumber = Int32.Parse(match.Groups["lineNumber"].Value);
					int charNumber = Int32.Parse(match.Groups["charNumber"].Value);
					this._shaderTextEditor.ActiveTextAreaControl.Caret.Line = lineNumber - 1;
					this._shaderTextEditor.ActiveTextAreaControl.Caret.Column = charNumber - 1;
					this._shaderTextEditor.Focus();
				}
				e.Handled = true;
			}
		}
		#endregion

		public void SaveFile()
		{
			_shaderTextEditor.SaveFile(_shaderTextEditor.FileName);
		}

		public void SaveFile(string fileName)
		{
			this._shaderTextEditor.SaveFile(fileName);
			this.codeTab.Header = Path.GetFileName(fileName);
		}

		public void OpenFile(string fileName)
		{
			this.codeTab.Focus();
			this.codeTab.Header = Path.GetFileName(fileName);
			this._shaderTextEditor.LoadFile(fileName);
			this._shaderTextEditor.Document.HighlightingStrategy = _hlslHS;
			this.RenderShader();
		}
	}
}
