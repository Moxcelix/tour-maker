public class SelectPanoramaController
{
    private readonly SelectPanoramaUsecase selectPanoramaUsecase;

    public SelectPanoramaController(SelectPanoramaUsecase selectPanoramaUsecase)
    {
        this.selectPanoramaUsecase = selectPanoramaUsecase;
    }

    public void SelectPanorama(string panoramaId)
    {
        selectPanoramaUsecase.Execute(panoramaId);
    }
}
