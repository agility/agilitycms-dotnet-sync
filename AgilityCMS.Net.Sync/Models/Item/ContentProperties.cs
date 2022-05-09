using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityCMS.Net.Sync.SDK.Models.Item
{
    public class ContentProperties
    {
        public int state { get; set; }
        public DateTime? modified { get; set; }
        public int versionID { get; set; }
        public string referenceName { get; set; }
        public string definitionName { get; set; }
        public int itemOrder { get; set; }
    }
}
