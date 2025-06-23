using Reactor.Utilities;
using UselessRoles.Buttons;
using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class HunterRole : Role
{
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

        TrapButton = RoleActionButton.Create<RoleActionButton>(hud);
        TrapButton.name = "TrapButton";
        TrapButton.buttonLabelText.text = "Trap";
        TrapButton.OnClickEvent += (_, _) =>
        {
            Logger<UselessRolesPlugin>.Message("Wowee you pressed the TRAP BUTTON !!! :D");
        };
    }
}
