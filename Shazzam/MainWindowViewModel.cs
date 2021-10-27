namespace Shazzam
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using Shazzam.Commands;

    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        public static readonly MainWindowViewModel Instance = new();

        private System.Windows.Media.Stretch imageStretch = System.Windows.Media.Stretch.Uniform;
        private GridLength codeRowHeight = new(5, GridUnitType.Star);
        private GridLength imageRowHeight = new(5, GridUnitType.Star);

        private MainWindowViewModel()
        {
            this.FullScreenImageCommand = new RelayCommand(this.FullScreenImageCommandExecute);
            this.FullScreenCodeCommand = new RelayCommand(this.FullScreenCodeExecute);
            this.ImageStretchCommand = new RelayCommand<string>(this.ImageStretchExecute);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public RelayCommand ExploreCompiledShadersCommand { get; } = new RelayCommand(ExploreCompiledShadersCommandExecute);

        public RelayCommand ExploreTextureMapsCommand { get; } = new RelayCommand(ExploreTextureMapsCommandExecute);

        public RelayCommand<string> BrowseToSiteCommand { get; } = new RelayCommand<string>(BrowseToSiteExecute);

        public RelayCommand FullScreenImageCommand { get; }

        public RelayCommand FullScreenCodeCommand { get; }

        public RelayCommand<string> ImageStretchCommand { get; }

        public System.Windows.Media.Stretch ImageStretch
        {
            get => this.imageStretch;
            set
            {
                if (value == this.imageStretch)
                {
                    return;
                }

                this.imageStretch = value;
                this.OnPropertyChanged();
            }
        }

        public GridLength CodeRowHeight
        {
            get => this.codeRowHeight;

            set
            {
                if (value == this.codeRowHeight)
                {
                    return;
                }

                this.codeRowHeight = value;
                this.OnPropertyChanged();
            }
        }

        public GridLength ImageRowHeight
        {
            get => this.imageRowHeight;

            set
            {
                if (value == this.imageRowHeight)
                {
                    return;
                }

                this.imageRowHeight = value;
                this.OnPropertyChanged();
            }
        }

        private static void ExploreTextureMapsCommandExecute()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (Directory.Exists(path))
            {
                Process.Start(path + Constants.Paths.TextureMaps);
            }
        }

        private static void BrowseToSiteExecute(string url)
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

        private static void ExploreCompiledShadersCommandExecute()
        {
            var path = Properties.Settings.Default.FolderPath_Output;
            if (Directory.Exists(path))
            {
                Process.Start(path);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FullScreenImageCommandExecute()
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

        private void FullScreenCodeExecute()
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

        private void ImageStretchExecute(string menuParameter)
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
    }
}
