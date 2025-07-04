using UnityEngine;
using UselessRoles.Utility;

namespace UselessRoles.Buttons;

public class RoleVentButton : RoleActionButton
{
    public Color HighlightColor = Color.red;
    
    public Vent Target
    {
        get => this._targetVent;
        set
        {
            // If it's the same vent (and not null), skip
            if(value == _targetVent && _targetVent != null)
                return;
            
            _targetVent.SetCustomOutline(HighlightColor, false, false);
            _targetVent = value;
            
            if (_targetVent && !isCoolingDown)
            {
                _targetVent.SetCustomOutline(HighlightColor, true, true);
                base.SetEnabled();
            }
            else base.SetDisabled();
        }
    }

    private Vent _targetVent;

    public override void Awake()
    {
        base.Awake();
        
        graphic.sprite = HudManager.Instance.ImpostorVentButton.graphic.sprite;
        SetText(HudManager.Instance.ImpostorVentButton.buttonLabelText.text, HighlightColor);
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        // Skip if the player is already in a vent
        if (PlayerControl.LocalPlayer.inVent)
            return;

        float closestDistance = float.MaxValue;
        Vent closestVent = null;

        foreach (var vent in GameObject.FindObjectsOfType<Vent>())
        {
            if (!vent || vent.UsableDistance < 0f)
                continue;

            float dist = Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), vent.transform.position);
            if (dist < vent.UsableDistance && dist < closestDistance)
            {
                closestDistance = dist;
                closestVent = vent;
            }
        }
        
        Target = closestDistance > closestVent?.UsableDistance ? null : closestVent;
    }
    
    public override void DoClick()
    {
        base.DoClick();
        UseTarget(out bool inVentNow);

        if (inVentNow)
            Cooldown = 0;
    }

    private void UseTarget(out bool inVentNow)
    {
        inVentNow = false;
        
        if (!CanUse())
            return;
        
        // AchievementManager.Instance.OnConsoleUse(Target);
        PlayerControl player = PlayerControl.LocalPlayer;

        if (player.walkingToVent)
            return;
            
        if (player.inVent)
        {
            player.MyPhysics.RpcExitVent(Target.Id);
            Target.SetButtons(false);
        }
        else
        {
            player.MyPhysics.RpcEnterVent(Target.Id);
            Target.SetButtons(true);
            
            inVentNow = true;
        }
    }

    private bool CanUse()
    {
        if (!Target)
            return false;
        
        var player = PlayerControl.LocalPlayer;
        var system = ShipStatus.Instance.Systems[SystemTypes.Ventilation].Cast<VentilationSystem>();
        
        bool couldUse = 
            !player.Data.IsDead && 
            !player.MustCleanVent(Target.Id) &&
            !system.IsVentCurrentlyBeingCleaned(Target.Id) || 
            (player.inVent && Vent.currentVent == Target) &&
            (player.CanMove || player.inVent);
        
        float distance = Vector2.Distance(player.GetTruePosition(), Target.transform.position);
        return couldUse && distance <= Target.UsableDistance && !PhysicsHelpers.AnythingBetween(player.Collider, player.Collider.bounds.center, Target.transform.position, Constants.ShipOnlyMask, useTriggers: false);
    }
}