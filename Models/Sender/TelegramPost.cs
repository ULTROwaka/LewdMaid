using Telegram.Bot.Types.InputFiles;

namespace LewdMaid.Models.Sender
{
    public class TelegramPost
    {
        public InputOnlineFile Image { get; set; }
        public InputOnlineFile PreviewImage { get; set; }
        public string ButtonUrl { get; set; }
        public string Text { get; set; }
    }
}