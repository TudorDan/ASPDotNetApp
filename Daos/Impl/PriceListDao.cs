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

        public async Task<bool> Add(PriceList priceList)
        {
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = @"INSERT INTO PriceLists
                    (ArticleId, PurchasePrice, MarkupPercentage, ValidStartDate, ValidEndDate, RetailPrice)
                    VALUES (@ArticleId, @PurchasePrice, @MarkupPercentage, @ValidStartDate, @ValidEndDate, @RetailPrice)
                    SELECT SCOPE_IDENTITY()";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@ArticleId", priceList.ArticleId);
                        command.Parameters.AddWithValue("@PurchasePrice", priceList.PurchasePrice ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@MarkupPercentage", priceList.MarkupPercentage ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ValidStartDate", priceList.ValidStartDate);
                        command.Parameters.AddWithValue("@ValidEndDate", priceList.ValidEndDate);
                        command.Parameters.AddWithValue("@RetailPrice", priceList.RetailPrice ?? (object)DBNull.Value);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
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
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = "DELETE FROM PriceLists WHERE Id = @Id";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
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
        }

        public async Task<PriceList?> Get(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = @"SELECT * FROM PriceLists WHERE Id = @id";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new PriceList
                                {
                                    Id = id,
                                    ArticleId = reader.GetInt32(1),
                                    PurchasePrice = reader.IsDBNull(2) ? null : reader.GetDecimal(2),
                                    MarkupPercentage = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                                    ValidStartDate = reader.GetDateTime(4),
                                    ValidEndDate = reader.GetDateTime(5),
                                    RetailPrice = reader.IsDBNull(6) ? null : reader.GetDecimal(6)
                                };
                            }
                            else
                            {
                                return null;
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

        public async Task<IEnumerable<PriceList>> SearchValidPrices(DateTime searchDate)
        {
            var priceLists = new List<PriceList>();
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = @"DECLARE @GivenDate DATE = @searchDate;
                                SELECT * FROM PriceLists
                                WHERE ValidStartDate < @GivenDate AND ValidEndDate > @GivenDate";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@searchDate", searchDate);
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

        public async Task<bool> Update(PriceList priceList)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = @"UPDATE PriceLists
                                SET PurchasePrice = @PurchasePrice, MarkupPercentage = @MarkupPercentage,
                                ValidStartDate = @ValidStartDate, ValidEndDate = @ValidEndDate, 
                                RetailPrice = @RetailPrice
                                WHERE Id = @Id";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@Id", priceList.Id);
                        command.Parameters.AddWithValue("@PurchasePrice", priceList.PurchasePrice ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@MarkupPercentage", priceList.MarkupPercentage ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ValidStartDate", priceList.ValidStartDate);
                        command.Parameters.AddWithValue("@ValidEndDate", priceList.ValidEndDate);
                        command.Parameters.AddWithValue("@RetailPrice", priceList.RetailPrice ?? (object)DBNull.Value);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
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
        }
    }
}
