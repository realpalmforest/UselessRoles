using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class HunterRole : Role
{
    public HunterRole()
    {
        Name = "Hunter";
        Description = "Use various traps to hunt down the impostors";
        Color = ColorTools.RoleColors[RoleType.Hunter];

        RoleType = RoleType.Hunter;
        TeamType = TeamType.Crewmate;
    }
}
