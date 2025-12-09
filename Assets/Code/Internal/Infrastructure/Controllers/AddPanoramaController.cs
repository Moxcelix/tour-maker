public class AddPanoramaController
{
    private readonly AddPanoramaUsecase addPanoramaUsecase;

    private readonly FileDialogService fileDialogService;
    private readonly PanoramaTextureService panoramaTextureService;
    private readonly IdGeneratorService idGeneratorService;
    private readonly TextureLoadService textureLoadService;
    private readonly TexturePathService texturePathService;

    private readonly TourMapView tourMapView;
    private readonly UIButton button;

    public AddPanoramaController(
        AddPanoramaUsecase addPanoramaUsecase,
        FileDialogService fileDialogService,
        PanoramaTextureService panoramaTextureService,
        IdGeneratorService idGeneratorService,
        TextureLoadService textureLoadService,
        TexturePathService texturePathService,
        TourMapView tourMapView,
        UIButton button)
    {
        this.addPanoramaUsecase = addPanoramaUsecase;
        this.fileDialogService = fileDialogService;
        this.panoramaTextureService = panoramaTextureService;
        this.idGeneratorService = idGeneratorService;
        this.textureLoadService = textureLoadService;
        this.texturePathService = texturePathService;
        this.tourMapView = tourMapView;
        this.button = button;

        this.button.Clicked += AddPanorama;
    }

    private void AddPanorama()
    {
        var filename = fileDialogService.OpenFileDialog();

        if (filename == null)
        {
            return;
        }

        var id = idGeneratorService.GenerateId();

        var texture = textureLoadService.LoadTexture(filename);

        panoramaTextureService.AddTexture(id, texture);

        texturePathService.RegisterPath(id, filename);

        var panorama = addPanoramaUsecase.Execute(id, filename);

        tourMapView.AddPanorama(panorama, texture);
    }
}
