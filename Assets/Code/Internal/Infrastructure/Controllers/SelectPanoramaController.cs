public class SelectPanoramaController
{
    private readonly SelectPanoramaUsecase selectPanoramaUsecase;

    public delegate void OnSelectPanorama(string panoramaId);
    public OnSelectPanorama OnSelectPanoramaEvent;

    public SelectPanoramaController(SelectPanoramaUsecase selectPanoramaUsecase)
    {
        this.selectPanoramaUsecase = selectPanoramaUsecase;
    }

    public void SelectPanorama(string panoramaId)
    {
        selectPanoramaUsecase.Execute(panoramaId);

        OnSelectPanoramaEvent?.Invoke(panoramaId);
    }
}
