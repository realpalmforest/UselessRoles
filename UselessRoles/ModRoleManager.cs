using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using System.Collections.Generic;
using UselessRoles.Network;
using UselessRoles.Roles;
using UselessRoles.Utility;

namespace UselessRoles;

public static class ModRoleManager
{
    public static readonly Dictionary<byte, Role> AssignedRoles = new();

    public static bool IntroShown { get; set; } = false;
    
    private static void SetPlayerRole(PlayerControl player, RoleType roleType)
    {
        // Inform the existing role instance of the switch locally
        if (player.AmOwner && AssignedRoles.TryGetValue(player.PlayerId, out Role existingRole))
            existingRole.OnRoleSwitch();
        
        Role role = roleType switch
        {
            RoleType.Impostor => new Roles.ImpostorRole(),
            RoleType.Hunter => new Roles.HunterRole(),

            _ => new Roles.CrewmateRole()
        };

        AssignedRoles[player.PlayerId] = role;
        role.Player = player;

        // If this role is for the local player, show it
        if (player.AmOwner)
        {
            role.OnReceive(); // When the local player receives the role

            // If the intro was already shown, start hud now
            if(IntroShown) role.OnHudStart(HudManager.Instance);
            
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

    [MethodRpc((byte)UselessRpcCalls.AssignRole, SendImmediately = true)]
    public static void RpcSetRole(PlayerControl player, uint roleType)
    {
        if (!player)
            return;

        SetPlayerRole(player, (RoleType)roleType);
    }
}
