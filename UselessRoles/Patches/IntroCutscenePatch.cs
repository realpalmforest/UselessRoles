using HarmonyLib;
using UselessRoles.Roles;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin))]
public static class IntroCutscenePatch
{
    public static void Prefix(IntroCutscene __instance)
    {
        ModifyIntroVisuals(__instance);

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
        }
    }


    private static void ModifyIntroVisuals(IntroCutscene intro)
    {
        Role role = PlayerControl.LocalPlayer.GetRole();

        intro.RoleText.text = role.Name;
        intro.RoleBlurbText.text = role.Description;

        intro.RoleText.color = role.Color;
        intro.RoleBlurbText.color = role.Color;
    }
}