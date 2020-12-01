using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewdMaid.Models
{
    public interface IPictureProvider
    {
        public IEnumerable<Picture> Provide(int count);
    }
}
