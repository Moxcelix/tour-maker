public class OpenTourUsecase
{
    private readonly ICurrentTourService _currentTourService;
    private readonly ITourRepository _repository;

    public OpenTourUsecase(
        ICurrentTourService currentTourService,
        ITourRepository repository)
    {
        _currentTourService = currentTourService;
        _repository = repository;
    }

    public Tour Execute(string name)
    {
        var tour = _repository.Get(name);

        _currentTourService.SetCurrentTour(tour);

        return tour;
    }
}
