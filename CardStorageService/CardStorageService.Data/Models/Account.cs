using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardStorageService.Data.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string EMail { get; set; }

        [StringLength(100)]
        public string PassowrdSalt { get; set; }

        [StringLength(100)]
        public string PassowrdHash { get; set; }

        [StringLength(255)]
        public string Surname { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        public bool Locked { get; set; }

        [InverseProperty(nameof(AccountSession.Account))]
        public virtual ICollection<AccountSession> Sessions { get; set; } = new HashSet<AccountSession>();
    }
}
