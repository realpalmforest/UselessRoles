﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UselessRoles.Roles;

namespace UselessRoles.Utility;

public static class PlayerTools
{
    public static bool HasRole(this PlayerControl player) => ModRoleManager.AssignedRoles.ContainsKey(player.PlayerId);
    
    public static Role GetRole(this PlayerControl player)
    {
        if (!ModRoleManager.AssignedRoles.TryGetValue(player.PlayerId, out var role))
            Logger<UselessRolesPlugin>.Error($"Player {player.Data.PlayerName} does not have an assigned role");
        
        return role;
    }

    public static bool Am(TeamType team) => PlayerControl.LocalPlayer.GetRole().TeamType == team;
    public static bool Am(RoleType role) => PlayerControl.LocalPlayer.GetRole().RoleType == role;

    public static bool IsSameTeam(PlayerControl player1, PlayerControl player2)
    {
        if (!player1 || !player2)
            return false;
        
        return player1.GetRole().TeamType == player2.GetRole().TeamType;
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
            .Where(plr => !plr.AmOwner)
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
    
    public static IEnumerable<PlayerControl> GetTeamPlayers(TeamType team)
        => PlayerControl.AllPlayerControls.ToArray().ToList().Where(player => player.GetRole().TeamType == team);
}