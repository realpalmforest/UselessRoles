using Reactor.Networking.Attributes;
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
            RoleType.Crewmate => new Crewmate(),
            RoleType.Impostor => new Impostor(),
            RoleType.Shapeshifter => new Shapeshifter(),
            RoleType.Hunter => new Hunter()
        };

        role.Player = player;
        role.OnAssign();

        if (player == PlayerControl.LocalPlayer)
        {
            player.cosmetics.SetNameColor(ColorTools.RoleColors[roleType]);
            player.cosmetics.SetName($"{player.name}\n{role.Name}");
        }

        Roles[player.PlayerId] = role;
        return role;
    }

    public static void AssignRole(PlayerControl player)
    {
        RoleType type = (RoleType)UselessRolesPlugin.Instance.Random.Next(0, 4);
        RPCSendRole(player, (uint)type);
    }

    [MethodRpc((byte)UselessRpcCalls.AssignRole)]
    private static void RPCSendRole(PlayerControl player, uint roleType)
    {
        if (player == null)
            return;

        SetPlayerRole(player, (RoleType)roleType);

        Logger<UselessRolesPlugin>.Info($"[RPC] Received role info for player {player.name}");
    }
}
