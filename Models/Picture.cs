using System;
using System.Collections.Generic;
using System.Text;

namespace LewdMaid.Models
{
    public class Picture : ReactiveUI.ReactiveObject
    {
        public string Hash { get; set; }
        public string PostUrl { get; set; }
        public string Url { get; set; }
        public string PreviewUrl { get; set; }
        public float Rating { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
