using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PanoramaTextureService : MonoBehaviour
{
    [System.Serializable]
    public class TextureData
    {
        public string Id;
        public Texture Texture;
    }

    [SerializeField] private List<TextureData> textureDatas = new();

    public Texture GetPanoramaTexture(string id)
    {
        return textureDatas
          .Where(data => data.Id == id)
          .Select(data => data.Texture)
          .FirstOrDefault();
    }
}
