using System;
using System.Collections;
using UnityEngine;
using UselessRoles.Utility;

namespace UselessRoles.Buttons;

public sealed class RoleVentButton : RoleActionButton
{
    public bool IsTargetInRange => Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), TargetVent.transform.position) <= TargetVent.UsableDistance;
    public Vent TargetVent { get; set; }
    
    
    public Color HighlightColor = Color.red;

    private SpriteRenderer highlightGraphic;
    
    public override void Awake()
    {
        base.Awake();
        
        SetText("Vent", HighlightColor);
        
        graphic.sprite = AssetTools.LoadSprite("UselessRoles.Resources.Vent_Button_Base.png");
        CreateHighlight();
    }

    private void CreateHighlight()
    {
        highlightGraphic = GameObject.Instantiate(graphic, graphic.transform.parent);
        for (int i = 0; i < highlightGraphic.transform.childCount; i++)
            GameObject.DestroyImmediate(highlightGraphic.transform.GetChild(i).gameObject);
        
        highlightGraphic.name = "VentButtonHighlight";
        highlightGraphic.sprite = AssetTools.LoadSprite("UselessRoles.Resources.Vent_Button_Color.png");
        highlightGraphic.color = HighlightColor;
    }
    
    public override void Update()
    {
        FindTarget();
        base.Update();
        UpdateHighlight();
    }

    private void FindTarget()
    {
        float closestDistance = float.MaxValue;
        Vent closestVent = null;

        foreach (var vent in GameObject.FindObjectsOfType<Vent>())
        {
            // Remove all outlines by default
            vent.SetCustomOutline(HighlightColor, false, false);
            
            if (!vent || vent.UsableDistance < 0f)
                continue;

            float dist = Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), vent.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestVent = vent;
            }
        }

        TargetVent = closestVent;

        if (CanClick())
            TargetVent.SetCustomOutline(HighlightColor, true, IsTargetInRange);
    }

    private void UpdateHighlight()
    {
        highlightGraphic.transform.localPosition = graphic.transform.localPosition;
        highlightGraphic.material.SetFloat("_Percent", graphic.material.GetFloat("_Percent"));
        
        // Sync colors between highlight graphic and base graphic
        if (canInteract)
        {
            highlightGraphic.color = new Color(HighlightColor.r, HighlightColor.g, HighlightColor.b, Palette.EnabledColor.a);
            highlightGraphic.material.SetFloat("_Desat", 0f);
        }
        else
        {
            highlightGraphic.color = new Color(HighlightColor.r, HighlightColor.g, HighlightColor.b, Palette.DisabledClear.a);
            highlightGraphic.material.SetFloat("_Desat", 1f);
        }
    }
    
    public override void DoClick()
    {
        base.DoClick();
        UseTarget();
    }
    
    private void UseTarget()
    {
        if (!CanClick())
            return;
        
        AchievementManager.Instance.OnConsoleUse(TargetVent.Cast<IUsable>());
        var player = PlayerControl.LocalPlayer;
        
        // Handle player entering vent
        // (Vent exiting is handles by AbilityEnd())
        if (!player.inVent)
        {
            player.MyPhysics.RpcEnterVent(TargetVent.Id);
            TargetVent.SetButtons(true);
        }
    }
    
    protected override bool CanClick()
    {
        var player = PlayerControl.LocalPlayer;
        var system = ShipStatus.Instance.Systems[SystemTypes.Ventilation].Cast<VentilationSystem>();
        
        bool canEnterVent = TargetVent &&
                            IsTargetInRange && 
                            !PhysicsHelpers.AnythingBetween(player.Collider, player.Collider.bounds.center, TargetVent.transform.position, Constants.ShipOnlyMask, useTriggers: false);

        return base.CanClick() &&
               (canEnterVent || player.inVent) &&
               !player.walkingToVent &&
               !player.Data.IsDead &&
               !player.MustCleanVent(TargetVent.Id) &&
               !system.IsVentCurrentlyBeingCleaned(TargetVent.Id);
    }

    protected override void AbilityEnd()
    {
        // Force the player to leave the vent when the ability ends
        PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(TargetVent.Id);
        TargetVent.SetButtons(false);
        
        base.AbilityEnd();
    }
}