namespace Shazzam.Plugins
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using KaxamlPlugins;
    using KaxamlPlugins.Controls;

    public partial class ColorPickerPlugin : UserControl
    {
        /// <summary>
        /// DependencyProperty for Colors
        /// </summary>
        public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register(
            nameof(Colors),
            typeof(ObservableCollection<Color>),
            typeof(ColorPickerPlugin),
            new FrameworkPropertyMetadata(default(ObservableCollection<Color>), OnColorsChanged));

        /// <summary>
        /// DependencyProperty for ColorString
        /// </summary>
        public static readonly DependencyProperty ColorStringProperty = DependencyProperty.Register(
            nameof(ColorString),
            typeof(string),
            typeof(ColorPickerPlugin),
            new FrameworkPropertyMetadata(default(string), OnColorStringChanged));

        private const char Delimiter = '|';
        private bool updateInternal;
        private DispatcherTimer colorChangedTimer;
        private Color colorChangedColor;

        public ColorPickerPlugin()
        {
            this.InitializeComponent();
            this.Colors = new ObservableCollection<Color>();
            this.ColorString = Shazzam.Properties.Settings.Default.ColorPickerColors;

            KaxamlInfo.EditSelectionChanged += this.KaxamlInfoEditSelectionChanged;
        }

        /// <summary>
        /// description of the property
        /// </summary>
        public ObservableCollection<Color> Colors
        {
            get => (ObservableCollection<Color>)this.GetValue(ColorsProperty);
            set => this.SetValue(ColorsProperty, value);
        }

        /// <summary>
        /// description of the property
        /// </summary>
        public string ColorString
        {
            get => (string)this.GetValue(ColorStringProperty);
            set => this.SetValue(ColorStringProperty, value);
        }

        private static void OnColorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ColorPickerPlugin owner)
            {
                if (args.NewValue is ObservableCollection<Color> c)
                {
                    c.CollectionChanged += owner.CCollectionChanged;
                }
            }
        }

        private static void OnColorStringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ColorPickerPlugin plugin)
            {
                var owner = plugin;
                Shazzam.Properties.Settings.Default.ColorPickerColors = args.NewValue as string;

                if (!owner.updateInternal)
                {
                    owner.updateInternal = true;

                    owner.Colors.Clear();
                    var colors = ((string)args.NewValue).Split(Delimiter);

                    foreach (var s in colors)
                    {
                        try
                        {
                            if (s.Length > 3)
                            {
                                var c = ColorPickerUtil.ColorFromString(s);
                                owner.Colors.Add(c);
                            }
                        }
                        catch
                        {
                        }
                    }

                    owner.updateInternal = false;
                }
            }
        }

        private void KaxamlInfoEditSelectionChanged(string selectedText)
        {
            // wish we could do this without a try catch!
            try
            {
                ColorConverter.ConvertFromString(KaxamlInfo.Editor.SelectedText);
                this.SyncButton.SetCurrentValue(IsEnabledProperty, true);
            }
            catch
            {
                this.SyncButton.SetCurrentValue(IsEnabledProperty, false);
                this.SyncButton.SetCurrentValue(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, false);
            }
        }

        private void SyncButtonChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = ColorConverter.ConvertFromString(KaxamlInfo.Editor.SelectedText);
                this.C.SetCurrentValue(ColorPicker.ColorProperty, c);

                this.C.ColorChanged += this.CColorChanged;
            }
            catch
            {
                this.SyncButton.SetCurrentValue(IsEnabledProperty, false);
            }
        }

        private void SyncButtonUnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.C.ColorChanged -= this.CColorChanged;
            }
            catch
            {
                this.SyncButton.SetCurrentValue(IsEnabledProperty, false);
            }
        }

        private void CColorChanged(object sender, ColorChangedEventArgs e)
        {
            if (this.SyncButton.IsChecked is bool isChecked &&
                isChecked)
            {
                try
                {
                    if (this.colorChangedTimer is null)
                    {
                        this.colorChangedTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(200), DispatcherPriority.Background, this.ColorChangedTimerTick, this.Dispatcher);
                    }

                    this.colorChangedTimer.Stop();
                    this.colorChangedTimer.Start();

                    this.colorChangedColor = e.Color;
                }
                catch
                {
                }
            }
        }

        private void ColorChangedTimerTick(object sender, EventArgs e)
        {
            this.colorChangedTimer.Stop();

            KaxamlInfo.Editor.ReplaceSelectedText(this.colorChangedColor.ToString());
            KaxamlInfo.Parse();
        }

        private void CopyColor(object o, EventArgs e)
        {
            Clipboard.SetText(this.C.Color.ToString());
        }

        private void SaveColor(object o, EventArgs e)
        {
            this.Colors.Add(this.C.Color);
        }

        private void RemoveColor(object o, EventArgs e)
        {
            var cm = (ContextMenu)ItemsControl.ItemsControlFromItemContainer(o as MenuItem);
            var lbi = (ListBoxItem)cm.PlacementTarget;
            this.Colors.Remove((Color)lbi.Content);
        }

        private void RemoveAllColors(object o, EventArgs e)
        {
            this.Colors.Clear();
        }

        private void SwatchMouseDown(object o, MouseEventArgs e)
        {
            this.C.SetCurrentValue(ColorPicker.ColorProperty, (o as FrameworkElement)?.DataContext);
        }

        private void CCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!this.updateInternal)
            {
                this.updateInternal = true;

                var s = string.Empty;
                foreach (var c in this.Colors)
                {
                    s = s + c.ToString() + Delimiter;
                }

                this.SetCurrentValue(ColorStringProperty, s);

                this.updateInternal = false;
            }
        }
    }
}
