using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trgovina.Core.Modeli;

namespace Trgovina.Tests
{
    [TestClass]
    public class KlaseTest
    {
        [TestMethod]
        public void StavkaRacunaUkupnoDobroRacuna()
        {
            Artikal artikal = new Artikal();
            artikal.Cijena = 5;
            StavkaRacuna stavka = new StavkaRacuna();
            stavka.Artikal = artikal;
            stavka.Kolicina = 3;
            Assert.AreEqual(15, stavka.Ukupno);
        }
        [TestMethod]
        public void RacunUkupnaCijenaDobroRacuna()
        {
            Artikal a1 = new Artikal();
            a1.Cijena = 10;
            Artikal a2 = new Artikal();
            a2.Cijena = 20;
            StavkaRacuna s1 = new StavkaRacuna();
            s1.Artikal = a1;
            s1.Kolicina = 2;
            StavkaRacuna s2 = new StavkaRacuna();
            s2.Artikal = a2;
            s2.Kolicina = 1;

            Racun racun = new Racun();

            racun.Stavke.Add(s1);
            racun.Stavke.Add(s2);
            Assert.AreEqual(40, racun.UkupnaCijena);
        }
        [TestMethod]
        public void KategorijaNazivJeIspravnoPostavljen()
        {
            Kategorija kategorija = new Kategorija();
            kategorija.Naziv = "Pića";
            Assert.AreEqual("Pića", kategorija.Naziv);
        }
    }
}
