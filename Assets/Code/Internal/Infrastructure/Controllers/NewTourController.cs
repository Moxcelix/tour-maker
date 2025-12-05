public class NewTourController
{
    private readonly NewTourUsecase newTourUsecase;

    public NewTourController(NewTourUsecase newTourUsecase)
    {
        this.newTourUsecase = newTourUsecase;
    }

    public void NewTour()
    {
        newTourUsecase.Execute("unsaved");
    }
}
