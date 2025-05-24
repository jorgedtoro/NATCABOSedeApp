using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NATCABOSede.Models;

[Table("TMarcoBizerba")]
public partial class TMarcoBizerba
{
    [Key]
    [Column("IdLineaMarco")]
    public short IdLineaMarco { get; set; }

    [Column("DeviceNoBizerba")]
    public short DeviceNoBizerba { get; set; }

    [Column("ScreeNo")]
    public short ScreeNo { get; set; }
}
