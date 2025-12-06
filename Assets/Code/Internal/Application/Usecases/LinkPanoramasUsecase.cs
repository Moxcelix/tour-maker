public class LinkPanoramasUsecase
{
    private readonly ICurrentTourService _currentTourService;
    private readonly ITourRepository _tourRepository;

    public LinkPanoramasUsecase(ICurrentTourService currentTourService, ITourRepository tourRepository)
    {
        _currentTourService = currentTourService;
        _tourRepository = tourRepository;
    }

    public Bridge Execute(string panorama1Id, string panorama2Id)
    {
        var tour = _currentTourService.GetCurrentTour();

        var bridge = tour.Link(panorama1Id, panorama2Id);

        _tourRepository.Update(tour);

        return bridge;
    }
}
