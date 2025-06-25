using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class AssetTools
{
    private delegate bool DLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
    private static DLoadImage _iCallLoadImage;

    private static bool LoadImage(Texture2D texture, byte[] data, bool markNonReadable)
    {
        _iCallLoadImage ??= IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");
        var il2CPPArray = (Il2CppStructArray<byte>)data;

        return _iCallLoadImage.Invoke(texture.Pointer, il2CPPArray.Pointer, markNonReadable);
    }

    public static Sprite LoadSprite(string assetPath)
    {
        Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream stream = assembly.GetManifestResourceStream(assetPath);

        if (stream != null)
        {
            var data = new byte[stream.Length];
            var read = stream.Read(data, 0, (int)stream.Length);
            LoadImage(texture, data, false);
        }

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), texture.width);
    }
}