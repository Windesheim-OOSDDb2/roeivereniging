using RoeiVereniging.Core.Models;
using System.Collections.Generic;
using System.Reflection;

namespace RoeiVereniging.Views.Components.Helpers;

public class SortingHelper
{
    public static IEnumerable<object> ApplyAZSort(IEnumerable<object> source, PropertyInfo prop, TableColumnDefinition column)
    {
        return Sort(source, x => prop.GetValue(x).ToString(), column);
    }

    public static IEnumerable<object> ApplyDateSort(IEnumerable<object> source, PropertyInfo prop, TableColumnDefinition column)
    {
        return Sort (source, x => (DateTime)prop.GetValue(x), column);
    }

    public static IEnumerable<object> ApplyTimeSort(IEnumerable<object> source, PropertyInfo prop, TableColumnDefinition column)
    {
        return Sort(source, x => ((DateTime)prop.GetValue(x)).TimeOfDay, column);
    }

    private static IEnumerable<object> Sort<Type>(IEnumerable<object> source, Func<object, Type> selector, TableColumnDefinition column)
    {
        bool asc = column.SelectedValue as bool? ?? true;

        return asc ? source.OrderBy(selector) : source.OrderByDescending(selector);
    }
}
