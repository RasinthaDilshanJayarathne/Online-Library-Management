using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class IssueReturnViewModel
    {
        public int ReservationId { get; set; }

        public string UserName { get; set; }

        public string ItemTitle { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; }
    }
}
