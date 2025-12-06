public class LinkPanoramasController
{
    private readonly LinkPanoramasUsecase linkPanoramasUsecase;

    public delegate void OnLinkPanorama(Bridge bridge);
    public event OnLinkPanorama OnLinkPanoramaEvent;

    public LinkPanoramasController(LinkPanoramasUsecase linkPanoramasUsecase)
    {
        this.linkPanoramasUsecase = linkPanoramasUsecase;
    }

    public void LinkPanoramas(string panorama1Id, string panorama2Id)
    {
        var bridge = linkPanoramasUsecase.Execute(panorama1Id, panorama2Id);

        OnLinkPanoramaEvent?.Invoke(bridge);
    }
}
