public class MovePanoramaController
{
    private readonly MovePanoramaUsecase movePanoramaUsecase;

    public delegate void OnMovePanorama(Panorama panorama);
    public event OnMovePanorama OnMovePanoramaEvent;

    public MovePanoramaController(
        MovePanoramaUsecase movePanoramaUsecase)
    {
        this.movePanoramaUsecase = movePanoramaUsecase;
    }

    public void MovePanorama(string panoramaId, float x, float y)
    {
        var panorama = movePanoramaUsecase.Execute(panoramaId, x, y);

        OnMovePanoramaEvent?.Invoke(panorama);
    }
}
