using ASPDotNetApp.Models;

namespace ASPDotNetApp.Daos
{
    public interface IPriceListDao
    {
        Task<IEnumerable<PriceList>> GetAll(int articleId);
        Task<bool> Add(PriceList priceList);
        Task<PriceList?> Get(int id);
        Task<bool> Update(PriceList priceList);
        Task<bool> Delete(int id);
        Task<IEnumerable<PriceListViewModel>> SearchValidPrices(DateTime searchDate);
    }
}
