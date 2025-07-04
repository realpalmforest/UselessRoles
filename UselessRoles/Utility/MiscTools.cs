using UnityEngine;

namespace UselessRoles.Utility;

public static class MiscTools
{
    public static void SetCustomOutline(this Vent vent, Color color, bool enabled, bool mainTarget)
    {
        vent?.myRend.material.SetFloat("_Outline", enabled ? 1f : 0);
        vent?.myRend.material.SetColor("_OutlineColor", color);
        vent?.myRend.material.SetColor("_AddColor", mainTarget ? color : Color.clear);
    }
}