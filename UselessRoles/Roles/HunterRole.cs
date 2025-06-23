using UnityEngine;
using UselessRoles.Buttons;
using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class HunterRole : Role
{
    public TrapButton TrapButton;

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

        var button = GameObject.Instantiate(hud.AbilityButton.gameObject, hud.AbilityButton.transform.parent);
        button.SetActive(true);

        GameObject.DestroyImmediate(button.GetComponent<AbilityButton>());
        TrapButton = button.AddComponent<TrapButton>();
    }
}
