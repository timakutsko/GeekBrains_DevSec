using Microsoft.EntityFrameworkCore;
using Search.DAL.Entity;
using System;

namespace Search.DAL
{
    public class DocumentDbContext : DbContext
    {
        public DocumentDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Document> Documents { get; set; }
        
        public DbSet<Word> Words { get; set; }

        public DbSet<WordDocument> WordDocuments { get; set; }
    }
}
