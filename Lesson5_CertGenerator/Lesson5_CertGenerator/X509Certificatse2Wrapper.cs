using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lesson5_CertGenerator
{
    public class X509Certificatse2Wrapper
    {
        private X509Certificate2 _cert = null;
        private string _group = null;
        private string _certGroupName = null;

        public X509Certificatse2Wrapper(X509Certificate2 cert, string group, string certGroupName)
        {
            _cert = cert;
            _group = group;
            _certGroupName = certGroupName;
        }

        public X509Certificate2 Cert { get { return _cert; } }

        public string PublishedFor { get { return _cert.GetNameInfo(X509NameType.SimpleName, false); } }

        public string Published { get { return _cert.GetNameInfo(X509NameType.SimpleName, true); } }

        public string ExpirationDate { get { return _cert.GetExpirationDateString(); } }

        public string Group { get { return _group; } }

        public string CertGroupName { get { return _certGroupName; } }

        public override string ToString()
        {
            return $"Group: {Group} ({CertGroupName})\nPublisherFor: {PublishedFor}\nPubleshed: {Published}\nExp: {ExpirationDate}\n";
        }

    }
}
