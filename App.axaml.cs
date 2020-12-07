using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using LewdMaid.Models;
using LewdMaid.Models.Sender;

using LewdMaid.ViewModels;
using LewdMaid.Views;

namespace LewdMaid
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(new Rule34xxxProvider(@"https://r34-json.herokuapp.com/posts?tags=-furry&limit=*limit*",
                    "*limit*"), new TelegramSender("468402185:AAGUZpYGp5dGsy-sFf2_3UT6zHs_tJbaVdg", -1001114198374)),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
