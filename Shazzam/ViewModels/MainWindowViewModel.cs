namespace Shazzam.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using Shazzam.Commands;

    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand ExploreCompiledShadersCommand { get; } = new RelayCommand(ExploreCompiledShadersCommand_Execute);

        private RelayCommand exploreTextureMapsCommand;

        public RelayCommand ExploreTextureMapsCommand
        {
            get
            {
                if (this.exploreTextureMapsCommand == null)
                {
                    this.exploreTextureMapsCommand = new RelayCommand(this.ExploreTextureMapsCommand_Execute);
                }

                return this.exploreTextureMapsCommand;
            }
        }

        private void ExploreTextureMapsCommand_Execute()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (Directory.Exists(path))
            {
                Process.Start(path + Constants.Paths.TextureMaps);
            }
        }

        private RelayCommand fullScreenImageCommand;

        public RelayCommand FullScreenImageCommand
        {
            get
            {
                if (this.fullScreenImageCommand == null)
                {
                    this.fullScreenImageCommand = new RelayCommand(this.FullScreenImageCommand_Execute);
                }

                return this.fullScreenImageCommand;
            }
        }

        private void FullScreenImageCommand_Execute()
        {
            if (this.CodeRowHeight != new GridLength(0, GridUnitType.Pixel))
            {
                this.CodeRowHeight = new GridLength(0, GridUnitType.Pixel);
                this.ImageRowHeight = new GridLength(5, GridUnitType.Star);
            }
            else
            {
                this.CodeRowHeight = new GridLength(5, GridUnitType.Star);
                this.ImageRowHeight = new GridLength(5, GridUnitType.Star);
            }
        }

        private RelayCommand fullScreenCodeCommand;

        public RelayCommand FullScreenCodeCommand
        {
            get
            {
                if (this.fullScreenCodeCommand == null)
                {
                    this.fullScreenCodeCommand = new RelayCommand(this.FullScreenCode_Execute);
                }

                return this.fullScreenCodeCommand;
            }
        }

        private void FullScreenCode_Execute()
        {
            if (this.ImageRowHeight != new GridLength(0, GridUnitType.Pixel))
            {
                this.ImageRowHeight = new GridLength(0, GridUnitType.Pixel);
                this.CodeRowHeight = new GridLength(5, GridUnitType.Star);
            }
            else
            {
                this.ImageRowHeight = new GridLength(5, GridUnitType.Star);
            }
        }

        private GridLength codeGridHeight = new GridLength(5, GridUnitType.Star);

        public GridLength CodeRowHeight
        {
            get => this.codeGridHeight;

            set
            {
                this.codeGridHeight = value;
                this.NotifyPropertyChanged(() => this.CodeRowHeight);
            }
        }

        private GridLength imageRowHeight = new GridLength(5, GridUnitType.Star);

        public GridLength ImageRowHeight
        {
            get => this.imageRowHeight;

            set
            {
                this.imageRowHeight = value;
                this.NotifyPropertyChanged(() => this.ImageRowHeight);
            }
        }

        private RelayCommand<string> imageStretchCommand;

        public RelayCommand<string> ImageStretchCommand
        {
            get
            {
                if (this.imageStretchCommand == null)
                {
                    this.imageStretchCommand = new RelayCommand<string>((param) => this.ImageStretch_Execute(param));
                }

                return this.imageStretchCommand;
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

        private System.Windows.Media.Stretch imageStretch = System.Windows.Media.Stretch.Uniform;

        public System.Windows.Media.Stretch ImageStretch
        {
            get => this.imageStretch;

            set
            {
                this.imageStretch = value;
                this.NotifyPropertyChanged(() => this.ImageStretch);
            }
        }

        private RelayCommand<string> browseToSiteCommand;

        public RelayCommand<string> BrowseToSiteCommand
        {
            get
            {
                if (this.browseToSiteCommand == null)
                {
                    this.browseToSiteCommand = new RelayCommand<string>((param) => BrowseToSite_Execute(param));
                }

                return this.browseToSiteCommand;
            }
        }

        private static void BrowseToSite_Execute(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                Console.WriteLine("Could not start process for " + url);
            }
        }

        private static void ExploreCompiledShadersCommand_Execute()
        {
            var path = Properties.Settings.Default.FolderPath_Output;
            if (Directory.Exists(path))
            {
                Process.Start(path);
            }
        }
    }
}
