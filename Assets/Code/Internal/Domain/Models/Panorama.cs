public class Panorama
{
    public string Id { get; }
    public string Name { get; private set; }

    public Panorama(string id)
    {
        Id = id;
    }

    public void Rename(string name)
    {
        Name = name;
    }
}
