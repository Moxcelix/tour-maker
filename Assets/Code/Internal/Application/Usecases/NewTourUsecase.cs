public class NewTourUsecase
{
    private readonly ICurrentTourService _currentTourService;
    private readonly ITourRepository _repository;

    public NewTourUsecase(
        ICurrentTourService currentTourService,
        ITourRepository repository)
    {
        _currentTourService = currentTourService;
        _repository = repository;
    }

    public void Execute(string name)
    {
        var tour = new Tour(name, new Panorama[] { }, new Bridge[] { });

        _repository.Create(tour);

        _currentTourService.SetCurrentTour(tour);
    }
}
