using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using ICSharpCode.TextEditor.Document;
using Microsoft.CSharp;
using Shazzam.CodeGen;
namespace Shazzam.Views {

	public partial class CodeTabView : UserControl {
		ICSharpCode.TextEditor.TextEditorControl _shaderTextEditor;
		ICSharpCode.TextEditor.TextEditorControl _csTextEditor;
		ICSharpCode.TextEditor.TextEditorControl _vbTextEditor;
		List<ShaderModelConstantRegister> _constantRegisters;
		ShaderEffect se2;

		private ShaderCompiler _compiler;
		public CodeTabView() {
			InitializeComponent();
			_shaderTextEditor = SetupEditor();
			_compiler = new ShaderCompiler();
			_compiler.Reset();
			outputTextBox.DataContext = _compiler;

			_csTextEditor = SetupEditor();
			_vbTextEditor = SetupEditor();

			_shaderTextEditor.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C #");
			this.formsHost.Child = _shaderTextEditor;

			_csTextEditor.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C #");
			this.formsHostCs.Child = _csTextEditor;
			this.formsHostVb.Child = _vbTextEditor;
			_vbTextEditor.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("VB");

			if (Properties.Settings.Default.LastFxFile != String.Empty)
			{
				FillEditControls(Properties.Settings.Default.LastFxFile, true);
			}
		}
		DispatcherTimer timer;
		DispatcherTimer animationTimer; // using timer instead of storyboard because there are issues with the custom slider

