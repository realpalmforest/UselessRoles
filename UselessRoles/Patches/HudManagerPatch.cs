using HarmonyLib;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatch
{
    [HarmonyPatch(nameof(HudManager.OnGameStart))]
    [HarmonyPostfix]
    public static void HudShowIntro_Postfix(HudManager __instance)
    {
        PlayerControl.LocalPlayer.GetRole().OnHudStart(__instance);
    }
}