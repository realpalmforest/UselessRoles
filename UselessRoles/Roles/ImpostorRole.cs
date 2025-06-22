using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class ImpostorRole : Role
{
    public ImpostorRole()
    {
        Name = "Impostor";
        Description = "Kill all the crewmates without getting voted out";
        Color = ColorTools.RoleColors[RoleType.Impostor];

        RoleType = RoleType.Impostor;
        TeamType = TeamType.Impostor;
    }
}
