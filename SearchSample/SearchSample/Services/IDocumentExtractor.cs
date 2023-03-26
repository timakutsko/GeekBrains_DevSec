using System.Collections.Generic;

namespace SearchSample.Services
{
    internal interface IDocumentExtractor
    {
        IEnumerable<string> DocumentSet();
    }
}
