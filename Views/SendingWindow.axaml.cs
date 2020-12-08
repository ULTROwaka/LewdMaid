using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using LewdMaid.Models.Sender;
using LewdMaid.ViewModels;

namespace LewdMaid.Views
{
    public class SendingWindow : Window
    {
        public SendingWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public SendingWindow(PictureViewModel picture, TelegramSender sender) : this()
        {
            DataContext = new SendingWindowViewModel(picture, sender);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
