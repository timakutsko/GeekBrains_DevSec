using System;
using System.Collections.Generic;

namespace Lesson4_ConfigFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.Title = Properties.Settings.Default.AppNameDebug;
#else
            Console.Title = Properties.Settings.Default.AppName;
#endif
            if (string.IsNullOrEmpty(Properties.Settings.Default.FIO) || Properties.Settings.Default.Age <= 0)
            {
                Console.WriteLine("Введите ФИО: ");
                Properties.Settings.Default.FIO = Console.ReadLine();

                Console.WriteLine("Укажите возраст: ");
                if (int.TryParse(Console.ReadLine(), out int age))
                    Properties.Settings.Default.Age = age;
                else
                {
                    Console.WriteLine("Возраст должен быть больше 0!");
                    Properties.Settings.Default.Age = 0;
                }

                Properties.Settings.Default.Save();
            }

            Console.WriteLine($"Вы указали ФИО: {Properties.Settings.Default.FIO}");
            Console.WriteLine($"Вы указали возраст: {Properties.Settings.Default.Age}");

            ConnectionString connectionString1 = new ConnectionString
            {
                DBName = "DB1",
                Host = "localhost",
                Password = "qwerty",
                UserName = "User1"
            };
            ConnectionString connectionString2 = new ConnectionString
            {
                DBName = "DB2",
                Host = "localhost",
                Password = "qwerty123",
                UserName = "User2"
            };
            List<ConnectionString> connectionStrings = new List<ConnectionString>
            {
                connectionString1,
                connectionString2
            };

            CacheProvider cacheProvider = new CacheProvider();
            cacheProvider.CacheConnections(connectionStrings);

            List<ConnectionString> connectionStringsFromFile = cacheProvider.GetConnectionFromCache();
            foreach (ConnectionString cs in connectionStringsFromFile)
                Console.WriteLine($"{cs.DBName}~{cs.Host}~{cs.UserName}~{cs.Password}");

            Console.ReadKey(true);
        }
    }
}
