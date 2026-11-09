using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace borne.core.Entities;

public sealed record BorneSessionState(
    int ReservationId,
    int TarifId,
    string TarifVersion,
    bool IsPaused,
    decimal LastCumulativeQuantity,
    decimal LastCumulativeAmount,
    DateTimeOffset UpdatedAtUtc
    );
