public class MovePanoramaUsecase
{
    private readonly ICurrentTourService _currentTourService;
    private readonly ITourRepository _tourRepository;

    public MovePanoramaUsecase(
        ICurrentTourService currentTourService,
        ITourRepository tourRepository)
    {
        _currentTourService = currentTourService;
        _tourRepository = tourRepository;
    }

    public Panorama Execute(string panoramaId, float x, float y)
    {
        var tour = _currentTourService.GetCurrentTour();

        var panorama = tour.GetPanorama(panoramaId);

        panorama.Move(x, y);

        _tourRepository.Update(tour);

        return panorama;
    }
}
