public class SelectPanoramaUsecase
{
    private readonly ICurrentTourService _currentTourService;
    private readonly IPanoramaPresenter _panoramaPresenter;

    public SelectPanoramaUsecase(
        ICurrentTourService currentTourService,
        IPanoramaPresenter panoramaPresenter)
    {
        _currentTourService = currentTourService;
        _panoramaPresenter = panoramaPresenter;
    }

    public Panorama Execute(string panoramaId)
    {
        var currentTour = _currentTourService.GetCurrentTour();

        var panorama = currentTour.GetPanorama(panoramaId);

        _panoramaPresenter.Present(panorama);

        return panorama;
    }
}
