﻿using Application.Helper.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using RandomDataGenerator;

namespace Tests
{
    [TestClass]
    public class RandomDataExhibitsTests : TestBase
    {
        #region Initialize and Cleanup
        public TestContext TestContext { get; set; }

        readonly static ContentItem ContentItemVideo = new ContentItem
        {
            Title = RandomString.GetRandomString(1, 200, isUsingSpecChars: true),
            Caption = RandomString.GetRandomString(1, 200, isUsingSpecChars: true),
            MediaSource = RandomUrl.GetRandomVideoUrl(),
            MediaType = "Video",
            Attribution = RandomString.GetRandomString(1, 200),
            Uri = RandomUrl.GetRandomVideoUrl()

        };

        readonly static ContentItem ContentItemImage = new ContentItem
        {
            Title = RandomString.GetRandomString(1, 200, isUsingSpecChars: true),
            Caption = RandomString.GetRandomString(1, 200, isUsingSpecChars: true),
            MediaSource = RandomUrl.GetRandomImageUrl(),
            MediaType = "Image",
            Attribution = RandomString.GetRandomString(1,200),
            Uri = RandomUrl.GetRandomImageUrl()

        };

        static readonly Exhibit Exhibit = new Exhibit
        {
            Title = RandomString.GetRandomString(1, 200, isUsingSpecChars: true),
            ContentItems = new Collection<Chronozoom.Entities.ContentItem> { ContentItemVideo, ContentItemImage }
        };

        private static Exhibit _newExhibit;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            BrowserStateManager.RefreshState();
            HomePageHelper.OpenSandboxPage();
            HomePageHelper.DeleteAllElementsLocally();

            ExhibitHelper.AddExhibitWithContentItem(Exhibit);
            _newExhibit = ExhibitHelper.GetNewExhibit();

        }

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (ExhibitHelper.IsExhibitFound(_newExhibit))
            {
                ExhibitHelper.DeleteExhibitByJavascript(_newExhibit);
            }

        }

        [TestCleanup]
        public void TestCleanup()
        {
            CreateScreenshotsIfTestFail(TestContext);
        }

        #endregion

        [TestMethod]
        public void random_new_exhibit_content_should_be_correctly()
        {
            for (int i = 0; i < Exhibit.ContentItems.Count; i++)
            {
                Assert.AreEqual(Exhibit.ContentItems[i].Title, _newExhibit.ContentItems[i].Title, "Content items titles are not equal");
                Assert.AreEqual(Exhibit.ContentItems[i].Caption, _newExhibit.ContentItems[i].Caption, "Content items descriptions are not equal");

                Assert.AreEqual(Exhibit.ContentItems[i].MediaType, _newExhibit.ContentItems[i].MediaType, true, "Content items mediaTypes are not equal");
                Assert.AreEqual(Exhibit.ContentItems[i].MediaSource, _newExhibit.ContentItems[i].MediaSource, "Content items mediaSourses are not equal");
                Assert.AreEqual(Exhibit.ContentItems[i].Attribution, _newExhibit.ContentItems[i].Attribution, "Content items attributions are not equal");
            }
        }

        [TestMethod]
        public void random_new_exhibit_should_have_a_title()
        {
            Assert.AreEqual(Exhibit.Title, _newExhibit.Title, "Titles are not equal");
        }

        [TestMethod]
        public void random_new_exhibit_should_have_a_correct_url()
        {
            for (int i = 0; i < Exhibit.ContentItems.Count; i++)
            {
                Assert.AreEqual(ExhibitHelper.GetExpectedYouTubeUri(Exhibit.ContentItems[i].Uri), _newExhibit.ContentItems[i].Uri,
                                "Content items Urls are not equal");
            }
        }


        [TestMethod]
        public void random_new_exhibit_should_have_a_content_items()
        {
            Assert.AreEqual(Exhibit.ContentItems.Count, _newExhibit.ContentItems.Count, "Content items count are not equal");
        }

        [TestMethod]
        public void random_new_exhibit_should_not_have_null_id()
        {
            Assert.IsNotNull(_newExhibit.Id);
        }

        [TestMethod]
        public void random_new_exhibit_should_be_deleted()
        {
            ExhibitHelper.DeleteExhibit(_newExhibit);
            Assert.IsFalse(ExhibitHelper.IsExhibitFound(_newExhibit));
        }
    }
}