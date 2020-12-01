using System;
using System.Collections.Generic;
using System.Text;

namespace LewdMaid.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public TagTypes TagType { get; set; }
    }

    public enum TagTypes
    {
        Author,
        Meta,
        Origin,
        Simple
    }
}
