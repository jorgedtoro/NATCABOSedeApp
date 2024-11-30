using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NATCABOSede.Models;

[Table("datosKPIS")]
public partial class DatosKpi
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idLinea")]
    public int IdLinea { get; set; }

    [Column("idCliente")]
    public int? IdCliente { get; set; }

    [Column("idProducto")]
    [StringLength(10)]
    public string? IdProducto { get; set; }

    [Column("paquetesTrabajados")]
    public double? PaquetesTrabajados { get; set; }

    [Column("minutosTrabajados")]
    public double? MinutosTrabajados { get; set; }

    [Column("numeroPersonas")]
    public int? NumeroPersonas { get; set; }

    [Column("pesoReal")]
    public double? PesoReal { get; set; }

    [Column("pesoObjetivo")]
    public double? PesoObjetivo { get; set; }

    [Column("paquetesTotales")]
    public double? PaquetesTotales { get; set; }

    [Column("paquetesRechazados")]
    public double? PaquetesRechazados { get; set; }

    [Column("horaInicio", TypeName = "datetime")]
    public DateTime? HoraInicio { get; set; }

    [Column("mediaPaquetesPorMinuto")]
    public double? MediaPaquetesPorMinuto { get; set; }

    [Column("tiempoTotal")]
    public double? TiempoTotal { get; set; }

    [Column("costeHora")]
    public double? CosteHora { get; set; }

    [Column("numeroPaquetes")]
    public int? NumeroPaquetes { get; set; }

    [Column("pesoMinimo")]
    public double? PesoMinimo { get; set; }

    [Column("paquetesProducidos")]
    public double? PaquetesProducidos { get; set; }
}
