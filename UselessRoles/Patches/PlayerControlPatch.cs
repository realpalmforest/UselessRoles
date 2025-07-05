using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(PlayerControl))]
internal static class PlayerControlPatch
{
    [HarmonyPatch(nameof(PlayerControl.MurderPlayer))]
    [HarmonyPostfix]
    internal static void MurderPlayer_Postfix(PlayerControl __instance, PlayerControl target, MurderResultFlags resultFlags)
    {
        // Run OnKilled() on the murder victim's role only
        if (!target.AmOwner)
            return;

        if (resultFlags.HasFlag(MurderResultFlags.Succeeded) && resultFlags.HasFlag(MurderResultFlags.DecisionByHost))
        {
            target.GetRole().OnKilled(__instance);
        }
    }
}