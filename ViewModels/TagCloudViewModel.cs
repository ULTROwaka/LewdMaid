using LewdMaid.Models;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;

namespace LewdMaid.ViewModels
{
    public class TagCloudViewModel : ViewModelBase
    {
        [Reactive]
        ObservableCollection<TagViewModel> Tags { get; set; }
        ReactiveCommand<Unit, Unit> SelectAll { get; set; }
        ReactiveCommand<Unit, Unit> DeselectAll { get; set; }
        ReactiveCommand<Unit, Unit> AutoSelect { get; set; }

        public TagCloudViewModel(IEnumerable<TagViewModel> tags)
        {
            Tags = new ObservableCollection<TagViewModel>(tags);

            SelectAll =  ReactiveCommand.Create(SelectingAll);
            DeselectAll = ReactiveCommand.Create(DeselectingAll);
        }

        private void SelectingAll()
        {
            foreach(var tag in Tags)
            {
                tag.State = true;
            }
        }

        private void DeselectingAll()
        {
            foreach (var tag in Tags)
            {
                tag.State = false;
            }
        }
    }
}
