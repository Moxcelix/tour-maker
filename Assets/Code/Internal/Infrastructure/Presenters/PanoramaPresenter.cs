using UnityEngine;

public class PanoramaPresenter : IPanoramaPresenter
{
    private readonly PanoramaTextureService panoramaTextureService;
    private Material currentSkyboxMaterial;

    public PanoramaPresenter(PanoramaTextureService panoramaTextureService)
    {
        this.panoramaTextureService = panoramaTextureService;
    }

    public void Present(Panorama panorama)
    {
        var texture = panoramaTextureService.GetPanoramaTexture(panorama.Id);

        if (currentSkyboxMaterial == null)
        {
            currentSkyboxMaterial = new Material(Shader.Find("Skybox/Panoramic"));
        }

        currentSkyboxMaterial.SetTexture("_MainTex", texture);
        currentSkyboxMaterial.SetFloat("_Exposure", 1.0f);
        currentSkyboxMaterial.SetFloat("_Rotation", 0f);

        RenderSettings.skybox = currentSkyboxMaterial;

        DynamicGI.UpdateEnvironment();
    }
}