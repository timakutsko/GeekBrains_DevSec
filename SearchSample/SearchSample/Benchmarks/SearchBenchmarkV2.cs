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
    public class SearchBenchmarkV2
    {
        private readonly TextIndexatorV2 _index = new TextIndexatorV2();
        private readonly string[] _docSet;

        public SearchBenchmarkV2()
        {
            _docSet = new DocumentExtractor().DocumentSet().ToArray();
            _index = new TextIndexatorV2();

            foreach (var item in _docSet)
                _index.AddStringToIndex(item);
        }
        [Params("intercontinental", " if ", "not")]
        public string Query { get; set; }


        [Benchmark(Baseline = true)]
        public void SearchEazy()
        {
            new SearcherV2().SearchBetterWithOutOutput(Query, _docSet).ToArray();
        }

        [Benchmark]
        public void SearchByIndex()
        {
            _index.SearchByIndex(Query).ToArray();
        }

    }
}
