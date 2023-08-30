using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ASPDotNetApp.Models
{
    public class PriceListViewModel
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? MarkupPercentage { get; set; }
        public DateTime ValidStartDate { get; set; }
        public DateTime ValidEndDate { get; set; }
        public decimal? RetailPrice { get; set; }
        public string ArticleName { get; set; }
    }
}
