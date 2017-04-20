namespace Cinch
{
    using System.Windows;
    using Microsoft.Win32;

    /// <summary>
    /// This class implements the IOpenFileService for WPF purposes.
    /// </summary>
    public class WPFOpenFileService : IOpenFileService
    {
        /// <summary>
        /// Embedded OpenFileDialog to pass back correctly selected
        /// values to ViewModel
        /// </summary>
        private OpenFileDialog ofd = new OpenFileDialog();

        /// <summary>
        /// This method should show a window that allows a file to be selected
        /// </summary>
        /// <param name="owner">The owner window of the dialog</param>
        /// <returns>A bool from the ShowDialog call</returns>
        public bool? ShowDialog(Window owner)
        {
            // Set embedded OpenFileDialog.Filter
            if (!string.IsNullOrEmpty(this.Filter))
            {
                this.ofd.Filter = this.Filter;
            }

            // Set embedded OpenFileDialog.InitialDirectory
            if (!string.IsNullOrEmpty(this.InitialDirectory))
            {
                this.ofd.InitialDirectory = this.InitialDirectory;
            }

            // return results
            return this.ofd.ShowDialog(owner);
        }

        /// <summary>
        /// FileName : Simply use embedded OpenFileDialog.FileName
        /// But DO NOT allow a Set as it will ONLY come from user
        /// picking a file
        /// </summary>
        public string FileName
        {
            get { return this.ofd.FileName; }

            set
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Filter : Simply use embedded OpenFileDialog.Filter
        /// </summary>
        public string Filter
        {
            get { return this.ofd.Filter; }
            set { this.ofd.Filter = value; }
        }

        /// <summary>
        /// Filter : Simply use embedded OpenFileDialog.InitialDirectory
        /// </summary>
        public string InitialDirectory
        {
            get { return this.ofd.InitialDirectory; }
            set { this.ofd.InitialDirectory = value; }
        }
    }
}
