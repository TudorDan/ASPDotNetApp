using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNetApp.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [DisplayName("VAT percentage")]
        [Range(0, 999.99, ErrorMessage = "VAT percentage must be between 0 and 999.99 only!")]
        public decimal VATPercentage { get; set; }
        [DisplayName("Purchase price")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Purchase price must be between 0 and 9999999999999999.99 only!")]
        public decimal? PurchasePrice { get; set; }
        [DisplayName("Retail price")]
        [Range(0, 9999999999999999.99, ErrorMessage = "VAT percentage must be between 0 and 9999999999999999.99 only!")]
        public decimal? RetailPrice { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
