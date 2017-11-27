namespace Shazzam.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class ShaderModelConstantRegisterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DoubleTemplate { get; set; }

        public DataTemplate PairTemplate { get; set; }

        public DataTemplate TripleTemplate { get; set; }

        public DataTemplate QuadrupleTemplate { get; set; }

        public DataTemplate ColorTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ShaderModelConstantRegister register)
            {
                if (register.RegisterType == typeof(double) ||
                    register.RegisterType == typeof(float))
                {
                    return this.DoubleTemplate;
                }

                if (register.RegisterType == typeof(Color))
                {
                    return this.ColorTemplate;
                }

                if (register.RegisterType == typeof(Point) ||
                    register.RegisterType == typeof(Vector) ||
                    register.RegisterType == typeof(Size))
                {
                    return this.PairTemplate;
                }

                if (register.RegisterType == typeof(Point3D) ||
                    register.RegisterType == typeof(Vector3D))
                {
                    return this.TripleTemplate;
                }

                if (register.RegisterType == typeof(Point4D))
                {
                    return this.QuadrupleTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
