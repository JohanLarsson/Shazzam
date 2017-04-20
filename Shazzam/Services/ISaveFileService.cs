namespace Cinch
{
    using System.Windows;

  /// <summary>
  /// This interface defines a interface that will allow
  /// a ViewModel to save a file
  /// </summary>
  public interface ISaveFileService
  {
    /// <summary>
    /// AddExtension
    /// </summary>
    bool AddExtension { get; set; }

    /// <summary>
    /// CheckFileExists
    /// </summary>
    bool CheckFileExists { get; set; }

    /// <summary>
    /// CheckPathExists
    /// </summary>
    bool CheckPathExists { get; set; }

    /// <summary>
    /// DefaultExt
    /// </summary>
    string DefaultExt { get; set; }

    /// <summary>
    /// CreatePrompt
    /// </summary>
    bool CreatePrompt { get; set; }

    /// <summary>
    /// FileName
    /// </summary>
    string FileName { get; set; }

    /// <summary>
    /// Filter
    /// </summary>
    string Filter { get; set; }

    /// <summary>
    /// FilterIndex
    /// </summary>
    int FilterIndex { get; set; }

    /// <summary>
    /// InitialDirectory
    /// </summary>
    string InitialDirectory { get; set; }

    /// <summary>
    /// RestoreDirectory
    /// </summary>
    bool RestoreDirectory { get; }

    /// <summary>
    /// SafeFileName
    /// </summary>
    string SafeFileName { get; }

    /// <summary>
    /// SafeFileNames
    /// </summary>
    string[] SafeFileNames { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// OverwritePrompt
    /// </summary>
    bool OverwritePrompt { get; set; }

    /// <summary>
    /// ValidateNames
    /// </summary>
    bool ValidateNames { get; set; }

    /// <summary>
    /// This method should show a window that allows a file to be saved
    /// </summary>
    /// <param name="owner">The owner window of the dialog</param>
    /// <returns>A bool from the ShowDialog call</returns>
    bool? ShowDialog(Window owner);
  }
}
