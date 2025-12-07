public class PanoramaPresenter : IPanoramaPresenter
{
    private readonly PanoramaTextureService panoramaTextureService;
    private readonly TextureViewService textureViewService;

    public PanoramaPresenter(
        PanoramaTextureService panoramaTextureService,
        TextureViewService textureViewService)
    {
        this.panoramaTextureService = panoramaTextureService;
        this.textureViewService = textureViewService;
    }

    public void Present(Panorama panorama)
    {
        var texture = panoramaTextureService.GetPanoramaTexture(panorama.Id);

        textureViewService.RenderTexture(texture);
    }
}