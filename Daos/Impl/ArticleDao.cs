using ASPDotNetApp.Models;
using Microsoft.Data.SqlClient;

namespace ASPDotNetApp.Daos.Impl
{
    public class ArticleDao : IArticleDao
    {
        private readonly IConfiguration _configuration;

        public ArticleDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Add(Article article)
        {
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = @"INSERT INTO Articles 
                                (Name, Code, VATPercentage, PurchasePrice, RetailPrice, Status)
                                VALUES (@Name, @Code, @VATPercentage, @PurchasePrice, @RetailPrice, @Status)
                                SELECT SCOPE_IDENTITY()";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@Name", article.Name);
                        command.Parameters.AddWithValue("@Code", article.Code);
                        command.Parameters.AddWithValue("@VATPercentage", article.VATPercentage);
                        command.Parameters.AddWithValue("@PurchasePrice", article.PurchasePrice ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@RetailPrice", article.RetailPrice ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Status", article.Status);
                        int newArticleId = Convert.ToInt32(await command.ExecuteScalarAsync());

                        // Add corresponding PriceList entry
                        var priceListCmdTxt = @"INSERT INTO PriceLists
                        (ArticleId, PurchasePrice, MarkupPercentage, ValidStartDate, ValidEndDate, RetailPrice)
                        VALUES (@ArticleId, @PLPurchasePrice, @MarkupPercentage, @ValidStartDate, @ValidEndDate, @PLRetailPrice)";
                        using (var priceListCommand = new SqlCommand(priceListCmdTxt, conn))
                        {
                            priceListCommand.Parameters.AddWithValue("@ArticleId", newArticleId);
                            priceListCommand.Parameters.AddWithValue("@PLPurchasePrice", article.PurchasePrice ?? (object)DBNull.Value);
                            priceListCommand.Parameters.AddWithValue("@MarkupPercentage", (object)DBNull.Value);
                            priceListCommand.Parameters.AddWithValue("@ValidStartDate", DateTime.Now);
                            priceListCommand.Parameters.AddWithValue("@ValidEndDate", DateTime.Now.AddYears(2));
                            priceListCommand.Parameters.AddWithValue("@PLRetailPrice", article.RetailPrice ?? (object)DBNull.Value);
                            await priceListCommand.ExecuteNonQueryAsync();
                        }
                        return newArticleId > 0;
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
                    var priceListCmdTxt = "DELETE FROM PriceLists WHERE ArticleId = @Id";
                    using (var priceListCommand = new SqlCommand(priceListCmdTxt, conn))
                    {
                        priceListCommand.Parameters.AddWithValue("@Id", id);
                        await priceListCommand.ExecuteNonQueryAsync();
                    }

                    var cmdTxt = "DELETE FROM Articles WHERE Id = @Id";
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

        public async Task<Article?> Get(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = @"SELECT * FROM Articles WHERE Id = @id";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Article
                                {
                                    Id = id,
                                    Name = reader.GetString(1),
                                    Code = reader.GetString(2),
                                    VATPercentage = reader.GetDecimal(3),
                                    PurchasePrice = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                                    RetailPrice = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                                    Status = reader.GetBoolean(6)
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

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articlesList = new List<Article>();
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = "SELECT * FROM Articles";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var article = new Article()
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Code = reader.GetString(2),
                                    VATPercentage = reader.GetDecimal(3),
                                    PurchasePrice = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                                    RetailPrice = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                                    Status = reader.GetBoolean(6)
                                };
                                articlesList.Add(article);
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
            return articlesList;
        }

        public async Task<bool> Update(Article article)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var cmdTxt = @"UPDATE Articles
                                SET Name = @Name, Code = @Code, VATPercentage = @VATPercentage, 
                                PurchasePrice = @PurchasePrice, RetailPrice = @RetailPrice, Status = @Status
                                WHERE Id = @Id";
                    using (var command = new SqlCommand(cmdTxt, conn))
                    {
                        command.Parameters.AddWithValue("@Id", article.Id);
                        command.Parameters.AddWithValue("@Name", article.Name);
                        command.Parameters.AddWithValue("@Code", article.Code);
                        command.Parameters.AddWithValue("@VATPercentage", article.VATPercentage);
                        command.Parameters.AddWithValue("@PurchasePrice", article.PurchasePrice ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@RetailPrice", article.RetailPrice ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Status", article.Status);
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
