using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class Crewmate : Role
{
    public override void OnAssign()
    {
        Name = "Crewmate";
        Description = "Complete your tasks and discover the impostors";
        Color = ColorTools.RoleColors[RoleType.Crewmate];
    }
}
