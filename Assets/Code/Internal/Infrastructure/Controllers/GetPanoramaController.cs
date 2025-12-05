
public class GetPanoramaController
{
    private readonly GetPanoramaUsecase getPanoramaUsecase;

    public GetPanoramaController(GetPanoramaUsecase getPanoramaUsecase)
    {
        this.getPanoramaUsecase = getPanoramaUsecase;
    }

    public Panorama GetPanorama(string id)
    {
        return getPanoramaUsecase.Execute(id);
    }
}
