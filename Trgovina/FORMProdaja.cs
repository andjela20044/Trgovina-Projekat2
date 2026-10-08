using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trgovina.Core.Modeli;
using Trgovina.Core.Repository;
using System.IO;
using Trgovina.Core.Dogadjaji;

namespace Trgovina
{
    public partial class FORMProdaja : Form
    {
        
        private KategorijaRepository kategorijaRepository;
        private ArtikalRepository artikalRepository;
        private RacunRepository racunRepository;
        private Racun trenutniRacun = new Racun();
        private List<StavkaRacuna> stavkeRacuna = new List<StavkaRacuna>();
        private Artikal izabraniArtikal;
        private Button izabranoDugmeKategorije;
        private Button izabranoDugmeArtikla;
        public event AlkoholDelegat PokusajKupovineAlkohola;
        XmlRepository xmlRepository = new XmlRepository();
       
        public FORMProdaja()
        {
            InitializeComponent();
            button7.Click += Klik;
            button8.Click += Klik; 
            button9.Click += Klik;
            button10.Click += Klik;
            button11.Click += Klik;
            button12.Click += Klik;
            button13.Click += Klik;
            button14.Click += Klik;
            button15.Click += Klik;
            button17.Click += Klik;
            kategorijaRepository = new KategorijaRepository();
            artikalRepository = new ArtikalRepository();
            racunRepository = new RacunRepository();
            PokusajKupovineAlkohola += UpisiPokusajKupovineAlkohola;
            UcitajKategorije();        

        }

        private void OsvjeziRacun()
        {
            listBox1.Items.Clear();
            decimal ukupno = 0;
            foreach (StavkaRacuna stavka in stavkeRacuna)
            {
                decimal iznos = stavka.Kolicina * stavka.Artikal.Cijena;
                ukupno += iznos;
                listBox1.Items.Add(stavka.Artikal.Naziv + " x " + stavka.Kolicina + " = " + iznos.ToString("0.00") + " din");
            }
            label1.Text = "Ukupno: " + ukupno.ToString("0.00") + " din";
        }
        private void UcitajKategorije()
        {
            flowLayoutPanel1.Controls.Clear();
            List<Kategorija> kategorije = kategorijaRepository.VratiSve();
            foreach (Kategorija kategorija in kategorije)
            {
                Button dugme = new Button();
                dugme.Text = kategorija.Naziv;
                dugme.Width = 172;
                dugme.Height = 45;
                dugme.BackColor = SystemColors.ButtonHighlight;
                dugme.Font = new Font("Constantia", 11, FontStyle.Regular);
                dugme.Tag = kategorija;
                dugme.Click += DugmeKategorija_Click;
                flowLayoutPanel1.Controls.Add(dugme);
            }
        }
        private void DugmeKategorija_Click(object sender, EventArgs e)
        {
            Button kliknutoDugme = sender as Button;
            if (kliknutoDugme == null)
                return;
            if (izabranoDugmeKategorije != null)
            {
                izabranoDugmeKategorije.BackColor = SystemColors.Control;
            }

            kliknutoDugme.BackColor = SystemColors.ActiveCaption;
            izabranoDugmeKategorije = kliknutoDugme;
            Kategorija kategorija = kliknutoDugme.Tag as Kategorija;
            if (kategorija != null)
            {
                UcitajArtikleZaKategoriju(kategorija.Id);
            }
        }
        private void UcitajArtikleZaKategoriju(int kategorijaId)
        {
            flowLayoutPanel2.Controls.Clear();
            List<Artikal> artikli = artikalRepository.VratiArtiklePoKategoriji(kategorijaId);
            foreach (Artikal artikal in artikli)
            {
                Button dugme = new Button();
                dugme.Text = artikal.Naziv;
                dugme.Width = 172;
                dugme.Height = 50;
                dugme.BackColor = SystemColors.ButtonHighlight;
                dugme.Font = new Font("Constantia", 10, FontStyle.Regular);
                dugme.Tag = artikal;
                dugme.Click += DugmeArtikal_Click;
                flowLayoutPanel2.Controls.Add(dugme);
            }
        }
        private void DugmeArtikal_Click(object sender, EventArgs e)
        {
            Button kliknutoDugme = sender as Button;
            if (kliknutoDugme == null)
                return;
            if (izabranoDugmeArtikla != null)
            {
                izabranoDugmeArtikla.BackColor = SystemColors.Control;
            }
            kliknutoDugme.BackColor = SystemColors.ActiveCaption;
            izabranoDugmeArtikla = kliknutoDugme;
            izabraniArtikal = kliknutoDugme.Tag as Artikal;
            if (izabraniArtikal != null)
            {
                textBox2.Text = izabraniArtikal.Naziv;
            }
        }
        //metoda za dugmiće s brojevima
        private void Klik(object sender, EventArgs e)
        {
            Button dugme = sender as Button;

            if (dugme == null)
                return;
            textBox1.Text += dugme.Text;
        }
        private void UpisiPokusajKupovineAlkohola(object sender, AlkoholArgs e)
        {
            string path = "pokusaji_alkohol.txt";
            string text = "Pokušaj kupovine alkohola poslije 22h" + Environment.NewLine +
                "Artikal: " + e.NazivArtikla + Environment.NewLine +
                "Vrijeme: " + e.VrijemePokusaja.ToString("dd.MM.yyyy HH:mm:ss") + Environment.NewLine +
                "--------------------------------------" + Environment.NewLine;

            File.AppendAllText(path, text);
        }

