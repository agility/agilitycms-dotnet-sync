using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgilityCMS.Net.Sync.SDK.Models.Page;

namespace AgilityCMS.Net.Sync.SDK.Models.Item
{
    public class ContentItems
    {
        public int contentID { get; set; }
        public ContentProperties properties { get; set; }
        public SEO seo { get; set; }
        public Dictionary<string, object> fields { get; set; }
    }
}
