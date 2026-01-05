using CommunityToolkit.Maui.Views;
using System.Globalization;

namespace RoeiVereniging.Views.components;

public partial class ConfirmationPopup : Popup
{
    public ConfirmationPopup(string title, string message, string footer, ImageSource? qrCodeImage)
    {
        InitializeComponent();
        MessageTitle.Text = title;
        MessageLabel.Text = message;
        MessageFooter.Text = footer;
        if (qrCodeImage == null)
        {
            QrCode.IsVisible = false;
            QrCodeImage.IsVisible = false;
        } else
        {
            QrCodeImage.Source = qrCodeImage;
        }
    }
    public ConfirmationPopup(string title, string message, string footer)
    {
        InitializeComponent();
        MessageTitle.Text = title;
        MessageLabel.Text = message;
        MessageFooter.Text = footer;
            QrCode.IsVisible = false;
            QrCodeImage.IsVisible = false;
        
    }

    private void OnDoneClicked(object sender, EventArgs e)
    {
        Close();
    }
}