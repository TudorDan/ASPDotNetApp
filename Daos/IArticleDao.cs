using ASPDotNetApp.Models;

namespace ASPDotNetApp.Daos
{
    public interface IArticleDao
    {
        Task<IEnumerable<Article>> GetAll();
        Task<bool> Add(Article article);
        Task<bool> Update(Article article);
        Task<Article?> Get(int id);
        Task<bool> Delete(int id);
    }
}
