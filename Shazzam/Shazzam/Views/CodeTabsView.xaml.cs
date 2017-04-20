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
    private int _dirtyCounter;
    private int _storedDocHash;

    public CodeTabView() {
      InitializeComponent();

      _shaderTextEditor = CreateTextEditor();
      _shaderTextEditor.Encoding = System.Text.Encoding.ASCII;
      //	_shaderTextEditor.TextChanged += new EventHandler(_shaderTextEditor_TextChanged);
      //	_shaderTextEditor.Document.TextContentChanged += new EventHandler(Document_TextContentChanged);
      _shaderTextEditor.Document.DocumentChanged += new DocumentEventHandler(Document_DocumentChanged);
      using (var stream = typeof(CodeTabView).Assembly.GetManifestResourceStream("Shazzam.Resources.HLSLSyntax.xshd")) {
        using (var reader = new XmlTextReader(stream)) {
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


    private void SetupInputBindings() {
      KeyBinding kb;

      RoutedUICommand ChangeToCodeTab = new RoutedUICommand("Change To Code Tab", "ChangeToCodeTab", typeof(CodeTabView));
      RoutedUICommand ChangeToEditTab = new RoutedUICommand("Change To Edit Tab", "ChangeToEditTab", typeof(CodeTabView));

      CommandBinding cb = new CommandBinding(ChangeToCodeTab, (s, e) => this.codeTabControl.SelectedItem = codeTab);
      kb = new KeyBinding(ChangeToCodeTab, Key.F9, ModifierKeys.Control);

      ShazzamSwitchboard.MainWindow.CommandBindings.Add(cb);
      //	this.CommandBindings.Add(cb);
      ShazzamSwitchboard.MainWindow.InputBindings.Add(kb);
      //	this.InputBindings.Add(kb);

      CommandBinding cb2 = new CommandBinding(ChangeToEditTab, (s, e) => this.codeTabControl.SelectedItem = InputControlsTab);
      kb = new KeyBinding(ChangeToEditTab, Key.F10, ModifierKeys.Control);



      ShazzamSwitchboard.MainWindow.CommandBindings.Add(cb2);
      ShazzamSwitchboard.MainWindow.InputBindings.Add(kb);
      //	this.CommandBindings.Add(cb2);
      //	this.InputBindings.Add(kb);

    }

    void Document_DocumentChanged(object sender, DocumentEventArgs e) {
      // this smells bad, but the TextEditor doesn't have a isDirty flag
      // the DocumentChanged event fires twice when a document is loaded.

      _dirtyCounter += 1;
      if (_dirtyCounter == 2) {
        _storedDocHash = _shaderTextEditor.Document.TextContent.GetHashCode();

      }

      if (_shaderTextEditor.Document.TextContent.GetHashCode() == _storedDocHash) {
        ShazzamSwitchboard.CodeTabView.dirtyStatusText.Visibility = Visibility.Collapsed;
      }
      else {
        ShazzamSwitchboard.CodeTabView.dirtyStatusText.Visibility = Visibility.Visible;
      }

    }

    void CodeTabView_Loaded(object sender, RoutedEventArgs e) {
      SetupBlurAnimation();
      SetupInputBindings();

    }

    private Storyboard blurStoryBoard = new Storyboard();
    private DoubleAnimation blurAnimation = new DoubleAnimation();
    private DoubleAnimation opacityAnimation = new DoubleAnimation();
    private BlurEffect blur = new BlurEffect { Radius = 0, RenderingBias = RenderingBias.Performance };
    public void SetupBlurAnimation() {
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
    public void CompileShader() {
      try {

        ShazzamSwitchboard.MainWindow.Effect = blur;
        versionNotSupported.Visibility = Visibility.Hidden;

        blurStoryBoard.Begin(this, true);

        if (Shazzam.Properties.Settings.Default.TargetFramework == "WPF_PS3") {
          if (RenderCapability.IsPixelShaderVersionSupported(3, 0)) {
            versionNotSupported.Visibility = Visibility.Hidden;
          }
          else {
            versionNotSupported.Visibility = Visibility.Visible;
          }
          _compiler.Compile(this.CodeText, ShaderProfile.PixelShader3);
          //_compiler.Compile(this.CodeText);
        }
        else {
          _compiler.Compile(this.CodeText, ShaderProfile.PixelShader2);
          //_compiler.Compile(this.CodeText);

        }


        compileStatusText.Text = String.Format("Last Compiled at: {0}", DateTime.Now.ToLongTimeString());

      }
      catch (CompilerException ex) {
        MessageBox.Show(ShazzamSwitchboard.MainWindow, ex.Message, "Could not compile", MessageBoxButton.OK, MessageBoxImage.Error);
      }


    }

    private ICSharpCode.TextEditor.TextEditorControl CreateTextEditor() {

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

    private void GenerateBlankInputControls() {
      TextBlock textBlock = new TextBlock
      {
        Foreground = Brushes.White,
        Margin = new Thickness(5),
        Text = "The current effect has no input parameters."
      };
      inputControlPanel.Children.Add(textBlock);
    }

    private void GenerateShaderInputControl(ShaderModelConstantRegister register) {
      string toolTipText = String.IsNullOrEmpty(register.Description) ? null : register.Description;

      TextBlock textBlock = new TextBlock
      {
        Foreground = Brushes.White,
        Margin = new Thickness(5),
        Inlines =
				{
					new Run { Foreground = (Brush)Application.Current.FindResource("HighlightBrush"), Text = register.RegisterName },
					new Run { Text = String.Format(" : {0}", register.RegisterType.Name) },
				},
        ToolTip = toolTipText
      };
      inputControlPanel.Children.Add(textBlock);

      Control control = null;
      if (register.RegisterType == typeof(Brush)) {
        control = new TexturePicker(register);

      }
      else if (register.RegisterType == typeof(double) || register.RegisterType == typeof(float)) {
        double minValue = Convert.ToDouble(register.MinValue);
        double maxValue = Convert.ToDouble(register.MaxValue);
        //double defaultValue = Double.Parse(register.DefaultValue.ToString(), NumberStyles.Any, null);
        double defaultValue = Convert.ToDouble(register.DefaultValue);
        control = new AdjustableSlider
        {
          Minimum = Math.Min(minValue, defaultValue),
          Maximum = Math.Max(maxValue, defaultValue),
          Value = defaultValue
        };
      }
      else if (register.RegisterType == typeof(Point) || register.RegisterType == typeof(Vector) || register.RegisterType == typeof(Size)) {
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
      else if (register.RegisterType == typeof(Point3D) || register.RegisterType == typeof(Vector3D)) {
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
      else if (register.RegisterType == typeof(Color)) {
        Color defaultValue = (Color)register.DefaultValue;
        //control = new Telerik.Windows.Controls.RadColorEditor
        //{

        //  HorizontalAlignment = HorizontalAlignment.Left,
        //  SelectedColor = defaultValue
        //};

        control = new AdjustableColor
        {

          HorizontalAlignment = HorizontalAlignment.Left,
          Value = defaultValue
        };
        //  ((control) as AdjustableColor).;
        //control = new TextBox
        //{
        //  Background = Brushes.LightYellow,
        //  Width = 150,
        //  HorizontalAlignment = HorizontalAlignment.Left,
        //  Text = defaultValue.ToString()
        //};
      }
      else if (register.RegisterType == typeof(Point4D)) {
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

      if (control != null) {
        control.Margin = new Thickness(15, 2, 25, 5);
        control.ToolTip = toolTipText;
        this.inputControlPanel.Children.Add(control);
        register.AffiliatedControl = control;
      }
    }

    private void BindShaderEffectToControls() {
      ShaderEffect shaderEffect = this.CurrentShaderEffect;
      if (shaderEffect != null) {
        foreach (ShaderModelConstantRegister register in this._shaderModel.Registers) {
          if (register.AffiliatedControl != null) {
            FieldInfo fieldInfo = shaderEffect.GetType().GetField(String.Format("{0}Property", register.RegisterName), BindingFlags.Public | BindingFlags.Static);
            if (fieldInfo != null) {
              DependencyProperty dependencyProperty = fieldInfo.GetValue(null) as DependencyProperty;
              if (dependencyProperty != null) {
                string controlPropertyName = "Value";
                
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
    private void FillEditControls() {
      inputControlPanel.Children.Clear();
      if (_shaderModel.Registers.Count == 0) {
        GenerateBlankInputControls();
      }
      _shaderModel.Registers.ForEach(GenerateShaderInputControl);

      _csTextEditor.Text = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("CSharp"), _shaderModel, false);
      _csTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(".cs");

      _vbTextEditor.Text = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("VisualBasic"), _shaderModel, false);
      _vbTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(".vb");

    }

    public void RenderShader() {
      CompileShader();
      if (_compiler.IsCompiled == false) {
        return;
      }

      try {
        this.InputControlsTab.IsEnabled = false;
        string path = Properties.Settings.Default.FolderPath_Output;
        if (!File.Exists(path + Constants.FileNames.TempShaderPs)) {
          return;
        }
        var ps = new PixelShader { UriSource = new Uri(path + Constants.FileNames.TempShaderPs) };

        this._shaderModel = Shazzam.CodeGen.CodeParser.ParseShader(this._shaderTextEditor.FileName, this.CodeText);
        string codeString = CreatePixelShaderClass.GetSourceText(new CSharpCodeProvider(), this._shaderModel, true);
        Assembly autoAssembly = CreatePixelShaderClass.CompileInMemory(codeString);

        if (autoAssembly == null) {
          MessageBox.Show(ShazzamSwitchboard.MainWindow, "Cannot compile the generated C# code.", "Compile error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }

        Type t = autoAssembly.GetType(String.Format("{0}.{1}", _shaderModel.GeneratedNamespace, _shaderModel.GeneratedClassName));
        this.FillEditControls();
        var outputFolder = String.Format("{0}{1}",
          Properties.Settings.Default.FolderPath_Output, _shaderModel.GeneratedClassName);
  
        if (!Directory.Exists(outputFolder)) {
          Directory.CreateDirectory(outputFolder);
        }
        

        _csTextEditor.SaveFile(String.Format("{0}\\{1}.cs", outputFolder, _shaderModel.GeneratedClassName));
        _vbTextEditor.SaveFile(String.Format("{0}\\{1}.vb", outputFolder, _shaderModel.GeneratedClassName));

        CreateFileCopies(outputFolder + @"\", _shaderModel.GeneratedClassName);
        this.CurrentShaderEffect = (ShaderEffect)Activator.CreateInstance(t, new object[] { ps });
        this.InputControlsTab.IsEnabled = true;
      }
      catch (Exception) {
        MessageBox.Show(ShazzamSwitchboard.MainWindow, "Cannot create a WPF shader from the code snippet.", "Compile error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
    private void CreateFileCopies(string path, string newFileName) {
      if (String.IsNullOrEmpty(Properties.Settings.Default.FilePath_LastFx)) {
        return;
      }
      string currentFileName = System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.FilePath_LastFx);
      if (File.Exists(Properties.Settings.Default.FolderPath_Output + Constants.FileNames.TempShaderPs)) {

        File.Copy(Properties.Settings.Default.FolderPath_Output + Constants.FileNames.TempShaderPs, path + newFileName + ".ps", true);
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
    internal ShaderEffect CurrentShaderEffect {
      get {
        return _currentShaderEffect;
      }
      private set {
        if (this._currentShaderEffect != value) {
          ShaderEffect oldShaderEffect = this._currentShaderEffect;
          this._currentShaderEffect = value;
          this.BindShaderEffectToControls();
          this.OnShaderEffectChanged(oldShaderEffect, this._currentShaderEffect);
        }
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

    private void OutputTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
      if (e.ClickCount == 2) {
        Match match = Regex.Match(this.OutputText, @"\((?<lineNumber>\d+),(?<charNumber>\d+)\)");
        if (match.Success) {
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

    public void SaveFile() {
      ResetDirty();
      //_dirtyCounter = 2;
      _storedDocHash = _shaderTextEditor.Document.TextContent.GetHashCode();
      _shaderTextEditor.SaveFile(_shaderTextEditor.FileName);
    }

    public void SaveFile(string fileName) {
      ResetDirty();
      //	_dirtyCounter = 2;
      _storedDocHash = _shaderTextEditor.Document.TextContent.GetHashCode();
      _shaderTextEditor.SaveFile(fileName);
      this.codeTabHeaderText.Text = Path.GetFileName(fileName);
    }

    public void OpenFile(string fileName) {
      SaveFileFirst();
      ResetDirty();
      _dirtyCounter = 0;
      this.codeTab.Focus();
      this.codeTabHeaderText.Text = Path.GetFileName(fileName);
      this._shaderTextEditor.LoadFile(fileName);
      this._shaderTextEditor.Document.HighlightingStrategy = _hlslHS;
      Shazzam.Properties.Settings.Default.FilePath_LastFx = fileName;
      Shazzam.Properties.Settings.Default.Save();
      this.RenderShader();
    }

    public void SaveFileFirst() {
      if (dirtyStatusText.Visibility == Visibility.Visible) {
        string message = "The fx file has unsaved changes.  Would you like to save your work?";
        if (MessageBox.Show(message, "Save file", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
          SaveFile();
        }
      }

    }
    private void ResetDirty() {

      _storedDocHash = 0;
      dirtyStatusText.Visibility = Visibility.Collapsed;
    }
  }
}
