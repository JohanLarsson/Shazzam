namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ElementCursorDecorator : Decorator
    {
        CursorAdorner _CursorAdorner;

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            // setup the adorner layer
            AdornerLayer _AdornerLayer = AdornerLayer.GetAdornerLayer(this);

            if (_AdornerLayer == null)
            {
                return;
            }

            if (this._CursorAdorner == null)
            {
                this._CursorAdorner = new CursorAdorner(this, this.CursorElement);
            }

            _AdornerLayer.Add(this._CursorAdorner);

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this._CursorAdorner != null)
            {
                AdornerLayer layer = VisualTreeHelper.GetParent(this._CursorAdorner) as AdornerLayer;
                layer.Remove(this._CursorAdorner);
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this._CursorAdorner.Offset = e.GetPosition(this);
            base.OnMouseMove(e);
        }

        static ElementCursorDecorator()
        {
            CursorProperty.OverrideMetadata(typeof(ElementCursorDecorator), new FrameworkPropertyMetadata(Cursors.None));
            ForceCursorProperty.OverrideMetadata(typeof(ElementCursorDecorator), new FrameworkPropertyMetadata(true));
        }

        public UIElement CursorElement
        {
            get { return (UIElement) this.GetValue(CursorElementProperty); }
            set { this.SetValue(CursorElementProperty, value); }
        }
        public static readonly DependencyProperty CursorElementProperty =
            DependencyProperty.Register("CursorElement", typeof(UIElement), typeof(ElementCursorDecorator), new UIPropertyMetadata(null));

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }
    }
}
