using CardStorageService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Data.Contexts
{
    public class CardStorageServiceDbContext : DbContext
    {
        public CardStorageServiceDbContext(DbContextOptions opt) : base(opt)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Card> Cards { get; set; }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<AccountSession> AccountSessions { get; set; }
    }
}
