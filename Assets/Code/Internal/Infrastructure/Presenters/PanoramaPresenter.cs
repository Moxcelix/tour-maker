public class PanoramaPresenter : IPanoramaPresenter
{
    private readonly PanoramaTextureService panoramaTextureService;
    private readonly TextureViewService textureViewService;
    private readonly RotationService rotationService;

    public PanoramaPresenter(
        PanoramaTextureService panoramaTextureService,
        TextureViewService textureViewService,
        RotationService rotationService)
    {
        this.panoramaTextureService = panoramaTextureService;
        this.textureViewService = textureViewService;
        this.rotationService = rotationService;
    }

    public void Present(Panorama panorama)
    {
        var texture = panoramaTextureService.GetPanoramaTexture(panorama.Id);

        textureViewService.RenderTexture(texture);
        rotationService.Rotate(panorama.Rotation);
    }
}