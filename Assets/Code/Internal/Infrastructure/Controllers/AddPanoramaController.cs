public class AddPanoramaController
{
    private readonly AddPanoramaUsecase addPanoramaUsecase;

    private readonly FileDialogService fileDialogService;
    private readonly PanoramaTextureService panoramaTextureService;
    private readonly IdGeneratorService idGeneratorService;
    private readonly TextureLoadService textureLoadService;

    public AddPanoramaController(
        AddPanoramaUsecase addPanoramaUsecase,
        FileDialogService fileDialogService,
        PanoramaTextureService panoramaTextureService,
        IdGeneratorService idGeneratorService,
        TextureLoadService textureLoadService)
    {
        this.addPanoramaUsecase = addPanoramaUsecase;
        this.fileDialogService = fileDialogService;
        this.panoramaTextureService = panoramaTextureService;
        this.idGeneratorService = idGeneratorService;
        this.textureLoadService = textureLoadService;
    }

    public void AddPanorama()
    {
        var filename = fileDialogService.OpenFileDialog();

        var id = idGeneratorService.GenerateId();

        var texture = textureLoadService.LoadTexture(filename);

        panoramaTextureService.AddTexture(id, texture);

        addPanoramaUsecase.Execute(id);
    }
}
