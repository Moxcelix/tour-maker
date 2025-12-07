public class PanoramaLinksPresenter : IPanoramaLinksPresenter
{
    private readonly NavigationArrowsView _navigationArrowsService;

    public PanoramaLinksPresenter(NavigationArrowsView navigationArrowsService)
    {
        _navigationArrowsService = navigationArrowsService;
    }

    public void Present(Panorama origin, Panorama[] panoramas)
    {
        _navigationArrowsService.Clear();

        _navigationArrowsService.NewArrows(origin, panoramas);
    }
}
