using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityCMS.Net.Sync.SDK.Models.Page
{
    public class PageItems
    {
        public int pageID { get; set; }
        public string? name { get; set; }
        public string? path { get; set; }
        public string? title { get; set; }
        public string? menuText { get; set; }
        public string? pageType { get; set; }
        public string? templateName { get; set; }
        public string? redirectUrl { get; set; }
        public bool? securePage { get; set; }
        public bool? excludeFromOutputCache { get; set; }
        public Visibility? visible { get; set; }
        public SEO? seo { get; set; }
        public Scripts? scripts { get; set; }
        public Properties? properties { get; set; }
        public ZoneInfo? zones { get; set; }
    }
}
