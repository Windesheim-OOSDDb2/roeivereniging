using CommunityToolkit.Maui.Views;
using System.Globalization;

namespace RoeiVereniging.Views.components;

public partial class ConfirmationPopup : Popup
{
    public ConfirmationPopup(string title, string text)
    {
        InitializeComponent();

        MessageTitle.Text = $"{title}";
        MessageLabel.Text = $"{text}";
    }

    private void OnDoneClicked(object sender, EventArgs e)
    {
        Close();
    }
}