using RoeiVereniging.Core.Models;
using RoeiVereniging.Views.Components.Helpers;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RoeiVereniging.Views.Components;

public partial class TableComponent : ContentView
{
    public int rowCounter = 0;
    public event Action? FilterRequested;
    private void OnFilterRequested()
    {
        ApplyFilters();
    }

    // to make filter functionality work, 2 annonymous objects are needed
    private List<object> _allItems = new();
    private readonly ObservableCollection<object> _filteredItems = new();

    public TableComponent()
	{
		InitializeComponent();
        TableView.ItemsSource = _filteredItems;
    }

    // Make rows clickable, and head to a route passing an id as aparam
    public static readonly BindableProperty RowClickedCommandProperty =
    BindableProperty.Create(
        nameof(RowClickedCommand),
        typeof(ICommand),
        typeof(TableComponent));

    public ICommand RowClickedCommand
    {
        get => (ICommand)GetValue(RowClickedCommandProperty);
        set => SetValue(RowClickedCommandProperty, value);
    }


    // Make Items and Columns bindable properties so they can be set from outside
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(TableComponent),
            propertyChanged: OnDataChange);

    public static readonly BindableProperty ColumnsProperty =
        BindableProperty.Create(
            nameof(Columns),
            typeof(IList<TableColumnDefinition>),
            typeof(TableComponent),
            propertyChanged: OnColumnsChanged);

    // Public properties to get/set the bindable properties
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public IList<TableColumnDefinition> Columns
    {
        get => (IList<TableColumnDefinition>)GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    // Callback for wjen ItemsSource changes
    private static void OnDataChange(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TableComponent)bindable;

        control._allItems = newValue is IEnumerable enumerable
            ? enumerable.Cast<object>().ToList()
            : new List<object>();

        control.ApplyFilters();
    }

    // Callback for when Columns changes
    private static void OnColumnsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TableComponent)bindable;
        control.BuildColumns();
    }

    // helper function to parse width strings (like "2*") into GridLength objects (typing the variable right away with gridlength didn't work >:c)
    // Can't be outside of class because I can't get access to GridLength type otherwise
    private static GridLength ParseWidthToGridlength(string width)
    {
        if (string.IsNullOrWhiteSpace(width))
        {
            return GridLength.Star;
        }

        if (width.Equals("Auto", StringComparison.OrdinalIgnoreCase))
        {
            return GridLength.Auto;
        }

        if (width.EndsWith("*"))
        {
            var factor = width.Replace("*", "");
            return string.IsNullOrEmpty(factor) ? GridLength.Star : new GridLength(double.Parse(factor), GridUnitType.Star);
        }

        // return the absolute value
        return new GridLength(double.Parse(width), GridUnitType.Absolute);
    }

    // Builds the table columns based on the Columns property
    // Called whenever Columns property changes or on initialization
    private void BuildColumns()
    {

        if (Columns == null || Columns.Count == 0)
        {
            return;
        }

        HeaderGrid.ColumnDefinitions.Clear();
        HeaderGrid.Children.Clear();

        // fill header columns based on what is within Columns.Header
        for (int i = 0; i < Columns.Count; i++)
        {
            HeaderGrid.ColumnDefinitions.Add(
                new ColumnDefinition { Width = ParseWidthToGridlength(Columns[i].Width) }
            );
            View headerView;
            switch (Columns[i].HeaderType)
            {
                case TableHeaderType.SortAZ:
                    headerView = CreateSortableHeader(Columns[i]);
                    break;

                case TableHeaderType.SortDate:
                    headerView = CreateSortableHeader(Columns[i]);
                    break;

                case TableHeaderType.SortTime:
                    headerView = CreateSortableHeader(Columns[i]);
                    break;

                case TableHeaderType.Select:
                    // if "Alles" is selected, no need to create select header
                    if (Columns[i].SelectedValue?.ToString() == "Alles")
                    {
                        headerView = null;
                        break;
                    }
                    headerView = CreateSelectHeader(Columns[i]);
                    break;

                case TableHeaderType.Button:
                    headerView = CreateButtonHeader(Columns[i]);
                    break;
                default:
                    headerView = CreateTextHeader(Columns[i]);
                    break;
            }
            if (headerView == null)
            {
               continue;
            }

        Grid.SetColumn(headerView, i);
            HeaderGrid.Children.Add(headerView);
        }

        TableView.ItemTemplate = new DataTemplate(() =>
        {
            var grid = new Grid { ColumnSpacing = 0, RowSpacing = 0, };

            if (rowCounter == 0)
            {
                grid.BackgroundColor = Color.FromArgb("#ffffff");
                rowCounter = 1;
            }
            else
            {
                grid.BackgroundColor = Color.FromArgb("#dee6f8");
                rowCounter = 0;
            }

            // Iterate through columns to create cells for each column
            for (int i = 0; i < Columns.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = ParseWidthToGridlength(Columns[i].Width) });

                var label = new Label
                {
                    FontSize = 16,
                    TextColor = Color.FromArgb("#082757"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Padding = new Thickness(14, 12)
                };

                // Bind the text of the label to the respective column data
                label.SetBinding(
                    Label.TextProperty,
                    new Binding(
                        Columns[i].BindingPath,
                        stringFormat: Columns[i].StringFormat
                ));

                Grid.SetColumn(label, i);

                // Add the label to the row
                grid.Children.Add(label);
            }

            // Create the click event for the row
            var tap = new TapGestureRecognizer();
            tap.Tapped += (obj, e) =>
            {
                // Execute RowClickedCommand only if it's valid
                if (RowClickedCommand?.CanExecute(grid.BindingContext) == true)
                {
                    RowClickedCommand.Execute(grid.BindingContext);
                }
            };

            // Add TapGestureRecognizer to the grid (which represents a row)
            grid.GestureRecognizers.Add(tap);

            return new VerticalStackLayout {
                Spacing = 0,
                Children = {
                    grid,
                    new BoxView
                    {
                        HeightRequest = 1,
                        BackgroundColor = Color.FromArgb("#0854d1")
                    }
                }
            };
        });
    }

    // Can't really make these outside of the class because they need access to maui control types
    private View CreateTextHeader(TableColumnDefinition column)
    {
        return new Label
        {
            Text = column.Header,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#ffffff"),
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            Padding = new Thickness(12, 8)
        };
    }
    private View CreateSortableHeader(TableColumnDefinition column)
    {
        View label = CreateTextHeader(column);

        TapGestureRecognizer tap = new();
        tap.Tapped += (obj, e) =>
        {
            column.SelectedValue = column.SelectedValue as bool? == true ? false : true;
            OnFilterRequested();
        };

        label.GestureRecognizers.Add(tap);
        return label;
    }

    private View CreateSelectHeader(TableColumnDefinition column)
    {
        Picker picker = new()
        {
            Title = column.Header,
            FontSize = 16,
            TextColor = Colors.White,
            BackgroundColor = Color.FromArgb("#082757"),
            TitleColor = Color.FromArgb("#ffffff"),
        };

        // for each unique item there will be a column added to the select
        picker.ItemsSource = GetUniqueColumnValues(column);

        picker.SelectedIndexChanged += (obj, e) =>
        {
            column.SelectedValue = picker.SelectedItem;
            OnFilterRequested();
        };

        return picker;
    }

    private View CreateButtonHeader(TableColumnDefinition column)
    {
        return new Button
        {
            Text = column.Header,
            FontSize = 16,
            BackgroundColor = Color.FromArgb("#5fa6e8"),
            TextColor = Color.FromArgb("#ffffff"),
            CornerRadius = 8,
            Padding = new Thickness(12, 6),
            Command = column.Command
        };
    }

    //helper function to get each unique column value and add it to the list
    private IList GetUniqueColumnValues(TableColumnDefinition column)
    {
        var values = new HashSet<object>();

        if (ItemsSource == null)
        {
            return values.ToList();
        }

        foreach (var item in ItemsSource)
        {
            var prop = item.GetType().GetProperty(column.BindingPath);
            if (prop == null)
            {
                continue;
            }

            var value = prop.GetValue(item);
            if (value != null)
            {
                values.Add(value);
            }
        }
        var list = values.ToList();
        list.Insert(0, "Toon alles");
        
        return list;
    }

    private void ApplyFilters()
    {
        IEnumerable<object> query = _allItems;

        // if columns is null just add all
        if (Columns == null || Columns.Count == 0)
        {
            _filteredItems.Clear();
            foreach (var item in _allItems)
            {
                _filteredItems.Add(item);
            }

            return;
        }

        foreach (var column in Columns)
        {
            // I use continue if null because not each column needs to have a filter applied
            if (column.SelectedValue == null)
            {
                continue;
            }

            var prop = query.FirstOrDefault()?.GetType().GetProperty(column.BindingPath);
            if (prop == null)
            {
                continue;
            }

            switch (column.HeaderType)
            {
                case TableHeaderType.Select:
                    // skip filters if Toon Alles is applied to show every row instead
                    if(column.SelectedValue.ToString() == "Toon alles")
                    {
                        continue;
                    }
                    query = query.Where(item =>
                        Equals(prop.GetValue(item), column.SelectedValue));
                    break;

                case TableHeaderType.SortAZ:
                    query = SortingHelper.ApplyAZSort(query, prop, column);
                    break;

                case TableHeaderType.SortDate:
                    query = SortingHelper.ApplyDateSort(query, prop, column);
                    break;

                case TableHeaderType.SortTime:
                    query = SortingHelper.ApplyTimeSort(query, prop, column);
                    break;
            }
        }

        _filteredItems.Clear();
        foreach (var item in query)
        {
            _filteredItems.Add(item);
        }
    }   
}