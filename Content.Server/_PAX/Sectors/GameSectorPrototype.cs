using Content.Server.Station;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Diagnostics;

namespace Content.Server.Sectors;

/// <summary>
/// Prototype data for a sector.
/// </summary>
[Prototype("sector"), PublicAPI]
[DebuggerDisplay("Sector [{ID} - {Name}]")]
public sealed partial class GameSectorPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Name of the Sector, shown when choosing FTL location
    /// </summary>
    [DataField("name", required: true)]
    public string Name { get; private set; } = default!;



}
