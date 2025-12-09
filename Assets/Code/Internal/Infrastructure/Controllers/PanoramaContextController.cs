public class PanoramaContextController
{
    private readonly LinkingView linkingView;
    private readonly TourMapView tourMapView;
    private readonly MouseContextMenu mouseContextMenu;

    public PanoramaContextController(
        LinkingView linkingView,
        MouseContextMenu mouseContextMenu,
        TourMapView tourMapView)
    {
        this.linkingView = linkingView;
        this.mouseContextMenu = mouseContextMenu;
        this.tourMapView = tourMapView;

        this.tourMapView.OnPanoramaRightClicked += ShowContextMenu;
    }

    private void ShowContextMenu(string panoramaId)
    {
        var list = GetContextMenu(panoramaId);

        mouseContextMenu.Show(list);
    }

    private MouseContextMenuList GetContextMenu(string panoramaId)
    {
        var items = new MouseContextMenuList.Item[]
        {
            new("Make transition", () => linkingView.StartLinking(panoramaId)),
        };

        return new MouseContextMenuList("Panorama", items);
    }
}
