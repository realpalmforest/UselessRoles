using HarmonyLib;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    [HarmonyPatch(nameof(HudManager.OnGameStart))]
    [HarmonyPostfix]
    public static void HudShowIntro_Postfix(HudManager __instance)
    {
        if (!AmongUsClient.Instance.AmHost)
            return;

        foreach (var player in PlayerControl.AllPlayerControls)
            player.GetRole().OnHudStart(__instance);
    }

    [HarmonyPatch(methodName: nameof(HudManager.SetHudActive), argumentTypes: [typeof(bool)])]
    [HarmonyPostfix]
    public static void SetHudActive_Postfix(HudManager __instance, bool isActive)
    {
        if (!AmongUsClient.Instance.AmHost)
            return;

        foreach (var player in PlayerControl.AllPlayerControls)
            player.GetRole().OnHudActive(__instance, isActive);
    }
}