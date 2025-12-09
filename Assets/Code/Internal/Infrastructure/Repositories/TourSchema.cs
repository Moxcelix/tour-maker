using System;

[Serializable]
public struct TourSchema
{
    public string name;
    public PanoramaSchema[] panoramas;
    public BridgeSchema[] bridges;
}

[Serializable]
public struct PanoramaSchema
{
    public string id;
    public string name;
    public float x;
    public float y;
    public string path;
}

[Serializable]
public struct BridgeSchema
{
    public string panorama_1_id;
    public string panorama_2_id;
}