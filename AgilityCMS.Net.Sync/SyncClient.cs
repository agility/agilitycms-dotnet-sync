using AgilityCMS.Net.Sync.SDK;
using AgilityCMS.Net.Sync.SDK.Models;
using AgilityCMS.Net.Sync.SDK.Models.Item;
using AgilityCMS.Net.Sync.SDK.Models.Page;
using Newtonsoft.Json;
using RestSharp;

namespace AgilityCMS.Net.Sync
{
    public class SyncClient
    {
        private readonly string _guid;
        private readonly string _apiKey;
        private readonly bool _isPreview;
        private SyncOptions _syncOptions;
        private RestClient client = null;
        static RestRequest syncApiRequest = null;
        /// <summary>
        /// Property for Store class to access the methods to GetItem, GetPage and GetList to prepare their respective objects.
        /// </summary>
        private Store _store = new Store();
        public Store store { get { return _store; } set { _store = value; } }
        /// <summary>
        /// Constructor to initialize values for Sync SDK.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="apiKey"></param>
        /// <param name="isPreview"></param>
        /// <param name="syncOptions"></param>
        public SyncClient(string guid, string apiKey, bool isPreview, SyncOptions syncOptions)
        {
            _guid = guid;
            _apiKey = apiKey;
            _isPreview = isPreview;
            _syncOptions = syncOptions;
        }
        /// <summary>
        /// This method will create folders for Token, Pages, Contents & Lists based on the exportPath provided by the user.
        /// </summary>
        public void PrepareFolders()
        {
            try
            {
                var mainPath = $"{_syncOptions.rootPath}\\agility_files\\{_guid}";

                if (string.IsNullOrEmpty(_syncOptions.rootPath))
                {
                    throw new Exception("Root path is missing in syncOptions object. Please provide a root path to setup the environment.");
                }
                if (string.IsNullOrEmpty(_syncOptions.locale))
                {
                    throw new Exception("Locale value if missing in syncOptions object. Please provide a locale to setup the environment.");
                }
                if (!Directory.Exists(mainPath))
                {
                    Directory.CreateDirectory(mainPath);
                }
                if (!_isPreview)
                {
                    if (!Directory.Exists($"{mainPath}\\live"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\live");
                    }
                    if (!Directory.Exists($"{mainPath}\\live\\{_syncOptions.locale}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\live\\{_syncOptions.locale}");
                    }
                    if (!Directory.Exists($"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.tokenFolder}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.tokenFolder}");
                    }
                    if (!Directory.Exists($"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.pagesFolder}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.pagesFolder}");
                    }
                    if (!Directory.Exists($"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.contentsFolder}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.contentsFolder}");
                    }
                    if (!Directory.Exists($"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.listsFolder}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.listsFolder}");
                    }
                }
                else
                {
                    if (!Directory.Exists($"{mainPath}\\preview"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\preview");
                    }
                    if (!Directory.Exists($"{mainPath}\\preview\\{_syncOptions.locale}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\preview\\{_syncOptions.locale}");
                    }
                    if (!Directory.Exists($"{mainPath}\\preview\\{_syncOptions.locale}\\{_syncOptions.tokenFolder}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\preview\\{_syncOptions.locale}\\{_syncOptions.tokenFolder}");
                    }
                    if (!Directory.Exists($"{mainPath}\\preview\\{_syncOptions.locale}\\{_syncOptions.pagesFolder}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\preview\\{_syncOptions.locale}\\{_syncOptions.pagesFolder}");
                    }
                    if (!Directory.Exists($"{mainPath}\\preview\\{_syncOptions.locale}\\{_syncOptions.contentsFolder}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\preview\\{_syncOptions.locale}\\{_syncOptions.contentsFolder}");
                    }
                    if (!Directory.Exists($"{mainPath}\\preview\\{_syncOptions.locale}\\{_syncOptions.listsFolder}"))
                    {
                        Directory.CreateDirectory($"{mainPath}\\preview\\{_syncOptions.locale}\\{_syncOptions.listsFolder}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// Method to create a client object for Fetch API.
        /// </summary>
        public void CreateClient()
        {
            try
            {
                var apiType = _isPreview ? "preview" : "fetch";
                var baseURL = _syncOptions.DetermineBaseURL(_guid);
                client = new RestClient($"{baseURL}/{_guid}/{apiType}");
                //client = new RestClient($"{(_guid != null && _guid.EndsWith("-d") ? _syncOptions.BaseUrlDev : _syncOptions.BaseUrl)}/{_guid}/{apiType}");
                client.AddDefaultHeader("accept", "application/json");
                client.AddDefaultHeader("APIKey", _apiKey ?? string.Empty);
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// SyncPages method performs following steps - 
        /// Retrives the value of the sync Token from ReadToken method.
        /// Calls the API to fill the "syncPages" object.
        /// Checks the token retrieved from the file is equal to the latest retrieved syncToken value. If not, overrrides the file with the new value.
        /// Export the pages file with the pageId.json file.
        /// Runs a loop till the value of syncToken is 0 and items collection has been processed and continously grab the object to see if the items collection has any value or not.
        /// </summary>
        public void SyncPages()
        {
            try
            {
                PrepareFolders();
                CreateClient();
                var busy = false;
                var waitMS = 0;
                long syncToken = 0;
                string tokenPath = string.Empty;
                var pageExport = string.Empty;
                if (!_isPreview)
                {
                    tokenPath += $"{_guid}\\live\\{_syncOptions.locale}\\{_syncOptions.tokenFolder}\\{_syncOptions.tokenFile}";
                    pageExport += $"{_guid}\\live\\{_syncOptions.locale}\\{_syncOptions.pagesFolder}";
                }
                else
                {
                    tokenPath += $"{_guid}\\preview\\{_syncOptions.locale}\\{_syncOptions.tokenFolder}\\{_syncOptions.tokenFile}";
                    pageExport += $"{_guid}\\preview\\{_syncOptions.locale}\\{_syncOptions.pagesFolder}";
                }
                do
                {
                    var pageTokenInfo = ReadToken(tokenPath);
                    var syncPages = GetSyncPages(pageTokenInfo.pageToken, _syncOptions.pageSize);
                    syncToken = syncPages.syncToken;
                    if (syncPages == null || (syncPages.busy))
                    {
                        waitMS += _syncOptions.retryInterval;
                        if (waitMS > _syncOptions.retryTimeout)
                        {
                            break;
                        }
                        if (!busy)
                        {
                            busy = true;
                        }
                        Thread.Sleep(_syncOptions.retryInterval);
                        continue;
                    }
                    if (busy)
                    {
                        waitMS = 0;
                        busy = false;
                    }

                    if (syncPages.items.Count > 0)
                    {
                        if (!IsSameToken(syncPages.syncToken, pageTokenInfo.pageToken) && syncPages.syncToken != 0)
                        {
                            pageTokenInfo.pageToken = syncPages.syncToken;
                            pageTokenInfo.lastSyncDate = DateTime.Now;
                            var tokenContent = JsonConvert.SerializeObject(pageTokenInfo);
                            Export(tokenContent, $"{tokenPath}");
                        }
                        foreach (var item in syncPages.items)
                        {
                            var jsonContent = JsonConvert.SerializeObject(item);
                            Export(jsonContent, Convert.ToString($"{pageExport}\\{item.pageID}"));
                        }
                    }
                } while (syncToken > 0 || busy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// SyncContent method performs following steps - 
        /// Retrives the value of the sync Token from ReadToken method.
        /// Calls the API to fill the "syncContent" object.
        /// Checks the token retrieved from the file is equal to the latest retrieved syncToken value. If not, overrrides the file with the new value.
        /// Export the pages file with the contentId.json file and referenceName,json file for combined item(s) who have same referenceName.
        /// Runs a loop till the value of syncToken is 0 and items collection has been processed and continously grab the object to see if the items collection has any value or not.
        /// </summary>
        public void SynContent()
        {
            try
            {
                PrepareFolders();
                CreateClient();
                var busy = false;
                var waitMS = 0;
                long syncToken = 0;
                string tokenPath = string.Empty;
                string contentExport = String.Empty;
                string listExport = String.Empty;

                if (!_isPreview)
                {
                    tokenPath += $"{_guid}\\live\\{_syncOptions.locale}\\{_syncOptions.tokenFolder}\\{_syncOptions.tokenFile}";
                    contentExport += $"{_guid}\\live\\{_syncOptions.locale}\\{_syncOptions.contentsFolder}";
                    listExport += $"{_guid}\\live\\{_syncOptions.locale}\\{_syncOptions.listsFolder}";
                }
                else
                {
                    tokenPath += $"{_guid}\\preview\\{_syncOptions.locale}\\{_syncOptions.tokenFolder}\\{_syncOptions.tokenFile}";
                    contentExport += $"{_guid}\\preview\\{_syncOptions.locale}\\{_syncOptions.contentsFolder}";
                    listExport += $"{_guid}\\preview\\{_syncOptions.locale}\\{_syncOptions.listsFolder}";
                }
                do
                {
                    var tokenInfo = ReadToken(tokenPath);
                    var syncContent = GetSyncContent(tokenInfo.itemToken, _syncOptions.pageSize);
                    syncToken = syncContent.syncToken;
                    if (syncContent == null || (syncContent.busy))
                    {
                        waitMS += _syncOptions.retryInterval;
                        if (waitMS > _syncOptions.retryInterval)
                        {
                            break;
                        }
                        if (!busy)
                        {
                            busy = true;
                        }
                        Thread.Sleep(_syncOptions.retryInterval);
                        continue;
                    }
                    if (busy)
                    {
                        waitMS = 0;
                        busy = false;
                    }
                    if (syncContent.items.Count > 0)
                    {
                        if (!IsSameToken(syncContent.syncToken, tokenInfo.itemToken) && syncContent.syncToken != 0)
                        {
                            tokenInfo.itemToken = syncContent.syncToken;
                            tokenInfo.lastSyncDate = DateTime.Now;
                            var tokenContent = JsonConvert.SerializeObject(tokenInfo);
                            Export(tokenContent, $"{tokenPath}");
                        }
                        foreach (var item in syncContent.items)
                        {
                            var jsonContent = JsonConvert.SerializeObject(item);
                            Export(jsonContent, Convert.ToString($"{contentExport}\\{item.contentID}"));
                        }
                        var referenceList = syncContent.items.GroupBy(x => x.properties.referenceName).ToList();
                        var jsonList = string.Empty;
                        foreach (var list in referenceList)
                        {
                            if (ListExists($"{_syncOptions.rootPath}\\agility_files\\{listExport}\\{list.Key}.json"))
                            {
                                var existingList = JsonConvert.DeserializeObject<List<ContentItems>>(File.ReadAllText($"{_syncOptions.rootPath}\\agility_files\\{listExport}\\{list.Key}.json"));
                                var concatenatedList = list.Concat(existingList);
                                jsonList = JsonConvert.SerializeObject(concatenatedList);
                            }
                            else
                            {
                                jsonList = JsonConvert.SerializeObject(list);
                            }

                            if (list.Key != null)
                                Export(jsonList, $"{listExport}\\{list.Key.ToString()}");
                        }
                    }
                } while (syncToken > 0 || busy);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// This method will execute the SyncPages API based on the syncToken and pageSize.
        /// Once the object is retrieved, will be deserialized as a Page object.
        /// The resultant Page object is returned from the method.
        /// </summary>
        /// <param name="pagesSyncToken"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private Page GetSyncPages(long pagesSyncToken, int pageSize)
        {
            try
            {
                syncApiRequest = new RestRequest($"/{_syncOptions.locale}/sync/pages?syncToken={pagesSyncToken}&pageSize={pageSize}", Method.GET);
                var restRespose = client.Execute<string>(syncApiRequest).Data;
                if (restRespose == null)
                {
                    throw new Exception("Unable to sync Pages. Please check the API Key, GUID, IsPreview and SyncOptions.locale values.");
                }
                var page = JsonConvert.DeserializeObject<Page>(restRespose);
                return page;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method will execute the SyncContent API based on the syncToken and pageSize.
        /// Once the object is retrieved, will be deserialized as a Page object.
        /// The resultant Content object is returned from the method.
        /// </summary>
        /// <param name="pagesSyncToken"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private Content GetSyncContent(long contentSyncToken, int pageSize)
        {
            try
            {
                syncApiRequest = new RestRequest($"/{_syncOptions.locale}/sync/items?syncToken={contentSyncToken}&pageSize={pageSize}", Method.GET);
                var restRespose = client.Execute<string>(syncApiRequest).Data;
                if (restRespose == null)
                {
                    throw new Exception("Unable to sync Content. Please check the API Key, GUID, IsPreview and SyncOptions.locale values.");
                }
                var content = JsonConvert.DeserializeObject<Content>(restRespose);
                return content;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Read the token for sync pages or sync content from the file system.
        /// Returns a TokenInfo object.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private TokenInfo ReadToken(string fileName)
        {
            try
            {
                if (File.Exists($"{_syncOptions.rootPath}\\agility_files\\{fileName}.json"))
                {
                    var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(File.ReadAllText($"{_syncOptions.rootPath}\\agility_files\\{fileName}.json"));
                    return tokenInfo;

                }
                return new TokenInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to check if the token from the file is equal to the token retrieved from the API.
        /// Returns false if tokens are different else true.
        /// </summary>
        /// <param name="newToken"></param>
        /// <param name="tokenInfo"></param>
        /// <returns>bool</returns>
        private bool IsSameToken(long newToken, long oldToken)
        {
            try
            {
                if (newToken != 0 && newToken != oldToken)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Export method takes string content and fileName as an input to generate files.
        /// Common method to generate Token, Pages, Content and Lists json files in their respective folders.
        /// If a file already exists, the method will delete the file first and generate a new file with the same name with the latest content.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        private void Export(string content, string fileName)
        {
            try
            {
                if (File.Exists($"{_syncOptions.rootPath}\\agility_files\\{fileName}.json"))
                {
                    File.Delete($"{_syncOptions.rootPath}\\agility_files\\{fileName}.json");
                }
                File.WriteAllText($"{_syncOptions.rootPath}\\agility_files\\{fileName}.json", content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to check if a list (referenceName.json) already exists to sync lists.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool ListExists(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method will clear out all json files generated from Sync Methods. This will loop into the rootPath and find all the .json files and delete them to reset the environment.
        /// </summary>
        public void ClearSync()
        {
            try
            {
                var mainPath = $"{_syncOptions.rootPath}\\agility_files\\{_guid}";
                var extensions = ".json";
                string[] files = Directory.GetFiles(mainPath, "*.*", SearchOption.AllDirectories)
                                    .Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0).ToArray();
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
    }
}