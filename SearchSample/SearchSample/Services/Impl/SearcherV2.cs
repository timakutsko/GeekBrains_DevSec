using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchSample.Services.Impl
{
    internal class SearcherV2
    {
        public void SearchEazy(string word, IEnumerable<string> data)
        {
            foreach (var item in data)
            {
                if (item.Contains(word, StringComparison.InvariantCultureIgnoreCase))
                    Console.WriteLine(PrettySearch(word, item));
            }
        }

        public void SearchBetter(string word, IEnumerable<string> data)
        {
            foreach (var item in data)
            {
                Console.WriteLine("================================================");
                int pos = 0;
                while (true)
                {
                    pos = item.IndexOf(word, pos);
                    if (pos >= 0)
                        Console.WriteLine(PrettySearch(item, pos));
                    else
                        break;

                    pos++;
                }
            }
        }

        public IEnumerable<string> SearchBetterWithOutOutput(string word, IEnumerable<string> data)
        {
            foreach (var item in data)
            {
                int pos = 0;
                while (true)
                {
                    pos = item.IndexOf(word, pos);
                    if (pos >= 0)
                        yield return PrettySearch(item, pos);
                    else
                        break;

                    pos++;
                }
            }
        }

        public IEnumerable<string> SearchInDocument(string word, string item)
        {
            int pos = 0;
            while (true)
            {
                pos = item.IndexOf(word, pos);
                if (pos >= 0)
                    yield return PrettySearch(item, pos);
                else
                    break;

                pos++;
            }
        }

        private string PrettySearch(string word, string text)
        {
            int pos = text.IndexOf(word);
            int start = Math.Max(0, pos - 50);
            int end = Math.Min(start + 100, text.Length - 1);
            return $"{(start == 0 ? "" : "...")}{text.Substring(start, end - start)}{(end == text.Length - 1 ? "" : "...")}";
        }

        private string PrettySearch(string text, int pos)
        {
            int start = Math.Max(0, pos - 50);
            int end = Math.Min(start + 100, text.Length - 1);
            return $"{(start == 0 ? "" : "...")}{text.Substring(start, end - start)}{(end == text.Length - 1 ? "" : "...")}";
        }
    }
}
