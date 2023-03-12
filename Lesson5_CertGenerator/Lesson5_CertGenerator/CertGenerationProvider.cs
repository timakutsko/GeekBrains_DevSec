using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Lesson5_CertGenerator
{
    public class CertGenerationProvider
    {
        public void GenerateRootCert(CertConfiguration certConf)
        {
            // Генерация пары ключей
            SecureRandom secRand = new SecureRandom();
            RsaKeyPairGenerator rsaGenerator = new RsaKeyPairGenerator();
            RsaKeyGenerationParameters rsaParameters = new RsaKeyGenerationParameters(new Org.BouncyCastle.Math.BigInteger("10001", 16), secRand, 1024, 80);
            rsaGenerator.Init(rsaParameters);
            AsymmetricCipherKeyPair asymmetricKeyPair = rsaGenerator.GenerateKeyPair();

            string issuer = $"CN={certConf.CertName}";
            
            // Имена файлов
            string p12FileName = $"{certConf.OutFolder}\\{certConf.CertName}.p12";
            string crtFileName = $"{certConf.OutFolder}\\{certConf.CertName}.crt";

            // Серийный номер сертификата
            byte[] serialNumber = Guid.NewGuid().ToByteArray();
            serialNumber[0] = (byte)(serialNumber[0] & 0x7F);

            X509V3CertificateGenerator certGen = new X509V3CertificateGenerator();
            certGen.SetSerialNumber(new Org.BouncyCastle.Math.BigInteger(1, serialNumber));
            certGen.SetIssuerDN(new X509Name(issuer));
            certGen.SetNotBefore(DateTime.Now.ToUniversalTime());
            certGen.SetNotAfter(DateTime.Now.ToUniversalTime() + new TimeSpan(certConf.CertDuration * 365, 0, 0, 0));
            certGen.SetSubjectDN(new X509Name(issuer));
            certGen.SetPublicKey(asymmetricKeyPair.Public);
            certGen.SetSignatureAlgorithm("MD5WITHRSA");
            certGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(asymmetricKeyPair.Public));
            certGen.AddExtension(X509Extensions.SubjectKeyIdentifier, false, new SubjectKeyIdentifierStructure(asymmetricKeyPair.Public));
            certGen.AddExtension(X509Extensions.BasicConstraints, false, new BasicConstraints(true));

            X509Certificate rootCert = certGen.Generate(asymmetricKeyPair.Private);

            // Подписанный сертификат
            byte[] rawCert = rootCert.GetEncoded();

            // Сохранение закрытой части сертификата
            try
            {
                using (FileStream fs = new FileStream(p12FileName, FileMode.Create))
                {
                    Pkcs12Store p12 = new Pkcs12Store();
                    X509CertificateEntry certEntry = new X509CertificateEntry(rootCert);
                    p12.SetKeyEntry(certConf.CertName, new AsymmetricKeyEntry(asymmetricKeyPair.Private), new X509CertificateEntry[] {certEntry});
                    p12.Save(fs, certConf.Password.ToCharArray(), secRand);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw new CertGenerationException($"Ошибка при сохранении закрытой части сертификата:\n {ex.Message}");
            }

            // Сохранение открытой части сертификата
            try
            {
                using (FileStream fs = new FileStream(crtFileName, FileMode.Create))
                {
                    fs.Write(rawCert, 0, rawCert.Length);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw new CertGenerationException($"Ошибка при сохранении открытой части сертификата:\n {ex.Message}");
            }
        }

        public void GenerateCert(CertConfiguration certConf)
        {
            X509Certificate rootCertInternal = DotNetUtilities.FromX509Certificate(certConf.RootCert);

            // Генерация пары ключей
            SecureRandom secRand = new SecureRandom();
            RsaKeyPairGenerator keyGen = new RsaKeyPairGenerator();
            RsaKeyGenerationParameters rsaPrms = new RsaKeyGenerationParameters(new Org.BouncyCastle.Math.BigInteger("10001", 16), secRand, 1024, 80);
            keyGen.Init(rsaPrms);
            AsymmetricCipherKeyPair asymmetricKeyPair = keyGen.GenerateKeyPair();

            string subject = $"CN={certConf.CertName}";

            // Имена файлов
            string p12FileName = $"{certConf.OutFolder}\\{certConf.CertName}.p12";

            // Серийный номер сертификата
            byte[] serialNumber = Guid.NewGuid().ToByteArray();
            serialNumber[0] = (byte)(serialNumber[0] & 0x7F);

            X509V3CertificateGenerator certGen = new X509V3CertificateGenerator();
            certGen.SetSerialNumber(new Org.BouncyCastle.Math.BigInteger(1, serialNumber));
            certGen.SetIssuerDN(rootCertInternal.IssuerDN);
            certGen.SetNotBefore(DateTime.Now.ToUniversalTime());
            certGen.SetNotAfter(DateTime.Now.AddDays(100));
            certGen.SetSubjectDN(new X509Name(subject));
            certGen.SetPublicKey(asymmetricKeyPair.Public);
            certGen.SetSignatureAlgorithm("MD5WITHRSA");
            certGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(rootCertInternal.GetPublicKey()));
            certGen.AddExtension(X509Extensions.SubjectKeyIdentifier, false, new SubjectKeyIdentifierStructure(asymmetricKeyPair.Public));
            KeyUsage keyUsage = new KeyUsage(certConf.CertName.EndsWith("CA") ? 182 : 176);
            certGen.AddExtension(X509Extensions.KeyUsage, false, keyUsage);

            ArrayList keyPurposes = new ArrayList();
            keyPurposes.Add(KeyPurposeID.IdKPServerAuth);
            keyPurposes.Add(KeyPurposeID.IdKPCodeSigning);
            keyPurposes.Add(KeyPurposeID.IdKPEmailProtection);
            keyPurposes.Add(KeyPurposeID.IdKPClientAuth);
            certGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(true));

            if (certConf.CertName.EndsWith("CA"))
                certGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(true));

            FieldInfo fi = typeof(X509V3CertificateGenerator).GetField("tbsGen", BindingFlags.NonPublic | BindingFlags.Instance);
            V3TbsCertificateGenerator v3TbsCertificateGenerator = (V3TbsCertificateGenerator)fi.GetValue(certGen);
            TbsCertificateStructure tbsCert = v3TbsCertificateGenerator.GenerateTbsCertificate();

            MD5 md5Provider = new MD5CryptoServiceProvider();
            byte[] tbsCertHash = md5Provider.ComputeHash(tbsCert.GetDerEncoded());

            RSAPKCS1SignatureFormatter signer = new RSAPKCS1SignatureFormatter();
            signer.SetHashAlgorithm("MD5");
            signer.SetKey(certConf.RootCert.PrivateKey);

            byte[] certSignature = signer.CreateSignature(tbsCertHash);
            X509Certificate signedCert = new X509Certificate(new X509CertificateStructure(
                tbsCert,
                new AlgorithmIdentifier(PkcsObjectIdentifiers.MD5WithRsaEncryption),
                new Org.BouncyCastle.Asn1.DerBitString(certSignature)));

            // Сохранение закрытой части сертификата
            try
            {
                using (FileStream fs = new FileStream(p12FileName, FileMode.Create))
                {
                    Pkcs12Store p12 = new Pkcs12Store();
                    X509CertificateEntry certEntry = new X509CertificateEntry(signedCert);
                    X509CertificateEntry rootCertEntry = new X509CertificateEntry(rootCertInternal);
                    p12.SetKeyEntry(certConf.CertName, new AsymmetricKeyEntry(asymmetricKeyPair.Private), new X509CertificateEntry[] { certEntry, rootCertEntry });
                    p12.Save(fs, certConf.Password.ToCharArray(), secRand);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw new CertGenerationException($"Ошибка при сохранении закрытой части сертификата:\n {ex.Message}");
            }
        }
    }
}
