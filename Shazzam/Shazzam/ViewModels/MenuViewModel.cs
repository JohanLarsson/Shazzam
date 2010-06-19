using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shazzam.Commands;
using System.Diagnostics;

namespace Shazzam.ViewModels
{
  internal class MenuViewModel : ViewModels.ViewModelBase
  {


    //#region ImageStretch
    //private RelayCommand _imageStretchCommand;
    //public RelayCommand ImageStretchCommand
    //{
    //  get
    //  {

    //    if (_imageStretchCommand == null)
    //    {
    //      _imageStretchCommand = new RelayCommand(ImageStretch);
    //    }
    //  }
    //}
    //private void ImageStretch()
    //{

    //}

    //#endregion


    #region BrowseToBlog
    private RelayCommand _browseToBlogCommand;
    public RelayCommand BrowseToBlogCommand
    {
      get
      {

        if (_browseToBlogCommand == null)
        {
          _browseToBlogCommand = new RelayCommand(BrowseToBlog_Callback);
        }
        return _browseToBlogCommand;
      }
    }
    private void BrowseToBlog_Callback()
    {
      try
      {
        Process.Start(Constants.Urls.ShazzamBlog);
      }
      catch
      {
        Console.WriteLine("Could not start process for " + Constants.Urls.ShazzamBlog);
      }
    }

    #endregion

    #region ShowForums
    private RelayCommand _showForumsCommand;
    public RelayCommand ShowForumsCommand
    {
      get
      {

        if (_showForumsCommand == null)
        {
          _showForumsCommand = new RelayCommand(ShowForums_Callback);
        }
        return _showForumsCommand;
      }
    }
    private void ShowForums_Callback()
    {
      try
      {
        Process.Start(Constants.Urls.ShazzamForums);
      }
      catch
      {
        Console.WriteLine("Could not start process for " + Constants.Urls.ShazzamForums);
      }
    }

    #endregion
    #region ShowTwitter
    private RelayCommand _showTwitterCommand;
    public RelayCommand ShowTwitterCommand
    {
      get
      {

        if (_showTwitterCommand == null)
        {
          _showTwitterCommand = new RelayCommand(ShowTwitter_Callback);
        }
        return _showTwitterCommand;
      }
    }
    private void ShowTwitter_Callback()
    {
      try
      {
        Process.Start(Constants.Urls.Twitter);
      }
      catch
      {
        Console.WriteLine("Could not start process for " + Constants.Urls.Twitter);
      }
    }

    #endregion

    #region BrowseToReportBug
    private RelayCommand _browseToReportBugCommand;
    public RelayCommand BrowseToReportBugCommand
    {
      get
      {

        if (_browseToReportBugCommand == null)
        {
          _browseToReportBugCommand = new RelayCommand(BrowseToReportBug_Callback);
        }
        return _browseToReportBugCommand;
      }
    }
    private void BrowseToReportBug_Callback()
    {
      try
      {
        Process.Start(Constants.Urls.ShazzamBug);
      }
      catch
      {
        Console.WriteLine("Could not start process for " + Constants.Urls.ShazzamBug);
      }
    }

    #endregion

  }
}
