using System.Globalization;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class GrupaRepository
    {
        private const string filePath = "data/grupe.csv";
        private const string clanstaPath = "data/clanstav.csv";
        public static Dictionary<int, Grupa> Data;

        public GrupaRepository()
        {
            if(Data == null)
            {
                Ucitaj();
            }
        }

        private void Ucitaj()
        {
            Data = new Dictionary<int, Grupa>();
            string[] lines = File.ReadAllLines(filePath);

            foreach(string line in lines)
            {
                string[] atributi = line.Split(",");
                int id = int.Parse(atributi[0]);
                string naziv = atributi[1];
                DateTime datum = DateTime.ParseExact(atributi[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Grupa grupa = new Grupa(id, naziv, datum);

                Data[id] = grupa;
            }

            string[] clanstva = File.ReadAllLines(clanstaPath);

            foreach(string line in clanstva)
            {
                string[] delovi = line.Split(",");
                int korisnikId = int.Parse(delovi[0]);
                int grupaId = int.Parse(delovi[1]);

                if(Data.ContainsKey(grupaId) && KorisnikRepozitorijum.Data.ContainsKey(korisnikId))
                {
                    Data[grupaId].Korisnici.Add(KorisnikRepozitorijum.Data[korisnikId]);
                }
            }
        }

        public void Sacuvaj()
        {
            List<string> lines = new List<string>();
            List<string> clanstva = new List<string>();

            foreach (Grupa gr in Data.Values)
            {
                lines.Add($"{gr.Id},{gr.Ime},{gr.DatumOsnivanja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

                foreach (Korisnik korisnik in gr.Korisnici) 
                {
                    clanstva.Add($"{korisnik.Id},{gr.Id}"); 
                }
            }

            File.WriteAllLines(filePath, lines);
            File.WriteAllLines(clanstaPath, clanstva); 
        }


    }
}
