using CrudAdoNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CrudAdoNet.Controllers
{
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PessoasController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("/api")]
        public string Get()
        {
            return $"Api desenvolvida por Felipe Muniz, foco para aprendizado. Criando CRUD com ADO.NET. {DateTime.Now.ToString("dd/MM/yyyy")}";
        }

        [HttpGet("/pessoas")]
        public List<Pessoas> GetAll()
        {
            List<Pessoas> list = new List<Pessoas>();
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from Pessoas", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Pessoas obj = new Pessoas();
                obj.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                obj.Nome = dt.Rows[i]["Nome"].ToString();
                obj.Email = dt.Rows[i]["Email"].ToString();
                obj.Documento = dt.Rows[i]["Documento"].ToString();

                list.Add(obj);
            }
            return list;
        }

        [HttpGet("/pessoas/{id}")]
        public Pessoas Get(int id)
        {
            Pessoas pessoa = new Pessoas();

            SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from pessoas where Id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                pessoa = new Pessoas
                {
                    Id = (int)reader["Id"],
                    Nome = reader["Nome"].ToString(),
                    Email = reader["Email"].ToString(),
                    Documento = reader["Documento"].ToString()
                };
            }
            con.Close();
            return pessoa;
        }

        [HttpPost("/pessoas")]
        public ActionResult<Pessoas> Post(Pessoas pessoa)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into pessoas values(@Nome, @Email, @Documento)", con);
            cmd.Parameters.AddWithValue("@Nome", pessoa.Nome);
            cmd.Parameters.AddWithValue("@Email", pessoa.Email);
            cmd.Parameters.AddWithValue("@Documento", pessoa.Documento);
            cmd.ExecuteNonQuery();
            con.Close();

            return pessoa;
        }

        [HttpPut("/pessoas")]
        public ActionResult<Pessoas> Put(Pessoas pessoa)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            con.Open();
            SqlCommand cmd = new SqlCommand("update pessoas set Nome = @nome, Email = @Email, Documento = @Documento where Id = @id", con);
            cmd.Parameters.AddWithValue("@Nome", pessoa.Nome);
            cmd.Parameters.AddWithValue("@Email", pessoa.Email);
            cmd.Parameters.AddWithValue("@Documento", pessoa.Documento);
            cmd.Parameters.AddWithValue("@id", pessoa.Id);
            cmd.ExecuteNonQuery();
            con.Close();

            return pessoa;
        }

        [HttpDelete("/pessoas/{id}")]
        public bool Delete(int id)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Pessoas where Id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }
    }
}
