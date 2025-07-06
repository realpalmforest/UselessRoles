using System.Collections.Generic;
using UnityEngine;
using UselessRoles.Buttons;
using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class HunterRole : Role
{
    public static readonly List<Vector2> TrapPositions = new List<Vector2>();
    
    public RoleActionButton TrapButton;

    public HunterRole()
    {
        Name = "Hunter";
        Description = "Use various traps to hunt down the impostors";
        Color = ColorTools.RoleColors[RoleType.Hunter];

        RoleType = RoleType.Hunter;
        TeamType = TeamType.Crewmate;
    }

    public override void OnHudStart(HudManager hud)
    {
        base.OnHudStart(hud);

        TrapButton = CreateButton<RoleActionButton>("TrapButton (Mod)");
        
        TrapButton.SetText("Trap", Color);
        TrapButton.graphic.sprite = AssetTools.LoadSprite("UselessRoles.Resources.Trap_Button.png");
        
        TrapButton.SetCooldowns(20, 25, 15);
        TrapButton.OnClickEvent += (_, _) =>
        {
            TrapPositions.Add(PlayerControl.LocalPlayer.GetTruePosition());
        };
    }
}
