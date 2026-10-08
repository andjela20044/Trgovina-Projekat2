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
    public class StatistikaRepository
    {
        private string connectionString;

        public StatistikaRepository() {
            connectionString = ConfigurationManager
        .ConnectionStrings["TrgovinaKonekcija"]
        .ConnectionString;
        }

        public List<StatistikaArtikala> VratiStatistikuZaDatum(DateTime datum)
        {
            List<StatistikaArtikala> statistika = new List<StatistikaArtikala>();

            using (SqlConnection konekcija = new SqlConnection(connectionString))
            {
                konekcija.Open();

                string sql = @"
                    SELECT 
                        a.Naziv AS NazivArtikla,
                        SUM(CASE 
                            WHEN CAST(r.Datum AS date) = @Datum 
                            THEN sr.Kolicina 
                            ELSE 0 
                        END) AS KolicinaZaDatum,
                        SUM(sr.Kolicina) AS UkupnaKolicina
                    FROM Artikli a
                    INNER JOIN StavkeRacuna sr 
                        ON a.Id = sr.ArtikalId
                    INNER JOIN Racuni r 
                        ON sr.RacunId = r.Id
                    GROUP BY a.Id, a.Naziv
                    HAVING SUM(CASE 
                            WHEN CAST(r.Datum AS date) = @Datum 
                            THEN sr.Kolicina 
                            ELSE 0 
                        END) > 0";

                using (SqlCommand komanda = new SqlCommand(sql, konekcija))
                {
                    komanda.Parameters.AddWithValue("@Datum", datum.Date);

                    using (SqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StatistikaArtikala s = new StatistikaArtikala();

                            s.NazivArtikla = reader["NazivArtikla"].ToString();
                            s.KolicinaZaDatum = Convert.ToDecimal(reader["KolicinaZaDatum"]);
                            s.UkupnaKolicina = Convert.ToDecimal(reader["UkupnaKolicina"]);

                            statistika.Add(s);
                        }
                    }
                }
            }

            return statistika;
        }

    }
}
