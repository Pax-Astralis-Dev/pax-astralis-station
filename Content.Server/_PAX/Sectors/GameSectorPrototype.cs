using System.Diagnostics;
using Content.Server._PAX.Maps;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server._PAX.Sectors;

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

    /// <summary>
    /// IFF tags
    /// </summary>
    [DataField("station", required: true)]
    public GameStationPrototype Station { get; private set; } = default!;

    [DataField("outposts", required: true)]
    public List<GameOutpostPrototype> Outposts { get; private set; } = default!;
}
