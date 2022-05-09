using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityCMS.Net.Sync.SDK
{
    public class SyncOptions
    {
        public string rootPath { get; set; }
        public int retryTimeout { get; set; } = 60000;
        public int retryInterval { get; set; } = 1000;
        public string locale { get; set; }
        public string tokenFolder { get;  } = "state";
        public string tokenFile { get;  } = "sync";
        public string pagesFolder { get;  } = "page";
        public string contentsFolder { get;  } = "item";
        public string listsFolder { get;  } = "list";
        public int pageSize { get; set; } = 100;

        public string BaseUrl { get; set; } = "https://api.aglty.io";

       // public readonly string BaseUrl = "https://api.aglty.io";

        //public readonly string BaseUrlDev = "https://api-dev.aglty.io";
        /// <summary>
        /// This method will check a portion of the incoming guid to determine the base url to be used for the application.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal string DetermineBaseURL(string guid)
        {
            if (guid.EndsWith("-d"))
            {
                BaseUrl = "https://api-dev.aglty.io";
            }
            else if (guid.EndsWith("-u"))
            {
                BaseUrl = "https://api.aglty.io";
            }
            else if (guid.EndsWith("-ca"))
            {
                BaseUrl = "https://api-ca.aglty.io";
            }
            else if (guid.EndsWith("-eu"))
            {
                BaseUrl = "https://api-eu.aglty.io";
            }
            else
            {
                BaseUrl = "https://api.aglty.io";
            }
            return BaseUrl;
        }
    }
}
