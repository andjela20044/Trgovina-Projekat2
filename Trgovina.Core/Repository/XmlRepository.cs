using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trgovina.Core.Modeli;

namespace Trgovina.Core.Repository
{
    public class XmlRepository
    {
        public void SacuvajKategorije(List<Kategorija> kategorije, string putanja)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Kategorija>));

            using (FileStream fs = new FileStream(putanja, FileMode.Create))
            {
                serializer.Serialize(fs, kategorije);
            }
        }

        public void SacuvajArtikle(List<Artikal> artikli, string putanja)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Artikal>));

            using (FileStream fs = new FileStream(putanja, FileMode.Create))
            {
                serializer.Serialize(fs, artikli);
            }
        }

        public void SacuvajRacun(Racun racun, string putanja)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Racun));

            using (FileStream fs = new FileStream(putanja, FileMode.Create))
            {
                serializer.Serialize(fs, racun);
            }
        }
    }
}
