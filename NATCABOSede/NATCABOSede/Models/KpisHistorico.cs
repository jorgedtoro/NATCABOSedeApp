using System;
using System.Collections.Generic;

namespace NATCABOSede.Models;

public partial class KpisHistorico
{
    public int? LineaId { get; set; }

    public string? NombreLinea { get; set; }

    public string? NombreCliente { get; set; }

    public string? Confeccion { get; set; }

    public DateTime? Fecha { get; set; }

    public double? ExtrapesoMarco { get; set; }

    public double? ExtrapesoBizerba { get; set; }

    public double? PmMarco { get; set; }

    public double? PmBizerba { get; set; }

    public double? PpmMarco { get; set; }

    public double? DesechoKg { get; set; }

    public double? DesechoPerc { get; set; }
}
