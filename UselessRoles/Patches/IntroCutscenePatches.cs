using HarmonyLib;
using Reactor.Utilities;
using UselessRoles.Roles;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

public static class IntroCutscenePatches
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin))]
    public static class IntroBeginPatch
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
            }
        }
    }

    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
    public static class ShowTeamImpostorPatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            Role role = PlayerControl.LocalPlayer.GetRole();

            __instance.TeamTitle.text = role.TeamType.ToString();
            __instance.TeamTitle.color = ColorTools.TeamColors[role.TeamType];

            __instance.BackgroundBar.material.color = ColorTools.TeamColors[role.TeamType];
        }
    }

    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
    public static class ShowTeamCrewmatePatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            Role role = PlayerControl.LocalPlayer.GetRole();

            __instance.TeamTitle.text = role.TeamType.ToString();
            __instance.TeamTitle.color = ColorTools.TeamColors[role.TeamType];

            __instance.BackgroundBar.material.color = ColorTools.TeamColors[role.TeamType];
        }
    }

    [HarmonyPatch(typeof(IntroCutscene._ShowRole_d__41), nameof(IntroCutscene._ShowRole_d__41.MoveNext))]
    public static class ShowRolePatch
    {
        public static void Postfix(IntroCutscene._ShowRole_d__41 __instance)
        {
            Logger<UselessRolesPlugin>.Message("ShowRolePatch executed");

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
}