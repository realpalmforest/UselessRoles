using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(HudManager))]
internal static class HudManagerPatches
{
    [HarmonyPatch(nameof(HudManager.OnGameStart))]
    [HarmonyPostfix]
    internal static void HudShowIntro_Postfix(HudManager __instance)
    {
        PlayerControl.LocalPlayer.GetRole().OnHudStart(__instance);
    }

    [HarmonyPatch(methodName: nameof(HudManager.SetHudActive), argumentTypes: [typeof(bool)])]
    [HarmonyPostfix]
    internal static void SetHudActive_Postfix(HudManager __instance, bool isActive)
    {
        PlayerControl.LocalPlayer.GetRole().OnHudActive(__instance, isActive);
    }
}