public class AddPanoramaUsecase
{
    private readonly ICurrentTourService currentTourService; 
    private readonly ITourRepository tourRepository; 
    private readonly IPanoramaPresenter panoramaPresenter;
    private readonly IPanoramaLinksPresenter panoramaLinksPresenter;
    private readonly PanoramaFactory panoramaFactory; 
    public AddPanoramaUsecase(
        ICurrentTourService currentTourService, 
        ITourRepository tourRepository,
        IPanoramaPresenter panoramaPresenter,
        IPanoramaLinksPresenter panoramaLinksPresenter,
        PanoramaFactory panoramaFactory)
    {
        this.currentTourService = currentTourService;
        this.tourRepository = tourRepository;
        this.panoramaPresenter = panoramaPresenter;
        this.panoramaLinksPresenter = panoramaLinksPresenter;
        this.panoramaFactory = panoramaFactory;
    }
    public Panorama Execute(string panoramaId, string panoramaName)
    {
        var currentTour = currentTourService.GetCurrentTour();

        var panorama = panoramaFactory.CreateEmptyPanorama(panoramaId, panoramaName);

        currentTour.AddPanorama(panorama);

        tourRepository.Update(currentTour);

        panoramaPresenter.Present(panorama);

        var links = currentTour.GetConnectedPanoramas(panoramaId);

        panoramaLinksPresenter.Present(panorama, links);

        return panorama;
    }
} 