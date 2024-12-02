using AgilityCMS.Net.Sync.SDK.Models.Item;
using AgilityCMS.Net.Sync.SDK.Models.Page;
using Newtonsoft.Json;


namespace AgilityCMS.Net.Sync.SDK
{
	public class Store
	{

		private readonly string _rootPath;

		public Store(string rootPath)
		{
			_rootPath = rootPath;
		}

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

				string fullPath = Path.Combine(_rootPath, locale, "item", $"{contentId}.json");

				if (File.Exists(fullPath))
				{
					items = new ContentItems();
					items = JsonConvert.DeserializeObject<ContentItems>(File.ReadAllText(fullPath));
				}

				return items;
			}
			catch (Exception ex)
			{
				throw new Exception("Error getting content item", ex);
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
				string fullPath = Path.Combine(_rootPath, locale, "page", $"{pageId}.json");
				PageItems items = null;
				if (File.Exists(fullPath))
				{
					items = new PageItems();
					items = JsonConvert.DeserializeObject<PageItems>(File.ReadAllText(fullPath));
				}
				return items;
			}
			catch (Exception ex)
			{
				throw new Exception("Error getting page", ex);
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

				string fullPath = Path.Combine(_rootPath, locale, "list", $"{referenceName}.json");

				if (File.Exists(fullPath))
				{
					lists = new List<ContentItems>();
					lists = JsonConvert.DeserializeObject<List<ContentItems>>(File.ReadAllText(fullPath));
				}
				return lists;
			}
			catch (Exception ex)
			{
				throw new Exception("Error getting list", ex);
			}
		}
	}
}
