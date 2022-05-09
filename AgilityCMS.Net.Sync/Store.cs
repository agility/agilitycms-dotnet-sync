using AgilityCMS.Net.Sync.SDK.Models.Item;
using AgilityCMS.Net.Sync.SDK.Models.Page;
using Newtonsoft.Json;


namespace AgilityCMS.Net.Sync.SDK
{
    public class Store
    {
        /// <summary>
        /// Method to fetch Item based on a contentId and locale from the file system. This will build an object of a ContentItems.
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public ContentItems GetContentItem(int contentId, string locale)
        {
            try
            {
                ContentItems items = null;

                if (File.Exists($"{locale}\\{contentId}.json"))
                {
                    items = new ContentItems();
                    items = JsonConvert.DeserializeObject<ContentItems>(File.ReadAllText($"{locale}\\{contentId}.json"));
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to fetch Page based on a pageId and locale from the file system. This will build an object of a PageItems.
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public PageItems GetPage(int pageId, string locale)
        {
            try
            {
                PageItems items = null;
                if (File.Exists($"{locale}\\{pageId}.json"))
                {
                    items = new PageItems();
                    items = JsonConvert.DeserializeObject<PageItems>(File.ReadAllText($"{locale}\\{pageId}.json"));
                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to fetch List based on a referenceName and locale from the file system. This will build an object of List of ContentItems.
        /// </summary>
        /// <param name="referenceName"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public List<ContentItems> GetContentList(string referenceName, string locale)
        {
            try
            {
                List<ContentItems> lists = null;
                if (File.Exists($"{locale}\\{referenceName}.json"))
                {
                    lists = new List<ContentItems>();
                    lists = JsonConvert.DeserializeObject<List<ContentItems>>(File.ReadAllText($"{locale}\\{referenceName}.json"));
                }
                return lists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
