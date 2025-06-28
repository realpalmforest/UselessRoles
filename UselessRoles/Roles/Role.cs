using UnityEngine;
using UselessRoles.Buttons;

namespace UselessRoles.Roles;

public abstract class Role
{
    public string Name { get; protected init; }
    public string Description { get; protected init; }

    public Color Color { get; protected init; }

    public RoleType RoleType { get; protected init; }
    public TeamType TeamType { get; protected init; }

    public PlayerControl Player { get; set; }

    public virtual void OnAssign()
    {
    }

    public virtual void OnHudStart(HudManager hud)
    {
    }

    public virtual void OnHudActive(HudManager hud, bool isActive)
    {
        // Show / Hide the buttons when the Hud's active state changes
        foreach (var button in hud.transform.FindChild("Buttons").FindChild("BottomRight").GetComponentsInChildren<RoleActionButton>())
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
    }
}
