using HarmonyLib;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(PlayerControl))]
internal static class PlayerControlPatch
{
    [HarmonyPatch(nameof(PlayerControl.MurderPlayer))]
    [HarmonyPostfix]
    public static void MurderPlayer_Postfix(PlayerControl __instance, PlayerControl target, MurderResultFlags resultFlags)
    {
        if (!target.AmOwner)
            return;

        if (resultFlags.HasFlag(MurderResultFlags.Succeeded) && resultFlags.HasFlag(MurderResultFlags.DecisionByHost))
        {
            target.GetRole().OnKilled(__instance);
        }
    }
}