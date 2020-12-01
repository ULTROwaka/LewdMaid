using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using LewdMaid.Models;

using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
            var assetUri = ImageDownloader.DownloadToAssets(@"C:/Users/ULTRO/Documents/LewdMaidImages",
                _picture.PreviewUrl ?? _picture.Url, _picture.Hash, _picture.Url.Split('.').Last());
            Preview = new Bitmap(assetUri);
        }
    }
}
