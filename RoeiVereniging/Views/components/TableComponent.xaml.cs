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

        TableView.ItemTemplate = new DataTemplate(() =>
        {
            var grid = new Grid { ColumnSpacing = 12 };

            for (int i = 0; i < Columns.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Columns[i].Width) });
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 14,
                    TextColor = Colors.Black,
                };
                label.SetBinding(Label.TextProperty, Columns[i].BindingPath);
                Grid.SetColumn(label, i);
                grid.Add(label);
            }

            return new Frame
            {
                StyleClass = new[] { "row-card" },
                Content = grid
            };
        });
    }

}