using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNetApp.Models
{
    public class PriceList
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        [DisplayName("Purchase price")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Purchase price must be between 0 and 9999999999999999.99 only!")]
        public decimal? PurchasePrice { get; set; }
        [DisplayName("Markup price")]
        [Range(0, 999.99, ErrorMessage = "VAT percentage must be between 0 and 999.99 only!")]
        public decimal? MarkupPercentage { get; set; }
        [Required]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime ValidStartDate { get; set; }
        [Required]
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime ValidEndDate { get; set; }
        [DisplayName("Retail price")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Retail price must be between 0 and 9999999999999999.99 only!")]
        public decimal? RetailPrice { get; set; }
    }
}
