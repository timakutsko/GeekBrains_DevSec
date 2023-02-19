using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Data.Models
{
    [Table("Cards")]
    public class Card
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Id клиента, кому принадлежит карта
        /// </summary>
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        /// <summary>
        /// Номер карты
        /// </summary>
        [Column]
        [StringLength(20)]
        public string CardNumber { get; set; }

        /// <summary>
        /// Имя карты
        /// </summary>
        [Column]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Код CVV
        /// </summary>
        [Column]
        [StringLength(50)]
        public string CVV2 { get; set; }

        /// <summary>
        /// Дата действия
        /// </summary>
        [Column]
        public DateTime ExpDate { get; set; }

        /// <summary>
        /// Ссылка на владельца карты (клиента)
        /// </summary>
        public virtual Client Client { get; set; }
    }
}
