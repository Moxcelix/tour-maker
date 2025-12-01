using UnityEngine;

public class Modules : MonoBehaviour
{
    // Domain
    private PanoramaFactory panoramaFactory;
    private IPanoramaPresenter panoramaPresenter;
    private ICurrentTourService currentTourService;
    private ITourRepository tourRepository;

    // Application
    private AddPanoramaUsecase addPanoramaUsecase;
    
    // Infrastructure
    private AddPanoramaController addPanoramaController;
    [SerializeField] private PanoramaTextureService panoramaTextureService;
    [SerializeField] private TextureViewService textureViewService;

    private FileDialogService fileDialogService;
    private IdGeneratorService idGeneratorService;
    private TextureLoadService textureLoadService;

    // Presentation
    [SerializeField] private AddPanoramaButton addPanoramaButton;

    private void Start()
    {
        panoramaPresenter = new PanoramaPresenter(panoramaTextureService, textureViewService);
        fileDialogService = new FileDialogService();
        idGeneratorService = new IdGeneratorService();
        textureLoadService = new TextureLoadService();

        panoramaFactory = new PanoramaFactory();
        currentTourService = new CurrentTourService();
        tourRepository = new TourRepository();

        addPanoramaUsecase = new AddPanoramaUsecase(
            currentTourService, 
            tourRepository, 
            panoramaPresenter, 
            panoramaFactory);

        addPanoramaController = new AddPanoramaController(
            addPanoramaUsecase, 
            fileDialogService, 
            panoramaTextureService, 
            idGeneratorService, 
            textureLoadService);

        addPanoramaButton.Initialize(addPanoramaController);
    }
}
