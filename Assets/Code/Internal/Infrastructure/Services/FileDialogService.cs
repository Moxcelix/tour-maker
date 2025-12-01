using SFB;

public class FileDialogService
{
    public string OpenFileDialog()
    {

        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

        return paths[0];
    }

}
