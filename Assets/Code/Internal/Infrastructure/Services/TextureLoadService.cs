using System.IO;
using UnityEngine;

public class TextureLoadService
{
    public Texture LoadTexture(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);

        Texture2D texture = new(2, 2, TextureFormat.RGBA32, false);

        texture.LoadImage(fileData);

        return texture;
    }
}
