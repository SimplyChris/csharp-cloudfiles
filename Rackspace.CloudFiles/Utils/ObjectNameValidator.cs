using System.Text.RegularExpressions;

namespace Rackspace.CloudFiles.utils
{
    public class ObjectNameValidator
    {
        public const int MAX_OBJECT_NAME_LENGTH = 1024;

        public static bool Validate(string objectName)
        {
            return objectName.IndexOf("?") < 0 &&
                   objectName.Length <= MAX_OBJECT_NAME_LENGTH;
        }
    }
}