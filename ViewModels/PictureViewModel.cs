using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using LewdMaid.Models;

using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LewdMaid.ViewModels
{
    public class PictureViewModel : ViewModelBase
    {
        public Picture Picture { get; private set; }
        [Reactive]
        public Bitmap Preview { get; set; }
        [Reactive]
        public string PosrUrl { get; set; }
        [Reactive]
        public ObservableCollection<TagViewModel> Tags { get; set; }

        public PictureViewModel(Picture picture)
        {
            Picture = picture;

            Tags = new ObservableCollection<TagViewModel>(Picture.Tags.Select(x => new TagViewModel(x)));

            PosrUrl = Picture.PostUrl;


            //Load Image
            LoadImage();
        }

        private Task LoadImage()
        {
            return Task.Factory.StartNew(() =>
            {
                var imageByteArray = ImageDownloader.DownloadToByteArray(Picture.PreviewUrl ?? Picture.Url);
                Stream stream = new MemoryStream(imageByteArray);
                Preview = new Bitmap(stream);
            },creationOptions: TaskCreationOptions.LongRunning);
        }
    }
}
