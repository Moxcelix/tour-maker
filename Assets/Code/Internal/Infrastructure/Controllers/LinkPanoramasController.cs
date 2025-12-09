public class LinkPanoramasController
{
    private readonly LinkPanoramasUsecase linkPanoramasUsecase;

    private readonly LinkingView linkingView;
    private readonly TourMapView tourMapView;
    private readonly NavigationArrowsView navigationArrowsView;

    public LinkPanoramasController(
        LinkPanoramasUsecase linkPanoramasUsecase,
        LinkingView linkingView,
        TourMapView tourMapView,
        NavigationArrowsView navigationArrowsView)
    {
        this.linkingView = linkingView;
        this.tourMapView = tourMapView;
        this.linkPanoramasUsecase = linkPanoramasUsecase;
        this.navigationArrowsView = navigationArrowsView;

        this.tourMapView.OnPanoramaLeftClicked += OnPanoramaLeftClicked;
        this.linkingView.OnLinkingCompleted += OnLink;
    }

    private void OnPanoramaLeftClicked(string panoramaId)
    {
        linkingView.CompleteLinking(panoramaId);
    }

    private void OnLink(string panorama1Id, string panorama2Id)
    {
        try
        {
            var bridge = linkPanoramasUsecase.Execute(panorama1Id, panorama2Id);

            navigationArrowsView.NewBridge(bridge);

            tourMapView.AddBridge(bridge);
        }
        catch { }
    }
}
