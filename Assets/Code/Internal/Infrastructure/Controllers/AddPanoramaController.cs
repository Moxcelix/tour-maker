public class AddPanoramaController
{
    private readonly AddPanoramaUsecase addPanoramaUsecase;

    private readonly FileDialogService fileDialogService;
    private readonly PanoramaTextureService panoramaTextureService;
    private readonly IdGeneratorService idGeneratorService;
    private readonly TextureLoadService textureLoadService;

    private readonly TourMapView tourMapView;

    public AddPanoramaController(
        AddPanoramaUsecase addPanoramaUsecase,
        FileDialogService fileDialogService,
        PanoramaTextureService panoramaTextureService,
        IdGeneratorService idGeneratorService,
        TextureLoadService textureLoadService,
        TourMapView tourMapView)
    {
        this.addPanoramaUsecase = addPanoramaUsecase;
        this.fileDialogService = fileDialogService;
        this.panoramaTextureService = panoramaTextureService;
        this.idGeneratorService = idGeneratorService;
        this.textureLoadService = textureLoadService;
        this.tourMapView = tourMapView;
    }

    public void AddPanorama()
    {
        var filename = fileDialogService.OpenFileDialog();

        if (filename == null)
        {
            return;
        }

        var id = idGeneratorService.GenerateId();

        var texture = textureLoadService.LoadTexture(filename);

        panoramaTextureService.AddTexture(id, texture);

        var panorama = addPanoramaUsecase.Execute(id, filename);

        tourMapView.AddPanorama(panorama, texture);
    }
}
