using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace borne.core.Entities;

public sealed record BorneTariffSchedule(
    Tarif Tarif,
    IReadOnlyList<Tranche> Tranches,
    string Version
    );
