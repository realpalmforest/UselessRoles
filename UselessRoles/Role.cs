using UnityEngine;

namespace UselessRoles;

public abstract class Role
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    public Color Color { get; protected set; }

    public PlayerControl Player { get; set; }

    public virtual void OnAssign()
    {
    }

    public virtual void OnMeeting()
    {
    }

    public virtual void OnGameEnd()
    {
    }
}
