using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.GameTicking;
using Content.Shared.CCVar;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Server.Maps;
using Robust.Shared.Map;

namespace Content.Server.Sectors;

public sealed class GameSectorManager
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    [ViewVariables(VVAccess.ReadOnly)]
    private readonly Dictionary<string, MapId> _sectors = new();


    private ISawmill _log = default!;

    public void Initialize()
    {
        _log = Logger.GetSawmill("sectorsel");
    }

    public IEnumerable<GameSectorPrototype> AllSectors()
    {
        return _prototypeManager.EnumeratePrototypes<GameSectorPrototype>();
    }

    public bool CheckSectorExists(string sectorId)
    {
        return TryLookupSector(sectorId, out _);
    }


    private bool TryLookupSector(string sectorId, [NotNullWhen(true)] out GameSectorPrototype? sector)
    {
        return _prototypeManager.TryIndex(sectorId, out sector);
    }

    public void AddSector(string sectorId, MapId sectorMap)
    {
        _sectors.Add(sectorId, sectorMap);
    }

    public MapId? GetSectorMapId(string sectorId)
    {
        _sectors.TryGetValue(sectorId, out var mapId);
        return mapId;
    }

    public Dictionary<string, MapId>.ValueCollection GetSectorMapIds()
    {
        return _sectors.Values;
    }
}
