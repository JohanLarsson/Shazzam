namespace KaxamlPlugins
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    public class KaxamlInfo
    {
        private static IKaxamlInfoTextEditor? editor;
        private static Window? mainWindow;
        private static Frame? frame;

        public delegate void EditSelectionChangedDelegate(string selectedText);

        public delegate void ParseRequestedDelegate();

        public delegate void ContentLoadedDelegate();

        public static event EditSelectionChangedDelegate? EditSelectionChanged;

        public static event ParseRequestedDelegate? ParseRequested;

        public static event ContentLoadedDelegate? ContentLoaded;

        public static event PropertyChangedEventHandler? PropertyChanged;

        public static IKaxamlInfoTextEditor? Editor
        {
            get => editor;
            set
            {
                // remove current event handler
                if (editor != null)
                {
                    editor.TextSelectionChanged -= EditorTextSelectionChanged;
                }

                editor = value;

                // add new event handler
                if (editor != null)
                {
                    editor.TextSelectionChanged += EditorTextSelectionChanged;
                }
            }
        }

        public static Window? MainWindow
        {
            get => mainWindow;
            set
            {
                mainWindow = value;
                NotifyPropertyChanged();
            }
        }

        public static Frame? Frame
        {
            get => frame;
            set
            {
                if (!ReferenceEquals(frame, value))
                {
                    frame = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static void Parse()
        {
            ParseRequested?.Invoke();
        }

        public static void RaiseContentLoaded()
        {
            ContentLoaded?.Invoke();
        }

        protected static void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string info = null)
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(info));
        }

        private static void EditorTextSelectionChanged(object sender, RoutedEventArgs e)
        {
            EditSelectionChanged?.Invoke(editor.SelectedText);
        }
    }
}
