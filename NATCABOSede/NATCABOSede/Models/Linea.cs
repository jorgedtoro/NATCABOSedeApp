using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NATCABOSede.Models;

public partial class Linea
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    public int NumeroLinea { get; set; }

    [StringLength(8)]
    public string NombreLinea { get; set; } = null!;
}
