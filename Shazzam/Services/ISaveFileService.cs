using System;
using System.Windows;


namespace Cinch
{
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
    Boolean CheckFileExists { get; set; }

    /// <summary>
    /// CheckPathExists
    /// </summary>
    Boolean CheckPathExists { get; set; }

    /// <summary>
    /// DefaultExt
    /// </summary>
    String DefaultExt { get; set; }


    /// <summary>
    /// CreatePrompt
    /// </summary>
    Boolean CreatePrompt { get; set; }


    /// <summary>
    /// FileName
    /// </summary>
    String FileName { get; set; }



    /// <summary>
    /// Filter
    /// </summary>
    String Filter { get; set; }

    /// <summary>
    /// FilterIndex
    /// </summary>
    Int32 FilterIndex { get; set; }

    /// <summary>
    /// InitialDirectory
    /// </summary>
    String InitialDirectory { get; set; }




    /// <summary>
    /// RestoreDirectory
    /// </summary>
    bool RestoreDirectory { get; }

    /// <summary>
    /// SafeFileName
    /// </summary>
    String SafeFileName { get; }

    /// <summary>
    /// SafeFileNames
    /// </summary>
    String[] SafeFileNames { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    string Title { get; set; }


    /// <summary>
    /// OverwritePrompt
    /// </summary>
    Boolean OverwritePrompt { get; set; }



    /// <summary>
    /// ValidateNames
    /// </summary>
    Boolean ValidateNames { get; set; }

    /// <summary>
    /// This method should show a window that allows a file to be saved
    /// </summary>
    /// <param name="owner">The owner window of the dialog</param>
    /// <returns>A bool from the ShowDialog call</returns>
    bool? ShowDialog(Window owner);
  }
}
