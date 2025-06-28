using UselessRoles.Buttons;
using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class ImpostorRole : Role
{
    public RoleTargetButton KillButton;

    public ImpostorRole()
    {
        Name = "Impostor";
        Description = "Kill all the crewmates without getting voted out";
        Color = ColorTools.RoleColors[RoleType.Impostor];

        RoleType = RoleType.Impostor;
        TeamType = TeamType.Impostor;
    }

    public override void OnHudStart(HudManager hud)
    {
        CreateKillButton(hud);
        base.OnHudStart(hud);
    }

    private void CreateKillButton(HudManager hud)
    {
        KillButton = RoleActionButton.Create<RoleTargetButton>();
        KillButton.name = "KillButton (Mod)";

        KillButton.SetText("Kill", Color);
        KillButton.graphic.sprite = hud.KillButton.graphic.sprite;

        KillButton.InfiniteUses = true;
        KillButton.ValidTargets = (player => player.GetRole().TeamType != this.TeamType && !player.Data.IsDead);
        KillButton.OnClickEvent += (_, _) => PlayerControl.LocalPlayer.RpcMurderPlayer(KillButton.Target, true);
    }
}
