public class CurrentTourService : ICurrentTourService
{
    public Tour GetCurrentTour()
    {
        return new Tour(new Panorama[] { });
    }

    public void SetCurrentTour(Tour tour)
    {

    }
}
