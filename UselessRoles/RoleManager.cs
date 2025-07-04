using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using System.Collections.Generic;
using UselessRoles.Network;
using UselessRoles.Roles;

namespace UselessRoles;

public static class RoleManager
{
    public static readonly Dictionary<byte, Role> Roles = new Dictionary<byte, Role>();

    private static void SetPlayerRole(PlayerControl player, RoleType roleType)
    {
        Role role = roleType switch
        {
            RoleType.Impostor => new Roles.ImpostorRole(),
            RoleType.Hunter => new Roles.HunterRole(),

            _ => new Roles.CrewmateRole()
        };

        role.Player = player;

        Roles[player.PlayerId] = role;
    }


    public static void AssignRole(PlayerControl player)
    {
        var type = player.PlayerId == 0 ? RoleType.Impostor : RoleType.Hunter;
        //RoleType type = (RoleType)Random.Shared.Next(0, 4);
        RpcSetRole(player, (uint)type);
    }

    [MethodRpc((byte)UselessRpcCalls.AssignRole)]
    public static void RpcSetRole(PlayerControl player, uint roleType)
    {
        if (player == null)
            return;

        SetPlayerRole(player, (RoleType)roleType);

        Logger<UselessRolesPlugin>.Info($"[RPC] Received role info for player {player.Data.PlayerName}");
    }
}
