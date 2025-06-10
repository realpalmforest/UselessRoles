using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class Crewmate : Role
{
    public Crewmate()
    {
        Name = "Crewmate";
        Description = "Complete your tasks and discover the impostors";
        Color = ColorTools.RoleColors[RoleType.Crewmate];

        RoleType = RoleType.Crewmate;
        TeamType = TeamType.Crewmate;
    }
}
