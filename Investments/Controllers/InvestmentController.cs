using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Investments.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Investments.Controllers
{
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("api/investment")]
    public class InvestmentController : ControllerBase
    {
        private IConfiguration configuration;

        public InvestmentController(IConfiguration _configuration)
        {   
            configuration = _configuration;
        }

        // GET api/values
        [HttpGet]
        public List<Investment> Get()
        {
            List<Investment> investimentos = new List<Investment>();

            var connectionString = configuration.GetConnectionString("investmentConnection");

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                var query = "SELECT * FROM investment";

                MySqlCommand comm = new MySqlCommand(query, conn);

                MySqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    Investment investment = new Investment();

                    investment.buyPrice = Convert.ToDecimal(reader["buyPrice"]);
                    investment.dateBuy = Convert.ToDateTime(reader["dateBuy"]);
                    if (reader["dateSell"] != null && !String.IsNullOrEmpty(reader["dateSell"].ToString()))
                    {
                        investment.dateSell = Convert.ToDateTime(reader["dateSell"]);
                    }
                    if (reader["feePrice"] != null && !String.IsNullOrEmpty(reader["feePrice"].ToString()))
                    {
                        investment.feePrice = Convert.ToDecimal(reader["feePrice"]);
                    }
                    investment.name = reader["name"].ToString();
                    if (reader["sellPrice"] != null && !String.IsNullOrEmpty(reader["sellPrice"].ToString()))
                    {
                        investment.sellPrice = Convert.ToDecimal(reader["sellPrice"]);
                    }

                    investimentos.Add(investment);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Deu erro ao fazer o get: " + ex);
            }

            return investimentos;
        }


        // POST 
        [HttpPost("post-investment")]
        public void Post([FromBody] Investment investment)
        {
            var connectionString = configuration.GetConnectionString("investmentConnection");

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                var query = "INSERT INTO investment (name, buyPrice, sellPrice, dateBuy, dateSell, feePrice) VALUES (@name, @buyPrice, @sellPrice, @dateBuy, @dateSell, @feePrice)";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.Add(new MySqlParameter("@name", investment.name));
                comm.Parameters.Add(new MySqlParameter("@buyPrice", investment.buyPrice));
                comm.Parameters.Add(new MySqlParameter("@sellPrice", investment.sellPrice));
                comm.Parameters.Add(new MySqlParameter("@dateBuy", investment.dateBuy));
                comm.Parameters.Add(new MySqlParameter("@dateSell", investment.dateSell));
                comm.Parameters.Add(new MySqlParameter("@feePrice", investment.feePrice));

                comm.ExecuteScalar();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // PUT 
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Investment investment)
        {
            var connectionString = configuration.GetConnectionString("investmentConnection");

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                var query = "UPDATE investment SET name = @name, buyPrice = @buyPrice, sellPrice = @sellPrice, dateBuy = @dateBuy, dateSell = @dateSell, feePrice = @feePrice WHERE idinvestment = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.Add(new MySqlParameter("@id", id)); 
                comm.Parameters.Add(new MySqlParameter("@name", investment.name));
                comm.Parameters.Add(new MySqlParameter("@buyPrice", investment.buyPrice));
                comm.Parameters.Add(new MySqlParameter("@sellPrice", investment.sellPrice));
                comm.Parameters.Add(new MySqlParameter("@dateBuy", investment.dateBuy));
                comm.Parameters.Add(new MySqlParameter("@dateSell", investment.dateSell));
                comm.Parameters.Add(new MySqlParameter("@feePrice", investment.feePrice));

                comm.ExecuteScalar();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // DELETE
        [HttpDelete("del/{id}")]
        public void Delete(int id)
        {
            var connectionString = configuration.GetConnectionString("investmentConnection");

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                var query = "DELETE FROM investment WHERE idinvestment = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.Add(new MySqlParameter("@id", id));

                comm.ExecuteScalar();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
