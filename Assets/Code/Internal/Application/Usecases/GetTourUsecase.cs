public class GetTourUsecase
{
    private readonly ICurrentTourService _currentTourService;

    public GetTourUsecase(ICurrentTourService currentTourService)
    {
        _currentTourService = currentTourService;
    }

    public Tour Execute()
    {
        return _currentTourService.GetCurrentTour();
    }
}
