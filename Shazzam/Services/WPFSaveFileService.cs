namespace Cinch
{
    using System.Windows;
    using Microsoft.Win32;

  // learn more about Cinch at
  // http://sachabarber.net/?page_id=523

  /// <summary>
  /// This class implements the ISaveFileService for WPF purposes.
  /// </summary>
  public class WPFSaveFileService : ISaveFileService
  {
        /// <summary>
        /// Embedded SaveFileDialog to pass back correctly selected
        /// values to ViewModel
        /// </summary>
        private SaveFileDialog sfd = new SaveFileDialog();

        /// <summary>
        /// This method should show a window that allows a file to be selected
        /// </summary>
        /// <param name="owner">The owner window of the dialog</param>
        /// <returns>A bool from the ShowDialog call</returns>
        public bool? ShowDialog(Window owner)
    {
      // Set embedded SaveFileDialog.Filter
      if (!string.IsNullOrEmpty(this.Filter))
            {
                this.sfd.Filter = this.Filter;
            }

            // Set embedded SaveFileDialog.InitialDirectory
            if (!string.IsNullOrEmpty(this.InitialDirectory))
            {
                this.sfd.InitialDirectory = this.InitialDirectory;
            }

            // Set embedded SaveFileDialog.OverwritePrompt
            this.sfd.OverwritePrompt = this.OverwritePrompt;

      // return results
      return this.sfd.ShowDialog(owner);
    }

    /// <summary>
    /// FileName : Simply use embedded SaveFileDialog.FileName
    /// But DO NOT allow a Set as it will ONLY come from user
    /// picking a file
    /// </summary>
    public string FileName
    {
      get { return this.sfd.FileName; }

      set
      {
        // Do nothing
      }
    }

    /// <summary>
    /// Filter : Simply use embedded SaveFileDialog.Filter
    /// </summary>
    public string Filter
    {
      get { return this.sfd.Filter; }
      set { this.sfd.Filter = value; }
    }

    /// <summary>
    /// Filter : Simply use embedded SaveFileDialog.InitialDirectory
    /// </summary>
    public string InitialDirectory
    {
      get { return this.sfd.InitialDirectory; }
      set { this.sfd.InitialDirectory = value; }
    }

    /// <summary>
    /// OverwritePrompt : Simply use embedded SaveFileDialog.OverwritePrompt
    /// </summary>
    public bool OverwritePrompt
    {
      get { return this.sfd.OverwritePrompt; }
      set { this.sfd.OverwritePrompt = value; }
    }

    /// <summary>
    /// Title : Simply use embedded SaveFileDialog.Title
    /// </summary>
    public string Title
    {
      get { return this.sfd.Title; }
      set { this.sfd.Title = value; }
    }

    /// <summary>
    /// CheckFileExists : Simply use embedded SaveFileDialog.CheckFileExists
    /// </summary>
    public bool CheckFileExists
    {
      get { return this.sfd.CheckFileExists; }
      set { this.sfd.CheckFileExists = value; }
    }

    /// <summary>
    /// CheckPathExists : Simply use embedded SaveFileDialog.CheckPathExists
    /// </summary>
    public bool CheckPathExists
    {
      get { return this.sfd.CheckPathExists; }
      set { this.sfd.CheckPathExists = value; }
    }

    /// <summary>
    /// CreatePrompt : Simply use embedded SaveFileDialog.CheckPathExists
    /// </summary>
    public bool CreatePrompt
    {
      get { return this.sfd.CreatePrompt; }
      set { this.sfd.CreatePrompt = value; }
    }

    /// <summary>
    /// DefaultExt : Simply use embedded SaveFileDialog.DefaultExt
    /// </summary>
    public string DefaultExt
    {
      get { return this.sfd.DefaultExt; }
      set { this.sfd.DefaultExt = value; }
    }

    /// <summary>
    /// AddExtension : Simply use embedded SaveFileDialog.AddExtension
    /// </summary>
    public bool AddExtension
    {
      get { return this.sfd.AddExtension; }
      set { this.sfd.AddExtension = value; }
    }

    /// <summary>
    /// FilterIndex : Simply use embedded SaveFileDialog.FilterIndex
    /// </summary>
    public int FilterIndex
    {
      get { return this.sfd.FilterIndex; }
      set { this.sfd.FilterIndex = value; }
    }

    /// <summary>
    /// RestoreDirectory : Simply use embedded SaveFileDialog.RestoreDirectory
    /// </summary>
    public bool RestoreDirectory
    {
      get { return this.sfd.RestoreDirectory; }
      set { this.sfd.RestoreDirectory = value; }
    }

    /// <summary>
    /// SafeFileName : Simply use embedded SaveFileDialog.SafeFileName
    /// </summary>
    public string SafeFileName
    {
      get { return this.sfd.SafeFileName; }
    }

    /// <summary>
    /// SafeFileNames : Simply use embedded SaveFileDialog.SafeFileNames
    /// </summary>
    public string[] SafeFileNames
    {
      get { return this.sfd.SafeFileNames; }

      set
      {
        // Do nothing
      }
    }

    /// <summary>
    /// ValidateNames : Simply use embedded SaveFileDialog.ValidateNames
    /// </summary>
    public bool ValidateNames
    {
      get { return this.sfd.ValidateNames; }
      set { this.sfd.ValidateNames = value; }
    }
    }
}
