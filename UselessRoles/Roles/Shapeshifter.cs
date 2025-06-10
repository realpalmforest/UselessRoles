using UselessRoles.Utility;

namespace UselessRoles.Roles;

public class Shapeshifter : Role
{
    public Shapeshifter()
    {
        Name = "Shapeshifter";
        Description = "Take on the form of crewmates and kill";
        Color = ColorTools.RoleColors[RoleType.Shapeshifter];

        RoleType = RoleType.Shapeshifter;
        TeamType = TeamType.Impostor;
    }
}
