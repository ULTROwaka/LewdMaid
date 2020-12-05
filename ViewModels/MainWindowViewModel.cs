﻿using Avalonia;
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
using System.Threading;
using System.Threading.Tasks;

namespace LewdMaid.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IPictureProvider _pictureProvider;
        private int _count = 30;
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

        public MainWindowViewModel(IPictureProvider provider)
        {
            _pictureProvider = provider;

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
    }
}
