using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.GetContainerItemListSpecs
{
    [TestFixture]
    public class When_retrieving_a_list_items_from_specific_container : TestBase
    {
        [Test]
        public void should_return_no_content_status_when_container_is_empty()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetContainerItemList getContainerItemsRequest = new GetContainerItemList(storageUrl, containerName,
                                                                                         storageToken);
                getContainerItemsRequest.UserAgent = "NASTTestUserAgent";


                GetContainerItemListResponse response =
                    new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));
                response.Dispose();
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
            }
        }

        [Test]
        public void should_return_a_list_of_items_when_container_is_not_empty()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);

                GetContainerItemList getContainerItemsRequest = new GetContainerItemList(storageUrl, containerName,
                                                                                         storageToken);
                getContainerItemsRequest.UserAgent = "NASTTestUserAgent";

                GetContainerItemListResponse response =
                    new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ContentType, Is.Not.Null);
                response.Dispose();
            }
        }

        //TODO: Verify documenation for the test is correct with Eric. https://ryder.racklabs.com/trac/wiki/HenHouseRestApi#GET1
        [Test]
        public void should_return_401_when_the_account_name_is_wrong()
        {
            Uri uri = new Uri("http://henhouse-1.stg.racklabs.com/v1/Persistent");
            GetContainerItemList getContainerItemsRequest = new GetContainerItemList(uri.ToString(), "#%", storageToken);
            getContainerItemsRequest.UserAgent = "NASTTestUserAgent";
            GetContainerItemListResponse response;
            try
            {
                response =
                    new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));
                response.Dispose();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (WebException)));
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNameLengthException))]
        public void Should_throw_an_exception_when_the_container_name_exceeds_the_maximum_number_of_characters_allowed()
        {
            Uri uri = new Uri("http://henhouse-1.stg.racklabs.com/v1/Persistent");
            GetContainerItemList getContainerItemList = new GetContainerItemList(uri.ToString(), new string('a', Constants.MaximumContainerNameLength + 1), storageToken);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_url_is_null()
        {
            GetContainerItemList getContainerItemList = new GetContainerItemList(null, "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_container_name_is_null()
        {
            GetContainerItemList getContainerItemList = new GetContainerItemList("a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_token_is_null()
        {
            GetContainerItemList getContainerItemList = new GetContainerItemList("a", "a", null);
        }

        [Test]
        public void Should_return_ten_objects_when_setting_the_limit_to_ten()
        {
            string containerName = Guid.NewGuid().ToString();

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                for (int i = 0; i < 12; ++i)
                    testHelper.PutItemInContainer(Constants.StorageItemName, i.ToString());

                Dictionary<GetItemListParameters, string> parameters = new Dictionary<GetItemListParameters, string>
                                                                           {{GetItemListParameters.Limit, "10"}};

                GetContainerItemList getContainerItemsRequest = new GetContainerItemList(storageUrl, containerName,
                                                                                         storageToken, parameters);
                getContainerItemsRequest.UserAgent = "NASTTestUserAgent";

                IResponseWithContentBody response =
                    new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                for (int i = 0; i < 12; ++i)
                    testHelper.DeleteItemFromContainer(i.ToString());

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ContentBody.Count, Is.EqualTo(10));

                response.Dispose();
            }
        }

        [Test]
        public void Should_return_objects_starting_with_2_when_setting_prefix_as_2()
        {
            string containerName = Guid.NewGuid().ToString();

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                for (int i = 0; i < 12; ++i)
                    testHelper.PutItemInContainer(Constants.StorageItemName, i.ToString());

                Dictionary<GetItemListParameters, string> parameters = new Dictionary<GetItemListParameters, string>
                                                                           {{GetItemListParameters.Prefix, "2"}};

                GetContainerItemList getContainerItemsRequest = new GetContainerItemList(storageUrl, containerName,
                                                                                         storageToken, parameters);
                getContainerItemsRequest.UserAgent = "NASTTestUserAgent";

                IResponseWithContentBody response =
                    new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                for (int i = 0; i < 12; ++i)
                    testHelper.DeleteItemFromContainer(i.ToString());

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.ContentBody.Count, Is.EqualTo(1));
                Assert.That(response.ContentBody[0], Is.EqualTo("2"));

                response.Dispose();
            }
        }

        [Test]
        public void Should_return_7_objects_when_the_offset_is_5()
        {
            string containerName = Guid.NewGuid().ToString();

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                for (int i = 0; i < 12; ++i)
                    testHelper.PutItemInContainer(Constants.StorageItemName, i.ToString());

                Dictionary<GetItemListParameters, string> parameters = new Dictionary<GetItemListParameters, string>
                                                                           {{GetItemListParameters.Offset, "5"}};

                GetContainerItemList getContainerItemsRequest = new GetContainerItemList(storageUrl, containerName,
                                                                                         storageToken, parameters);
                getContainerItemsRequest.UserAgent = "NASTTestUserAgent";

                GetContainerItemListResponse response =
                    new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                for (int i = 0; i < 12; ++i)
                    testHelper.DeleteItemFromContainer(i.ToString());

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));

                //Assert.That(response.ContentBody.Count, Is.EqualTo(7));

                response.Dispose();
            }
        }

        [Test]
        public void Should_fail_when_an_invalid_paramter_is_passed()
        {
            string containerName = Guid.NewGuid().ToString();

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                for (int i = 0; i < 12; ++i)
                    testHelper.PutItemInContainer(Constants.StorageItemName, i.ToString());

                try
                {
                    Dictionary<GetItemListParameters, string> parameters =
                        new Dictionary<GetItemListParameters, string> {{(GetItemListParameters) int.MaxValue, "2"}};

                    GetContainerItemList getContainerItemsRequest = new GetContainerItemList(storageUrl, containerName,
                                                                                             storageToken, parameters);
                }
                catch (NotImplementedException ne)
                {
                    Assert.That(ne, Is.TypeOf(typeof (NotImplementedException)));
                }
                finally
                {
                    for (int i = 0; i < 12; ++i)
                        testHelper.DeleteItemFromContainer(i.ToString());
                }
            }
        }
    }

    [TestFixture]
    public class When_requesting_a_list_of_containers_with_non_alphanumeric_characters : TestBase
    {
        [Test]
        public void should_not_throw_an_exception_when_the_container_name_starts_with_pound()
        {
            GetContainerItemList getContainerItemList = new GetContainerItemList(storageUrl, "#container", storageToken);

            GetContainerItemListResponse response =
                new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                    new CloudFilesRequest((getContainerItemList)));

            response.Dispose();
            Assert.That(true);
        }

        [Test]
        public void should_not_throw_an_exception_when_the_container_contains_utf8_characters()
        {
            Encoding u8 = Encoding.UTF8;


            string containerName = '\u07FF' + "container";
            GetContainerItemList getContainerItemList = new GetContainerItemList(storageUrl, containerName, storageToken);

            IResponseWithContentBody response =
                new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                    new CloudFilesRequest((getContainerItemList)));

            response.Dispose();
            foreach (string s in response.ContentBody)
                Console.WriteLine(s);
            Assert.That(true);
        }

        [Test]
        public void should_not_throw_an_exception_when_the_container_contains_out_of_range_utf8_characters()
        {

            string containerName = '\uD8CC' + "container";
            GetContainerItemList getContainerItemList = new GetContainerItemList(storageUrl, containerName, storageToken);

            IResponseWithContentBody response =
                new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                    new CloudFilesRequest((getContainerItemList)));

            foreach (string s in response.ContentBody)
                Console.WriteLine(s);
            response.Dispose();
            Assert.That(true);
        }

        [Test]
        public void should_not_throw_an_exception_when_the_container_contains_utf8_characters_3()
        {

            string containerName = '\uDCFF' + "container";
            GetContainerItemList getContainerItemList = new GetContainerItemList(storageUrl, containerName, storageToken);

            IResponseWithContentBody response =
                new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                    new CloudFilesRequest((getContainerItemList)));

            response.Dispose();
            foreach (string s in response.ContentBody)
                Console.WriteLine(s);
            Assert.That(true);
        }
    }

    [TestFixture]
    public class When_requesting_a_list_of_items_in_a_container_in_a_json_serialized_format : TestBase
    {
        [Test]
        public void should_get_json_serialized_list_of_items()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                
                testHelper.PutItemInContainer(Constants .StorageItemName, Constants.StorageItemName);

                GetContainerItemList getContainerItemsRequest = new GetContainerItemList(storageUrl, containerName, storageToken, Format.JSON);
                getContainerItemsRequest.UserAgent = "NASTTestUserAgent";

                GetContainerItemListResponse response =
                        new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                try
                {
                    List<string> contentBody = response.ContentBody;
//                    foreach (string s in response.ContentBody)
//                        Console.WriteLine(s);
                    string expectedItem = "{\"name\": \"TestStorageItem.txt\", \"hash\": \"5c66108b7543c6f16145e25df9849f7f\", \"bytes\": 34, \"content_type\": \"text\\u002fplain\", \"last_modified\": \"" + String.Format("{0:yyyy-MM}", DateTime.Now);
                    bool expectedItemFound = false;
                    foreach(string s in contentBody)
                    {
                        expectedItemFound = (s.IndexOf(expectedItem) > -1);
                    }
                    testHelper.DeleteItemFromContainer(Constants.StorageItemName);

                    Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.ContentType, Is.EqualTo("application/json; charset=utf-8"));
                    Assert.That(expectedItemFound, Is.True, "Expected text " + expectedItem + " was not found");
                }
                finally
                {
                   response.Dispose();
                }
                
            }
        }


    }

    [TestFixture]
    public class When_requesting_a_list_of_items_in_a_container_in_a_xml_serialized_format : TestBase
    {
        [Test]
        public void should_get_xml_serialized_list_of_items()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {

                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);

                GetContainerItemList getContainerItemsRequest = new GetContainerItemList(storageUrl, containerName, storageToken, Format.XML);
                getContainerItemsRequest.UserAgent = "NASTTestUserAgent";

                GetContainerItemListResponse response =
                        new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));


                if(response.ContentBody.Count == 0)
                    Assert.Fail("There was nothing in the content body in the response");

                string contentBody = "";
                foreach(string s in response.ContentBody)
                {
                    contentBody += s;
                }
                
                response.Dispose();
                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(contentBody);
                }
                catch (XmlException e)
                {
                    Console.WriteLine(e.Message);
                }
//                Console.WriteLine(xmlDocument.InnerXml);
                string expectedItem = "<container name=\"" + containerName + "\"><object><name>TestStorageItem.txt</name><hash>5c66108b7543c6f16145e25df9849f7f</hash><bytes>34</bytes><content_type>text/plain</content_type><last_modified>" + String.Format("{0:yyyy-MM}", DateTime.Now);
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ContentType, Is.EqualTo("application/xml; charset=utf-8"));
                Assert.That(contentBody.IndexOf(expectedItem) > 1, Is.True, "Expected text " + expectedItem + " was not found");

            }
        }


    }
}