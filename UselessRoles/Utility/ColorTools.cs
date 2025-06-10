using System.Collections.Generic;
using UnityEngine;
using UselessRoles.Roles;

namespace UselessRoles.Utility;

public static class ColorTools
{
    public static Color FromRGB(byte r, byte g, byte b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public static Dictionary<RoleType, Color> RoleColors = new Dictionary<RoleType, Color>()
    {
        { RoleType.Crewmate, Color.cyan.RGBMultiplied(1.5f) },
        { RoleType.Impostor, Color.red },
        { RoleType.Shapeshifter, Color.red.RGBMultiplied(0.65f) },
        { RoleType.Hunter, FromRGB(76, 120, 63) },
    };

    public static Dictionary<TeamType, Color> TeamColors = new Dictionary<TeamType, Color>()
    {
        { TeamType.Crewmate, Color.cyan.RGBMultiplied(1.2f) },
        { TeamType.Impostor, Color.red }
    };
}
