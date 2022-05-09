using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityCMS.Net.Sync.SDK.Models
{
    public class TokenInfo
    {
        public long itemToken { get; set; }
        public long pageToken { get; set; }
        public DateTime lastSyncDate { get; set; }
    }
}
