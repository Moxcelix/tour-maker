public class Panorama
{
    public string Id { get; }
    public string Name { get; private set; }
    public float PositionX { get; private set; }
    public float PositionY {  get; private set; }
    public float Rotation {  get; private set; }

    public Panorama(string id, string name, float x = 0, float y = 0, float rotation = 0)
    {
        Id = id;
        Name = name;
        PositionX = x;
        PositionY = y;
        Rotation = rotation;
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