		private void ClosePopup(object o, EventArgs e) {
			this.messagePopup.IsOpen = false;
			timer.Stop();
		}
		private void AnimateSliders(object o, EventArgs e) {
			var aSlider =new  Shazzam.Controls.AdjustableSlider();
			
			foreach (var item in inputControlPanel.Children)
			{
				if (item.GetType()==typeof(Shazzam.Controls.AdjustableSlider))
				{
					aSlider = (Shazzam.Controls.AdjustableSlider)item;
				
					if (aSlider.Value >= aSlider.Maximum)
					{
						aSlider.IsForwardAnimation = false;
					}
					if (aSlider.Value <= aSlider.Minimum)
					{
						aSlider.IsForwardAnimation=true;
					}
					
					aSlider.Value += aSlider.IsForwardAnimation ? aSlider.AnimationValue : - aSlider.AnimationValue ;
				}
			}
		}
		public void CompileShader() {
			try
			{
				_compiler.Compile(this.CodeText);
				this.messagePopup.IsOpen = true;
				this.messagePopup.StaysOpen = false;
				timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 2000), DispatcherPriority.Background, ClosePopup, this.Dispatcher);
				timer.Start();
				animationTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Normal, AnimateSliders, this.Dispatcher);
				animationTimer.Start();
			}
			catch (CompilerException ex)
			{

				MessageBox.Show(ex.Message);
			}

		}

		private ICSharpCode.TextEditor.TextEditorControl SetupEditor() {

			var currentEditor = new ICSharpCode.TextEditor.TextEditorControl();
			currentEditor.ShowLineNumbers = true;
			currentEditor.ShowInvalidLines = false; // dont show error squiggle on empty lines
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

		public List<ShaderModelConstantRegister> GetShaderModelConstantRegisters(string fileName) {
			_constantRegisters = new List<ShaderModelConstantRegister>();
			foreach (var line in Shazzam.CodeGen.CodeParser.GetRegisterLines(fileName))
			{
				ShaderModelConstantRegister currentConstantRegister = new ShaderModelConstantRegister();

				currentConstantRegister = ShaderModelConstantRegister.Parse(line);

				_constantRegisters.Add(currentConstantRegister);

			}
			return _constantRegisters;
		}

		private void GenerateBlankInputControls() {
			var tb = new TextBlock();
			tb.Foreground = new SolidColorBrush(Colors.White);
			tb.Margin = new Thickness(5, 2, 5, 5);
			tb.FontSize = 20.0;
			tb.Text = "Current effect has no input parameters.";
			inputControlPanel.Children.Add(tb);
		}
		private void GenerateShaderInputControls(ShaderModelConstantRegister register) {

			var tb = new TextBlock();
			tb.Foreground = new SolidColorBrush(Colors.White);
			tb.Margin = new Thickness(5, 2, 5, 5);
			tb.Text = register.VariableName;
			DockPanel.SetDock(tb, Dock.Top);

			inputControlPanel.Children.Add(tb);
			//	Type x = typeof(System.Double);
			if (register.VariableType == typeof(System.Double))
			{
				tb.Text += " : (double)";

				var slider = new Shazzam.Controls.AdjustableSlider();
				slider.Margin = new Thickness(25, 2, 25, 5);
				slider.Minimum = 0;
				slider.Maximum = 1;
				register.AffliatedControl = slider;
				DockPanel.SetDock(slider, Dock.Top);
				inputControlPanel.Children.Add(slider);
				slider.ValueChanged += new RoutedEventHandler(slider_ValueChanged);
				//	slider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(slider_ValueChanged);
			}
			else if (register.VariableType == typeof(System.Windows.Point))
			{
				tb.Text += " : (point)";
				var pointTextBox = new TextBox();
				//	tb2.Text = ".5, .5";
				pointTextBox.Margin = new Thickness(25, 2, 25, 5);
				pointTextBox.MinWidth = 150;
				pointTextBox.HorizontalAlignment = HorizontalAlignment.Left;
				register.AffliatedControl = pointTextBox;
				DockPanel.SetDock(pointTextBox, Dock.Top);
				inputControlPanel.Children.Add(pointTextBox);
				pointTextBox.TextChanged += new TextChangedEventHandler(pointTextBox_TextChanged);
			}
			else if (register.VariableType == typeof(System.Windows.Media.Color))
			{
				tb.Text += " : (color)";

				var colorTextBox = new TextBox();
				colorTextBox.Background = new SolidColorBrush(Colors.LightYellow);
				colorTextBox.Margin = new Thickness(25, 2, 25, 5);
				colorTextBox.MinWidth = 150;
				colorTextBox.HorizontalAlignment = HorizontalAlignment.Left;

				register.AffliatedControl = colorTextBox;
				DockPanel.SetDock(colorTextBox, Dock.Top);
				inputControlPanel.Children.Add(colorTextBox);

				colorTextBox.TextChanged += new TextChangedEventHandler(colorTextBox_TextChanged);
			}

		}

		void slider_ValueChanged(object sender, RoutedEventArgs e) {
			//Console.WriteLine(e.NewValue);
			foreach (var item in _constantRegisters)
			{
				if (item.AffliatedControl == sender)
				{

					var slide = sender as Shazzam.Controls.AdjustableSlider;

					Type t = se2.GetType();

					var pi = t.GetProperty(item.VariableName);
					if (pi == null)
					{
						MessageBox.Show("Cannot parse the slider value.   Try compiling the Shader effect.");
						return;
					}
					pi.SetValue(se2, slide.Value, null);
				}
			}
		}

		public void FillEditControls(string fileName, Boolean loadShaderTextBox) {
			codeTab.Focus();
			if (loadShaderTextBox)
			{
				if (File.Exists(fileName))
				{
					_shaderTextEditor.LoadFile(fileName, false, false);
					codeTab.Header = System.IO.Path.GetFileName(fileName);
					Properties.Settings.Default.LastFxFile = fileName;
					Properties.Settings.Default.FolderFX = System.IO.Path.GetDirectoryName(fileName);
					Properties.Settings.Default.Save();
				}
				else
				{
					Properties.Settings.Default.LastFxFile = "";
					Properties.Settings.Default.FolderFX = "";
					Properties.Settings.Default.Save();
					return;
				}

				_shaderTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(".cs");



			}

			inputControlPanel.Children.Clear();
			_constantRegisters = GetShaderModelConstantRegisters(fileName);
			if (_constantRegisters.Count == 0)
			{
				GenerateBlankInputControls();
			}
			_constantRegisters.ForEach(GenerateShaderInputControls);

			_csTextEditor.Text = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("CSharp"), _constantRegisters);
			_csTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(".cs");
			_vbTextEditor.Text = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("VisualBasic"), _constantRegisters);
			_vbTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(".vb");

		}

		void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			Console.WriteLine(e.NewValue);
			foreach (var item in _constantRegisters)
			{
				if (item.AffliatedControl == sender)
				{

					Slider slide = sender as Slider;

					Type t = se2.GetType();

					var pi = t.GetProperty(item.VariableName);
					if (pi == null)
					{
						MessageBox.Show("Cannot parse the slider value.   Try compiling the Shader effect.");
						return;
					}
					pi.SetValue(se2, slide.Value, null);
				}
			}
		}
		void pointTextBox_TextChanged(object sender, TextChangedEventArgs e) {
			foreach (var item in _constantRegisters)
			{
				if (item.AffliatedControl == sender)
				{

					var textBox = sender as TextBox;

					Type t = se2.GetType();

					var pi = t.GetProperty(item.VariableName);
					try
					{
						Point p = Point.Parse(textBox.Text);
						pi.SetValue(se2, p, null);
					}
					catch (Exception ex)
					{
						//ignore error for now
					}
				}
			}
		}

		void colorTextBox_TextChanged(object sender, TextChangedEventArgs e) {
			foreach (var item in _constantRegisters)
			{
				if (item.AffliatedControl == sender)
				{

					var textBox = sender as TextBox;

					Type t = se2.GetType();

					var pi = t.GetProperty(item.VariableName);
					try
					{
						Color c = (Color)ColorConverter.ConvertFromString(textBox.Text);
						pi.SetValue(se2, c, null);
					}
					catch (Exception ex)
					{
						//ignore error for now
					}
				}
			}
		}
		public void EnableControls(Boolean isEnabled) {
			InputControlsTab.IsEnabled = isEnabled;
			vbTab.IsEnabled = isEnabled;
			csharpTab.IsEnabled = isEnabled;
		}
		public void RenderShader() {
			string filePath = System.IO.Path.GetTempFileName();
			_shaderTextEditor.SaveFile(filePath);
			FillEditControls(filePath, false);
			CompileShader();
			if (_compiler.IsCompiled == false)
			{

				return;
			}

			try
			{
				var ps = new PixelShader();
				EnableControls(false);
				string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Constants.Paths.GeneratedShaders;
				ps.UriSource = new Uri(path + Constants.FileNames.TempShaderPs);
				if (!File.Exists(path + Constants.FileNames.TempShaderPs))
				{
					return;
				}
				string codeString = Shazzam.CodeGen.CreatePixelShaderClass.GetSourceText(new CSharpCodeProvider(), _constantRegisters);
				Assembly autoAssembly = Shazzam.CodeGen.CreatePixelShaderClass.CompileInMemory(codeString);
				if (autoAssembly == null)
				{
					MessageBox.Show("Cannot create the WPF shader from the code snippet...");
					return;
				}
				Type t = autoAssembly.GetType("Shazzam.Shaders.AutoGenShaderEffect");

				se2 = (ShaderEffect)Activator.CreateInstance(t, new object[] { ps });

				this.CurrentShaderEffect = se2;
				OnShaderEffectChanged(null, null);
				EnableControls(true);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Cannot create the WPF shader from the code snippet...");
			}
		}

		#region Properties

		public String CodeText {
			get {

				return _shaderTextEditor.Text;
			}
			set {
				_shaderTextEditor.Text = value;
			}
		}
		public String OutputText {
			get {
				return outputTextBox.Text;
			}
			set {
				outputTextBox.Text = value;
			}
		}
		private ShaderEffect _currentShaderEffect;
		internal ShaderEffect CurrentShaderEffect {
			get {

				return _currentShaderEffect;
			}
			set {
				_currentShaderEffect = value;

			}
		}

		#endregion
		#region Events

		public event RoutedPropertyChangedEventHandler<object> ShaderEffectChanged {
			add {
				AddHandler(ShaderEffectChangedEvent, value);
			}

			remove {
				RemoveHandler(ShaderEffectChangedEvent, value);
			}
		}

		public static readonly RoutedEvent ShaderEffectChangedEvent = EventManager.RegisterRoutedEvent(
						"ShaderEffectChanged",
						RoutingStrategy.Bubble,
						typeof(RoutedPropertyChangedEventHandler<object>),
						typeof(CodeTabView)
				);
		protected virtual void OnShaderEffectChanged(object oldItem, object newItem) {

			RoutedPropertyChangedEventArgs<object> args =
					new RoutedPropertyChangedEventArgs<object>(newItem, newItem);
			args.RoutedEvent = CodeTabView.ShaderEffectChangedEvent;
			RaiseEvent(args);
		}

		#endregion

		public void SaveFile() {

			_shaderTextEditor.SaveFile(_shaderTextEditor.FileName);
		}
		public void SaveFile(string fileName) {

			_shaderTextEditor.SaveFile(fileName);
		}

		public void OpenCodeFile() {
			var ofd = new Microsoft.Win32.OpenFileDialog();
			ofd.Filter = "shaders|*.fx|All Files|*.*";

			if (Properties.Settings.Default.FolderFX != string.Empty)
			{
				ofd.InitialDirectory = Properties.Settings.Default.FolderFX;
			}
			if (ofd.ShowDialog() == true)
			{
				FillEditControls(ofd.FileName, true);
				RenderShader();
				OnShaderEffectChanged(null, null);

			}
		}
	}
}
