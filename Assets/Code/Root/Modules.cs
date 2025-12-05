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
    private NewTourUsecase newTourUsecase;
    
    // Infrastructure
    private AddPanoramaController addPanoramaController;
    private NewTourController newTourController;
    [SerializeField] private PanoramaTextureService panoramaTextureService;
    [SerializeField] private TextureViewService textureViewService;

    private FileDialogService fileDialogService;
    private IdGeneratorService idGeneratorService;
    private TextureLoadService textureLoadService;

    // Presentation
    [SerializeField] private AddPanoramaButton addPanoramaButton;
    [SerializeField] private NewTourButton newTourButton;

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
        newTourUsecase = new NewTourUsecase(
            currentTourService, 
            tourRepository);

        addPanoramaController = new AddPanoramaController(
            addPanoramaUsecase, 
            fileDialogService, 
            panoramaTextureService, 
            idGeneratorService, 
            textureLoadService);
        newTourController = new NewTourController(newTourUsecase);

        newTourController.NewTour();

        addPanoramaButton.Initialize(addPanoramaController);
        newTourButton.Initialize(newTourController);
    }
}
