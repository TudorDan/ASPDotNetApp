using ASPDotNetApp.Models;

namespace ASPDotNetApp.Daos
{
    public interface IPriceListDao
    {
        Task<IEnumerable<PriceList>> GetAll(int articleId);
    }
}
