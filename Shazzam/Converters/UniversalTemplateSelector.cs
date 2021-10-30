namespace Shazzam
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    [ContentProperty(nameof(Templates))]
    public class UniversalTemplateSelector : DataTemplateSelector
    {
#pragma warning disable CA1002 // Do not expose generic lists
        public List<DataTemplate> Templates { get; } = new();
#pragma warning restore CA1002 // Do not expose generic lists

        public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
        {
            if (item is null)
            {
                return base.SelectTemplate(null, container);
            }

            foreach (var dataTemplate in this.Templates)
            {
                if (dataTemplate.DataType is Type dataType &&
                    dataType.IsInstanceOfType(item))
                {
                    return dataTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
