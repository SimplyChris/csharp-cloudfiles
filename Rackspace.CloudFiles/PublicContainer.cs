using System;
using Rackspace.CloudFiles.Interfaces;

namespace Rackspace.CloudFiles
{
    public class PublicContainer:Container
    {
        public PublicContainer(string containerName, IAccount request) : base(containerName, request)
        {
        }
        public Uri CdnUri
        {
            get; private set;
        }
        public int TTL
        {
            get; private set;
        }
        public bool LogRetention
        {
            get; private set;
        }
        public string UserAgentACL
        {
            get; private set;
        }
        public string ReferrerACL
        {
            get; private set;
        }
 
    }
}