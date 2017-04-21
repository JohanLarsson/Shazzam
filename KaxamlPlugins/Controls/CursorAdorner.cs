namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    internal sealed class CursorAdorner : Adorner
    {
        UIElement _Cursor;

        private Point _Offset;
        public Point Offset
        {
            get { return this._Offset; }
            set
            {
                this._Offset = value;
                this.InvalidateArrange();
            }
        }

        public CursorAdorner(ElementCursorDecorator Owner, UIElement Cursor)
            : base(Owner)
        {
            this._Cursor = Cursor;
            this.AddVisualChild(this._Cursor);
        }

        protected override Visual GetVisualChild(int index)
        {
            return this._Cursor;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._Cursor.Arrange(new Rect(this.Offset, this._Cursor.DesiredSize));
            return finalSize;
        }

    }
}