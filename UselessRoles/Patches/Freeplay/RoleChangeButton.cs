using Reactor.Utilities.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UselessRoles.Roles;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

[RegisterInIl2Cpp]
public class RoleChangeButton : TaskAddButton
{
	public new RoleType Role
	{
		get => role;
		set
		{
			role = value;
			
			FileImage.color = ColorTools.RoleColors[role];
			RolloverHandler.OutColor = ColorTools.RoleColors[role];
		}
	}
	
	private new RoleType role;

	public new void Start() { } // Remove vanilla functionality

	public void Initialize(RoleType roleType)
	{
		var original = GetComponent<TaskAddButton>();
		
		Text = original.Text;
		RolloverHandler = original.RolloverHandler;
		FileImage = original.FileImage;
		Overlay = original.Overlay;
		CheckImage = original.CheckImage;
		
		GameObject.DestroyImmediate(original);
		
		gameObject.name = $"{roleType.ToString()}RoleButton";
		Role = roleType;
		Text.text = roleType.ToString();
		Overlay.sprite = CheckImage;
		
		Button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
		Button.OnClick.AddListener((UnityAction)(() =>
		{
			ModRoleManager.RpcSetRole(PlayerControl.LocalPlayer, (uint) Role);
		}));
	}
	
	public new void Update()
	{
		Overlay.enabled = PlayerControl.LocalPlayer.HasRole() && PlayerTools.Am(Role);
	}
}