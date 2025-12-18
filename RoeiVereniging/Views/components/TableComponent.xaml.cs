using RoeiVereniging.Core.Models;
using System.Collections;

namespace RoeiVereniging.Views.Components;

public partial class TableComponent : ContentView
{
	public TableComponent()
	{
		InitializeComponent();
	}

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

    private static void OnDataChange(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TableComponent)bindable;
        control.TableView.ItemsSource = (IEnumerable)newValue;
    }

    private static void OnColumnsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TableComponent)bindable;
        control.BuildColumns();
    }

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
                new ColumnDefinition { Width = ParseWidthToGridlength(Columns[i].Width) });

            var headerLabel = new Label
            {
                Text = Columns[i].Header,
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Grid.SetColumn(headerLabel, i);
            HeaderGrid.Children.Add(headerLabel);
        }

        TableView.ItemTemplate = new DataTemplate(() =>
        {
            var grid = new Grid { ColumnSpacing = 12 };

            for (int i = 0; i < Columns.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = ParseWidthToGridlength(Columns[i].Width) });
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 14,
                    TextColor = Colors.Black,
                };
                label.SetBinding(Label.TextProperty, Columns[i].BindingPath);
                Grid.SetColumn(label, i);
                grid.Children.Add(label);
            }

            return new Frame
            {
                StyleClass = new[] { "row-card" },
                Content = grid
            };
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