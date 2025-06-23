using Reactor.Utilities;
using Reactor.Utilities.Attributes;

namespace UselessRoles.Buttons;

[RegisterInIl2Cpp]
public class TrapButton : RoleActionButton
{
    public override void Awake()
    {
        base.Awake();

        name = "TrapButton";
        buttonLabelText.text = "Trap";
    }

    public override void DoClick()
    {
        base.DoClick();

        Logger<UselessRolesPlugin>.Message("Wowee you pressed the TRAP BUTTON !!! :D");
    }
}