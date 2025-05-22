namespace WebApplication2.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime {  get; set; }
        public string Prezime { get; set; }
        public DateTime Datum { get; set; }
        public Grupa? Grupa { get; set; }

       // public List<Grupa> Grupe { get; set; } = new List<Grupa>();

        public Korisnik(int id,string korisnickoIme, string ime, string prezime, DateTime datum)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            Datum = datum;
        }
    }

    
}
