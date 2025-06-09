using HarmonyLib;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin))]
public class IntroCutscenePatch
{
    public static void Prefix(IntroCutscene __instance)
    {
        if (!AmongUsClient.Instance.AmHost)
            return;

        var players = PlayerControl.AllPlayerControls;

        if (players.Count == 0)
            return;

        foreach (var player in players)
        {
            RoleManager.AssignRole(player);

            if (!player.AmOwner)
                continue;

            __instance.RoleText.text = "RoleText";
            __instance.RoleBlurbText.text = "RoleBlurbText";
        }
    }
}
