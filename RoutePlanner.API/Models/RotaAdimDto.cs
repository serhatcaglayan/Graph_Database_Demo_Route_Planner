namespace RoutePlanner.API.Models
{
    public class RotaAdimDto
    {
        public string Isim    { get; set; }
        public string Tur     { get; set; }   // metro | otobus
        public string Hat     { get; set; }
        public double Sure    { get; set; }
        public double Mesafe  { get; set; }
        public double Maliyet { get; set; }
    }
}
