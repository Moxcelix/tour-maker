using System.Collections.Generic;

public class TexturePathService
{
    private readonly Dictionary<string, string> panoramaPathes = new Dictionary<string, string>();

    public void RegisterPath(string panoramaId, string path)
    {
        panoramaPathes[panoramaId] = path;
    }

    public string GetPath(string panoramaId)
    {
        return panoramaPathes[panoramaId];
    }
}
