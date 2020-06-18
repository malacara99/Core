using Buster.Models;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buster.Data
{
    public class PromotionRepository
    {
        private readonly string _connection;
        public PromotionRepository(IConfiguration configuration) => _connection = configuration.GetConnectionString("DefaultConnectionString");
        
        public async Task<List<PromotionDTO>> GetAll() 
        {
            var response = new List<PromotionDTO>();
            using (SqlConnection sql = new SqlConnection(_connection)) 
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetPromotions_GETL", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await sql.OpenAsync();

                    using(var reader = await cmd.ExecuteReaderAsync()) 
                    {
                    while(await reader.ReadAsync()) 
                        {
                            response.Add(mapToPromotion(reader));
                        }
                    }
                }
            }
            return response;
        }

        public async Task<PromotionDTO> GetById(int Id)
        {
            var response = new PromotionDTO();
            using (SqlConnection sql = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetPromotionsbyId_GETL", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = mapToPromotion(reader);
                        }
                    }
                }
            }
            return response;
        }

        public async Task Insert(PromotionDTO promotion) 
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connection))
                {
                    using (SqlCommand command = new SqlCommand("usp_GetPromotionsbyId_INS", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Description", promotion.Description));
                        command.Parameters.Add(new SqlParameter("@Active", promotion.Active));
                        command.Parameters.Add(new SqlParameter("@Cve", promotion.CvePromotion));
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return;
                    }
                }
            }
            catch(Exception ex) 
            {
            
            }
        
        }

        private PromotionDTO mapToPromotion(SqlDataReader reader) 
        {
            return new PromotionDTO()
            {
                PromotionId = (int)reader["PromotionId"],
                CvePromotion = reader["CvePromotion"].ToString(),
                Active = (bool)reader["Active"],
                Description = reader["Description"].ToString()

            };
        }


    }
}
