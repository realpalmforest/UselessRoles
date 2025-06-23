using System;
using Reactor.Utilities.Attributes;
using TMPro;
using UnityEngine;

namespace UselessRoles.Buttons;

[RegisterInIl2Cpp]
public class RoleActionButton : ActionButton
{
    public int UsesRemaining = 3;

    public float DefaultCooldown = 10f;
    public float Cooldown = 10f;

    //public float defaultfillUpTime = 2;
    //public float fillUpTime = 2;

    public event EventHandler OnClickEvent;

    public virtual void Awake()
    {
        base.graphic = this.GetComponentInChildren<SpriteRenderer>();
        base.glyph = this.transform.FindChild("Ability").GetComponentInChildren<ActionMapGlyphDisplay>();

        base.buttonLabelText = this.transform.FindChild("Ability").FindChild("Text_TMP").GetComponent<TextMeshPro>();
        base.cooldownTimerText = this.transform.FindChild("Ability").FindChild("CooldownTimer_TMP").GetComponent<TextMeshPro>();
        base.usesRemainingText = this.transform.FindChild("Uses").FindChild("Text_TMP").GetComponent<TextMeshPro>();

        base.usesRemainingSprite = this.transform.FindChild("Uses").GetComponent<SpriteRenderer>();

        base.name = "RoleActionButton";
    }

    public override void DoClick()
    {
        if (!base.isActiveAndEnabled)
            return;
        if (!PlayerControl.LocalPlayer)
            return;
        if (!LobbyBehaviour.Instance)
            return;

        UsesRemaining--;
        Cooldown = DefaultCooldown;
        isCoolingDown = true;

        OnClickEvent?.Invoke(this, EventArgs.Empty);
    }

    public virtual void FixedUpdate()
    {
        if (isCoolingDown)
        {
            Cooldown -= Time.deltaTime;

            if (Cooldown <= 0f)
            {
                Cooldown = 0f;
                base.ResetCoolDown();
            }
        }

        base.SetCoolDown(Cooldown, DefaultCooldown);
        base.SetUsesRemaining(UsesRemaining);
    }

    public static T Create<T>(HudManager hud) where T : RoleActionButton
    {
        var button = GameObject.Instantiate(hud.AbilityButton.gameObject, hud.AbilityButton.transform.parent);
        button.SetActive(true);

        GameObject.DestroyImmediate(button.GetComponent<AbilityButton>());
        return button.AddComponent<T>();
    }
}