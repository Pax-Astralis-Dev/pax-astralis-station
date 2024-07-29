using Content.Shared.Chat.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Crawl;

/// <summary>
/// Allows players to lay down and stand up using emotes.
/// </summary>
[RegisterComponent, Access(typeof(CrawlSystem))]
public sealed partial class CrawlComponent : Component
{
    [DataField("layEmote", customTypeSerializer: typeof(PrototypeIdSerializer<EmotePrototype>))]
    public string LayEmoteId = "LayDown";

    [DataField("standEmote", customTypeSerializer: typeof(PrototypeIdSerializer<EmotePrototype>))]
    public string StandEmoteId = "StandUp";

    [DataField("crawlAction", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string CrawlAction = "ActionCrawl";

    [DataField("crawlActionEntity")]
    public EntityUid? CrawlActionEntity;

    [ViewVariables]
    public bool Laying = false;

    [ViewVariables]
    public float CrawlSpeed = 0.33f;
}
