
public class RotatePanoramaController 
{
    private readonly RotatePanoramaUsecase rotatePanoramaUsecase;

    private readonly PanoramaDataMenu panoramaDataMenu;

    public RotatePanoramaController(
        RotatePanoramaUsecase rotatePanoramaUsecase, 
        PanoramaDataMenu panoramaDataMenu)
    {
        this.rotatePanoramaUsecase = rotatePanoramaUsecase;
        this.panoramaDataMenu = panoramaDataMenu;

        this.panoramaDataMenu.OnAngleValueChanged += Rotate;
    }

    private void Rotate(string panoramaId, float angle)
    {
        rotatePanoramaUsecase.Execute(panoramaId, angle);
    }
}
