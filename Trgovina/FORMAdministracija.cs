using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trgovina.Core.Repository;
using Trgovina.Core.Modeli;
using System.IO;
using Trgovina.Core.Dogadjaji;
using System.Configuration;



namespace Trgovina
{
    public partial class FORMAdministracija : Form
    {
        private KategorijaRepository kategorijaRepository;
        private ArtikalRepository artikalRepository;
        private StatistikaRepository statistikaRepository;
        private XmlRepository xmlRepository = new XmlRepository();
      
        public FORMAdministracija()//konstruktor
        {
            InitializeComponent();
            kategorijaRepository = new KategorijaRepository();
            artikalRepository = new ArtikalRepository();
            statistikaRepository = new StatistikaRepository();
            UcitajKategorijeUListBox();
            UcitajArtikle();
            UcitajKategorije();
        }
        private void UcitajKategorije()
        {
            listBox1.Items.Clear();
            List<Kategorija> kategorije = kategorijaRepository.VratiSve();
            foreach (Kategorija k in kategorije)
            {
                listBox1.Items.Add(k.Id + " | " + k.Naziv);
            }
        }
        private void UcitajKategorijeUListBox()
        {
            comboBox1.DataSource = null;
            comboBox1.DataSource = kategorijaRepository.VratiSve();
            comboBox1.DisplayMember = "Naziv";
            comboBox1.ValueMember = "Id";
        }
        private void UcitajArtikle()
        {
            listBox2.DataSource = null;
            listBox2.DataSource = artikalRepository.VratiSve();
        }
        private void OcistiPoljaArtikla()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = -1;
            listBox2.ClearSelected();
        }




