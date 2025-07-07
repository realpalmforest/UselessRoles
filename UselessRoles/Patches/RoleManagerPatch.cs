using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Il2CppSystem;
using InnerNet;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(RoleManager))]
internal static class RoleManagerPatch
{
    [HarmonyPatch(nameof(RoleManager.SelectRoles))]
    [HarmonyPrefix]
    internal static bool SelectRoles_Prefix()
    {
        ModRoleManager.IntroShown = false;
        RemoveVanillaRoles();
        
        if (!AmongUsClient.Instance.AmHost) return false;

        // The host assigns modded roles to everyone
        foreach (var player in PlayerControl.AllPlayerControls)
            ModRoleManager.AssignRole(player);

        return false;
    }

    private static void RemoveVanillaRoles()
    {
        Il2CppSystem.Collections.Generic.List<ClientData> clients = new Il2CppSystem.Collections.Generic.List<ClientData>();
        AmongUsClient.Instance.GetAllClients(clients);

        // Get a list of all valid player infos
        List<NetworkedPlayerInfo> playerInfos = clients.ToArray().ToList().Where(c =>
            c.Character != null &&
            c.Character.Data != null &&
            !c.Character.Data.Disconnected &&
            !c.Character.Data.IsDead)
            .OrderBy(c => c.Id)
            .Select(c => c.Character.Data)
            .ToList();
        
        IGameOptions gameOptions = GameOptionsManager.Instance.CurrentGameOptions;
        LogicRoleSelection logic = GameManager.Instance.LogicRoleSelection;
        
        logic.AssignRolesForTeam(playerInfos.ToIl2CppList(), gameOptions, RoleTeamTypes.Impostor, 0, new Nullable<RoleTypes>(RoleTypes.Impostor));
        logic.AssignRolesForTeam(playerInfos.ToIl2CppList(), gameOptions, RoleTeamTypes.Crewmate, playerInfos.Count, new Nullable<RoleTypes>(RoleTypes.Crewmate));
    }
}