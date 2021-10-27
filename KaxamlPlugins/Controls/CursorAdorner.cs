namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    public sealed class CursorAdorner : Adorner
    {
        private readonly UIElement cursor;

        private Point offset;

        public CursorAdorner(ElementCursorDecorator owner, UIElement cursor)
            : base(owner)
        {
            this.cursor = cursor;
            this.AddVisualChild(cursor);
        }

        public Point Offset
        {
            get => this.offset;
            set
            {
                this.offset = value;
                this.InvalidateArrange();
            }
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            return this.cursor;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.cursor.Arrange(new Rect(this.Offset, this.cursor.DesiredSize));
            return finalSize;
        }
    }
}
