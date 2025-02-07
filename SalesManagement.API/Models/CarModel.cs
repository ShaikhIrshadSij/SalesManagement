using System.ComponentModel.DataAnnotations;

namespace SalesManagement.API.Models
{
    public class CarModel
    {
        public int ModelId { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Class { get; set; }

        [Required]
        public string ModelName { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
        public string ModelCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Features { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateOfManufacturing { get; set; }

        public bool IsActive { get; set; }

        public int SortOrder { get; set; }

        public List<string> ImagePaths { get; set; }
    }
}
