using ASPDotNetApp.Models;
using Microsoft.Data.SqlClient;

namespace ASPDotNetApp.Daos.Impl
{
    public class PriceListDao : IPriceListDao
    {
        private readonly IConfiguration _configuration;

        public PriceListDao(IConfiguration iconfiguration)
        {
            _configuration = iconfiguration;
        }

        public async Task<IEnumerable<PriceList>> GetAll(int articleId)
        {
            var priceLists = new List<PriceList>();
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = @"SELECT * FROM PriceLists WHERE ArticleId = @articleId";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@articleId", articleId);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync()) 
                            {
                                var priceList = new PriceList
                                {
                                    Id = reader.GetInt32(0),
                                    ArticleId = reader.GetInt32(1),
                                    PurchasePrice = reader.IsDBNull(2) ? null : reader.GetDecimal(2),
                                    MarkupPercentage = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                                    ValidStartDate = reader.GetDateTime(4),
                                    ValidEndDate = reader.GetDateTime(5),
                                    RetailPrice = reader.IsDBNull(6) ? null : reader.GetDecimal(6)
                                };
                                priceLists.Add(priceList);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                await Console.Out.WriteLineAsync($"SQL Exception: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Generic Exception: {ex.Message}");
                throw;
            }
            return priceLists;
        }
    }
}
