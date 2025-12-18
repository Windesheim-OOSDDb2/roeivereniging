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

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    private static void OnDataChange(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TableComponent)bindable;
        control.TableView.ItemsSource = (IEnumerable)newValue;
    }
}