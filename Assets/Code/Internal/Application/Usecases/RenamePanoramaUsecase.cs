public class RenamePanoramaUsecase
{
    private readonly ICurrentTourService currentTourService;
    private readonly ITourRepository tourRepository;

    public RenamePanoramaUsecase(
        ICurrentTourService currentTourService,
        ITourRepository tourRepository)
    {
        this.currentTourService = currentTourService;
        this.tourRepository = tourRepository;
    }

    public Panorama Execute(string panoramaId, string newName)
    {
        var currentTour = currentTourService.GetCurrentTour();

        var panorama = currentTour.GetPanorama(panoramaId);

        panorama.Rename(newName);

        tourRepository.Update(currentTour);

        return panorama;
    }
}
