using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
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
            RoleType.Impostor => new Roles.ImpostorRole(),
            RoleType.Shapeshifter => new Roles.ShapeshifterRole(),
            RoleType.Hunter => new Roles.HunterRole(),

            _ => new Roles.CrewmateRole()
        };

        role.Player = player;
        role.OnAssign();

        if (player == PlayerControl.LocalPlayer)
        {
            player.cosmetics.SetNameColor(ColorTools.RoleColors[role.RoleType]);
            player.cosmetics.SetName($"{player.name}\n{role.Name}");
        }

        Roles[player.PlayerId] = role;
        return role;
    }

    public static void AssignRole(PlayerControl player)
    {
        RoleType type;

        if (player.PlayerId == 0)
            type = RoleType.Impostor;
        else type = RoleType.Hunter;

        //RoleType type = (RoleType)Random.Shared.Next(0, 4);
        RpcSendRole(player, (uint)type);
    }

    [MethodRpc((byte)UselessRpcCalls.AssignRole)]
    private static void RpcSendRole(PlayerControl player, uint roleType)
    {
        if (player == null)
            return;

        SetPlayerRole(player, (RoleType)roleType);

        Logger<UselessRolesPlugin>.Info($"[RPC] Received role info for player {player.name}");
    }
}
