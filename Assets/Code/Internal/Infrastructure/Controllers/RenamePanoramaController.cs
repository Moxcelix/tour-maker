public class RenamePanoramaController
{
    private readonly RenamePanoramaUsecase renamePanoramaUsecase;
    private readonly PanoramaDataMenu panoramaDataMenu;
    private readonly TourMapView tourMapView;

    public RenamePanoramaController(
        RenamePanoramaUsecase renamePanoramaUsecase, 
        PanoramaDataMenu panoramaDataMenu, 
        TourMapView tourMapView)
    {
        this.renamePanoramaUsecase = renamePanoramaUsecase;
        this.panoramaDataMenu = panoramaDataMenu;
        this.tourMapView = tourMapView;

        this.panoramaDataMenu.OnNameValueChanged += RenamePanorama;
    }

    public void RenamePanorama(string id, string name)
    {
        var panorama = renamePanoramaUsecase.Execute(id, name);

        tourMapView.RenamePanorama(id, name);
    }
}
