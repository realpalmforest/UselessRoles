using System;
using System.Collections;
using UnityEngine;
using UselessRoles.Utility;

namespace UselessRoles.Buttons;

public sealed class RoleVentButton : RoleActionButton
{
    public Color HighlightColor = Color.red;
    
    public bool IsTargetInRange => Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), TargetVent.transform.position) <= TargetVent.UsableDistance;
    public Vent TargetVent { get; set; }
    
    public override void Awake()
    {
        base.Awake();
        
        graphic.sprite = HudManager.Instance.ImpostorVentButton.graphic.sprite;
        SetText(HudManager.Instance.ImpostorVentButton.buttonLabelText.text, HighlightColor);
    }
    
    public override void Update()
    {
        FindTarget();
        base.Update();
    }

    private void FindTarget()
    {
        float closestDistance = float.MaxValue;
        Vent closestVent = null;

        foreach (var vent in GameObject.FindObjectsOfType<Vent>())
        {
            // Remove all outlines by default
            vent.SetCustomOutline(HighlightColor, false, false);
            
            if (!vent || vent.UsableDistance < 0f)
                continue;

            float dist = Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), vent.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestVent = vent;
            }
        }

        TargetVent = closestVent;

        if (CanClick())
            TargetVent.SetCustomOutline(HighlightColor, true, IsTargetInRange);
    }
    
    public override void DoClick()
    {
        base.DoClick();
        UseTarget();
    }
    
    private void UseTarget()
    {
        if (!CanClick())
            return;
        
        AchievementManager.Instance.OnConsoleUse(TargetVent.Cast<IUsable>());
        var player = PlayerControl.LocalPlayer;
        
        // Handle player entering vent
        // (Vent exiting is handles by AbilityEnd())
        if (!player.inVent)
        {
            player.MyPhysics.RpcEnterVent(TargetVent.Id);
            TargetVent.SetButtons(true);
        }
    }
    
    protected override bool CanClick()
    {
        var player = PlayerControl.LocalPlayer;
        var system = ShipStatus.Instance.Systems[SystemTypes.Ventilation].Cast<VentilationSystem>();
        
        bool canEnterVent = TargetVent &&
                            IsTargetInRange && 
                            !PhysicsHelpers.AnythingBetween(player.Collider, player.Collider.bounds.center, TargetVent.transform.position, Constants.ShipOnlyMask, useTriggers: false);

        return base.CanClick() &&
               (canEnterVent || player.inVent) &&
               !player.walkingToVent &&
               !player.Data.IsDead &&
               !player.MustCleanVent(TargetVent.Id) &&
               !system.IsVentCurrentlyBeingCleaned(TargetVent.Id);
    }

    protected override void AbilityEnd()
    {
        // Force the player to leave the vent when the ability ends
        PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(TargetVent.Id);
        TargetVent.SetButtons(false);
        
        base.AbilityEnd();
    }
}