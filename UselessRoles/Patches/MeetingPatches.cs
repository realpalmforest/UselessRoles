using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(MeetingHud))]
internal static class MeetingPatches
{
    [HarmonyPatch(nameof(MeetingHud.Awake))]
    [HarmonyPrefix]
    internal static void MeetingStart_Prefix(MeetingHud __instance)
    {
        PlayerControl.LocalPlayer.GetRole().OnMeetingStart(__instance);
    }

    [HarmonyPatch(nameof(MeetingHud.OnDestroy))]
    [HarmonyPostfix]
    internal static void MeetingEnd_Postfix(MeetingHud __instance)
    {
        PlayerControl.LocalPlayer.GetRole().OnMeetingEnd(__instance);
    }
}