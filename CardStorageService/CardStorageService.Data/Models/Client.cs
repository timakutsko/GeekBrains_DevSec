using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Data.Models
{
    [Table("Clients")]
    public class Client
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Column]
        [StringLength(255)]
        public string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Column]
        [StringLength(255)]
        public string FirstName { get; set; }

        /// <summary>
        /// Коллеция карт клиента
        /// </summary>
        [InverseProperty(nameof(Card.Client))]
        public virtual ICollection<Card> Cards { get; set; } = new HashSet<Card>();
    }
}