        //unesi dugme
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (izabraniArtikal == null)
            {
                MessageBox.Show("Izaberite artikal.");
                return;
            }//TEST ZA ALKOHOL
            if (izabraniArtikal.KategorijaId == 4 && DateTime.Now.Hour >= 22)
            {
                if (PokusajKupovineAlkohola != null)
                {
                    AlkoholArgs args = new AlkoholArgs();
                    args.NazivArtikla = izabraniArtikal.Naziv;
                    args.VrijemePokusaja = DateTime.Now;

                    PokusajKupovineAlkohola(this, args);
                }

                MessageBox.Show("Prodaja alkoholnih pića nije dozvoljena poslije 22h.");
                return;
            }

            decimal kolicina = 1;

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (!decimal.TryParse(textBox1.Text, out kolicina))
                {
                    MessageBox.Show("Neispravna količina.");
                    return;
                }
            }

            StavkaRacuna stavka = new StavkaRacuna();

            stavka.Artikal = izabraniArtikal;
            stavka.ArtikalId = izabraniArtikal.Id;
            stavka.Kolicina = kolicina;

            stavkeRacuna.Add(stavka);

            OsvjeziRacun();

            textBox1.Clear();
            textBox2.Clear();

            izabraniArtikal = null;

        }
        //dugme za brisanje označene stavke sa spiska
        private void button4_Click(object sender, EventArgs e)
        {
            int ind = listBox1.SelectedIndex;
            if (ind == -1)
            {
                MessageBox.Show("Izaberite stavku koju želite obrisati.");
                return;
            }
            stavkeRacuna.RemoveAt(ind);
            OsvjeziRacun();
        }
        //dugme za placanje i pravljenje racuna
        private void button5_Click(object sender, EventArgs e)
        {
            if (stavkeRacuna.Count == 0)
            {
                MessageBox.Show("Račun je prazan.");
                return;
            }
            textBox3.Clear();
            textBox3.AppendText("FISKALNI RAČUN" + Environment.NewLine);
            textBox3.AppendText("===========================" + Environment.NewLine);
            textBox3.AppendText("Datum: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + Environment.NewLine);
            textBox3.AppendText("===========================" + Environment.NewLine);
            decimal ukupno = 0;
            foreach (StavkaRacuna stavka in stavkeRacuna)
            {
                decimal iznos = stavka.Kolicina * stavka.Artikal.Cijena;
                ukupno += iznos;

                textBox3.AppendText(stavka.Artikal.Naziv +" x " +stavka.Kolicina +" = " +iznos.ToString("0.00") +" din" +  Environment.NewLine);
            }

            textBox3.AppendText("===========================" + Environment.NewLine);
            textBox3.AppendText("UKUPNO: " + ukupno.ToString("0.00") + " din");
            tabControl1.SelectedTab = tabPage2;
        }
        //naplata-čuvanje računa
        private void button18_Click(object sender, EventArgs e)
        {
            if (stavkeRacuna.Count == 0)
            {
                MessageBox.Show("Račun je prazan.");
                return;
            }
            Racun racun = new Racun();
            racun.Datum = DateTime.Now;
            foreach (StavkaRacuna stavka in stavkeRacuna)
            {
                racun.Stavke.Add(stavka);
            }

            racunRepository.SacuvajRacun(racun);
            xmlRepository.SacuvajRacun(racun, "racun_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml");
            MessageBox.Show("Račun je uspješno sačuvan.");
            stavkeRacuna.Clear();
            OsvjeziRacun();

            listBox1.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            izabraniArtikal = null;
        }
        //nova transakcija
        private void button1_Click(object sender, EventArgs e)
        {
            stavkeRacuna.Clear();

            OsvjeziRacun();
            if (izabranoDugmeArtikla != null)
            {
                izabranoDugmeArtikla.BackColor = SystemColors.Control;
            }
            if (izabranoDugmeKategorije != null)
            {
                izabranoDugmeKategorije.BackColor = SystemColors.Control;
            }
            listBox1.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            label1.Text = "-----------------";
            izabraniArtikal = null;

            tabControl1.SelectedTab = tabPage1;
        }
          //zarezDugme
        private void button16_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains(","))
            {
                textBox1.Text += ",";
            }
        }

        private void FORMProdaja_Load(object sender, EventArgs e)
        {

        }
    }
}
