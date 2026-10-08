using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina.Core.Modeli
{
    public class Racun
    {

        public int Id {  get; set; }    
        public DateTime Datum { get; set; }
        public List<StavkaRacuna> Stavke { get; set; }
        = new List<StavkaRacuna>();
        public decimal UkupnaCijena => Stavke.Sum(s => s.Ukupno);
    }
}
