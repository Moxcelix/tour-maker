using UnityEngine;

public class TextureViewService : MonoBehaviour
{
    private Material currentSkyboxMaterial;

    [SerializeField] private MeshRenderer meshRenderer;

    public void RenderTexture(Texture texture)
    {
        if (currentSkyboxMaterial == null)
        {
            currentSkyboxMaterial = new Material(Shader.Find("Skybox/Panoramic"));
        }

        currentSkyboxMaterial.SetTexture("_MainTex", texture);
        currentSkyboxMaterial.SetFloat("_Exposure", 1.0f);
        currentSkyboxMaterial.SetFloat("_Rotation", 0f);

        meshRenderer.material = currentSkyboxMaterial;
    }

    private void OnDestroy()
    {
        if (currentSkyboxMaterial != null)
        {
            Destroy(currentSkyboxMaterial);
        }
    }
}
