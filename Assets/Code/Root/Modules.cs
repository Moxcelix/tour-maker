using UnityEngine;
using UnityEngine.UIElements;

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
    private OpenTourUsecase openTourUsecase;
    private RotatePanoramaUsecase rotatePanoramaUsecase;
    
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
    private LoadTourController loadTourController;
    private RotatePanoramaController rotatePanoramaController;

    [SerializeField] private PanoramaTextureService panoramaTextureService;
    [SerializeField] private TextureViewService textureViewService;
    [SerializeField] private NavigationArrowsView navigationArrowsView;
    [SerializeField] private RotationService rotationService;

    private FileDialogService fileDialogService;
    private IdGeneratorService idGeneratorService;
    private TextureLoadService textureLoadService;
    private TexturePathService texturePathService;

    // Presentation
    [SerializeField] private TourMapView tourMapView;
    [SerializeField] private PanoramaDataMenu panoramaDataMenu;
    [SerializeField] private MouseContextMenu mouseContextMenu;

    [SerializeField] private LinkingView linkingView;
    [SerializeField] private UIButton getTourButton;
    [SerializeField] private UIButton addPanoramaButton;
    [SerializeField] private UIButton newTourButton;
    [SerializeField] private UIButton openTourButton;

    private void Start()
    {
        panoramaPresenter = new PanoramaPresenter(panoramaTextureService, textureViewService, rotationService);
        panoramaLinksPresenter = new PanoramaLinksPresenter(navigationArrowsView);
        fileDialogService = new FileDialogService();
        idGeneratorService = new IdGeneratorService();
        textureLoadService = new TextureLoadService();

        panoramaFactory = new PanoramaFactory();
        currentTourService = new CurrentTourService();
        texturePathService = new TexturePathService();
        tourRepository = new TourRepository(texturePathService);

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
        openTourUsecase = new OpenTourUsecase(currentTourService, tourRepository);
        rotatePanoramaUsecase = new RotatePanoramaUsecase(currentTourService, tourRepository, panoramaPresenter);

        addPanoramaController = new AddPanoramaController(
            addPanoramaUsecase, 
            fileDialogService, 
            panoramaTextureService, 
            idGeneratorService, 
            textureLoadService,
            texturePathService,
            tourMapView,
            addPanoramaButton);
        newTourController = new NewTourController(
            newTourUsecase, 
            newTourButton);
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
        //getTourController = new GetTourController(
        //    getTourUsecase, 
        //    getTourButton, 
        //    tourMapView, 
        //    panoramaTextureService,
        //    texturePathService, 
        //    textureLoadService);
        panoramaContextController = new PanoramaContextController(
            linkingView, 
            mouseContextMenu, 
            tourMapView);
        loadTourController = new LoadTourController(
            openTourUsecase,
            openTourButton, 
            tourMapView,
            panoramaTextureService, 
            texturePathService, 
            textureLoadService);
        rotatePanoramaController = new RotatePanoramaController(rotatePanoramaUsecase, panoramaDataMenu);

        panoramaDataMenu.Hide();
    }
}
