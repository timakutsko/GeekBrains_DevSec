using Search.DAL;
using Search.DAL.Entity;
using System;
using System.IO;

namespace SearchSample.Services.Impl
{
    internal class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentDbContext _context;

        public DocumentRepository(DocumentDbContext context)
        {
            _context = context;
        }

        public void LoadDocuments()
        {
            using (StreamReader sr = new StreamReader(AppContext.BaseDirectory + "data.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string[] docArr = sr.ReadLine().Split('\t');
                    if (docArr.Length > 1 && int.TryParse(docArr[0], out int id))
                    {
                        _context.Documents.Add(new Document
                        {
                            Id = id,
                            Content = docArr[1]
                        });

                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
