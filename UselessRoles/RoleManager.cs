using Reactor.Networking.Attributes;
using System.Collections.Generic;
using UselessRoles.Network;
using UselessRoles.Roles;
using UselessRoles.Utility;

namespace UselessRoles;

public static class RoleManager
{
    public static Dictionary<byte, Role> Roles = new Dictionary<byte, Role>();

    public static Role SetPlayerRole(PlayerControl player, RoleType roleType)
    {
        Role role = roleType switch
        {
            RoleType.Crewmate => new Crewmate(),
            RoleType.Impostor => new Impostor(),
            RoleType.Shapeshifter => new Shapeshifter(),
            RoleType.Hunter => new Hunter()
        };

        role.Player = player;
        role.OnAssign();

        player.cosmetics.SetNameColor(ColorTools.RoleColors[roleType]);
        player.cosmetics.SetName($"{player.name}\n{role.Name}");

        Roles[player.PlayerId] = role;
        return role;
    }

    public static void AssignRole(PlayerControl player)
    {
        RoleType type = (RoleType)UselessRolesPlugin.Random.Next(0, 4);
        RPCSendRole(player, (uint)type);
    }

    [MethodRpc((byte)UselessRpcCalls.AssignRole)]
    public static void RPCSendRole(PlayerControl player, uint roleType)
    {
        if (player == null)
            return;

        RoleManager.SetPlayerRole(player, (RoleType)roleType);

        UselessRolesPlugin.Logger.LogInfo($"[RPC] Received role info for player {player.name}");
    }
}
