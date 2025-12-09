public class SelectPanoramaController
{
    private readonly SelectPanoramaUsecase selectPanoramaUsecase;
    private readonly TourMapView tourMapView;
    private readonly NavigationArrowsView navigationArrowsView;
    private readonly PanoramaDataMenu panoramaDataMenu;


    public SelectPanoramaController(
        SelectPanoramaUsecase selectPanoramaUsecase, 
        TourMapView tourMapView, 
        NavigationArrowsView navigationArrowsView,
        PanoramaDataMenu panoramaDataMenu)
    {
        this.selectPanoramaUsecase = selectPanoramaUsecase;
        this.tourMapView = tourMapView;
        this.navigationArrowsView = navigationArrowsView;    
        this.panoramaDataMenu = panoramaDataMenu;

        this.tourMapView.OnPanoramaLeftClicked += OnPanoramaSelected;
        this.navigationArrowsView.OnNavigationArrowClicked += OnPanoramaSelected;
    }

    private void OnPanoramaSelected(string panoramaId)
    {
        var panorama = selectPanoramaUsecase.Execute(panoramaId);

        tourMapView.SelectPanorama(panoramaId);

        panoramaDataMenu.Show(panorama);
    }
}

