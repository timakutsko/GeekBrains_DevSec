using Search.DAL;
using Search.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchSample.Services.Impl
{
    internal class TextIndexatorV1
    {
        private readonly DocumentDbContext _context;
        private readonly Lexer _lexer = new Lexer();

        public TextIndexatorV1(DocumentDbContext context)
        {
            _context = context;
        }

        public void BuilIndex()
        {
            foreach (var doc in _context.Documents.ToArray())
            {
                foreach (var token in _lexer.GetTokens(doc.Content))
                {
                    var word = _context.Words.FirstOrDefault(w => w.Text == token);
                    int wordId = 0;
                    if (word == null)
                    {
                        var wordObj = new Word
                        {
                            Text = token
                        };
                        _context.Words.Add(wordObj);
                        _context.SaveChanges();
                        wordId = wordObj.Id;
                    }
                    else
                        wordId = word.Id;

                    var wordDoc = _context.WordDocuments.FirstOrDefault(wd => wd.WordId == wordId && wd.DocumentId == doc.Id);
                    if (wordDoc == null)
                    {
                        _context.WordDocuments.Add(new WordDocument
                        {
                            DocumentId =  doc.Id,
                            WordId = wordId,
                        });
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
