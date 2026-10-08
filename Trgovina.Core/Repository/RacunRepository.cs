using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trgovina.Core.Modeli;

namespace Trgovina.Core.Repository
{
    public class RacunRepository
    {
        string connectionString = "Data Source=DESKTOP-KK5EFEA;Initial Catalog=TrgovinaDatabase;Integrated Security=True";
        public int DodajRacun(Racun racun)
        {
            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();

                string sql = @"INSERT INTO Racuni (Datum, Ukupno)
                       VALUES (@Datum, @Ukupno);

                       SELECT SCOPE_IDENTITY();";

                using (SqlCommand komanda = new SqlCommand(sql, konekcija))
                {
                    komanda.Parameters.AddWithValue("@Datum", racun.Datum);
                    komanda.Parameters.AddWithValue("@Ukupno", racun.UkupnaCijena);

                    return Convert.ToInt32(komanda.ExecuteScalar());
                }
            }
        }

        public List<Racun> VratiSveRacune()
        {
            List<Racun> racuni = new List<Racun>();

            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();

                string sql = "SELECT Id, Datum, Ukupno FROM Racuni";

                using (SqlCommand komanda = new SqlCommand(sql, konekcija))
                {
                    using (SqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Racun racun = new Racun();
                            racun.Id = Convert.ToInt32(reader["Id"]);
                            racun.Datum = Convert.ToDateTime(reader["Datum"]);
                            racuni.Add(racun);
                        }
                    }
                }
            }

            return racuni;
        }

        public void SacuvajRacun(Racun racun)
        {
            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();

                SqlTransaction transakcija = konekcija.BeginTransaction();
                try
                {
                    string sqlRacun = @"INSERT INTO Racuni (Datum, Ukupno)
                                VALUES (@Datum, @Ukupno);
                                SELECT SCOPE_IDENTITY();";

                    int racunId;

                    using (SqlCommand komanda = new SqlCommand(sqlRacun, konekcija, transakcija))
                    {
                        komanda.Parameters.AddWithValue("@Datum", racun.Datum);
                        komanda.Parameters.AddWithValue("@Ukupno", racun.UkupnaCijena);

                        racunId = Convert.ToInt32(komanda.ExecuteScalar());
                    }

                    foreach (StavkaRacuna stavka in racun.Stavke)
                    {
                        string sqlStavka = @"INSERT INTO StavkeRacuna
                                     (RacunId, ArtikalId, Kolicina, CijenaPoJedinici)
                                     VALUES
                                     (@RacunId, @ArtikalId, @Kolicina, @CijenaPoJedinici)";

                        using (SqlCommand komanda = new SqlCommand(sqlStavka, konekcija, transakcija))
                        {
                            komanda.Parameters.AddWithValue("@RacunId", racunId);
                            komanda.Parameters.AddWithValue("@ArtikalId", stavka.ArtikalId);
                            komanda.Parameters.AddWithValue("@Kolicina", stavka.Kolicina);
                            komanda.Parameters.AddWithValue("@CijenaPoJedinici", stavka.Artikal.Cijena);

                            komanda.ExecuteNonQuery();
                        }
                    }

                    transakcija.Commit();
                }
                catch
                {
                    transakcija.Rollback();
                    throw;
                }
            }
        }
    }
}
