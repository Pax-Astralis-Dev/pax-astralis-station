using Content.Server.Actions;
using Content.Server.Chat.Systems;
using Content.Shared.Actions.Events;
using Content.Shared.Body.Systems;
using Content.Shared.Buckle.Components;
using Content.Shared.Humanoid;
using Content.Shared.Mobs;
using Content.Shared.Movement.Systems;
using Content.Shared.Standing;

namespace Content.Server.Crawl;

public sealed class CrawlSystem : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _modifier = default!;
    [Dependency] private readonly StandingStateSystem _standingSystem = default!;
    [Dependency] private readonly SharedBodySystem _bodySystem = default!;
    [Dependency] private readonly ActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CrawlComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<CrawlComponent, ComponentShutdown>(OnShutdown);
        //SubscribeLocalEvent<CrawlComponent, BuckledEvent>(OnBuckled);
        //SubscribeLocalEvent<CrawlComponent, UnbuckledEvent>(OnUnbuckled);
        SubscribeLocalEvent<CrawlComponent, MobStateChangedEvent>(OnMobStateChanged);
        SubscribeLocalEvent<CrawlComponent, EmoteEvent>(OnEmote);
        SubscribeLocalEvent<CrawlComponent, CrawlActionEvent>(OnCrawlAction);
        SubscribeLocalEvent<CrawlComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshMovementSpeedModifiers);
    }

    private void OnMapInit(EntityUid uid, CrawlComponent component, MapInitEvent args)
    {
        // try to add crawl action when component added
        _actions.AddAction(uid, ref component.CrawlActionEntity, component.CrawlAction);
    }

    // On shutdown, make sure people are standing and reset movement speed
    private void OnShutdown(EntityUid uid, CrawlComponent component, ComponentShutdown args)
    {
        _standingSystem.Stand(uid);
        _bodySystem.UpdateMovementSpeed(uid);

        // remove lay crawl when component removed
        _actions.RemoveAction(uid, component.CrawlActionEntity);
    }

    // If crawling, stand back up when buckled
    //private void OnBuckled(EntityUid uid, CrawlComponent component, ref BuckledEvent args)
//{
   //     if (component.Laying)
   //         _standingSystem.Stand(args.Buckle.Owner);
   // }

    // If crawling, return to crawling when unbuckled
    //private void OnUnbuckled(EntityUid uid, CrawlComponent component, ref UnbuckledEvent args)
    //{
      //  if (component.Laying)
      //      _standingSystem.Down(args.Buckle.Owner, dropHeldItems: false);
    //}

    private void OnMobStateChanged(EntityUid uid, CrawlComponent component, MobStateChangedEvent args)
    {
        // Hoping this should work fine as going crit - dead or dead - crit shouldn't matter, and crit - alive would stand you up anyways.
        component.Laying = false;
    }

    // Checks every player emote.t
    private void OnEmote(EntityUid uid, CrawlComponent component, ref EmoteEvent args)
    {
        if (args.Handled)
            return;

        if (!HasComp<HumanoidAppearanceComponent>(uid))
            return;

        // If they're not laying down & they emote to lay down, make them.
        if (!component.Laying && args.Emote.ID == component.LayEmoteId)
        {
            component.Laying = true;
            _standingSystem.Down(uid, dropHeldItems: false);
            _modifier.RefreshMovementSpeedModifiers(uid);
        }
        else if (component.Laying && args.Emote.ID == component.StandEmoteId) // If they are laying down and want to stand, reset their movement speed.
        {
            component.Laying = false;
            _standingSystem.Stand(uid);
            _modifier.RefreshMovementSpeedModifiers(uid);
        }
    }

    private void OnCrawlAction(EntityUid uid, CrawlComponent component, ref CrawlActionEvent args)
    {
        if (args.Handled)
            return;

        if (!HasComp<HumanoidAppearanceComponent>(uid))
            return;

        // If they're not laying down & they emote to lay down, make them.
        if (!component.Laying)
        {
            component.Laying = true;
            _standingSystem.Down(uid, dropHeldItems: false);
            _modifier.RefreshMovementSpeedModifiers(uid);
        }
        else if (component.Laying) // If they are laying down and want to stand, reset their movement speed.
        {
            component.Laying = false;
            _standingSystem.Stand(uid);
            _modifier.RefreshMovementSpeedModifiers(uid);
        }
    }

    // Sets their movement speed to component.CrawlSpeed if they're laying down.
    private void OnRefreshMovementSpeedModifiers(EntityUid uid, CrawlComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        if (component.Laying)
            args.ModifySpeed(component.CrawlSpeed, component.CrawlSpeed);

        if (!component.Laying)
            args.ModifySpeed(1f, 1f);
    }
}
