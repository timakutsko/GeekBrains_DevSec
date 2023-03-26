using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Search.DAL;
using SearchSample.Benchmarks;
using SearchSample.Services;
using SearchSample.Services.Impl;
using System;
using System.Linq;

namespace SearchSample
{
    internal class Sample01
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddDbContext<DocumentDbContext>(options =>
                    {
                        options.UseSqlServer(@"data source=LAPTOP-6LEDO1V3;initial catalog=SearchDatabase;User Id=SearchUser;Password=12345;TrustServerCertificate=True;App=EntityFramework");
                    });
                    services.AddTransient<IDocumentRepository, DocumentRepository>();
                })
                .Build();

            #region Import data to DB
            // Запускается 1 раз для переноса из текстового файла в БД
            //host.Services.GetRequiredService<IDocumentRepository>().LoadDocuments();
            #endregion

            #region Test with search
            //string[] docSet = new DocumentExtractor().DocumentSet().ToArray();

            //// Простой поиск слова методом перебора
            //new SearcherV1().SearchEazy(" if ", docSet);
            //Console.WriteLine(new string('\u2550', Console.WindowWidth));
            //Console.WriteLine();

            //// Простой поиск слова методом перебора c улучшенным выводом
            //new SearcherV2().SearchEazy(" if ", docSet);
            //Console.WriteLine(new string('\u2550', Console.WindowWidth));
            //Console.WriteLine();

            //// Простой поиск слова методом перебора c улучшенным выводом и улучшенным поиском
            //new SearcherV2().SearchBetter(" if ", docSet);
            //Console.WriteLine(new string('\u2550', Console.WindowWidth));
            //Console.WriteLine();
            #endregion

            #region Benchmark V1
            //BenchmarkSwitcher.FromAssembly(typeof(Sample01).Assembly).Run(args, new DebugInProcessConfig());
            //BenchmarkRunner.Run<SearchBenchmarkV1>();
            #endregion

            #region Create indexes
            // Запускается 1 раз для индексов и записи в БД
            //TextIndexatorV1 textIndexatorV1 = new TextIndexatorV1(host.Services.GetService<DocumentDbContext>());
            //textIndexatorV1.BuilIndex();
            #endregion

            #region Benchmark V2
            BenchmarkSwitcher.FromAssembly(typeof(Sample01).Assembly).Run(args, new DebugInProcessConfig());
            BenchmarkRunner.Run<SearchBenchmarkV2>();
            #endregion

        }
    }
}
