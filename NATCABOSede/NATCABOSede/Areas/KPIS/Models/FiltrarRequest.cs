namespace NATCABOSede.Areas.KPIS.Models
{
    public class FiltrarRequest
    {
        public int? LineaId { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }
}
