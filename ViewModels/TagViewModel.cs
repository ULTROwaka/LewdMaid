using LewdMaid.Models;

using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Text;

namespace LewdMaid.ViewModels
{
    public class TagViewModel : ViewModelBase
    {
        private readonly Tag _tag;

        [Reactive]
        public string Text { get; set; }
        [Reactive]
        public bool State { get; set; }

        public TagViewModel(Tag tag)
        {
            _tag = tag;
            Text = _tag.Name;
        }
    }
}
