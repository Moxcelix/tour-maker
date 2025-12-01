public class AddPanoramaUsecase
{
    private readonly ICurrentTourService currentTourService; 
    private readonly ITourRepository tourRepository; 
    private readonly PanoramaFactory panoramaFactory; 
    public AddPanoramaUsecase(
        ICurrentTourService currentTourService, 
        ITourRepository tourRepository, 
        PanoramaFactory panoramaFactory)
    {
        this.currentTourService = currentTourService;
        this.tourRepository = tourRepository;
        this.panoramaFactory = panoramaFactory;
    }
    public void Execute(string panoramaId)
    {
        var currentTour = currentTourService.GetCurrentTour();

        var panorama = panoramaFactory.CreateEmptyPanorama(panoramaId);

        currentTour.AddPanorama(panorama);

        tourRepository.Update(currentTour);
    }
} 