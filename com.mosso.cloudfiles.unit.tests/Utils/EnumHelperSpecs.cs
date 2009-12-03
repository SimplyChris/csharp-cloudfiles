using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rackspace.CloudFiles.utils;
using DescriptionAtt = System.ComponentModel.DescriptionAttribute;

namespace Rackspace.CloudFiles.unit.tests.Utils.EnumHelperSpecs
{
    [TestFixture]
    public class When_accessing_the_description_attribute_of_an_enum
    {
        [Test]
        public void Should_be_able_to_get_the_description_text()
        {
            Assert.That(EnumHelper.GetDescription(TestEnum.Test1), Is.EqualTo("Test 1 Attribute"));
            Assert.That(EnumHelper.GetDescription(TestEnum.Test2), Is.EqualTo("Test 2 Attribute"));
            Assert.That(EnumHelper.GetDescription(TestEnum.Test3), Is.EqualTo("Test 3 Attribute"));
        }
    }

    internal enum TestEnum
    {
        [DescriptionAtt("Test 1 Attribute")] Test1,
        [DescriptionAtt("Test 2 Attribute")] Test2,
        [DescriptionAtt("Test 3 Attribute")] Test3
    }
}