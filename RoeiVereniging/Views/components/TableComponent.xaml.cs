using RoeiVereniging.Core.Models;
using System.Collections;

namespace RoeiVereniging.Views.Components;

public partial class TableComponent : ContentView
{
	public TableComponent()
	{
		InitializeComponent();
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
        control.TableView.ItemsSource = (IEnumerable)newValue;
    }

    // Callback for when Columns changes
    private static void OnColumnsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TableComponent)bindable;
        control.BuildColumns();
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

            var headerLabel = new Label
            {
                Text = Columns[i].Header,
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(12, 8),
                BackgroundColor = Color.FromArgb("#0854D1")
            };

            Grid.SetColumn(headerLabel, i);
            HeaderGrid.Children.Add(headerLabel);
        }

        TableView.ItemTemplate = new DataTemplate(() =>
        {
            var grid = new Grid { ColumnSpacing = 0, RowSpacing = 0, BackgroundColor = Color.FromArgb("#5fa6e8") };

            for (int i = 0; i < Columns.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = ParseWidthToGridlength(Columns[i].Width) });
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 14,
                    TextColor = Colors.Black,
                    BackgroundColor = Color.FromArgb("#5fa6e8"),
                    Padding = new Thickness(12, 8),
                };
                label.SetBinding(
                    Label.TextProperty, 
                    new Binding(
                        Columns[i].BindingPath,
                        stringFormat: Columns[i].StringFormat
                ));
                Grid.SetColumn(label, i);
                grid.Children.Add(label);
            }

            return grid;
        });
    }

    // helper function to parse width strings (like "2*") into GridLength objects (typing the variable right away with gridlength didn't work >:c)
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

}