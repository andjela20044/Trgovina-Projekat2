using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina.Core.Modeli
{
    public class StatistikaArtikala
    {//DTO
        public string NazivArtikla { get; set; }
        public decimal KolicinaZaDatum { get; set; }
        public decimal UkupnaKolicina { get; set; }
    }
}
