///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Specialized;
using System.Web;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// DeleteContainer
    /// </summary>
    public class DeleteContainer : BaseRequest
    {
        /// <summary>
        /// DeleteContainer constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameLengthException">Thrown when the container name length exceeds the maximum container length allowed</exception>
        public DeleteContainer(string storageUrl, string containerName, string storageToken)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken)
                || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            if (containerName.Length > Constants.MAXIMUM_CONTAINER_NAME_LENGTH)
                throw new ContainerNameLengthException("Container name " + containerName + " exceeds " +
                                                       Constants.MAXIMUM_CONTAINER_NAME_LENGTH + " character limit");

            Uri = new Uri(storageUrl + "/" + HttpUtility.UrlEncode(containerName).Replace("+", "%20"));
            Method = "DELETE";

            Headers.Add("X-Storage-Token", HttpUtility.UrlEncode(storageToken));
        }
    }
}