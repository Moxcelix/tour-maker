public class Panorama
{
    public string Id { get; }
    public string Name { get; private set; }
    public float PositionX { get; private set; }
    public float PositionY {  get; private set; }

    public Panorama(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public void Rename(string name)
    {
        Name = name;
    }

    public void Move(float x, float y)
    {
        PositionX = x;
        PositionY = y;
    }
}
