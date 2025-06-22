using UnityEngine;

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
        if (TeamType == TeamType.Impostor)
        {
            KillButton killBtn = Object.Instantiate(hud.KillButton, hud.KillButton.transform.parent);
            killBtn.graphic.enabled = true;
            killBtn.gameObject.SetActive(true);

            VentButton ventBtn = Object.Instantiate(hud.ImpostorVentButton, hud.ImpostorVentButton.transform.parent);
            ventBtn.graphic.enabled = true;
            ventBtn.gameObject.SetActive(true);
        }
    }

    public virtual void OnMeeting()
    {
    }

    public virtual void OnGameEnd()
    {
    }
}
