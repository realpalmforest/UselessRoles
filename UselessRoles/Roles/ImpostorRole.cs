using UnityEngine;
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
        Description = "Kill the crew without getting caught";
        Color = ColorTools.RoleColors[RoleType.Impostor];

        RoleType = RoleType.Impostor;
        TeamType = TeamType.Impostor;
    }

    public override void OnHudStart(HudManager hud)
    {
        base.OnHudStart(hud);
        
        CreateSabotageButton(hud);
        CreateKillButton(hud);
        VentButton = CreateButton<RoleVentButton>("VentButton (Mod)");
        VentButton.DefaultDuration = 10;
        VentButton.InfiniteUses = false;
    }

    protected void CreateKillButton(HudManager hud)
    {
        KillButton = CreateButton<RoleTargetButton>("KillButton (Mod)");
        KillButton.SetCooldowns(15, 25, 20);
        
        KillButton.SetText(hud.KillButton.buttonLabelText.text, Color);
        KillButton.graphic.sprite = AssetTools.LoadSprite("UselessRoles.Resources.Kill_Button.png");
        
        KillButton.ValidTargets = player => !PlayerTools.IsSameTeam(Player, KillButton.Target);
        KillButton.OnClickEvent += (_, _) => PlayerControl.LocalPlayer.RpcMurderPlayer(KillButton.Target, true);
    }
    
    protected void CreateSabotageButton(HudManager hud)
    {
        SabotageButton = CreateButton<RoleActionButton>("SabotageButton (Mod)");
        SabotageButton.SetCooldowns(0, 0, 0);
        
        SabotageButton.SetText(hud.SabotageButton.buttonLabelText.text, Color);
        SabotageButton.graphic.sprite = AssetTools.LoadSprite("UselessRoles.Resources.Sabotage_Button.png");
        
        if(!GameManager.Instance.SabotagesEnabled())
            return;
        
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
        
        SabotageButton.OnUpdateEvent += (_, _) =>
        {
            if(!GameManager.Instance.SabotagesEnabled())
                return;
            
            // Disable the sabotage button if in vent or petting
            if (PlayerControl.LocalPlayer.inVent || PlayerControl.LocalPlayer.petting)
                SabotageButton.SetDisabled();
            else SabotageButton.SetEnabled();
        };
    }
    
    
}
