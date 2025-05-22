namespace WebApplication2.Models
{
    public class Grupa
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public DateTime DatumOsnivanja { get; set; }

       // public List<Korisnik> Korisnici { get; set; } = new List<Korisnik>();

        public Grupa(int id, string ime, DateTime datumOsnivanja)
        {
            Id = id;
            Ime = ime;
            this.DatumOsnivanja = datumOsnivanja;
        }
    }
}
