using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;
using UselessRoles.Utility;

namespace UselessRoles.Buttons;

public class RoleTargetButton : RoleActionButton
{
    public Func<PlayerControl, bool> ValidTargets;
    
    public Color HighlightColor = Color.red;
    public float UseRange = 1.5f;
    
    public PlayerControl Target { get; set; }

    public override void Update()
    {
        FindTarget();
        base.Update();
    }

    private void FindTarget()
    {
        try
        {
            var localPlayer = PlayerControl.LocalPlayer;
            var closestPlayer = localPlayer.FindClosestPlayer(ValidTargets);
            
            if (closestPlayer)
            {
                // Calculate the distance to the new target
                float distance = Vector2.Distance(localPlayer.GetTruePosition(), closestPlayer.GetTruePosition());
                
                // If the new target is too far away, make the new target null
                if (distance > UseRange)
                    closestPlayer = null;
                // If the new target is dead, make the new target null
                else if(closestPlayer.Data.IsDead)
                    closestPlayer = null;
            }
            
            // Update target outlines and set target
            UpdateOutline(closestPlayer, Target);
            Target = closestPlayer;
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
    
    protected override bool CanClick() => base.CanClick() && 
                                          Target && 
                                          PlayerControl.LocalPlayer.CanMove && 
                                          !PlayerControl.LocalPlayer.inVent && 
                                          !PlayerControl.LocalPlayer.walkingToVent;
}