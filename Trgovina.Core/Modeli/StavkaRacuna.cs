using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina.Core.Modeli
{
    public class StavkaRacuna
    {
        public int Id { get; set; }
        public int RacunId { get; set; }
        public int ArtikalId { get; set; }
        public Artikal Artikal { get; set; }
        public decimal Kolicina { get; set; }
        public decimal Ukupno => Artikal.Cijena * Kolicina;
    }
}
