﻿using System;
using System.Collections.Generic;

namespace NATCABOSede.Models;

public partial class KpisHistorico
{
    public int? LineaId { get; set; }

    public string? SName { get; set; }

    public string? Lote { get; set; }

    public string? Confeccion { get; set; }

    public int? NPaquetes { get; set; }

    public int? NMinutos { get; set; }

    public int? NOperarios { get; set; }

    public double? TotalWeight { get; set; }

    public double? FTarget { get; set; }

    public double? KpiPpm { get; set; }

    public double? KpiPm { get; set; }

    public double? KpiExtrapeso { get; set; }
    //public double? KpiFTT { get; set; }
    //public double? KpiMOD { get; set; }

    public DateTime? Fecha { get; set; }
}
