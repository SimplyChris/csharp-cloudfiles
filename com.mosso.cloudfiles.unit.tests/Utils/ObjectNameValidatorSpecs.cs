using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rackspace.CloudFiles.utils;

namespace Rackspace.CloudFiles.Specs.Utils
{
    [TestFixture]
    public class When_a_object_name_has_no_invalid_characters
    {
        [Test]
        public void should_be_valid()
        {
            var objectName = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()-+={}[]|\'\":;,.<>~`/";
            Assert.That(ObjectNameValidator.Validate(objectName), Is.True);

        }
    }

    [TestFixture]
    public class When_a_object_name_has_a_question_mark
    {
        [Test]
        public void should_be_invalid()
        {
            var objectName = "objectName?withQuestionMark";
            Assert.That(ObjectNameValidator.Validate(objectName), Is.False);

        }
    }

    [TestFixture]
    public class When_a_object_name_is_valid_length
    {
        [Test]
        public void should_be_valid()
        {
            var objectName = new string('a', ObjectNameValidator.MAX_OBJECT_NAME_LENGTH);
            Assert.That(ObjectNameValidator.Validate(objectName), Is.True);

        }
    }

    [TestFixture]
    public class When_a_object_name_is_invalid_length
    {
        [Test]
        public void should_be_invalid()
        {
            var objectName = new string('a', ObjectNameValidator.MAX_OBJECT_NAME_LENGTH + 1);
            Assert.That(ObjectNameValidator.Validate(objectName), Is.False);

        }
    }
}