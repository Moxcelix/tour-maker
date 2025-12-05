public class GetPanoramaUsecase
{
    private readonly ICurrentTourService _currentTourService;

    public GetPanoramaUsecase(ICurrentTourService currentTourService)
    {
        _currentTourService = currentTourService;
    }

    public Panorama Execute(string id)
    {
        var currantTour = _currentTourService.GetCurrentTour();

        var panorama = currantTour.GetPanorama(id);

        return panorama;
    }
}
