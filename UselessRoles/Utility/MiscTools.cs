using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UselessRoles.Utility;

public static class MiscTools
{
    public static void SetCustomOutline(this Vent vent, Color color, bool enabled, bool mainTarget)
    {
        vent?.myRend.material.SetFloat("_Outline", enabled ? 1f : 0);
        vent?.myRend.material.SetColor("_OutlineColor", color);
        vent?.myRend.material.SetColor("_AddColor", mainTarget ? color : Color.clear);
    }
    
    public static Il2CppSystem.Collections.Generic.List<T> ToIl2CppList<T>(this List<T> list)
    {
        var casted = new Il2CppSystem.Collections.Generic.List<T>();
        
        foreach (T element in list)
            casted.Add(element);
        
        return casted;
    }

    public static Sprite Clone(this Sprite sprite) => Sprite.Create(
        sprite.texture,
        sprite.rect,
        sprite.pivot,
        sprite.pixelsPerUnit,
        0,
        SpriteMeshType.FullRect,
        sprite.border,
        false
    );
}