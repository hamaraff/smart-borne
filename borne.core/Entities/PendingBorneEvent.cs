using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace borne.core.Entities;

public sealed record PendingBorneEvent(
    Guid EventId,
    string EventType,
    string PayloadJson,
    DateTimeOffset createdAtUtc,
    int Attempts
    );

