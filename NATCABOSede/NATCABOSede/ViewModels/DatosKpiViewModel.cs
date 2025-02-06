namespace NATCABOSede.ViewModels
{
    public class DatosKpiViewModel
    {
        // Información general
        public string NombreLinea { get; set; }
        public string Cliente { get; set; }
        public string Producto { get; set; }

        // KPIs
        public double PPM { get; set; }
        public double PPM_Disc { get; set; }
        public double PM { get; set; }
        public double PM_Disc { get; set; }
        public double ExtraPeso { get; set; }
        public double ExtraPeso_Disc { get; set; }
        public double CosteMOD { get; set; }
        public double FTT { get; set; }
        public double PesoTotalDesperdicio { get; set; }
        public double PorcentajeTotalDesperdicio { get; set; }

        // Horas y porcentajes
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFinAproximada { get; set; }
        public double PorcentajePedido { get; set; }

        //Propiedades para manejo de colores
        public double ppm_objetivo { get; set; }

        public double Ppm_5min { get; set; }

        public string PpmCardClass { get; set; }
    }
}
