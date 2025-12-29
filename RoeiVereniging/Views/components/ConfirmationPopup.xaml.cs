using CommunityToolkit.Maui.Views;
using System.Globalization;

namespace RoeiVereniging.Views.components;

public partial class ConfirmationPopup : Popup
{
    public ConfirmationPopup(string title, string message, string footer, [Optional] ImageSource? qrCodeImage)
    {
        InitializeComponent();
        MessageTitle.Text = title;
        MessageLabel.Text = message;
        MessageFooter.Text = footer;
        if (qrCodeImage == null)
        {
            QrCodeImage.Source = "noimage.png";
            QrCode.IsVisible = false;
            QrCodeImage.IsVisible = false;
        } else
        {
            QrCodeImage.Source = qrCodeImage;
        }
    }

    private void OnDoneClicked(object sender, EventArgs e)
    {
        Close();
    }
}