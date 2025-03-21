using System;
using System.Collections.Generic;

namespace NATCABOSede.Models;

public partial class DatosKpisHistorico
{
    public double? Mod { get; set; }

    public double? PpmMarco { get; set; }

    public double? PpmBizerba { get; set; }

    public double? PmMarco { get; set; }

    public double? PmBizerba { get; set; }

    public double? ExtrapesoMarco { get; set; }

    public double? ExtrapesoBizerba { get; set; }

    public double? Ftt { get; set; }

    public double? DesechoKg { get; set; }

    public double? DesechoPerc { get; set; }

    public string? NombreLinea { get; set; }

    public string? Confeccion { get; set; }

    public string? NombreCliente { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdLinea { get; set; }

    public double? CosteHora { get; set; }

    public double? CosteKg { get; set; }

    public double? PesoObjetivo { get; set; }

    public int? PaquetesTotales { get; set; }

    public double? PesoTotalReal { get; set; }

    public int? NumberOfMinutes { get; set; }

    public int? DeviceMachineNo { get; set; }

    public double? PmBizerbaTotal { get; set; }

    public int? PaquetesValidosDisc { get; set; }

    public int? PaquetesRechazadosDisc { get; set; }

    public int? PesoTotalRealDisc { get; set; }

    public int? PaquetesTotalesDisc { get; set; }

    public DateTime? HoraInicioProduccion { get; set; }

    public double? TotalHours { get; set; }

    public int? PaquetesValidos { get; set; }
}
