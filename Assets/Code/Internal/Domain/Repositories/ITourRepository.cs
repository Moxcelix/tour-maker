public interface ITourRepository
{
    public void Create(Tour tour);
    public void Update(Tour tour);
    public void Delete(string tourId);
    public Tour Get(string tourId);
}
