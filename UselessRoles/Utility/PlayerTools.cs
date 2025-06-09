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
}
