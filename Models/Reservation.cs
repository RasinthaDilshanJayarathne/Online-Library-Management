using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        [ForeignKey("Message")]
        public int? MessageId { get; set; }  // Make this nullable

        [Required]
        public DateTime Reservation_Date { get; set; }

        public string Status { get; set; }

        public User User { get; set; }
        public Item Item { get; set; }
        public Message Message { get; internal set; }
    }
}
