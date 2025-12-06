public class LinkPanoramasController
{
    private readonly LinkPanoramasUsecase linkPanoramasUsecase;

    public LinkPanoramasController(LinkPanoramasUsecase linkPanoramasUsecase)
    {
        this.linkPanoramasUsecase = linkPanoramasUsecase;
    }

    public void LinkPanoramas(string panorama1Id, string panorama2Id)
    {
        linkPanoramasUsecase.Execute(panorama1Id, panorama2Id);
    }
}
