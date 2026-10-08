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
    public class KategorijaRepository
    {
        private readonly string connectionString;
        public KategorijaRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["TrgovinaKonekcija"]
                .ConnectionString;
        }


        public List<Kategorija> VratiSve()
        {
            List<Kategorija> kategorije = new List<Kategorija>();
            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();
                string upit = "SELECT Id, Naziv FROM Kategorije";

                using (SqlCommand komanda = new SqlCommand(upit, konekcija))
                using (SqlDataReader reader = komanda.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Kategorija kategorija = new Kategorija();

                        kategorija.Id = (int)reader["Id"];
                        kategorija.Naziv = reader["Naziv"].ToString();

                        kategorije.Add(kategorija);
                    }
                }
            }

            return kategorije;
        }
        public void Dodaj(Kategorija kategorija)
        {
            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();
                string upit = "INSERT INTO Kategorije (Naziv) VALUES (@Naziv)";
                using (SqlCommand komanda = new SqlCommand(upit, konekcija))
                {
                    komanda.Parameters.AddWithValue("@Naziv", kategorija.Naziv);
                    komanda.ExecuteNonQuery();
                }
            }
        }
        public void Izmijeni(int id, string noviNaziv)
        {
            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();
                string upit ="UPDATE Kategorije SET Naziv=@Naziv WHERE Id=@Id";
                using (SqlCommand komanda = new SqlCommand(upit, konekcija))
                {
                    komanda.Parameters.AddWithValue("@Id", id);
                    komanda.Parameters.AddWithValue("@Naziv", noviNaziv);
                    komanda.ExecuteNonQuery();
                }
            }
        }
        public void Obrisi(int id)
        {
            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();
                string upit ="DELETE FROM Kategorije WHERE Id=@Id";

                using (SqlCommand komanda =new SqlCommand(upit, konekcija))
                {
                    komanda.Parameters.AddWithValue("@Id", id);
                    komanda.ExecuteNonQuery();
                }
            }
        }

    }
}
