public class MovePanoramaController
{
    private readonly MovePanoramaUsecase movePanoramaUsecase;

    private readonly NavigationArrowsView navigationArrowsView;
    private readonly TourMapView tourMapView;

    public MovePanoramaController(
        MovePanoramaUsecase movePanoramaUsecase, 
        NavigationArrowsView navigationArrowsView,
        TourMapView tourMapView)
    {
        this.movePanoramaUsecase = movePanoramaUsecase;
        this.navigationArrowsView = navigationArrowsView;
        this.tourMapView = tourMapView;

        this.tourMapView.OnPanoramaMoved += OnMove;
    }

    private void OnMove(string panoramaId, float x, float y)
    {
        var panorama = movePanoramaUsecase.Execute(panoramaId, x, y);

        navigationArrowsView.MovePanorama(panorama);
    }
}
