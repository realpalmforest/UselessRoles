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
    
    public override void FixedUpdate()
    {
        UpdateCooldown();
        
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

        if (TargetVent && !isCoolingDown)
            TargetVent.SetCustomOutline(HighlightColor, true, IsTargetInRange);
        
        RunFixedUpdateEvent();
    }
    
    public override void DoClick()
    {
        if (!CanClick()) return;
        
        RunButtonClickEvent();
        UseTarget();
    }

    private void UseTarget()
    {
        if (!CanClick())
            return;
        
        AchievementManager.Instance.OnConsoleUse(TargetVent.Cast<IUsable>());
        var player = PlayerControl.LocalPlayer;
            
        if (player.inVent)
        {
            player.MyPhysics.RpcExitVent(TargetVent.Id);
            TargetVent.SetButtons(false);
            
            // Reset cooldown to default when leaving a vent
            Cooldown = DefaultCooldown;
            base.SetCoolDown(Cooldown, DefaultCooldown);
            
            // Reduce uses remaining only when leaving a vent
            if (!InfiniteUses) UsesRemaining--;
        }
        else
        {
            player.MyPhysics.RpcEnterVent(TargetVent.Id);
            TargetVent.SetButtons(true);
            
            // No cooldown for leaving a vent
            Cooldown = 0;
            base.SetCoolDown(Cooldown, DefaultCooldown);
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
}