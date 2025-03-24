namespace NATCABOSede.ViewModels
{
    public class ObjetivosConfeccionVM
    {
        public string SCode { get; set; }
        public string SName { get; set; }
        public short? PercExtraOper { get; set; }
        public double? PpmObj { get; set; }
        public double? ModObj { get; set; }
        public double? FttObj { get; set; }
    }

    public class ConfigurarObjetivosViewModel
    {
        public List<ObjetivosConfeccionVM> Confecciones { get; set; } = new();
    }
}
