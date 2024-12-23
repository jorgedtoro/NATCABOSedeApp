using System;
using System.Collections.Generic;

namespace NATCABOSede.Models;

public partial class DatosKpi
{
    public int Id { get; set; }

    public short IdLinea { get; set; }

    public string? NombreLinea { get; set; }

    public string? NombreLote { get; set; }

    public string? NombreProducto { get; set; }

    public string? NombreCliente { get; set; }

    public DateTime? HoraInicioLote { get; set; }

    public int? PaquetesValidos { get; set; }

    public int? MinutosTrabajados { get; set; }

    public int? NumeroOperadores { get; set; }

    public double? PesoObjetivo { get; set; }

    public int? PaquetesRequeridos { get; set; }

    public double? PpmObjetivo { get; set; }

    public double? PesoTotalDesperdicio { get; set; }

    public double? CosteHora { get; set; }

    public double? CosteKg { get; set; }

    public int? PaquetesTotales { get; set; }

    public int? PaquetesRechazadosDisc { get; set; }

    public double? PesoTotalReal { get; set; }

    public DateTime? HoraInicioProduccion { get; set; }

    public DateTime? HoraUltimoPaquete { get; set; }

    public int? PaquetesTotalesDisc { get; set; }

    public double? PesoTotalRealDisc { get; set; }
}
