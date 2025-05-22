using System.Globalization;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class GrupaRepository
    {
        private const string filePath = "data/grupe.csv";
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
        }

        public void Sacuvaj()
        {
            List<string> lines = new List<string>();

            foreach (Grupa gr in Data.Values)
            {
               
                lines.Add($"{gr.Id},{gr.Ime},{gr.DatumOsnivanja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
