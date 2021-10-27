namespace KaxamlPlugins.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class TextDragger : Decorator
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(TextDragger),
            new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data),
            typeof(object),
            typeof(TextDragger),
            new UIPropertyMetadata(null));

        private bool isClipboardSet;
        private bool isDragging;

        static TextDragger()
        {
            CursorProperty.OverrideMetadata(typeof(TextDragger), new FrameworkPropertyMetadata(Cursors.Hand));
        }

        public string? Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public object? Data
        {
            get => this.GetValue(DataProperty);
            set => this.SetValue(DataProperty, value);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    Clipboard.SetText(this.Text);
                    this.isClipboardSet = true;
                }
                else if (this.Data != null)
                {
                    Clipboard.SetText(this.Data.ToString());
                    this.isClipboardSet = true;
                }
            }
            else if (e.ClickCount == 2)
            {
                if (this.isClipboardSet)
                {
                    KaxamlInfo.Editor.InsertStringAtCaret(this.Text);
                    this.isClipboardSet = false;
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // need this to ensure hittesting
            drawingContext.DrawRectangle(
                Brushes.Transparent,
                null,
                new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            base.OnRender(drawingContext);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && (!this.isDragging))
            {
                this.StartDrag();
            }
            else if (e.LeftButton == MouseButtonState.Released)
            {
                this.isDragging = false;
            }

            base.OnPreviewMouseMove(e);
        }

        private void StartDrag()
        {
            var obj = new DataObject(DataFormats.Text, this.Text);
            if (this.Data != null)
            {
                obj.SetData(this.Data.GetType(), this.Data);
            }

            try
            {
                DragDrop.DoDragDrop(this, obj, DragDropEffects.Copy);
            }
            catch
            {
            }
        }
    }
}
