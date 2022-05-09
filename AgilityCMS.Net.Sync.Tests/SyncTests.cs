using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgilityCMS.Net.Sync.SDK;
using System;
using Newtonsoft.Json;
using AgilityCMS.Net.Sync.SDK.Models.Page;
using System.IO;
using AgilityCMS.Net.Sync.SDK.Models.Item;
using System.Collections.Generic;

namespace AgilityCMS.Net.Sync.Tests
{
    [TestClass]
    public class SyncTests
    {
        private readonly SyncOptions _syncOptions;
        private readonly string _guid;
        private readonly string _apiKey;
        private readonly bool _isPreview;
        private SyncClient _syncClient;
        public SyncTests()
        {
            _syncOptions = new SyncOptions();
            _syncOptions.rootPath = @"<<Your Local Path>>";
            _syncOptions.locale = "<<Mention the Locale example en-us>>";
            _guid = "<<Provide your Instance GUID>>";
            _apiKey = "<<Provide your API Key>>";
            _isPreview = false;
            _syncClient = new SyncClient(_guid, _apiKey, _isPreview, _syncOptions);
        }
        [TestMethod]
        public void CreateEnvironment()
        {
            try
            {
                _syncClient.PrepareFolders();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
        [TestMethod]
        public void TestClient()
        {
            try
            {
                Assert.IsNotNull(_guid, $"Please provide a value to guid.");
                Assert.IsNotNull(_syncOptions.rootPath, $"Please provide a path to SyncOptions.rootpath.");
                Assert.IsNotNull(_syncOptions.locale, $"Please provide a value to SyncOptions.locale.");
                Assert.IsNotNull(_apiKey, $"Please provide an API Key");
                _syncClient.CreateClient();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void SyncPage()
        {
            try
            {
                Assert.IsNotNull(_guid, $"Please provide a value to guid.");
                Assert.IsNotNull(_syncOptions.rootPath, $"Please provide a path to SyncOptions.rootpath.");
                Assert.IsNotNull(_syncOptions.locale, $"Please provide a value to SyncOptions.locale.");
                Assert.IsNotNull(_apiKey, $"Please provide an API Key");
                _syncClient.SyncPages();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod]
        public void SyncContent()
        {
            try
            {
                Assert.IsNotNull(_guid, $"Please provide a value to guid.");
                Assert.IsNotNull(_syncOptions.rootPath, $"Please provide a path to SyncOptions.rootpath.");
                Assert.IsNotNull(_syncOptions.locale, $"Please provide a value to SyncOptions.locale.");
                Assert.IsNotNull(_apiKey, $"Please provide an API Key");
                _syncClient.SynContent();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void CompareSyncPage()
        {
            try
            {
                var mainPath = $"{_syncOptions.rootPath}\\agility_files\\{_guid}";
                var expectedSyncPage = JsonConvert.DeserializeObject<PageItems>(File.ReadAllText(@"<<Provide your local file path extracted for a Page Id>>"));
                _syncClient.SyncPages();
                var actualSyncPage = _syncClient.store.GetPage(15, $"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.pagesFolder}");
                Assert.AreEqual(expectedSyncPage.pageID, actualSyncPage.pageID);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod]
        public void CompareSyncItem()
        {
            try
            {
                var mainPath = $"{_syncOptions.rootPath}\\agility_files\\{_guid}";
                var expectedSyncContent = JsonConvert.DeserializeObject<ContentItems>(File.ReadAllText(@"<<Provide your local file path extracted for a Content Id>>"));

                var expectedList = JsonConvert.DeserializeObject<List<ContentItems>>(File.ReadAllText(@"<<Provide your local file path extracted for a Reference Name>>"));
                _syncClient.SynContent();
                var actualSyncContent1 = _syncClient.store.GetContentItem(290, $"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.contentsFolder}");
                Assert.AreEqual(expectedSyncContent.contentID, actualSyncContent1.contentID);

                var actualList1 = _syncClient.store.GetContentList("contentfortestingwithmorefields", $"{ mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.listsFolder}");
                Assert.AreEqual(expectedList[0].properties.referenceName, actualList1[0].properties.referenceName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void ClearSync()
        {
            try
            {
                _syncClient.ClearSync();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}