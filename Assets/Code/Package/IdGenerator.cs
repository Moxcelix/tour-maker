using System;

public static class IdGenerator
{
    public static string GenerateTimeHash()
    {
        long ticks = DateTime.UtcNow.Ticks;
        return GetHash(ticks);
    }

    private static string GetHash(long value)
    {
        unchecked
        {
            int hash = (int)(value ^ (value >> 32));

            hash = ((hash << 5) - hash) + (int)(value & 0xFFFFFFFF);

            return hash.ToString("X8");
        }
    }
}