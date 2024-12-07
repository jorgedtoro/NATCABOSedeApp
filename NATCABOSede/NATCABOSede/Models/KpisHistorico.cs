using System;
using System.Collections.Generic;

namespace NATCABOSede.Models;

public partial class KpisHistorico
{
    public int? Id { get; set; }

    public string? SName { get; set; }

    public int? Lote { get; set; }

    public string? Confeccion { get; set; }

    public int? NPaquetes { get; set; }

    public int? NMinutos { get; set; }

    public int? NOperaciones { get; set; }

    public double? TotalWeight { get; set; }

    public int? FTarget { get; set; }

    public double? KpiPpm { get; set; }

    public double? KpiPm { get; set; }

    public double? KpiExtrapeso { get; set; }

    public int? LineaId { get; set; }

    public DateTime? Fecha { get; set; }
}
