using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;
using UselessRoles.Utility;

namespace UselessRoles.Buttons;

public class RoleTargetButton : RoleActionButton
{
    public float UseRange = 1.5f;
    public Func<PlayerControl, bool> ValidTargets;

    public Color HighlightColor = Color.red;

    public PlayerControl Target
    {
        get => _target;
        set
        {
            if (!PlayerControl.LocalPlayer)
                return;
            if (!canInteract || isCoolingDown)
                return;
            if (!value)
                return;
            
            // Calculate the distance to the new target
            float distance = Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), value.GetTruePosition());
            var player = PlayerControl.LocalPlayer;
            
            // If the new target is too far away, make the new target null
            if (distance > UseRange)
                value = null;
            // If the new target is in a vent, make the new target null
            else if(!player.CanMove || player.inVent || player.walkingToVent || player.inMovingPlat)
                value = null;
            // If the new target is the same as before, return
            else if (value == _target)
                return;

            // Update outline and old target
            UpdateOutline(value, _target);
            _target = value;
        }
    }

    private PlayerControl _target;


    public new void FixedUpdate()
    {
        UpdateCooldown();

        try
        {
            Target = PlayerControl.LocalPlayer.FindClosestPlayer(ValidTargets);
        }
        catch (Exception ex) when (ex is ArgumentNullException)
        {
            Logger<UselessRolesPlugin>.Error($"{ex.Message}\nNo predicate provided for ValidTargets of {this.GetType().FullName}");
        }
        
        RunFixedUpdateEvent();
    }

    private void UpdateOutline(PlayerControl newTarget, PlayerControl oldTarget)
    {
        // If there is an old target, remove its outline
        if (oldTarget)
            oldTarget.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(HighlightColor));
        // If the button is cooled down and there is a new target, give it an outline
        if (newTarget && base.canInteract && !base.isCoolingDown)
            newTarget.cosmetics.SetOutline(true, new Il2CppSystem.Nullable<Color>(HighlightColor));
    }
    
    protected override bool CanClick() => base.CanClick() && Target;
}