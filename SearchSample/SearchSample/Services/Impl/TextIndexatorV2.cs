using System.Collections.Generic;
using System.Linq;

namespace SearchSample.Services.Impl
{
    internal class TextIndexatorV2
    {
        private readonly Dictionary<string, HashSet<int>> _index = new Dictionary<string, HashSet<int>>();
        private readonly List<string> _content = new List<string>();
        private readonly Lexer _lexer = new Lexer();
        private readonly SearcherV2 _searcher = new SearcherV2();

        public void AddStringToIndex(string text)
        {
            int docId = _content.Count;
            foreach (var token in _lexer.GetTokens(text))
            {
                if (_index.TryGetValue(token, out var set))
                    set.Add(docId);
                else
                    _index.Add(token, new HashSet<int> { docId });
            }

            _content.Add(text);
        }

        public IEnumerable<string> SearchByIndex(string word)
        {
            var docList = Search(word);
            foreach (var docId in docList)
            {
                foreach (var match in _searcher.SearchInDocument(word, _content[docId]))
                    yield return match;
            }
        }

        private IEnumerable<int> Search(string word)
        {
            word = word.ToLowerInvariant();
            if (_index.TryGetValue(word, out var set))
                return set;

            return Enumerable.Empty<int>();
        }
    }
}
