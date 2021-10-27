namespace Shazzam.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class ShaderModelConstantRegisterTemplateSelector : DataTemplateSelector
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
                ShaderModelConstantRegister register
                    when register.RegisterType == typeof(double) ||
                         register.RegisterType == typeof(float) ||
                         register.RegisterType == typeof(int)
                    => this.SingleTemplate,
                ShaderModelConstantRegister register
                    when register.RegisterType == typeof(Color)
                    => this.ColorTemplate,
                ShaderModelConstantRegister register
                    when register.RegisterType == typeof(Point) ||
                         register.RegisterType == typeof(Vector) ||
                         register.RegisterType == typeof(Size)
                    => this.PairTemplate,
                ShaderModelConstantRegister register
                    when register.RegisterType == typeof(Point3D) ||
                         register.RegisterType == typeof(Vector3D)
                    => this.TripleTemplate,
                ShaderModelConstantRegister register
                    when register.RegisterType == typeof(Point4D)
                    => this.QuadrupleTemplate,
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}
