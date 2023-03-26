using BenchmarkDotNet.Attributes;
using SearchSample.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchSample.Benchmarks
{
    [MemoryDiagnoser] // Учет памяти при тестировании
    [WarmupCount(1)] // Прогрев теста
    [IterationCount(5)] // Количество выполнений теста, на выходе - среднее значение
    public class SearchBenchmarkV1
    {
        private readonly string[] _docSet;

        public SearchBenchmarkV1()
        {
            _docSet = new DocumentExtractor().DocumentSet().ToArray();
        }

        [Benchmark]
        public void SearchEazy()
        {
            new SearcherV2().SearchBetterWithOutOutput(" if ", _docSet).ToArray();
        }

    }
}
