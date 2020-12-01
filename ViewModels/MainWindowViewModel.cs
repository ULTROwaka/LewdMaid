using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using LewdMaid.Models;

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

namespace LewdMaid.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive]
        ObservableCollection<PictureViewModel> AllPictures { get; set; }
        [Reactive]
        PictureViewModel SelectedPicture { get; set; }
        [Reactive]
        TagCloudViewModel Cloud { get; set; }
        ReactiveCommand<Unit, Unit> Refresh { get; }
        ReactiveCommand<Unit, Unit> OpenPostInBrowser { get; }

        public MainWindowViewModel(IPictureProvider provider)
        {
            this.WhenAnyValue(x => x.SelectedPicture)
                .Where(x => x != null)
                .Subscribe(x => Cloud = new TagCloudViewModel(x.Tags));

            var pictures = provider.Provide(5);
            AllPictures = new ObservableCollection<PictureViewModel>(pictures.Select(x => new PictureViewModel(x)));

            if (AllPictures.Count > 0)
            {
                SelectedPicture = AllPictures[0];
            }        

            Refresh = ReactiveCommand.Create(() => 
                {
                    AllPictures.Clear();
                    var pictures = provider.Provide(10).Select(x => new PictureViewModel(x));                   
                    foreach (var picture in pictures)
                    {
                        AllPictures.Add(picture);
                    }
                    SelectedPicture = AllPictures[0];
                });

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
    }
}
