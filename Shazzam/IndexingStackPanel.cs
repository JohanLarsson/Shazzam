namespace Magic.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class IndexingStackPanel : StackPanel
    {
        public static int GetIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(IndexProperty);
        }

        public static void SetIndex(DependencyObject obj, int value)
        {
            obj.SetValue(IndexProperty, value);
        }

        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.RegisterAttached("Index", typeof(int), typeof(IndexingStackPanel), new UIPropertyMetadata(default(int)));

        public static SelectionLocation GetSelectionLocation(DependencyObject obj)
        {
            return (SelectionLocation)obj.GetValue(SelectionLocationProperty);
        }

        public static void SetSelectionLocation(DependencyObject obj, SelectionLocation value)
        {
            obj.SetValue(SelectionLocationProperty, value);
        }

        public static readonly DependencyProperty SelectionLocationProperty =
            DependencyProperty.RegisterAttached("SelectionLocation", typeof(SelectionLocation), typeof(IndexingStackPanel), new UIPropertyMetadata(default(SelectionLocation)));

        public static StackLocation GetStackLocation(DependencyObject obj)
        {
            return (StackLocation)obj.GetValue(StackLocationProperty);
        }

        public static void SetStackLocation(DependencyObject obj, StackLocation value)
        {
            obj.SetValue(StackLocationProperty, value);
        }

        public static readonly DependencyProperty StackLocationProperty =
            DependencyProperty.RegisterAttached("StackLocation", typeof(StackLocation), typeof(IndexingStackPanel), new UIPropertyMetadata(default(StackLocation)));

        public static IndexOddEven GetIndexOddEven(DependencyObject obj)
        {
            return (IndexOddEven)obj.GetValue(IndexOddEvenProperty);
        }

        public static void SetIndexOddEven(DependencyObject obj, IndexOddEven value)
        {
            obj.SetValue(IndexOddEvenProperty, value);
        }

        public static readonly DependencyProperty IndexOddEvenProperty =
            DependencyProperty.RegisterAttached("IndexOddEven", typeof(IndexOddEven), typeof(IndexingStackPanel), new UIPropertyMetadata(default(IndexOddEven)));

        protected override Size MeasureOverride(Size constraint)
        {
            int index = 0;
            bool isEven = true;
            bool foundSelected = false;

            foreach (UIElement element in this.Children)
            {
                if (this.IsItemsHost)
                {
                    var selectorParent = this.TemplatedParent as Selector;

                    if (selectorParent != null)
                    {
                        var selectedElement = selectorParent.ItemContainerGenerator.ContainerFromItem(selectorParent.SelectedItem) as UIElement;

                        if (selectedElement != null)
                        {
                            if (element == selectedElement)
                            {
                                element.SetValue(SelectionLocationProperty, SelectionLocation.Selected);
                                foundSelected = true;
                            }
                            else if (foundSelected)
                            {
                                element.SetValue(SelectionLocationProperty, SelectionLocation.After);
                            }
                            else
                            {
                                element.SetValue(SelectionLocationProperty, SelectionLocation.Before);
                            }
                        }
                    }
                }

                // StackLocation
                if (this.Children.Count - 1 == 0)
                {
                    element.SetValue(StackLocationProperty, StackLocation.FirstAndLast);
                }
                else if (index == 0)
                {
                    element.SetValue(StackLocationProperty, StackLocation.First);
                }
                else if (index == this.Children.Count - 1)
                {
                    element.SetValue(StackLocationProperty, StackLocation.Last);
                }
                else
                {
                    element.SetValue(StackLocationProperty, StackLocation.Middle);
                }

                // IndexOddEven
                if (isEven)
                {
                    element.SetValue(IndexOddEvenProperty, IndexOddEven.Even);
                }
                else
                {
                    element.SetValue(IndexOddEvenProperty, IndexOddEven.Odd);
                }

                element.SetValue(IndexProperty, index);
                index++;
            }

            return base.MeasureOverride(constraint);
        }
    }

    public enum StackLocation
    {
        First,
        Middle,
        Last,
        FirstAndLast
    }

    public enum SelectionLocation
    {
        Before,
        Selected,
        After
    }

    public enum IndexOddEven
    {
        Odd,
        Even
    }
}
