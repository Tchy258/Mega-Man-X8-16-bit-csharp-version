using Godot;
using System.Globalization;

public class UserDataFile : Node
{
    public string CurrentLanguage { get; private set; } = "en";

    public override void _Ready()
    {
        CurrentLanguage = GetStoredLanguage();
    }
    private string GetStoredLanguage()
    {
        var config = new ConfigFile();
        Error loadError = config.Load("user://settings.cfg");

        if (loadError == Error.Ok)
        {
            return (string)config.GetValue("General", "Language", GetSystemLanguage());
        }

        return GetSystemLanguage();
    }

    public void SetLanguage(string languageCode)
    {
        CurrentLanguage = languageCode;

        var config = new ConfigFile();
        config.SetValue("General", "Language", languageCode);
        config.Save("user://settings.cfg");
    }

    private string GetSystemLanguage()
    {
        return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    }
}
