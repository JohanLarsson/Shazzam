using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using KaxamlPlugins;

namespace Kaxaml.Plugins.Controls
{
    public class TextDragger : Decorator
    {
        static TextDragger()
        {
            CursorProperty.OverrideMetadata(typeof(TextDragger), new FrameworkPropertyMetadata(Cursors.Hand));
        }

        public string Text
        {
            get { return (string) this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextDragger), new UIPropertyMetadata(string.Empty));

        public object Data
        {
            get { return (object) this.GetValue(DataProperty); }
            set { this.SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(TextDragger), new UIPropertyMetadata(null));

        bool IsClipboardSet = false;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    Clipboard.SetText(this.Text);
                    this.IsClipboardSet = true;
                }
                else if (this.Data != null)
                {
                    Clipboard.SetText(this.Data.ToString());
                    this.IsClipboardSet = true;
                }
            }
            else if (e.ClickCount == 2)
            {
                if (this.IsClipboardSet)
                {
                    KaxamlInfo.Editor.InsertStringAtCaret(this.Text);
                    this.IsClipboardSet = false;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // need this to ensure hittesting
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            base.OnRender(drawingContext);
        }

        bool IsDragging = false;

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && (!this.IsDragging))
            {
                this.StartDrag();
            }
            else if (e.LeftButton == MouseButtonState.Released)
            {
                this.IsDragging = false;
            }

            base.OnPreviewMouseMove(e);
        }

        private void StartDrag()
        {
            DataObject obj = new DataObject(DataFormats.Text, this.Text);

            if (obj != null)
            {
                if (this.Data != null) obj.SetData(this.Data.GetType(), this.Data);

                try
                {
                    DragDrop.DoDragDrop(this, obj, DragDropEffects.Copy);
                }
                catch { }
            }
        }

    }
}