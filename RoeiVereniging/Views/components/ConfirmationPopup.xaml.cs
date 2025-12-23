using CommunityToolkit.Maui.Views;
using System.Globalization;

namespace RoeiVereniging.Views.components;

public partial class ConfirmationPopup : Popup
{
    public ConfirmationPopup(string title, string text, string footer)
    {
        InitializeComponent();

        MessageTitle.Text = $"{title}";
        MessageLabel.Text = $"{text}";
        MessageFooter.Text = $"{footer}";
    }

    private void OnDoneClicked(object sender, EventArgs e)
    {
        Close();
    }
}