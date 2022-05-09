using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityCMS.Net.Sync.SDK.Models.Page
{
    public class ZoneInfo
    {
        public List<MainContentZone> MainContentZone { get; set; }
    }
    public class MainContentZone
    {
        public string Module { get; set; }
        public Item item { get; set; }
    }

    public class Item
    {
        public int contentid { get; set; }
        public bool? fulllist { get; set; }
    }
}
