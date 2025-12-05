public class CurrentTourService : ICurrentTourService
{
    private Tour currentTour = null;

    public Tour GetCurrentTour()
    {
        return currentTour;
    }

    public void SetCurrentTour(Tour tour)
    {
        currentTour = tour;
    }
}
