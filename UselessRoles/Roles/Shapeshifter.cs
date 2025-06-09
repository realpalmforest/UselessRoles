using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class Shapeshifter : Role
{
    public override void OnAssign()
    {
        Name = "Shapeshifter";
        Description = "Take on the form of crewmates and kill";
        Color = ColorTools.RoleColors[RoleType.Shapeshifter];
    }
}
