namespace NATCABOSede.ViewModels
{
    public class DiscriminadoraGroupViewModel
    {
        public short? DeviceNoBizerba { get; set; }
        public List<LineaKpiViewModel> Lineas { get; set; } = new();
    }

    public class LineaKpiViewModel
    {
        public string NombreLinea { get; set; }
        public double PPM { get; set; }
        public double Objetivo { get; set; }
    }

}
