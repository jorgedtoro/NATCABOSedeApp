namespace NATCABOSede.ViewModels
{
    public class DatosKpiViewModel
    {
        // Información general
        public string Cliente { get; set; }
        public string Producto { get; set; }
       
        // KPIs
        public double PPM { get; set; }
        public double PM { get; set; }
        public double ExtraPeso { get; set; }
        public double CosteMOD { get; set; }

        // Horas y porcentajes
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFinAproximada { get; set; }
        public double PorcentajePedido { get; set; }
    }
}
