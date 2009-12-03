using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rackspace.CloudFiles.domain;

namespace Rackspace.CloudFiles.unit.tests.domain.StorageItemSpecs
{
    [TestFixture]
    public class When_creating_a_storage_object
    {
        private StorageItem storageItem;

        [SetUp]
        public void SetUp()
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.META_KEY1, Constants.META_VALUE1},
                                                          {Constants.META_KEY2, Constants.META_VALUE2}
                                                      };

            storageItem = new StorageItem(Constants.STORAGE_OBJECT_FILE_NAME, metadata, "text/plain", 0, new DateTime());
        }

        [TearDown]
        public void TearDown()
        {
            storageItem.Dispose();
        }

        [Test]
        public void Should_have_storage_object_name()
        {
            
            Assert.That(storageItem.ObjectName, Is.EqualTo(Constants.STORAGE_OBJECT_FILE_NAME));
        }

        [Test]
        public void Should_have_content_type()
        {
            Assert.That(storageItem.ContentType, Is.EqualTo(Constants.STORAGE_OBJECT_CONTENT_TYPE));
        }

        [Test]
        public void Should_have_file_stream()
        {
            Assert.That(storageItem.ObjectName, Is.Not.Null);
        }
        [Test]
        public void Should_have_last_modified_time()
        {
            Assert.That(storageItem.LastModified, Is.Not.Null);
        }
        [Test]
        public void Should_have_meta_tags()
        {
            Assert.That(storageItem.Metadata[Constants.META_KEY1], Is.EqualTo(Constants.META_VALUE1));
            Assert.That(storageItem.Metadata[Constants.META_KEY2], Is.EqualTo(Constants.META_VALUE2));
        }
    }
}