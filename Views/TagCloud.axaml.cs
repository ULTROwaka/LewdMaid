using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using LewdMaid.ViewModels;

namespace LewdMaid.Views
{
    public class TagCloud : UserControl
    {
        public TagCloud()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
