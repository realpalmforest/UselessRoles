using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using System.Collections.Generic;
using UselessRoles.Network;
using UselessRoles.Roles;
using UselessRoles.Utility;

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

        Roles[player.PlayerId] = role;
        role.Player = player;

        // If this role is for the local player, show it
        if (player.AmOwner)
        {
            role.OnReceive(); // When the local player receives the role
            player.ShowRoleUnderName(); // Show the local player role under name
            
            Logger<UselessRolesPlugin>.Info($"[RPC] Received local role: {role.Name}");
            return;
        }
        
        Logger<UselessRolesPlugin>.Info($"[RPC] Received role for player: {player.Data.PlayerName}");
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
        if (!player)
            return;

        SetPlayerRole(player, (RoleType)roleType);
    }
}
