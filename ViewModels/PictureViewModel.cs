using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using LewdMaid.Models;

using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewdMaid.ViewModels
{
    public class PictureViewModel : ViewModelBase
    {
        private readonly Picture _picture;
        [Reactive]
        public Bitmap Preview { get; set; }
        [Reactive]
        public string PosrUrl { get; set; }
        [Reactive]
        public ObservableCollection<TagViewModel> Tags { get; set; }

        public PictureViewModel(Picture picture)
        {
            _picture = picture;

            Tags = new ObservableCollection<TagViewModel>(_picture.Tags.Select(x => new TagViewModel(x)));

            PosrUrl = _picture.PostUrl;


            //Load Image
            LoadImage();
        }

        private Task LoadImage()
        {
            return Task.Run(() =>
            {
                var imageByteArray = ImageDownloader.DownloadToByteArray(_picture.PreviewUrl ?? _picture.Url);
                Stream stream = new MemoryStream(imageByteArray);
                Preview = new Bitmap(stream);
            });
        }
    }
}
