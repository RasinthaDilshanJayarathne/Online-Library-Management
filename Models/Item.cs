using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{

    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author/Artist is required.")]
        [StringLength(200, ErrorMessage = "Author/Artist cannot be longer than 200 characters.")]
        public string AuthorArtist { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [StringLength(50, ErrorMessage = "Type cannot be longer than 50 characters.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Publisher is required.")]
        [StringLength(100, ErrorMessage = "Publisher cannot be longer than 100 characters.")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Year Published is required.")]
        [Range(1000, 9999, ErrorMessage = "Year Published must be a valid year.")]
        public int YearPublished { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        [StringLength(50, ErrorMessage = "Language cannot be longer than 50 characters.")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Cost is required.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 10000.00, ErrorMessage = "Cost must be between 0.01 and 10000.00.")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Actual Stock is required.")]
        [Range(0, 10000, ErrorMessage = "Actual Stock must be between 0 and 10000.")]
        public int ActualStock { get; set; }

        [Required(ErrorMessage = "Available Stock is required.")]
        [Range(0, 10000, ErrorMessage = "Available Stock must be between 0 and 10000.")]
        public int Available { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters.")]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot be longer than 500 characters.")]
        public string ImageUrl { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        // Navigation properties
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<IssueReturn> IssueReturns { get; set; }

        // Constructor to initialize collections
        public Item()
        {
            Reservations = new HashSet<Reservation>();
            IssueReturns = new HashSet<IssueReturn>();
        }
    }
}
