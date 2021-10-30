namespace Shazzam
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class RegisterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? SingleTemplate { get; set; }

        public DataTemplate? PairTemplate { get; set; }

        public DataTemplate? TripleTemplate { get; set; }

        public DataTemplate? QuadrupleTemplate { get; set; }

        public DataTemplate? ColorTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                Register register
                    when register.Type == typeof(double) ||
                         register.Type == typeof(float) ||
                         register.Type == typeof(int)
                    => this.SingleTemplate,
                Register register
                    when register.Type == typeof(Color)
                    => this.ColorTemplate,
                Register register
                    when register.Type == typeof(Point) ||
                         register.Type == typeof(Vector) ||
                         register.Type == typeof(Size)
                    => this.PairTemplate,
                Register register
                    when register.Type == typeof(Point3D) ||
                         register.Type == typeof(Vector3D)
                    => this.TripleTemplate,
                Register register
                    when register.Type == typeof(Point4D)
                    => this.QuadrupleTemplate,
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}
