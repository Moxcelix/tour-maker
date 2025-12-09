using UnityEngine;

public class Modules : MonoBehaviour
{
    // Domain
    private PanoramaFactory panoramaFactory;
    private IPanoramaPresenter panoramaPresenter;
    private IPanoramaLinksPresenter panoramaLinksPresenter;
    private ICurrentTourService currentTourService;
    private ITourRepository tourRepository;

    // Application
    private AddPanoramaUsecase addPanoramaUsecase;
    private NewTourUsecase newTourUsecase;
    private GetPanoramaUsecase getPanoramaUsecase;
    private RenamePanoramaUsecase renamePanoramaUsecase;
    private SelectPanoramaUsecase selectPanoramaUsecase;
    private MovePanoramaUsecase movePanoramaUsecase;
    private LinkPanoramasUsecase linkPanoramaUsecase;
    private GetTourUsecase getTourUsecase;
    
    // Infrastructure
    private AddPanoramaController addPanoramaController;
    private NewTourController newTourController;
    private GetPanoramaController getPanoramaController;
    private RenamePanoramaController renamePanoramaController;
    private SelectPanoramaController selectPanoramaController;
    private MovePanoramaController movePanoramaController;
    private LinkPanoramasController linkPanoramasController;
    private GetTourController getTourController;
    private PanoramaContextController panoramaContextController;

    [SerializeField] private PanoramaTextureService panoramaTextureService;
    [SerializeField] private TextureViewService textureViewService;
    [SerializeField] private NavigationArrowsView navigationArrowsView;

    private FileDialogService fileDialogService;
    private IdGeneratorService idGeneratorService;
    private TextureLoadService textureLoadService;

    // Presentation
    [SerializeField] private AddPanoramaButton addPanoramaButton;
    [SerializeField] private NewTourButton newTourButton;
    [SerializeField] private TourMapView tourMapView;
    [SerializeField] private PanoramaDataMenu panoramaDataMenu;
    [SerializeField] private MouseContextMenu mouseContextMenu;

    [SerializeField] private LinkingView linkingView;
    [SerializeField] private UIButton getTourButton;

    private void Start()
    {
        panoramaPresenter = new PanoramaPresenter(panoramaTextureService, textureViewService);
        panoramaLinksPresenter = new PanoramaLinksPresenter(navigationArrowsView);
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
            panoramaLinksPresenter,
            panoramaFactory);
        newTourUsecase = new NewTourUsecase(
            currentTourService, 
            tourRepository);
        getPanoramaUsecase = new GetPanoramaUsecase(
            currentTourService);
        renamePanoramaUsecase = new RenamePanoramaUsecase(
            currentTourService, 
            tourRepository);
        selectPanoramaUsecase = new SelectPanoramaUsecase(
            currentTourService, 
            panoramaPresenter, 
            panoramaLinksPresenter);
        movePanoramaUsecase = new MovePanoramaUsecase(
            currentTourService, 
            tourRepository);
        linkPanoramaUsecase = new LinkPanoramasUsecase(
            currentTourService, 
            tourRepository);
        getTourUsecase = new GetTourUsecase(currentTourService);

        addPanoramaController = new AddPanoramaController(
            addPanoramaUsecase, 
            fileDialogService, 
            panoramaTextureService, 
            idGeneratorService, 
            textureLoadService,
            tourMapView);
        newTourController = new NewTourController(
            newTourUsecase);
        getPanoramaController = new GetPanoramaController(
            getPanoramaUsecase);
        renamePanoramaController = new RenamePanoramaController(
            renamePanoramaUsecase,
            panoramaDataMenu,
            tourMapView);
        selectPanoramaController = new SelectPanoramaController(
            selectPanoramaUsecase,
            tourMapView,
            navigationArrowsView,
            panoramaDataMenu);
        movePanoramaController = new MovePanoramaController(
            movePanoramaUsecase,
            navigationArrowsView,
            tourMapView);
        linkPanoramasController = new LinkPanoramasController(
            linkPanoramaUsecase,
            linkingView,
            tourMapView,
            navigationArrowsView);
        getTourController = new GetTourController(
            getTourUsecase, 
            getTourButton, 
            tourMapView, 
            panoramaTextureService);
        panoramaContextController = new PanoramaContextController(
            linkingView, 
            mouseContextMenu, 
            tourMapView);

        addPanoramaButton.Initialize(addPanoramaController);

        newTourController.NewTour();
        panoramaDataMenu.Hide();
    }
}
