using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UselessRoles.Roles;
using UselessRoles.Utility;
using Object = UnityEngine.Object;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(TaskAdderGame))]
internal static class FreeplayLaptopPatches
{
    private static TaskFolder rolesFolder;
    
    [HarmonyPatch(nameof(TaskAdderGame.PopulateRoot))]
    [HarmonyPostfix]
    internal static void PopulateRoot_Postfix(TaskAdderGame __instance)
    {
        // Avoid creating the folder multiple times
        if (rolesFolder != null)
            return;
            
        rolesFolder = GameObject.Instantiate(__instance.RootFolderPrefab, __instance.transform);
        rolesFolder.gameObject.SetActive(false);
        rolesFolder.FolderName = "Useless Roles";
        
        __instance.Root.SubFolders.Add(rolesFolder);
        
        Logger<UselessRolesPlugin>.Info("Successfully Created UselessRoles folder in the freeplay laptop");
    }
    
    [HarmonyPatch(nameof(TaskAdderGame.ShowFolder))]
    [HarmonyPostfix]
    internal static void ShowFolder_Postfix(TaskAdderGame __instance, TaskFolder taskFolder)
    {
        if (rolesFolder == null)
        {
            Logger<UselessRolesPlugin>.Error("Failed to populate UselessRoles freeplay laptop folder:\nCustom folder not created.");
            return;
        }

        if (taskFolder.FolderName != rolesFolder.FolderName)
            return;

        // Custom grid placement values to arrange files
        float xCursor = 0f;
        float yCursor = 0f;
        float maxHeight = 0f;
        
        // Create a role button for every modded role
        RoleType[] roles = MiscTools.GetEnumValues<RoleType>().ToArray();
        for (int i = 0; i < roles.Length; i++)
        {
            var role = roles[i];
            RoleChangeButton roleButton;
            
            try
            {
                roleButton = Object.Instantiate(__instance.RoleButton, __instance.TaskParent).gameObject.AddComponent<RoleChangeButton>();
                roleButton.SafePositionWorld = __instance.SafePositionWorld;
                roleButton.Initialize(role);
            
                __instance.AddFileAsChild(rolesFolder, roleButton, ref xCursor, ref yCursor, ref maxHeight);
            }
            catch (Exception)
            {
                Logger<UselessRolesPlugin>.Error($"Failed to create {role.ToString()} RoleChangeButton");
                continue;
            }
            
            Logger<UselessRolesPlugin>.Info($"Successfully created {role.ToString()} RoleChangeButton");
            
            // Some UI navigation stuff, I think???
            // Logic just copied from AU code
            if (!roleButton || !roleButton.Button)
                continue;
            ControllerManager.Instance.AddSelectableUiElement(roleButton.Button);

            if (i != 0)
                continue;
            if (__instance.restorePreviousSelectionFound)
            {
                ControllerManager.Instance.SetDefaultSelection(__instance.restorePreviousSelectionFound);
                __instance.restorePreviousSelectionByFolderName = string.Empty;
                __instance.restorePreviousSelectionFound = null;
            }
            else
            {
                ControllerManager.Instance.SetDefaultSelection(roleButton.Button);
            }
        }
    }
}