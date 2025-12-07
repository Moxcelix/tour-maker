public class SelectPanoramaUsecase
{
    private readonly ICurrentTourService _currentTourService;
    private readonly IPanoramaPresenter _panoramaPresenter;
    private readonly IPanoramaLinksPresenter _panoramaLinksPresenter;

    public SelectPanoramaUsecase(
        ICurrentTourService currentTourService,
        IPanoramaPresenter panoramaPresenter,
        IPanoramaLinksPresenter panoramaBrigesPresenter)
    {
        _currentTourService = currentTourService;
        _panoramaPresenter = panoramaPresenter;
        _panoramaLinksPresenter = panoramaBrigesPresenter;
    }

    public Panorama Execute(string panoramaId)
    {
        var currentTour = _currentTourService.GetCurrentTour();

        var panorama = currentTour.GetPanorama(panoramaId);

        _panoramaPresenter.Present(panorama);

        var panoramaLinks = currentTour.GetConnectedPanoramas(panoramaId);

        _panoramaLinksPresenter.Present(panorama, panoramaLinks);

        return panorama;
    }
}
