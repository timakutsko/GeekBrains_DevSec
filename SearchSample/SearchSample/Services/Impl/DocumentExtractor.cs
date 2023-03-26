using System;
using System.Collections.Generic;
using System.IO;

namespace SearchSample.Services.Impl
{
    internal class DocumentExtractor : IDocumentExtractor
    {
        public IEnumerable<string> DocumentSet()
        {
            return ReadDocuments(AppContext.BaseDirectory + "data.txt");
        }

        private IEnumerable<string> ReadDocuments(string name)
        {
            using (StreamReader sr = new StreamReader(name))
            {
                while (!sr.EndOfStream)
                {
                    var docArr = sr.ReadLine()?.Split('\t');
                    yield return docArr[1];
                }
            }
        }
    }
}
