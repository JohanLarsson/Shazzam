namespace Kaxaml.Plugins.ColorPicker
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Kaxaml.Plugins.Controls;
    using KaxamlPlugins;

    public partial class ColorPickerPlugin : UserControl
    {
        private const char DELIMITER = '|';
        private bool updateInternal = false;
        private DispatcherTimer colorChangedTimer;
        private Color colorChangedColor;

        public ColorPickerPlugin()
        {
            this.InitializeComponent();
            this.Colors = new ObservableCollection<Color>();
            this.ColorString = Shazzam.Properties.Settings.Default.ColorPickerColors;

            KaxamlInfo.EditSelectionChanged += this.KaxamlInfo_EditSelectionChanged;
        }

        private void KaxamlInfo_EditSelectionChanged(string selectedText)
        {
            // wish we could do this without a try catch!
            try
            {
                var c = (Color)ColorConverter.ConvertFromString(KaxamlInfo.Editor.SelectedText);
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
                this.C.SetCurrentValue(Controls.ColorPicker.ColorProperty, c);

                this.C.ColorChanged += this.C_ColorChanged;
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
                this.C.ColorChanged -= this.C_ColorChanged;
            }
            catch
            {
                this.SyncButton.SetCurrentValue(IsEnabledProperty, false);
            }
        }

        private void C_ColorChanged(object sender, ColorChangedEventArgs e)
        {
            if ((bool)this.SyncButton.IsChecked)
            {
                try
                {
                    if (this.colorChangedTimer == null)
                    {
                        this.colorChangedTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(200), DispatcherPriority.Background, this._ColorChangedTimer_Tick, this.Dispatcher);
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

        private void _ColorChangedTimer_Tick(object sender, EventArgs e)
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
            var c = (Color)(o as FrameworkElement).DataContext;
            this.C.Color = c;
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
        /// DependencyProperty for Colors
        /// </summary>
        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register("Colors", typeof(ObservableCollection<Color>), typeof(ColorPickerPlugin), new FrameworkPropertyMetadata(default(ObservableCollection<Color>), OnColorsChanged));

        /// <summary>
        /// PropertyChangedCallback for Colors
        /// </summary>
        private static void OnColorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ColorPickerPlugin)
            {
                var owner = (ColorPickerPlugin)obj;

                var c = args.NewValue as ObservableCollection<Color>;
                if (c != null)
                {
                    c.CollectionChanged += owner.c_CollectionChanged;
                }
            }
        }

        private void c_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!this.updateInternal)
            {
                this.updateInternal = true;

                var s = string.Empty;
                foreach (var c in this.Colors)
                {
                    s = s + c.ToString() + DELIMITER;
                }

                this.ColorString = s;

                this.updateInternal = false;
            }
        }

        /// <summary>
        /// description of the property
        /// </summary>
        public string ColorString
        {
            get => (string)this.GetValue(ColorStringProperty);
            set => this.SetValue(ColorStringProperty, value);
        }

        /// <summary>
        /// DependencyProperty for ColorString
        /// </summary>
        public static readonly DependencyProperty ColorStringProperty = DependencyProperty.Register(
            "ColorString",
            typeof(string),
            typeof(ColorPickerPlugin),
            new FrameworkPropertyMetadata(default(string), OnColorStringChanged));

        /// <summary>
        /// PropertyChangedCallback for ColorString
        /// </summary>
        private static void OnColorStringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ColorPickerPlugin)
            {
                var owner = (ColorPickerPlugin)obj;
                Shazzam.Properties.Settings.Default.ColorPickerColors = args.NewValue as string;

                if (!owner.updateInternal)
                {
                    owner.updateInternal = true;

                    owner.Colors.Clear();
                    var colors = (args.NewValue as string).Split(DELIMITER);

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
    }
}