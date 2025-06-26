using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;
using UselessRoles.Utility;
using static UnityEngine.GraphicsBuffer;

namespace UselessRoles.Buttons;

[RegisterInIl2Cpp]
public class RoleTargetButton : RoleActionButton
{
    public float UseRange = 50f;
    public Func<PlayerControl, bool> ValidTargets;

    public Color HighlightColor = Color.red;

    public PlayerControl Target
    {
        get => _target;
        set
        {
            if (!PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data == null || !PlayerControl.LocalPlayer.Data.Role)
                return;

            _target = value;

            if (_target)
            {
                _target.SetOutlineColor(HighlightColor);
                base.SetEnabled();
            }
            else
            {
                _target.SetOutlineColor(null);
                base.SetDisabled();
            }

            Logger<UselessRolesPlugin>.Message($"Target of {this.GetType().FullName} set to {_target.name}");
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        try
        {
            Target = PlayerControl.LocalPlayer.FindClosestPlayer(ValidTargets);
        }
        catch (Exception ex) when (ex is NullReferenceException)
        {
            Logger<UselessRolesPlugin>.Error($"{ex.Message}\nNo predicate provided for ValidTargets of {this.GetType().FullName}");
        }
    }

    private PlayerControl _target;
}
