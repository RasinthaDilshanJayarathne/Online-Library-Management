using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class ReservationViewModel
    {
        public int ReservationId { get; set; }

        public string UserName { get; set; }

        public string ItemTitle { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Reservation_Date { get; set; }

        public string Status { get; set; }
        public string ImageUrl { get; set; } // Add this property


        [Required]
        [StringLength(500)]
        public string Message { get; set; }
    }
}
