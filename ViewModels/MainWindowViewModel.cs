using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using LewdMaid.Models;
using LewdMaid.Models.Sender;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LewdMaid.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IPictureProvider _pictureProvider;
        private int _count = 30;
        private readonly TelegramSender _sender;
        [Reactive]
        ObservableCollection<PictureViewModel> AllPictures { get; set; }
        //Post Candidates
        [Reactive]
        ObservableCollection<PictureViewModel> SelectedPictures { get; set; }
        [Reactive]
        PictureViewModel SelectedPicture { get; set; }
        [Reactive]
        TagCloudViewModel Cloud { get; set; }
        ReactiveCommand<Unit, IEnumerable<Picture>> Refresh { get; }
        ReactiveCommand<Unit, Unit> OpenPostInBrowser { get; }
        ReactiveCommand<Unit, Unit> AddPicture { get; }
        ReactiveCommand<Unit, Unit> RemovePicture { get; }
        ReactiveCommand<Unit, Unit> SendPicture { get;  }

        public MainWindowViewModel(IPictureProvider provider, TelegramSender sender)
        {
            _pictureProvider = provider;
            _sender = sender;

            this.WhenAnyValue(x => x.SelectedPicture)
                .Where(x => x != null)
                .Subscribe(x => Cloud = new TagCloudViewModel(x.Tags));

            var pictures = provider.Provide(5);
            AllPictures = new ObservableCollection<PictureViewModel>(pictures.Select(x => new PictureViewModel(x)));
            if (AllPictures.Count > 0)
            {
                SelectedPicture = AllPictures[0];
            }
            SelectedPictures = new ObservableCollection<PictureViewModel>();

            Refresh = ReactiveCommand.CreateFromTask(GetPicturesAsync);
            Refresh.Subscribe(x =>
            {
                AllPictures.Clear();
                foreach (var picture in x)
                {
                    AllPictures.Add(new PictureViewModel(picture));
                }
                SelectedPicture = AllPictures[0];
            });
            SendPicture = ReactiveCommand.CreateFromTask(SendPictureAsync);
            AddPicture = ReactiveCommand.Create(() => { SelectedPictures.Add(SelectedPicture); });
            RemovePicture = ReactiveCommand.Create(() => { SelectedPictures.Remove(SelectedPicture); });
            OpenPostInBrowser = ReactiveCommand.Create(() => 
            {
                var info = new ProcessStartInfo
                {
                    FileName = SelectedPicture.PosrUrl,
                    UseShellExecute = true
                };
                Process.Start(info); 
            });
        }

        private Task<IEnumerable<Picture>> GetPicturesAsync()
        {
            return Task.Factory.StartNew( () =>
                {
                    return _pictureProvider.Provide(_count);
                });
        }

        private Task SendPictureAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                var post = new TelegramPost()
                {
                    ButtonUrl = SelectedPicture.PosrUrl,
                    Image = new Telegram.Bot.Types.InputFiles.InputOnlineFile(SelectedPicture.Picture.Url),
                    Text = string.Join(" ", SelectedPicture.Tags.Where(x => x.State).Select(x => x.Text))
                };

                _sender.Send(post);
            });
        }
    }
}
