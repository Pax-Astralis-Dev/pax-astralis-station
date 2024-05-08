using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Server.GameTicking;
using Content.Server.Maps;
using Robust.Shared.Prototypes;
using Robust.Server.GameObjects;
using Robust.Server.Maps;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server._PAX.Sectors;

public sealed class GameSectorManager
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;

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

    public MapId PreLoadSectors(IMapManager mapManager, MapLoaderSystem mapLoaderSystem, GameTicker game)
    {
        if (GetSectorMapIds().Count > 0)
            return GetSectorMapIds().FirstOrDefault(MapId.Nullspace);

        var sectors = AllSectors();

        foreach (var sector in sectors)
        {
            var sectorMapId = mapManager.CreateMap();
            AddSector(sector.ID, sectorMapId);
            mapManager.AddUninitializedMap(sectorMapId);


            var offset = new Vector2(5, 5);
            if (sector.Station.Location.Distance != null)
            {
                var distance = sector.Station.Location.Distance;
                offset = _random.NextVector2((float) (distance - 500f), (float) (distance + 500f));
                Logger.Info("Spawning based on distance " + sector.Station.ID + " at X:" + offset.X + " Y:" + offset.Y);
            }

            if (sector.Station.Location.Position != null)
            {
                var pos = sector.Station.Location.Position;
                offset = new Vector2i(pos.X, pos.Y);
                Logger.Info("Spawning based on position " + sector.Station.ID + " at X:" + offset.X + " Y:" + offset.Y);
            }

            if (_prototypeManager.TryIndex<GameMapPrototype>(sector.Station.StationId, out var gameMap))
            {
                game.LoadGameMap(gameMap, sectorMapId, new MapLoadOptions
                {
                    Offset = offset
                }, sector.Name + " | " + sector.Station.StationName);
            }

            foreach (var outpost in sector.Outposts)
            {
                offset = new Vector2(5, 5);
                if (outpost.Location.Distance != null)
                {
                    var distance = outpost.Location.Distance;
                    offset = _random.NextVector2((float) (distance - 500f), (float) (distance + 500f));
                    Logger.Info("Spawning based on distance " + outpost.ID + " at X:" + offset.X + " Y:" +
                                offset.Y);
                }

                if (outpost.Location.Position != null)
                {
                    var pos = outpost.Location.Position;
                    offset = new Vector2i(pos.X, pos.Y);
                    Logger.Info("Spawning based on position " + outpost.ID + " at X:" + offset.X + " Y:" +
                                offset.Y);
                }

                if (mapLoaderSystem.TryLoad(sectorMapId, outpost.Path.ToRelativeSystemPath(), out var _,
                        new MapLoadOptions
                        {
                            Offset = offset
                        }))
                {
                }
            }
        }

        return GetSectorMapIds().FirstOrDefault(MapId.Nullspace);
    }

    public void LoadSectors(IMapManager mapManager)
    {
        var sectors = GetSectorMapIds();
        foreach (var mapId in sectors)
        {
            mapManager.DoMapInitialize(mapId);
        }
    }
}
