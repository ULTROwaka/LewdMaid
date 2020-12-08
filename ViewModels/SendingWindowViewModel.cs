using LewdMaid.Models.Sender;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;

namespace LewdMaid.ViewModels
{
    public class SendingWindowViewModel : ViewModelBase
    {
        private readonly TelegramSender _sender;
        [Reactive]
        public PictureViewModel Picture { get; set; }
        [Reactive]
        public bool IsDeferre { get; set; }
        [Reactive]
        public DateTime Date { get; set; }
        [Reactive]
        public int Hours { get; set; }
        [Reactive]
        public int Minutes { get; set; }
        ReactiveCommand<Unit, Unit> SendPicture { get; }

        public SendingWindowViewModel(PictureViewModel picture, TelegramSender sender)
        {
            Picture = picture;
            _sender = sender;
            SendPicture = ReactiveCommand.CreateFromTask(SendPictureAsync);
        }

        private Task SendPictureAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                var post = new TelegramPost()
                {
                    ButtonUrl = Picture.PosrUrl,
                    Image = new Telegram.Bot.Types.InputFiles.InputOnlineFile(Picture.Picture.Url),
                    Text = string.Join(" ", Picture.Tags.Where(x => x.State).Select(x => x.Text))
                };

                _sender.Send(post);
            });
        }
    }
}
