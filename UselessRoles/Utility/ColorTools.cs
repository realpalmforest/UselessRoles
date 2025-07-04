using System.Collections.Generic;
using UnityEngine;
using UselessRoles.Roles;

namespace UselessRoles.Utility;

public static class ColorTools
{
    public static readonly Dictionary<RoleType, Color> RoleColors = new()
    {
        { RoleType.Impostor, Color.red },
        { RoleType.Crewmate, Color.cyan.RGBMultiplied(1.5f) },
        { RoleType.Hunter, new Color32(76, 120, 63, 255) },
    };

    public static readonly Dictionary<TeamType, Color> TeamColors = new()
    {
        { TeamType.Impostor, Color.red },
        { TeamType.Crewmate, Color.cyan.RGBMultiplied(1.2f) }
    };
    
    public static string GetHex(this Color color)
    {
        Color32 color32 = color; // Automatically converts to 0–255 range
        return $"#{color32.r:X2}{color32.g:X2}{color32.b:X2}";
    }
}
