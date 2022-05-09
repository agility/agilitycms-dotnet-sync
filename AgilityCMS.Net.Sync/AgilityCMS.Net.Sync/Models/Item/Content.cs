using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityCMS.Net.Sync.SDK.Models.Item
{
    public class Content
    {
        public long syncToken { get; set; }
        public List<ContentItems> items { get; set; }
        public bool busy { get; set; }
    }
}
