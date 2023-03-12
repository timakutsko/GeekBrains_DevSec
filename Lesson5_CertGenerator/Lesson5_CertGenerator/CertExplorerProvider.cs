using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lesson5_CertGenerator
{
    public class CertExplorerProvider
    {
        private string[] _certStores = new string[]
        {
            "LocalMachine.My",
            "CurrentUser.My",
            "LocalMachine.Root",
            "CurrentUser.Root",
        };
        private List<X509Certificatse2Wrapper> _cerList;
        private bool _requirePrivateKey;
        
        private const string CURRENT_USER_MY = "Текущий пользователь - Личные";
        private const string LOACL_MACHINE_MY = "Локальный компьютер - Личные";
        private const string CURRENT_USER_ROOT = "Текущий пользователь - Доверенные корневые центры сертификации";
        private const string LOACL_MACHINE_ROOT = "Локальный компьютер - Доверенные корневые центры сертификации";

        public CertExplorerProvider(bool requirePrivateKey)
        {
            _requirePrivateKey = requirePrivateKey;
        }

        public List<X509Certificatse2Wrapper> Certificates
        {
            get { return _cerList; }
        }

        public void LoadCertificates()
        {
            _cerList = new List<X509Certificatse2Wrapper>();
            foreach (var store in _certStores)
            {
                _cerList.AddRange(LoadStore(store));
            }
        }
        public static StoreName ExtractStoreName(string store)
        {
            return (StoreName)Enum.Parse(typeof(StoreName), store.Substring(store.IndexOf('.') + 1));
        }

        public static StoreLocation ExtractStoreLoaction(string store)
        {
            return (StoreLocation)Enum.Parse(typeof(StoreLocation), store.Substring(0, store.IndexOf('.')));
        }

        private List<X509Certificatse2Wrapper> LoadStore(string storeName)
        {
            X509Store store = new X509Store(ExtractStoreName(storeName), ExtractStoreLoaction(storeName));
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            try
            {
                return CertificateToView(store.Certificates, storeName);
            }
            finally
            {
                store.Close();
            }
        }

        private List<X509Certificatse2Wrapper> CertificateToView(X509Certificate2Collection certificates, string groupName)
        {
            List<X509Certificatse2Wrapper> certList = new List<X509Certificatse2Wrapper>();
            foreach (X509Certificate2 cert in certificates)
            {
                string groupDesc = null;
                switch (groupName)
                {
                    case "LocalMachine.My":
                        groupDesc = CURRENT_USER_MY;
                        break;
                    case "CurrentUser.My":
                        groupDesc = LOACL_MACHINE_MY;
                        break;
                    case "LocalMachine.Root":
                        groupDesc = CURRENT_USER_ROOT;
                        break;
                    case "CurrentUser.Root":
                        groupDesc = LOACL_MACHINE_ROOT;
                        break;
                }
                if (_requirePrivateKey)
                {
                    if (cert.HasPrivateKey)
                        certList.Add(new X509Certificatse2Wrapper(cert, groupName, groupDesc));
                }
                else
                    certList.Add(new X509Certificatse2Wrapper(cert, groupName, groupDesc));
            }
            return certList;
        }
    }
}
