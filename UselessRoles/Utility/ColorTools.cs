using System.Collections.Generic;
using UnityEngine;

namespace UselessRoles.Utility;

public static class ColorTools
{
    public static Color FromRGB(byte r, byte g, byte b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public static Dictionary<RoleType, Color> RoleColors = new Dictionary<RoleType, Color>()
    {
        { RoleType.Crewmate, Color.cyan.RGBMultiplied(1.3f) },
        { RoleType.Impostor, Color.red },
        { RoleType.Shapeshifter, Color.red.RGBMultiplied(0.65f) },
        { RoleType.Hunter, ColorTools.FromRGB(219, 161, 26) },
    };
}
