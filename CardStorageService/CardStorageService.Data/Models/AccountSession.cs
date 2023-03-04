using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardStorageService.Data.Models
{
    public class AccountSession
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(384)]
        public string SessionToken { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
