using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNetApp.Models
{
    public class PriceList
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        [DisplayName("Purchase price")]
        public decimal? PurchasePrice { get; set; }
        [DisplayName("Markup price")]
        public decimal? MarkupPercentage { get; set; }
        [Required]
        [DisplayName("Valid start")]
        public DateTime ValidStartDate { get; set; }
        [Required]
        [DisplayName("Valid end")]
        public DateTime ValidEndDate { get; set; }
        [DisplayName("Retail price")]
        public decimal? RetailPrice { get; set; }
    }
}
