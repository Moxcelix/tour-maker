public class MovePanoramaController
{
    private readonly MovePanoramaUsecase movePanoramaUsecase;

    public MovePanoramaController(MovePanoramaUsecase movePanoramaUsecase)
    {
        this.movePanoramaUsecase = movePanoramaUsecase;
    }

    public void MovePanorama(string panoramaId, float x, float y)
    {
        movePanoramaUsecase.Execute(panoramaId, x, y);
    }
}
