using System.Globalization;
using WebApplication2.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication2.Repositories
{
    public class KorisnikRepozitorijum
    {
        private const string filePath = "data/korisnici.csv";
        public static Dictionary<int, Korisnik> Data;

        public KorisnikRepozitorijum()
        {
            if (Data == null)
            {
                Ucitaj();
            }
        }

        private void Ucitaj()
        {
            Data = new Dictionary<int, Korisnik>();
            string[] linije = File.ReadAllLines(filePath);
            foreach (string linija in linije)
            {
                string[] delovi = linija.Split(',');
                int id = int.Parse(delovi[0]);
                string korisnickoIme = delovi[1];
                string ime = delovi[2];
                string prezime = delovi[3];
                DateTime datum = DateTime.ParseExact(delovi[4], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Korisnik korisnik = new Korisnik(id, korisnickoIme, ime, prezime, datum);
                Data[id] = korisnik;
            }
        }

        public void Sacuvaj()
        {
            List<string> linije = new List<string>();
            foreach (Korisnik korisnik in Data.Values)
            {
                string linija = $"{korisnik.Id},{korisnik.KorisnickoIme},{korisnik.Ime},{korisnik.Prezime},{korisnik.Datum:yyyy-MM-dd}";
                linije.Add(linija);
            }
            File.WriteAllLines(filePath, linije);
        }
    }
}
    
