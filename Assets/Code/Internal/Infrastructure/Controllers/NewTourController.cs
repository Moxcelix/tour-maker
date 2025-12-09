public class NewTourController
{
    private readonly NewTourUsecase newTourUsecase;

    private readonly UIButton button;

    public NewTourController(NewTourUsecase newTourUsecase, UIButton button)
    {
        this.newTourUsecase = newTourUsecase;
        this.button = button;

        this.button.Clicked += NewTour;
    }

    private void NewTour()
    {
        newTourUsecase.Execute("test");
    }
}
