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
    private GetPanoramaUsecase getPanoramaUsecase;
    private RenamePanoramaUsecase renamePanoramaUsecase;
    
    // Infrastructure
    private AddPanoramaController addPanoramaController;
    private NewTourController newTourController;
    private GetPanoramaController getPanoramaController;
    private RenamePanoramaController renamePanoramaController;

    [SerializeField] private PanoramaTextureService panoramaTextureService;
    [SerializeField] private TextureViewService textureViewService;

    private FileDialogService fileDialogService;
    private IdGeneratorService idGeneratorService;
    private TextureLoadService textureLoadService;

    // Presentation
    [SerializeField] private AddPanoramaButton addPanoramaButton;
    [SerializeField] private NewTourButton newTourButton;
    [SerializeField] private TourMapView tourMapView;
    [SerializeField] private PanoramaDataMenu panoramaDataMenu;

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
        getPanoramaUsecase = new GetPanoramaUsecase(
            currentTourService);
        renamePanoramaUsecase = new RenamePanoramaUsecase(
            currentTourService, 
            tourRepository);

        addPanoramaController = new AddPanoramaController(
            addPanoramaUsecase, 
            fileDialogService, 
            panoramaTextureService, 
            idGeneratorService, 
            textureLoadService);
        newTourController = new NewTourController(
            newTourUsecase);
        getPanoramaController = new GetPanoramaController(
            getPanoramaUsecase);
        renamePanoramaController = new RenamePanoramaController(
            renamePanoramaUsecase);

        addPanoramaButton.Initialize(addPanoramaController);
        newTourButton.Initialize(newTourController);
        tourMapView.Initialize(
            addPanoramaController, 
            getPanoramaController, 
            renamePanoramaController, 
            panoramaDataMenu);
        panoramaDataMenu.Initialize(renamePanoramaController);

        newTourController.NewTour();
        panoramaDataMenu.Hide();
    }
}
