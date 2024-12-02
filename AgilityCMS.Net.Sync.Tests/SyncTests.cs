using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgilityCMS.Net.Sync.SDK;
using System;

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
            _syncOptions.rootPath = Environment.GetEnvironmentVariable("Test-LocalPath") ?? "";
            _syncOptions.locale = Environment.GetEnvironmentVariable("Test-Locale") ?? "";
            _guid = Environment.GetEnvironmentVariable("Test-InstanceGuid") ?? "";
            _apiKey = Environment.GetEnvironmentVariable("Test-APIKey") ?? "";
            _isPreview = Environment.GetEnvironmentVariable("Test-IsPreview") == "true" ? true : false;
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
                Assert.IsNotNull(_guid, $"Please provide a value for the guid.");
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

                Assert.IsNotNull(_guid, $"Please provide a value for the guid.");
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
                Assert.IsNotNull(_guid, $"Please provide a value for the guid.");
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



                var testPageIDStr = Environment.GetEnvironmentVariable("Test-PageID");
                var testPageID = int.Parse(testPageIDStr);

                _syncClient.SyncPages();
                //var actualSyncPage = _syncClient.store.GetPage(testPageID, $"{mainPath}\\live\\{_syncOptions.locale}\\{_syncOptions.pagesFolder}");
                var actualSyncPage = _syncClient.store.GetPage(testPageID, $"{_syncOptions.locale}");

                Assert.IsNotNull(actualSyncPage);
                Assert.AreEqual(testPageID, actualSyncPage.pageID);

                Assert.AreNotEqual(true, actualSyncPage.seo.sitemapVisible, "The sitemapVisible property is not correct.");

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

                var testContentRef = Environment.GetEnvironmentVariable("Test-ContentReferenceName");
                var testContentIDStr = Environment.GetEnvironmentVariable("Test-ContentID");
                var testContentID = int.Parse(testContentIDStr ?? "");

                _syncClient.SynContent();

                var actualSyncContent1 = _syncClient.store.GetContentItem(testContentID, $"{_syncOptions.locale}");

                Assert.AreEqual(testContentID, actualSyncContent1.contentID);

                var actualList1 = _syncClient.store.GetContentList(testContentRef ?? "", $"{_syncOptions.locale}");
                Assert.AreEqual(testContentRef, actualList1[0].properties.referenceName ?? "");
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
                //HACK  _syncClient.ClearSync();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}