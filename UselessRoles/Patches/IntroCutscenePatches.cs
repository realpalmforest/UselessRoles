using System.Collections;
using System.Linq;
using UselessRoles.Roles;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(IntroCutscene))]
internal static class IntroCutscenePatches
{
    [HarmonyPatch(nameof(IntroCutscene.CoBegin))]
    [HarmonyPriority(Priority.First)]
    [HarmonyPrefix]
    public static void CutsceneBegin_Prefix()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        
        // The host assigns roles to everyone
        foreach (var player in PlayerControl.AllPlayerControls)
            RoleManager.AssignRole(player);
    }

    [HarmonyPatch(nameof(IntroCutscene.SelectTeamToShow))]
    [HarmonyPostfix]
    public static void SelectTeammates_Postfix(ref Il2CppSystem.Collections.Generic.List<PlayerControl> __result)
    {
        var sameTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();

        foreach (var player in GameData.Instance.AllPlayers)
        {
            if (player.Disconnected)
                continue;

            var control = player.Object;
            if (!control || control == PlayerControl.LocalPlayer)
                continue;

            if (PlayerTools.Am(control.GetRole().TeamType))
                sameTeam.Add(control);
        }

        // Local player should be displayed at the front
        sameTeam.Add(PlayerControl.LocalPlayer);

        __result = sameTeam;
    }

    [HarmonyPatch(nameof(IntroCutscene.BeginCrewmate))]
    [HarmonyPostfix]
    public static void ShowTeamCrewmate_Postfix(IntroCutscene __instance)
    {
        Role role = PlayerControl.LocalPlayer.GetRole();
        
        // Show the team name 
        __instance.TeamTitle.text = role.TeamType.ToString();
        __instance.TeamTitle.color = ColorTools.TeamColors[role.TeamType];

        // Show impostor count if not impostor
        if(role.TeamType == TeamType.Impostor)
            __instance.ImpostorText.gameObject.SetActive(false);
        else
        {
            int impCount = PlayerTools.GetTeamPlayers(TeamType.Impostor).Count();
            
            if(impCount == 1)
                __instance.ImpostorText.text = $"There is <color={ColorTools.TeamColors[TeamType.Impostor].GetHex()}>{impCount} impostor</color> among us";
            else __instance.ImpostorText.text = $"There are <color={ColorTools.TeamColors[TeamType.Impostor].GetHex()}>{impCount} impostors</color> among us";
        }
        
        __instance.BackgroundBar.material.color = ColorTools.TeamColors[role.TeamType];
    }

    [HarmonyPatch(typeof(IntroCutscene._ShowRole_d__41), nameof(IntroCutscene._ShowRole_d__41.MoveNext))]
    [HarmonyPostfix]
    public static void ShowRole_Postfix(IntroCutscene._ShowRole_d__41 __instance)
    {
        if (__instance.__1__state != 1)
            return;

        Role role = PlayerControl.LocalPlayer.GetRole();

        __instance.__4__this.RoleText.text = role.Name;
        __instance.__4__this.RoleBlurbText.text = role.Description;

        __instance.__4__this.RoleText.color = role.Color;
        __instance.__4__this.YouAreText.color = role.Color;
        __instance.__4__this.RoleBlurbText.color = role.Color;
    }
}