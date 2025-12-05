public class RenamePanoramaController
{
    private readonly RenamePanoramaUsecase renamePanoramaUsecase;

    public delegate void OnRenamePanorama(string id, string panoramaName);
    public event OnRenamePanorama OnRenamePanoramaEvent;

    public RenamePanoramaController(RenamePanoramaUsecase renamePanoramaUsecase)
    {
        this.renamePanoramaUsecase = renamePanoramaUsecase;
    }

    public void RenamePanorama(string id, string name)
    {
        var panorama = renamePanoramaUsecase.Execute(id, name);

        OnRenamePanoramaEvent?.Invoke(panorama.Id, panorama.Name);
    }
}
