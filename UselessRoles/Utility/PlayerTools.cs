using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UselessRoles.Roles;

namespace UselessRoles.Utility;

public static class PlayerTools
{
    public static PlayerControl GetPlayerById(byte playerId)
    {
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.PlayerId == playerId)
                return player;
        }

        return null;
    }

    public static Role GetRole(this PlayerControl player)
    {
        return RoleManager.Roles[player.PlayerId];
    }

    public static void ShowRoleUnderName(this PlayerControl player)
    {
        var role = player.GetRole();

        player.cosmetics.SetNameColor(ColorTools.RoleColors[role.RoleType]);
        player.cosmetics.SetName($"{player.name}\n{role.Name}");
    }

    public static PlayerControl FindClosestPlayer(this PlayerControl player, Func<PlayerControl, bool> predicate)
    {
        var players = PlayerControl.AllPlayerControls
            .ToArray()
            .Where(predicate)
            .ToList();

        Vector2 myPos = player.GetTruePosition();

        players.Sort(delegate (PlayerControl a, PlayerControl b)
        {
            float magnitude2 = (a.GetTruePosition() - myPos).magnitude;
            float magnitude3 = (b.GetTruePosition() - myPos).magnitude;
            if (magnitude2 > magnitude3)
            {
                return 1;
            }
            if (magnitude2 < magnitude3)
            {
                return -1;
            }
            return 0;
        });

        return players.Count > 0 ? players[0] : null;
    }
    
    public static bool Am(TeamType team) => PlayerControl.LocalPlayer.GetRole().TeamType == team;

    public static List<PlayerControl> GetTeamPlayers(TeamType team)
    {
        var players = new List<PlayerControl>();
        
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if(player.GetRole().TeamType == team)
                players.Add(player);
        }
        
        return players;
    }
}