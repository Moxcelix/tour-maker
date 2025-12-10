public class RotatePanoramaUsecase
{
    private readonly ICurrentTourService _currentTourService;
    private readonly ITourRepository _tourRepository;
    private readonly IPanoramaPresenter _panoramaPresenter;

    public RotatePanoramaUsecase(
        ICurrentTourService currentTourService,
        ITourRepository tourRepository,
        IPanoramaPresenter panoramaPresenter)
    {
        _currentTourService = currentTourService;
        _tourRepository = tourRepository;
        _panoramaPresenter = panoramaPresenter;
    }

    public Panorama Execute(string panoramaId, float angle)
    {
        var tour = _currentTourService.GetCurrentTour();

        var panorama = tour.GetPanorama(panoramaId);

        panorama.Rotate(angle);

        _panoramaPresenter.Present(panorama);

        _tourRepository.Update(tour);

        return panorama;
    }
}
