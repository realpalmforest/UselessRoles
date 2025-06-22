using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class ShapeshifterRole : Role
{
    public ShapeshifterRole()
    {
        Name = "Shapeshifter";
        Description = "Take on the form of crewmates and kill";
        Color = ColorTools.RoleColors[RoleType.Shapeshifter];

        RoleType = RoleType.Shapeshifter;
        TeamType = TeamType.Impostor;
    }
}
