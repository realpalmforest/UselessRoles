using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;
using UselessRoles.Utility;

namespace UselessRoles.Buttons;

[RegisterInIl2Cpp]
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
            if (!PlayerControl.LocalPlayer || !PlayerControl.LocalPlayer.Data || !PlayerControl.LocalPlayer.Data.Role)
                return;
            if (!canInteract || isCoolingDown)
                return;
            if (!value)
                return;


            // Calculate the distance to the new target
            float distance = Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), value.GetTruePosition());

            // If the new target is too far away, make the new target null
            if (distance > UseRange)
                value = null;
            // If the new target is the same as before, return
            else if (value == _target)
                return;

            // If there is a new target and the button is cooled down, enable the button
            if (value && base.canInteract && !base.isCoolingDown)
                base.SetEnabled();
            else base.SetDisabled();

            // Update outline and old target
            UpdateOutline(value, _target);
            _target = value;
        }
    }

    private PlayerControl _target;


    public new void FixedUpdate()
    {
        base.FixedUpdate();

        try
        {
            Target = PlayerControl.LocalPlayer.FindClosestPlayer(ValidTargets);
        }
        catch (Exception ex) when (ex is ArgumentNullException)
        {
            Logger<UselessRolesPlugin>.Error($"{ex.Message}\nNo predicate provided for ValidTargets of {this.GetType().FullName}");
        }
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
}