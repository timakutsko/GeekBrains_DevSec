using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Lesson4_ConfigFile
{
    public class CacheProvider
    {
        private static byte[] _additionalEntropy = { 1, 3, 3, 4, 5, 5, 7 };

        public void CacheConnections(List<ConnectionString> connectionStrings)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConnectionString>));
                xmlSerializer.Serialize(xmlWriter, connectionStrings);

                byte[] protectedData = CreateProtect(memoryStream.ToArray());
                File.WriteAllBytes($"{AppDomain.CurrentDomain.BaseDirectory}data.protected", protectedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Serialize data error: {ex.Message}");
            }
        }

        public List<ConnectionString> GetConnectionFromCache()
        {
            try
            {
                byte[] protectedData = File.ReadAllBytes($"{AppDomain.CurrentDomain.BaseDirectory}data.protected");
                byte[] unprotectedData = Unprotect(protectedData);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConnectionString>));
                return (List<ConnectionString>)xmlSerializer.Deserialize(new MemoryStream(unprotectedData));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialize data error: {ex.Message}");
                return null;
            }

        }

        private byte[] CreateProtect(byte[] data)
        {
            try
            {
                return ProtectedData.Protect(data, _additionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Protected error: {ex.Message}");
                return null;
            }
        }

        private byte[] Unprotect(byte[] data)
        {
            try
            {
                return ProtectedData.Unprotect(data, _additionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Unprotected error: {ex.Message}");
                return null;
            }
        }
    }
}
