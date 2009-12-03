using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rackspace.CloudFiles.domain;

namespace Rackspace.CloudFiles.unit.tests.domain
{
    
    [TestFixture]
    public class When_checking_account
    {
        private AccountInformation accountInformation;

        [SetUp]
        public void SetUp()
        {
            accountInformation = new AccountInformation("3", "1024");
        }

        [Test]
        public void should_return_the_number_of_containers()
        {
            Assert.That(accountInformation.ContainerCount, Is.EqualTo(3));
        }

        [Test]
        public void should_return_the_bytes_used()
        {
            Assert.That(accountInformation.BytesUsed, Is.EqualTo(1024));
        }
    }

    [TestFixture]
    public class When_creating_account_with_invalid_arguments
    {
        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void should_throw_an_exception_when_the_container_name_is_empty()
        {
            new AccountInformation("", "");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void should_throw_an_exception_when_the_container_name_is_null()
        {
            new AccountInformation(null, "");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void should_throw_an_exception_when_the_bytes_used_is_empty()
        {
            new AccountInformation("3", "");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void should_throw_an_exception_when_the_bytes_used_is_null()
        {
            new AccountInformation("3", null);
        }

        [Test]
        [ExpectedException(typeof (FormatException))]
        public void should_throw_an_exception_when_the_container_name_is_invalid()
        {
            new AccountInformation("hello", "1231");
        }

        [Test]
        [ExpectedException(typeof (FormatException))]
        public void should_throw_an_exception_when_the_bytes_used_is_invalid()
        {
            new AccountInformation("1231", "hello");
        }
    }
}