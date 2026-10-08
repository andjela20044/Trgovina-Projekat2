using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trgovina.Core.Modeli;


namespace Trgovina.Core.Repository
{
    public class ArtikalRepository
    {
        private readonly string connectionString;

        public ArtikalRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TrgovinaKonekcija"].ConnectionString;
        }
        


        public List<Artikal> VratiSve()
        {
            List<Artikal> artikli = new List<Artikal>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = "SELECT Id, Naziv, KategorijaId, Cijena, JedinicaMjere FROM Artikli ORDER BY Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        artikli.Add(new Artikal
                        {
                            Id = reader.GetInt32(0),
                            Naziv = reader.GetString(1),
                            KategorijaId = reader.GetInt32(2),
                            Cijena = reader.GetDecimal(3),
                            JedinicaMjere = reader.GetString(4)
                        });
                    }
                }
            }
           
            return artikli;
        }
        public List<Artikal> VratiArtiklePoKategoriji(int kategorijaId)
        {
            List<Artikal> artikli = new List<Artikal>();

            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();
                string sql = "SELECT Id, Naziv, KategorijaId, Cijena, JedinicaMjere FROM Artikli WHERE KategorijaId = @KategorijaId";
                using (SqlCommand komanda = new SqlCommand(sql, konekcija))
                {
                    komanda.Parameters.AddWithValue("@KategorijaId", kategorijaId);

                    using (SqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artikal artikal = new Artikal();

                            artikal.Id = Convert.ToInt32(reader["Id"]);
                            artikal.Naziv = reader["Naziv"].ToString();
                            artikal.KategorijaId = Convert.ToInt32(reader["KategorijaId"]);
                            artikal.Cijena = Convert.ToDecimal(reader["Cijena"]);
                            artikal.JedinicaMjere = reader["JedinicaMjere"].ToString();

                            artikli.Add(artikal);
                        }
                    }
                }
            }

            return artikli;
        }
        public void Dodaj(Artikal artikal)
        {
            using (SqlConnection con = new SqlConnection(connectionString)) { 
            con.Open();
            string sql = @"INSERT INTO Artikli (Naziv, KategorijaId, Cijena, JedinicaMjere)
                           VALUES (@Naziv, @KategorijaId, @Cena, @JedinicaMere)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Naziv", artikal.Naziv);
                    cmd.Parameters.AddWithValue("@KategorijaId", artikal.KategorijaId);
                    cmd.Parameters.AddWithValue("@Cena", artikal.Cijena);
                    cmd.Parameters.AddWithValue("@JedinicaMere", artikal.JedinicaMjere);

                    cmd.ExecuteNonQuery();
                }
                
            }
           
        }
        public void Izmijeni(Artikal artikal)
        {
            using (SqlConnection con = new SqlConnection(connectionString)) { 
            con.Open();

            string sql = @"UPDATE Artikli 
                           SET Naziv=@Naziv, KategorijaId=@KategorijaId, Cijena=@Cena, JedinicaMjere=@JedinicaMere
                           WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", artikal.Id);
                    cmd.Parameters.AddWithValue("@Naziv", artikal.Naziv);
                    cmd.Parameters.AddWithValue("@KategorijaId", artikal.KategorijaId);
                    cmd.Parameters.AddWithValue("@Cena", artikal.Cijena);
                    cmd.Parameters.AddWithValue("@JedinicaMere", artikal.JedinicaMjere);

                    cmd.ExecuteNonQuery();
                }
        }
        }
        public void Obrisi(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = "DELETE FROM Artikli WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
