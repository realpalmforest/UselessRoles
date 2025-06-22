using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class CrewmateRole : Role
{
    public CrewmateRole()
    {
        Name = "Crewmate";
        Description = "Complete your tasks and discover the impostors";
        Color = ColorTools.RoleColors[RoleType.Crewmate];

        RoleType = RoleType.Crewmate;
        TeamType = TeamType.Crewmate;
    }
}
