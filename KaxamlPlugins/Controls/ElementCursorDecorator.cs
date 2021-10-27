namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ElementCursorDecorator : Decorator
    {
        public static readonly DependencyProperty CursorElementProperty = DependencyProperty.Register(
            nameof(CursorElement),
            typeof(UIElement),
            typeof(ElementCursorDecorator),
            new UIPropertyMetadata(null));

        private CursorAdorner? cursorAdorner;

        static ElementCursorDecorator()
        {
            CursorProperty.OverrideMetadata(typeof(ElementCursorDecorator), new FrameworkPropertyMetadata(Cursors.None));
            ForceCursorProperty.OverrideMetadata(typeof(ElementCursorDecorator), new FrameworkPropertyMetadata(true));
        }

        public UIElement? CursorElement
        {
            get => (UIElement?)this.GetValue(CursorElementProperty);
            set => this.SetValue(CursorElementProperty, value);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            // setup the adorner layer
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);

            if (adornerLayer is null)
            {
                return;
            }

            if (this.cursorAdorner is null)
            {
                this.cursorAdorner = new CursorAdorner(this, this.CursorElement);
            }

            adornerLayer.Add(this.cursorAdorner);

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this.cursorAdorner != null)
            {
                var layer = VisualTreeHelper.GetParent(this.cursorAdorner) as AdornerLayer;
                layer?.Remove(this.cursorAdorner);
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.cursorAdorner != null)
            {
                this.cursorAdorner.Offset = e.GetPosition(this);
                base.OnMouseMove(e);
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }
    }
}
