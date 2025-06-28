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

        TrapButton = RoleActionButton.Create<RoleActionButton>();
        TrapButton.name = "TrapButton";

        TrapButton.SetText("Trap", Color);
        TrapButton.graphic.sprite = AssetTools.LoadSprite("UselessRoles.Resources.Trap.png");
        TrapButton.graphic.SetCooldownNormalizedUvs();

        TrapButton.InfiniteUses = true;
        TrapButton.OnClickEvent += (_, _) =>
        {
            Logger<UselessRolesPlugin>.Message("Wowee you pressed the TRAP BUTTON !!! :D");
        };
    }
}
