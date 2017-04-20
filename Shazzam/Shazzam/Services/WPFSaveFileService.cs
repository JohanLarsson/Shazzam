using System;
using System.Windows;
using Microsoft.Win32;


namespace Cinch
{
  // learn more about Cinch at
  // http://sachabarber.net/?page_id=523 

  /// <summary>
  /// This class implements the ISaveFileService for WPF purposes.
  /// </summary>
  public class WPFSaveFileService : ISaveFileService
  {
    #region Data

    /// <summary>
    /// Embedded SaveFileDialog to pass back correctly selected
    /// values to ViewModel
    /// </summary>
    private SaveFileDialog sfd = new SaveFileDialog();

    #endregion

    #region ISaveFileService Members
    /// <summary>
    /// This method should show a window that allows a file to be selected
    /// </summary>
    /// <param name="owner">The owner window of the dialog</param>
    /// <returns>A bool from the ShowDialog call</returns>
    public bool? ShowDialog(Window owner)
    {
      //Set embedded SaveFileDialog.Filter
      if (!String.IsNullOrEmpty(this.Filter))
        sfd.Filter = this.Filter;

      //Set embedded SaveFileDialog.InitialDirectory
      if (!String.IsNullOrEmpty(this.InitialDirectory))
        sfd.InitialDirectory = this.InitialDirectory;

      //Set embedded SaveFileDialog.OverwritePrompt
      sfd.OverwritePrompt = this.OverwritePrompt;

      //return results
      return sfd.ShowDialog(owner);
    }


    /// <summary>
    /// FileName : Simply use embedded SaveFileDialog.FileName
    /// But DO NOT allow a Set as it will ONLY come from user
    /// picking a file
    /// </summary>
    public string FileName
    {
      get { return sfd.FileName; }
      set
      {
        //Do nothing
      }
    }

    /// <summary>
    /// Filter : Simply use embedded SaveFileDialog.Filter
    /// </summary>
    public string Filter
    {
      get { return sfd.Filter; }
      set { sfd.Filter = value; }
    }

    /// <summary>
    /// Filter : Simply use embedded SaveFileDialog.InitialDirectory
    /// </summary>
    public string InitialDirectory
    {
      get { return sfd.InitialDirectory; }
      set { sfd.InitialDirectory = value; }
    }

    /// <summary>
    /// OverwritePrompt : Simply use embedded SaveFileDialog.OverwritePrompt
    /// </summary>
    public bool OverwritePrompt
    {
      get { return sfd.OverwritePrompt; }
      set { sfd.OverwritePrompt = value; }
    }

    /// <summary>
    /// Title : Simply use embedded SaveFileDialog.Title
    /// </summary>
    public string Title
    {
      get { return sfd.Title; }
      set { sfd.Title = value; }
    }

    /// <summary>
    /// CheckFileExists : Simply use embedded SaveFileDialog.CheckFileExists
    /// </summary>
    public Boolean CheckFileExists
    {
      get { return sfd.CheckFileExists; }
      set { sfd.CheckFileExists = value; }
    }

    /// <summary>
    /// CheckPathExists : Simply use embedded SaveFileDialog.CheckPathExists
    /// </summary>
    public Boolean CheckPathExists
    {
      get { return sfd.CheckPathExists; }
      set { sfd.CheckPathExists = value; }
    }

    /// <summary>
    /// CreatePrompt : Simply use embedded SaveFileDialog.CheckPathExists
    /// </summary>
    public Boolean CreatePrompt
    {
      get { return sfd.CreatePrompt; }
      set { sfd.CreatePrompt = value; }
    }

    /// <summary>
    /// DefaultExt : Simply use embedded SaveFileDialog.DefaultExt
    /// </summary>
    public String DefaultExt
    {
      get { return sfd.DefaultExt; }
      set { sfd.DefaultExt = value; }
    }

    /// <summary>
    /// AddExtension : Simply use embedded SaveFileDialog.AddExtension
    /// </summary>
    public Boolean AddExtension
    {
      get { return sfd.AddExtension; }
      set { sfd.AddExtension = value; }
    }

    /// <summary>
    /// FilterIndex : Simply use embedded SaveFileDialog.FilterIndex
    /// </summary>
    public Int32 FilterIndex
    {
      get { return sfd.FilterIndex; }
      set { sfd.FilterIndex = value; }
    }

    /// <summary>
    /// RestoreDirectory : Simply use embedded SaveFileDialog.RestoreDirectory
    /// </summary>
    public Boolean RestoreDirectory
    {
      get { return sfd.RestoreDirectory; }
      set { sfd.RestoreDirectory = value; }
    }

    /// <summary>
    /// SafeFileName : Simply use embedded SaveFileDialog.SafeFileName
    /// </summary>
    public String SafeFileName
    {
      get { return sfd.SafeFileName; }

    }

    /// <summary>
    /// SafeFileNames : Simply use embedded SaveFileDialog.SafeFileNames
    /// </summary>
    public String[] SafeFileNames
    {
      get { return sfd.SafeFileNames; }
      set
      {
        //Do nothing
      }
    }

    /// <summary>
    /// ValidateNames : Simply use embedded SaveFileDialog.ValidateNames
    /// </summary>
    public Boolean ValidateNames
    {
      get { return sfd.ValidateNames; }
      set { sfd.ValidateNames = value; }
    }
    #endregion
  }
}
