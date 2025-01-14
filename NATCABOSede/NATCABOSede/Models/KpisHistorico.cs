using System;
using System.Collections.Generic;

namespace NATCABOSede.Models;

public partial class KpisHistorico
{
    public int? LineaId { get; set; }

    public string? NombreLinea { get; set; }

    //public string? Lote { get; set; }

    public string? Confeccion { get; set; }
    public string? NombreCliente { get; set; }

    //public int? NPaquetes { get; set; }

    //public int? NMinutos { get; set; }

    //
    //public double? TotalWeight { get; set; }

    //
    public double? PPM_Marco { get; set; }

    public double? PM_Marco { get; set; }
    public double? PM_Bizerba { get; set; }

    public double? Extrapeso_Marco { get; set; }
    public double? Extrapeso_Bizerba { get; set; }
    public double? Desecho_Kg { get; set; }
    public double? Desecho_Perc { get; set; }
    public double? FTT { get; set; }
    public double? MOD { get; set; }

    public DateTime? Fecha { get; set; }
}
