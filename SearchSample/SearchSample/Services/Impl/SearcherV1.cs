using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchSample.Services.Impl
{
    /// <summary>
    /// Поиск слова методом перебора каждого слова строки
    /// </summary>
    internal class SearcherV1
    {
        public void SearchEazy(string word, IEnumerable<string> data)
        {
            foreach (var item in data)
            {
                if (item.Contains(word, StringComparison.InvariantCultureIgnoreCase))
                    Console.WriteLine(item);
            }
        }
    }
}
