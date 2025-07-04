using System.Collections;
using UnityEngine;
using UselessRoles.Buttons;
using UselessRoles.Utility;

namespace UselessRoles.Roles;

public abstract class Role
{
    public PlayerControl Player { get; set; }

    public RoleType RoleType { get; protected init; }
    public TeamType TeamType { get; protected init; }

    public Color Color { get; protected init; }

    public string Name { get; protected init; }
    public string Description { get; protected init; }

    public virtual void OnAssign()
    {
        if (TeamType == TeamType.Impostor)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.GetRole().TeamType == TeamType)
                    player.ShowRoleUnderName();
            }
        }
    }

    public virtual void OnHudStart(HudManager hud)
    {

    }

    public virtual void OnHudActive(HudManager hud, bool isActive)
    {
        if (Player.Data.IsDead)
            return;

        // Show / Hide the buttons when the Hud's active state changes
        foreach (RoleActionButton button in GetButtons())
        {
            if (isActive)
                button.Show();
            else button.Hide();
        }
    }

    public virtual void OnMeetingStart(MeetingHud meeting)
    {
    }

    public virtual void OnMeetingEnd(MeetingHud meeting)
    {
        foreach (RoleActionButton button in GetButtons())
        {
            button.Cooldown = button.MeetingCooldown;
        }
    }

    public virtual void OnKilled(PlayerControl murderer)
    {
        // Hide all modded buttons on death
        foreach (RoleActionButton button in GetButtons())
        {
            button.Hide();
        }

        Player.cosmetics.SetName(Player.Data.PlayerName);
        Player.cosmetics.SetNameColor(Color.white);
    }
    
    
    public IEnumerable GetButtons()
    {
        Transform buttonContainer = HudManager.Instance.transform.FindChild("Buttons").FindChild("BottomRight");
        for (int i = 0; i < buttonContainer.childCount; i++)
        {
            GameObject button = buttonContainer.GetChild(i).gameObject;

            if (button.TryGetComponent<RoleActionButton>(out var roleButton))
            {
                yield return roleButton;
            }
        }
    }
}
