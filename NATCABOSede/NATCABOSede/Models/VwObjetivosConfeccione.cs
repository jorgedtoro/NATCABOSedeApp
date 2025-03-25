using System;
using System.Collections.Generic;

namespace NATCABOSede.Models;

public partial class VwObjetivosConfeccione
{
    public string SCode { get; set; } = null!;

    public string SName { get; set; } = null!;

    public short? PercExtraOper { get; set; }

    public double? PpmObj { get; set; }

    public double? ModObj { get; set; }

    public double? FttObj { get; set; }
}
