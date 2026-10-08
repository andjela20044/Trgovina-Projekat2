using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina.Core.Modeli
{
    public class Artikal
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int KategorijaId {  get; set; }  
        public decimal Cijena { get; set; }   
        public string JedinicaMjere { get; set; }
        public string ToString2()
        {
            return Id + "|" + Naziv + " " + Cijena + "/" + JedinicaMjere;
        }
        public override string ToString()
        {
            return $"{Id}|{Naziv} {Cijena}/{JedinicaMjere}";
        }
    }
}
