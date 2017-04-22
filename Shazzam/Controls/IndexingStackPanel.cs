namespace Shazzam.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class IndexingStackPanel : StackPanel
    {
        public static readonly DependencyProperty IndexProperty = DependencyProperty.RegisterAttached(
            "Index",
            typeof(int),
            typeof(IndexingStackPanel),
            new UIPropertyMetadata(default(int)));

        public static readonly DependencyProperty SelectionLocationProperty = DependencyProperty.RegisterAttached(
            "SelectionLocation",
            typeof(SelectionLocation),
            typeof(IndexingStackPanel),
            new UIPropertyMetadata(default(SelectionLocation)));

        public static readonly DependencyProperty StackLocationProperty = DependencyProperty.RegisterAttached(
            "StackLocation",
            typeof(StackLocation),
            typeof(IndexingStackPanel),
            new UIPropertyMetadata(default(StackLocation)));

        public static readonly DependencyProperty IndexOddEvenProperty = DependencyProperty.RegisterAttached(
            "IndexOddEven",
            typeof(IndexOddEven),
            typeof(IndexingStackPanel),
            new UIPropertyMetadata(default(IndexOddEven)));

        public static int GetIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(IndexProperty);
        }

        public static void SetIndex(DependencyObject obj, int value)
        {
            obj.SetValue(IndexProperty, value);
        }

        public static SelectionLocation GetSelectionLocation(DependencyObject obj)
        {
            return (SelectionLocation)obj.GetValue(SelectionLocationProperty);
        }

        public static void SetSelectionLocation(DependencyObject obj, SelectionLocation value)
        {
            obj.SetValue(SelectionLocationProperty, value);
        }

        public static StackLocation GetStackLocation(DependencyObject obj)
        {
            return (StackLocation)obj.GetValue(StackLocationProperty);
        }

        public static void SetStackLocation(DependencyObject obj, StackLocation value)
        {
            obj.SetValue(StackLocationProperty, value);
        }

        public static IndexOddEven GetIndexOddEven(DependencyObject obj)
        {
            return (IndexOddEven)obj.GetValue(IndexOddEvenProperty);
        }

        public static void SetIndexOddEven(DependencyObject obj, IndexOddEven value)
        {
            obj.SetValue(IndexOddEvenProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var index = 0;
            var isEven = true;
            var foundSelected = false;

            foreach (UIElement element in this.Children)
            {
                if (this.IsItemsHost)
                {
                    var selectorParent = this.TemplatedParent as Selector;

                    if (selectorParent?.ItemContainerGenerator.ContainerFromItem(selectorParent.SelectedItem) is UIElement selectedElement)
                    {
                        if (ReferenceEquals(element, selectedElement))
                        {
                            element.SetCurrentValue(SelectionLocationProperty, SelectionLocation.Selected);
                            foundSelected = true;
                        }
                        else if (foundSelected)
                        {
                            element.SetCurrentValue(SelectionLocationProperty, SelectionLocation.After);
                        }
                        else
                        {
                            element.SetCurrentValue(SelectionLocationProperty, SelectionLocation.Before);
                        }
                    }
                }

                // StackLocation
                if (this.Children.Count - 1 == 0)
                {
                    element.SetCurrentValue(StackLocationProperty, StackLocation.FirstAndLast);
                }
                else if (index == 0)
                {
                    element.SetCurrentValue(StackLocationProperty, StackLocation.First);
                }
                else if (index == this.Children.Count - 1)
                {
                    element.SetCurrentValue(StackLocationProperty, StackLocation.Last);
                }
                else
                {
                    element.SetCurrentValue(StackLocationProperty, StackLocation.Middle);
                }

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (isEven)
                {
                    element.SetCurrentValue(IndexOddEvenProperty, IndexOddEven.Even);
                }
                else
                {
                    element.SetCurrentValue(IndexOddEvenProperty, IndexOddEven.Odd);
                }

                element.SetCurrentValue(IndexProperty, index);
                index++;
            }

            return base.MeasureOverride(constraint);
        }
    }
}
