using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityCMS.Net.Sync.SDK.Models.Page
{
    public class Page
    {
        public long syncToken { get; set; }
        public List<PageItems> items { get; set; }
        public bool busy { get; set; }
    }
}
