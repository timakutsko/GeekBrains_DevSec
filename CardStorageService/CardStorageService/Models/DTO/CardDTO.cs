using System;

namespace CardStorageService.Models.DTO
{
    public class CardDTO
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id клиента, кому принадлежит карта
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Номер карты
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Имя карты
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код CVV
        /// </summary>
        public string CVV2 { get; set; }

        /// <summary>
        /// Дата действия
        /// </summary>
        public DateTime ExpDate { get; set; }
    }
}
