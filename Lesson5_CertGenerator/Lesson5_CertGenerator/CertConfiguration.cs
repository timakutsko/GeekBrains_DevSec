using System.Security.Cryptography.X509Certificates;

namespace Lesson5_CertGenerator
{
    public class CertConfiguration
    {
        public X509Certificate2 RootCert { get; set; }

        public int CertDuration { get; set; }

        public string CertName { get; set; }

        public string Password { get; set; }

        public string OutFolder { get; set; }

        public string Email { get; set; }
    }
}
