using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class Impostor : Role
{
    public override void OnAssign()
    {
        Name = "Impostor";
        Description = "Kill all the crewmates without getting voted out";
        Color = ColorTools.RoleColors[RoleType.Impostor];
    }
}
