using System;
using Shazzam.Commands;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Reflection;

namespace Shazzam.ViewModels
{
  public class MainWindowViewModel : ViewModels.ViewModelBase
  {
    #region NewShader
    //private RelayCommand _newShaderCommand;
    //public RelayCommand NewShaderCommand
    //{
    //  get
    //  {

    //    if (_newShaderCommand == null)
    //    {
    //      _newShaderCommand = new RelayCommand(NewShaderCommand_Execute);
    //    }
    //    return _newShaderCommand;
    //  }
    //}
    //private void NewShaderCommand_Execute()
    //{
    //  var dialog = new Cinch.WPFSaveFileService();
    //  dialog.Title = "New File Name";
    //  if (Properties.Settings.Default.FolderPath_FX != string.Empty)
    //  {
    //    dialog.InitialDirectory = Properties.Settings.Default.FolderPath_FX;
    //  }
    //  dialog.CheckPathExists = true;
    //  dialog.CreatePrompt = true;
    //  dialog.Filter = "Shader File (*.fx) |*.fx";
    //  if (dialog.ShowDialog(null) == true)
    //  {
    //    if (!IsValidFileName(dialog.SafeFileName))
    //    { return; }
    //    FileStream temp = new FileStream(dialog.FileName, FileMode.Create, FileAccess.ReadWrite);
    //    StreamWriter writer = new StreamWriter(temp);
    //    writer.Write(Properties.Resources.NewShaderText);
    //    writer.Close();
    //    LoadShaderEditor(dialog);
    //  }

    //}

    #endregion


    #region ExploreCompiledShaders
    private RelayCommand _exploreCompiledShadersCommand;
    public RelayCommand ExploreCompiledShadersCommand
    {
      get
      {

        if (_exploreCompiledShadersCommand == null)
        {
          _exploreCompiledShadersCommand = new RelayCommand(ExploreCompiledShadersCommand_Execute);
        }
        return _exploreCompiledShadersCommand;
      }
    }
    private void ExploreCompiledShadersCommand_Execute()
    {
      string path = Properties.Settings.Default.FolderPath_Output;
      if (System.IO.Directory.Exists(path))
      {
        System.Diagnostics.Process.Start(path);
      }

    }

    #endregion

    #region ExploreTextureMaps
    private RelayCommand _exploreTextureMapsCommand;
    public RelayCommand ExploreTextureMapsCommand
    {
      get
      {

        if (_exploreTextureMapsCommand == null)
        {
          _exploreTextureMapsCommand = new RelayCommand(ExploreTextureMapsCommand_Execute);
        }
        return _exploreTextureMapsCommand;
      }
    }
    private void ExploreTextureMapsCommand_Execute()
    {
      string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      if (System.IO.Directory.Exists(path))
      {
        System.Diagnostics.Process.Start(path + Constants.Paths.TextureMaps);
      }

    }

    #endregion


    #region FullScreenImage
    private RelayCommand _fullScreenImageCommand;
    public RelayCommand FullScreenImageCommand
    {
      get
      {

        if (_fullScreenImageCommand == null)
        {
          _fullScreenImageCommand = new RelayCommand(FullScreenImageCommand_Execute);
        }
        return _fullScreenImageCommand;
      }
    }
    private void FullScreenImageCommand_Execute()
    {
      if (CodeRowHeight != new GridLength(0, GridUnitType.Pixel))
      {

        CodeRowHeight = new GridLength(0, GridUnitType.Pixel);
        ImageRowHeight = new GridLength(5, GridUnitType.Star);
      }
      else
      {

        CodeRowHeight = new GridLength(5, GridUnitType.Star);
        ImageRowHeight = new GridLength(5, GridUnitType.Star);
      }
    }

    #endregion


    #region FullScreenCode
    private RelayCommand _fullScreenCodeCommand;
    public RelayCommand FullScreenCodeCommand
    {
      get
      {

        if (_fullScreenCodeCommand == null)
        {
          _fullScreenCodeCommand = new RelayCommand(FullScreenCode_Execute);
        }
        return _fullScreenCodeCommand;
      }
    }
    private void FullScreenCode_Execute()
    {
      if (ImageRowHeight != new GridLength(0, GridUnitType.Pixel))
      {
        ImageRowHeight = new GridLength(0, GridUnitType.Pixel);
        CodeRowHeight = new GridLength(5, GridUnitType.Star);
      }
      else
      {
        ImageRowHeight = new GridLength(5, GridUnitType.Star);

      }
    }

    #endregion

    private GridLength _codeGridHeight = new GridLength(5, GridUnitType.Star);
    public GridLength CodeRowHeight
    {
      get
      {
        return _codeGridHeight;
      }
      set
      {
        _codeGridHeight = value;
        NotifyPropertyChanged(() => this.CodeRowHeight);
      }
    }

    private GridLength _imageRowHeight = new GridLength(5, GridUnitType.Star);
    public GridLength ImageRowHeight
    {
      get
      {
        return _imageRowHeight;
      }
      set
      {
        _imageRowHeight = value;
        NotifyPropertyChanged(() => this.ImageRowHeight);
      }
    }

    #region ImageStretch
    private RelayCommand<String> _imageStretchCommand;
    public RelayCommand<string> ImageStretchCommand
    {
      get
      {

        if (_imageStretchCommand == null)
        {
          _imageStretchCommand = new RelayCommand<String>((param) => this.ImageStretch_Execute(param));
        }
        return _imageStretchCommand;
      }
    }
    private void ImageStretch_Execute(string menuParameter)
    {
      switch (menuParameter)
      {
        case "none":
          this.ImageStretch = System.Windows.Media.Stretch.None;
          break;
        case "fill":
          this.ImageStretch = System.Windows.Media.Stretch.Fill;
          break;
        case "uniform":
          this.ImageStretch = System.Windows.Media.Stretch.Uniform;
          break;
        case "uniformtofill":
          this.ImageStretch = System.Windows.Media.Stretch.UniformToFill;
          break;
        default:
          this.ImageStretch = System.Windows.Media.Stretch.Uniform;

          break;
      }
    }

    #endregion
    private System.Windows.Media.Stretch _imageStretch = System.Windows.Media.Stretch.Uniform;
    public System.Windows.Media.Stretch ImageStretch
    {
      get
      {
        return _imageStretch;
      }
      set
      {
        _imageStretch = value;
        NotifyPropertyChanged(() => this.ImageStretch);
      }
    }

    private RelayCommand<string> _browseToSiteCommand;
    public RelayCommand<string> BrowseToSiteCommand {
      get {

        if (_browseToSiteCommand == null) {
          _browseToSiteCommand = new RelayCommand<string>((param) => BrowseToSite_Execute(param));
        }
        return _browseToSiteCommand;
      }
    }

       public void BrowseToSite_Execute (string url)
       {
         try {
           Process.Start(url);
         }
         catch {
           Console.WriteLine("Could not start process for " + url);
         }
       }

   

  }
}
