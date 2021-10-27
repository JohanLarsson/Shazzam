namespace KaxamlPlugins.Controls
{
    using System.Windows;

    public class DropDownColorPicker : ColorPicker
    {
        static DropDownColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownColorPicker), new FrameworkPropertyMetadata(typeof(DropDownColorPicker)));
        }
    }
}
