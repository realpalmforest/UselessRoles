using UselessRoles.Buttons;
using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class ImpostorRole : Role
{
    public RoleTargetButton KillButton;
    public RoleActionButton SabotageButton;
    public RoleVentButton VentButton;
    
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
        base.OnHudStart(hud);
        
        CreateSabotageButton(hud);
        CreateKillButton(hud);
        VentButton = RoleActionButton.Create<RoleVentButton>("VentButton (Mod)");
    }

    protected void CreateKillButton(HudManager hud)
    {
        KillButton = RoleActionButton.Create<RoleTargetButton>("KillButton (Mod)");
        KillButton.SetCooldowns(30, 45, 40);
        
        KillButton.SetText(hud.KillButton.buttonLabelText.text, Color);
        KillButton.graphic.sprite = hud.KillButton.graphic.sprite;
        
        KillButton.ValidTargets = (player => player.GetRole().TeamType != this.TeamType && !player.Data.IsDead);
        KillButton.OnClickEvent += (_, _) => PlayerControl.LocalPlayer.RpcMurderPlayer(KillButton.Target, true);
    }
    
    protected void CreateSabotageButton(HudManager hud)
    {
        SabotageButton = RoleActionButton.Create<RoleActionButton>("SabotageButton (Mod)");
        SabotageButton.SetCooldowns(0, 0, 0);
        
        SabotageButton.SetText(hud.SabotageButton.buttonLabelText.text, Color);
        SabotageButton.graphic.sprite = hud.SabotageButton.graphic.sprite;
        
        SabotageButton.OnClickEvent += (_, _) =>
        {
            if(PlayerControl.LocalPlayer.inVent)
                return;
            if(!GameManager.Instance.SabotagesEnabled())
                return;
            if(!PlayerTools.Am(TeamType.Impostor))
                return;
        
            HudManager.Instance.ToggleMapVisible(new MapOptions
            {
                Mode = MapOptions.Modes.Sabotage
            });
            
            SabotageButton.SetCooldowns(0, 0, 0);
        };
        
        SabotageButton.OnFixedUpdateEvent += (_, _) =>
        {
            if (!GameManager.Instance || !PlayerControl.LocalPlayer)
            {
                SabotageButton.ToggleVisible(false);
                SabotageButton.SetDisabled();
            }
            else if (PlayerControl.LocalPlayer.inVent || !GameManager.Instance.SabotagesEnabled() || PlayerControl.LocalPlayer.petting)
            {
                SabotageButton.ToggleVisible(PlayerTools.Am(TeamType.Impostor) && GameManager.Instance.SabotagesEnabled());
                SabotageButton.SetDisabled();
            }
            else
            {
                SabotageButton.SetEnabled();
            }
        };
    }
}
