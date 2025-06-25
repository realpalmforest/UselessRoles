using System;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UselessRoles.Buttons;

[RegisterInIl2Cpp]
public class RoleActionButton : ActionButton
{
    public HudManager Hud;
    public PassiveButton PButton;

    public int UsesRemaining
    {
        get => _usesRemaining;
        set
        {
            _usesRemaining = Math.Max(0, value);
            base.SetUsesRemaining(_usesRemaining);

            if (_usesRemaining == 0)
                base.SetDisabled();
        }
    }

    public float DefaultCooldown = 10f;
    public float Cooldown = 10f;

    //public float defaultfillUpTime = 2;
    //public float fillUpTime = 2;

    public event EventHandler OnClickEvent;

    private int _usesRemaining = 3;

    public virtual void Awake()
    {
        base.graphic = this.GetComponentInChildren<SpriteRenderer>();
        base.glyph = this.transform.FindChild("Ability").FindChild("InputDisplayGlyph").GetComponent<ActionMapGlyphDisplay>();

        base.buttonLabelText = this.transform.FindChild("Ability").FindChild("Text_TMP").GetComponent<TextMeshPro>();
        base.cooldownTimerText = this.transform.FindChild("Ability").FindChild("CooldownTimer_TMP").GetComponent<TextMeshPro>();
        base.usesRemainingText = this.transform.FindChild("Uses").FindChild("Text_TMP").GetComponent<TextMeshPro>();

        base.usesRemainingSprite = this.transform.FindChild("Uses").GetComponent<SpriteRenderer>();

        base.name = "RoleActionButton";

        PButton = this.GetComponent<PassiveButton>();
        PButton.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        PButton.OnClick.AddListener((UnityAction)DoClick); 
    }

    public override void DoClick()
    {
        if (!base.isActiveAndEnabled)
            return;
        if (!PlayerControl.LocalPlayer)
            return;
        if (Hud.IsIntroDisplayed)
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
    }

    public static T Create<T>(HudManager hud) where T : RoleActionButton
    {
        var button = GameObject.Instantiate(hud.AbilityButton.gameObject, hud.AbilityButton.transform.parent);
        button.SetActive(true);

        GameObject.DestroyImmediate(button.GetComponent<AbilityButton>());
        T btn = button.AddComponent<T>();
        btn.Hud = hud;

        return btn;
    }
}