using Reactor.Utilities.Attributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UselessRoles.Buttons;

[RegisterInIl2Cpp]
public class RoleActionButton : ActionButton
{
    public float MeetingCooldown = 10f;
    public float DefaultCooldown = 10f;
    public float Cooldown = 10f;

    public float DefaultDuration = 0f;
    public float Duration = 0f;
    public bool IsAbilityActive => Duration > 0f;
    
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
        if(!CanClick()) return;
        
        // If the default duration is 0, or the ability is currently active, end the ability
        if(DefaultDuration == 0f || IsAbilityActive)
        {
            Duration = 0;
            AbilityEnd();
        }
        else if(!IsAbilityActive)
            Duration = DefaultDuration;
        
        RunButtonClickEvent();
    }

    public virtual void Update()
    {
        UpdateCooldown();
        RunUpdateEvent();
    }

    protected void UpdateCooldown()
    {
        // If the ability is currently active
        if (DefaultDuration > 0 && Duration > 0)
        {
            Duration -= Time.deltaTime;
            Duration = Math.Clamp(Duration, 0f, DefaultDuration);
            
            if (Duration == 0)
                AbilityEnd();

            base.SetFillUp(Duration, DefaultDuration);
        }
        
        // If cooling down
        if (DefaultCooldown > 0 && Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
            Cooldown = Math.Clamp(Cooldown, 0f, DefaultCooldown);
            
            // Remove cooldown if the button is out of uses
            if (!InfiniteUses && UsesRemaining == 0)
                Cooldown = 0;
            
            base.SetCoolDown(Cooldown, DefaultCooldown);
        }
        
        if (CanClick())
            base.SetEnabled();
        else base.SetDisabled();
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

    protected virtual bool CanClick() => base.isActiveAndEnabled && 
                                         !HudManager.Instance.IsIntroDisplayed && 
                                         (!isCoolingDown || IsAbilityActive) && 
                                         (InfiniteUses || UsesRemaining > 0);
    
    
    protected virtual void AbilityEnd()
    {
        if (!InfiniteUses) UsesRemaining--;
        Cooldown = DefaultCooldown;
        
        OnAbilityEndEvent?.Invoke(this, EventArgs.Empty);
    }
    public event EventHandler OnAbilityEndEvent;
    
    public event EventHandler OnClickEvent;
    protected virtual void RunButtonClickEvent() => OnClickEvent?.Invoke(this, EventArgs.Empty);
    public event EventHandler OnUpdateEvent;
    protected virtual void RunUpdateEvent() => OnUpdateEvent?.Invoke(this, EventArgs.Empty);
    
    public static T Create<T>(string name = "RoleActionButton") where T : RoleActionButton
    {
        var button = GameObject.Instantiate(HudManager.Instance.AbilityButton.gameObject, HudManager.Instance.AbilityButton.transform.parent);
        button.SetActive(true);
        button.name = name;
        
        GameObject.DestroyImmediate(button.GetComponent<AbilityButton>());
        return button.AddComponent<T>();
    }
}