using Reactor.Utilities.Attributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UselessRoles.Buttons;

[RegisterInIl2Cpp]
public class RoleActionButton : ActionButton
{
    public event EventHandler OnClickEvent;
    public event EventHandler OnFixedUpdateEvent;

    public float MeetingCooldown = 10f;
    public float DefaultCooldown = 10f;
    public float Cooldown = 10f;

    public int UsesRemaining
    {
        get => _usesRemaining;
        set
        {
            _usesRemaining = Math.Max(0, value);
            base.SetUsesRemaining(_usesRemaining);
        }
    }

    public bool InfiniteUses
    {
        get => _infiniteUses;
        set
        {
            _infiniteUses = value;

            this.usesRemainingText.gameObject.SetActive(!value);
            this.usesRemainingSprite.gameObject.SetActive(!value);
        }
    }

    private bool _infiniteUses;
    private int _usesRemaining = 3;

    protected PassiveButton passiveButton;
    
    public virtual void Awake()
    {
        base.graphic = this.GetComponentInChildren<SpriteRenderer>();
        base.glyph = this.transform.FindChild("Ability").FindChild("InputDisplayGlyph").GetComponent<ActionMapGlyphDisplay>();

        base.buttonLabelText = this.transform.FindChild("Ability").FindChild("Text_TMP").GetComponent<TextMeshPro>();
        base.cooldownTimerText = this.transform.FindChild("Ability").FindChild("CooldownTimer_TMP").GetComponent<TextMeshPro>();
        base.usesRemainingText = this.transform.FindChild("Uses").FindChild("Text_TMP").GetComponent<TextMeshPro>();

        base.usesRemainingSprite = this.transform.FindChild("Uses").GetComponent<SpriteRenderer>();

        passiveButton = this.GetComponent<PassiveButton>();
        passiveButton.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        passiveButton.OnClick.AddListener((UnityAction)DoClick);

        InfiniteUses = true;
    }

    public override void DoClick()
    {
        if (!base.isActiveAndEnabled)
            return;
        if (!PlayerControl.LocalPlayer)
            return;
        if (HudManager.Instance.IsIntroDisplayed)
            return;
        if (isCoolingDown)
            return;
        if (!canInteract)
            return;

        if (!InfiniteUses)
            UsesRemaining--;

        Cooldown = DefaultCooldown;

        OnClickEvent?.Invoke(this, EventArgs.Empty);
    }

    public virtual void FixedUpdate()
    {
        if (Cooldown > 0f)
        {
            Cooldown -= Time.fixedDeltaTime;

            Cooldown = Math.Clamp(Cooldown, 0f, DefaultCooldown);
            base.SetCoolDown(Cooldown, DefaultCooldown);
        }

        if ((UsesRemaining <= 0 && !InfiniteUses) || isCoolingDown)
            base.SetDisabled();
        else base.SetEnabled();
        
        OnFixedUpdateEvent?.Invoke(this, EventArgs.Empty);
    }

    public void SetText(string text, Color? color)
    {
        buttonLabelText.text = text;

        // buttonLabelText.faceColor = color ?? Color.black;
        buttonLabelText.outlineColor = color ?? Color.black;
    }

    public void SetCooldowns(float cooldown, float defaultCooldown, float meetingCooldown)
    {
        Cooldown = cooldown;
        DefaultCooldown = defaultCooldown;
        MeetingCooldown = meetingCooldown;

        isCoolingDown = cooldown > 0;
    }
    
    public static T Create<T>(string name = "RoleActionButton") where T : RoleActionButton
    {
        var button = GameObject.Instantiate(HudManager.Instance.AbilityButton.gameObject, HudManager.Instance.AbilityButton.transform.parent);
        button.SetActive(true);
        button.name = name;
        
        GameObject.DestroyImmediate(button.GetComponent<AbilityButton>());
        return button.AddComponent<T>();
    }
}