        //prvo dugme za dodavanje kategorije
        private void DodajKategoriju_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Unesite naziv kategorije.");
                return;
            }
            Kategorija kategorija = new Kategorija();
            kategorija.Naziv = textBox1.Text.Trim();
            kategorijaRepository.Dodaj(kategorija);
            xmlRepository.SacuvajKategorije(kategorijaRepository.VratiSve(), Path.Combine(Application.StartupPath, "kategorije.xml"));
            textBox1.Clear();
            UcitajKategorije();
            MessageBox.Show("Kategorija je uspješno dodata.");
        }
        //izmjeni kategoriju
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Izaberite kategoriju.");
                return;
            }
            string red = listBox1.SelectedItem.ToString();
            int id = int.Parse(red.Split('|')[0]);
            kategorijaRepository.Izmijeni(id,textBox1.Text.Trim());
            UcitajKategorije();
            xmlRepository.SacuvajKategorije(kategorijaRepository.VratiSve(),Path.Combine(Application.StartupPath, "kategorije.xml"));
            MessageBox.Show("Kategorija izmijenjena.");
        }
        //dugme za brisanje
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Izaberite kategoriju.");
                return;
            }

            string red = listBox1.SelectedItem.ToString();
            int id = int.Parse(red.Split('|')[0]);
            kategorijaRepository.Obrisi(id);
            textBox1.Clear();
            UcitajKategorije();
            xmlRepository.SacuvajKategorije(kategorijaRepository.VratiSve(), Path.Combine(Application.StartupPath, "kategorije.xml"));
            MessageBox.Show("Kategorija obrisana.");
        }
        //lista kategorija koje imamo
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            string red = listBox1.SelectedItem.ToString();
            string[] dijelovi = red.Split('|');
            textBox1.Text = dijelovi[1].Trim();
        }
        //dodaj artikal
        private void button6_Click(object sender, EventArgs e)
        {

            if (!decimal.TryParse(textBox3.Text, out decimal cijena))
            {
                MessageBox.Show("Cijena mora biti broj.");
                textBox3.Clear();
                textBox3.Focus();
                return;
            }
            Artikal artikal = new Artikal();
                    
            artikal.Naziv = textBox2.Text;
            artikal.Cijena = cijena;
            artikal.JedinicaMjere = textBox4.Text;
            artikal.KategorijaId = (int)comboBox1.SelectedValue;

            artikalRepository.Dodaj(artikal);
            xmlRepository.SacuvajArtikle(artikalRepository.VratiSve(),Path.Combine(Application.StartupPath, "artikli.xml"));
            UcitajArtikle();
            OcistiPoljaArtikla();
        }
        //izmjeni artikal
        private void button5_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(textBox3.Text, out decimal cijena))
            {
                MessageBox.Show("Cijena mora biti broj.");
                textBox3.Clear();   
                textBox3.Focus();
                return;
            }
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Izaberite artikal.");
                return;
            }

            Artikal artikal = (Artikal)listBox2.SelectedItem;

            artikal.Naziv = textBox2.Text;
            artikal.Cijena = cijena;
            artikal.JedinicaMjere = textBox4.Text;
            artikal.KategorijaId = (int)comboBox1.SelectedValue;

            artikalRepository.Izmijeni(artikal);

            UcitajArtikle();
            xmlRepository.SacuvajArtikle(artikalRepository.VratiSve(), Path.Combine(Application.StartupPath, "artikli.xml"));

            MessageBox.Show("Artikal uspješno izmijenjen.");
            OcistiPoljaArtikla();
        }
        //obrisi artikal
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Izaberite artikal koji želite obrisati.");
                return;
            }

            Artikal artikal = (Artikal)listBox2.SelectedItem;

            artikalRepository.Obrisi(artikal.Id);

            UcitajArtikle();
            xmlRepository.SacuvajArtikle(artikalRepository.VratiSve(), Path.Combine(Application.StartupPath, "artikli.xml"));

            OcistiPoljaArtikla();
           // textBox2.Clear();
            //textBox3.Clear();
            //textBox4.Clear();

            MessageBox.Show("Artikal je uspješno obrisan.");
        } 
        //listbox sa artiklima
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;

            Artikal artikal = (Artikal)listBox2.SelectedItem;

            textBox2.Text = artikal.Naziv;
            textBox3.Text = artikal.Cijena.ToString();
            textBox4.Text = artikal.JedinicaMjere;
            comboBox1.SelectedValue = artikal.KategorijaId;
        }
        //statistika
        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            DateTime datum = dateTimePicker1.Value.Date;

            List<StatistikaArtikala> lista =
                statistikaRepository.VratiStatistikuZaDatum(datum);

            int y = 20;

            foreach (StatistikaArtikala s in lista)
            {
                Label lblNaziv = new Label();
                lblNaziv.Text = s.NazivArtikla;
                lblNaziv.Left = 20;
                lblNaziv.Top = y;
                lblNaziv.Width = 200;

                ProgressBar pb = new ProgressBar();
                pb.Left = 240;
                pb.Top = y;
                pb.Width = 250;
                pb.Height = 25;

                int procenat = 0;

                if (s.UkupnaKolicina > 0)
                {
                    procenat = (int)((s.KolicinaZaDatum / s.UkupnaKolicina) * 100);
                }

                pb.Minimum = 0;
                pb.Maximum = 100;
                pb.Value = Math.Min(procenat, 100);

                Label lblKolicina = new Label();
                lblKolicina.Text = s.KolicinaZaDatum.ToString();
                lblKolicina.Left = 510;
                lblKolicina.Top = y;
                lblKolicina.Width = 50;

                panel1.Controls.Add(lblNaziv);
                panel1.Controls.Add(pb);
                panel1.Controls.Add(lblKolicina);

                y += 40;
            }

            if (lista.Count == 0)
            {
                Label lblNema = new Label();
                lblNema.Text = "Nema prodatih artikala za izabrani datum.";
                lblNema.Left = 20;
                lblNema.Top = 20;
                lblNema.Width = 300;

                panel1.Controls.Add(lblNema);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
