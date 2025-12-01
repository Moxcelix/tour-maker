public class IdGeneratorService
{
    public string GenerateId()
    {
        return IdGenerator.GenerateTimeHash();
    }
}
