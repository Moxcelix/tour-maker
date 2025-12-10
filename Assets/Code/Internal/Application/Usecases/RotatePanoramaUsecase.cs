public class RotatePanoramaUsecase
{
    private readonly ICurrentTourService _currentTourService;
    private readonly ITourRepository _tourRepository;

    public RotatePanoramaUsecase(
        ICurrentTourService currentTourService,
        ITourRepository tourRepository)
    {
        _currentTourService = currentTourService;
        _tourRepository = tourRepository;
    }

    public Panorama Execute(string panoramaId, float angle)
    {
        var tour = _currentTourService.GetCurrentTour();

        var panorama = tour.GetPanorama(panoramaId);

        panorama.Rotate(angle);

        _tourRepository.Update(tour);

        return panorama;
    }
}
