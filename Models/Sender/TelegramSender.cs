using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace LewdMaid.Models.Sender
{
    public class TelegramSender
    {
        private readonly TelegramBotClient _botClient;
        private readonly long _chatId;

        public TelegramSender(string token, long chatId)
        {
            _botClient = new TelegramBotClient(token);
            _chatId = chatId;
        }

        public void Send(TelegramPost post)
        {
            var toPostButton = new InlineKeyboardButton()
            {
                Text = "Post",
                Url = post.ButtonUrl
            };
            var keyboard = new InlineKeyboardMarkup(toPostButton);
            try
            {
                _botClient.SendPhotoAsync(_chatId, post.Image, caption: post.Text, replyMarkup: keyboard, disableNotification: true).Wait();
            }
            catch (AggregateException ae)
            {
                _botClient.SendPhotoAsync(_chatId, post.PreviewImage, caption: post.Text, replyMarkup: keyboard, disableNotification: true);
            }
            
        }
    }
}
