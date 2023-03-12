using System;

namespace Lesson5_CertGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            CertGenerationProvider certGenerationProvider = new CertGenerationProvider();

            while (true)
            {
                Console.WriteLine("~~ Приветсвует центр генерации сертификаторв! ~~\n");
                Console.WriteLine("1. Создать корневой сертификат");
                Console.WriteLine("2. Создать сертификат");
                Console.Write("Выбирите подпрограмму (0 - выход из приложения):");
                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    switch (value)
                    {
                        case 0:
                            Console.WriteLine("Работа с приложением завершена!");
                            Console.ReadKey();
                            return;
                        
                        case 1:
                            CertConfiguration rootCertConfiguration = new CertConfiguration
                            {
                                CertName = "MyTestCert CA",
                                CertDuration = 5,
                                Email = "myEmail@gmail.com",
                                OutFolder = @"D:\MyStudy\GeekBrains_DevSec_Cert",
                                Password = "Qwerty123",
                            };
                            certGenerationProvider.GenerateRootCert(rootCertConfiguration);
                            break;
                        
                        case 2:
                            int cnt = 0;
                            CertExplorerProvider provider = new CertExplorerProvider(true);
                            provider.LoadCertificates();
                            foreach (var cert in provider.Certificates)
                                Console.WriteLine($"{cnt++} >> {cert}");
                            Console.Write("Введите номер корневого сертификата: ");

                            CertConfiguration certConfiguration = new CertConfiguration
                            {
                                RootCert = provider.Certificates[int.Parse(Console.ReadLine())].Cert,
                                CertName = "ITiti",
                                Email = "myEmail@gmail.com",
                                OutFolder = @"D:\MyStudy\GeekBrains_DevSec_Cert",
                                Password = "Qwerty123",
                            };
                            certGenerationProvider.GenerateCert(certConfiguration);
                            break;
                        
                        default:
                            Console.WriteLine("Некорректная команда - такой код отсутсвует. Повторите ввод.");
                            break;
                    }
                }
                else
                    Console.WriteLine("Некорректная команда - принимаются только цифры. Повторите ввод.");

            }
        }
    }
}
