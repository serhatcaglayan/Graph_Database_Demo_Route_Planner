namespace RoutePlanner.API.Models
{
    public class IstasyonDto
    {
        public string Id          { get; set; }
        public string Isim        { get; set; }
        public double Enlem       { get; set; }
        public double Boylam      { get; set; }
        public string Hat         { get; set; }
        public List<string> HatListesi { get; set; } = new();
        public List<string> TurListesi { get; set; } = new();
    }
}
